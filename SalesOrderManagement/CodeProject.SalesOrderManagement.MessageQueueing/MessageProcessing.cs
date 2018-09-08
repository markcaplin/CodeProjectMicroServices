using System;
using CodeProject.SalesOrderManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.SalesOrderManagement.BusinessRules;
using CodeProject.SalesOrderManagement.Data.Entities;
using CodeProject.SalesOrderManagement.Data.Transformations;
using CodeProject.Shared.Common.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using CodeProject.Shared.Common.Utilities;
using CodeProject.Shared.Common.Models.MessageQueuePayloads;
using CodeProject.Shared.Common.Utilties;
using CodeProject.Shared.Common.Interfaces;
using Newtonsoft.Json;

namespace CodeProject.SalesOrderManagement.Business.MessageService
{
	public class MessageProcessing : IMessageQueueProcessing
	{
		ISalesOrderManagementDataService _salesOrderManagementDataService;

		public IConfiguration configuration { get; }

		/// <summary>
		/// SalesOrder Management Message Processing
		/// </summary>
		/// <param name="salesOrderManagementDataService"></param>
		public MessageProcessing(ISalesOrderManagementDataService salesOrderManagementDataService)
		{
			_salesOrderManagementDataService = salesOrderManagementDataService;
		}
		/// <summary>
		///  Send Queue Messages
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseModel<List<MessageQueue>>> SendQueueMessages(IMessageQueueing messageQueueing)
		{

			ResponseModel<List<MessageQueue>> returnResponse = new ResponseModel<List<MessageQueue>>();
			returnResponse.Entity = new List<MessageQueue>();
			try
			{
				_salesOrderManagementDataService.OpenConnection();
				_salesOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				List<TransactionQueueOutbound> transactionQueue = await _salesOrderManagementDataService.GetOutboundTransactionQueue();
				foreach (TransactionQueueOutbound transactionQueueItem in transactionQueue)
				{
					MessageQueue message = new MessageQueue();
					message.ExchangeName = transactionQueueItem.ExchangeName;
					message.TransactionQueueId = transactionQueueItem.TransactionQueueOutboundId;
					message.TransactionCode = transactionQueueItem.TransactionCode;
					message.Payload = transactionQueueItem.Payload;

					ResponseModel<MessageQueue> messageQueueResponse = messageQueueing.SendMessage(message);
					if (messageQueueResponse.ReturnStatus == true)
					{
						transactionQueueItem.SentToExchange = true;
						transactionQueueItem.DateSentToExchange = DateTime.UtcNow;
						await _salesOrderManagementDataService.UpdateOutboundTransactionQueue(transactionQueueItem);

						returnResponse.Entity.Add(message);
					}
				}

				await _salesOrderManagementDataService.UpdateDatabase();

				_salesOrderManagementDataService.CommitTransaction();
				_salesOrderManagementDataService.CloseConnection();


			}
			catch (Exception ex)
			{
				_salesOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_salesOrderManagementDataService.CloseConnection();
			}

			return returnResponse;

		}

		/// <summary>
		/// Commit Inbound Message
		/// </summary>
		/// <param name="messageQueue"></param>
		/// <returns></returns>
		public async Task<ResponseModel<MessageQueue>> CommitInboundMessage(MessageQueue messageQueue)
		{

			ResponseModel<MessageQueue> returnResponse = new ResponseModel<MessageQueue>();

			try
			{
				_salesOrderManagementDataService.OpenConnection();
				_salesOrderManagementDataService.BeginTransaction((int)IsolationLevel.ReadCommitted);

				TransactionQueueInbound transactionQueue = new TransactionQueueInbound();
				transactionQueue.ExchangeName = messageQueue.ExchangeName;
				transactionQueue.SenderTransactionQueueId = messageQueue.TransactionQueueId;
				transactionQueue.TransactionCode = messageQueue.TransactionCode;
				transactionQueue.Payload = messageQueue.Payload;

				await _salesOrderManagementDataService.CreateInboundTransactionQueue(transactionQueue);

				await _salesOrderManagementDataService.UpdateDatabase();

				_salesOrderManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_salesOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_salesOrderManagementDataService.CloseConnection();
			}

			returnResponse.Entity = messageQueue;

			return returnResponse;

		}

		/// <summary>
		/// Process Messages
		/// </summary>
		/// <returns></returns>
		public async Task<ResponseModel<List<MessageQueue>>> ProcessMessages()
		{

			ResponseModel<List<MessageQueue>> returnResponse = new ResponseModel<List<MessageQueue>>();
			returnResponse.Entity = new List<MessageQueue>();
			try
			{
				_salesOrderManagementDataService.OpenConnection();
		
				List<TransactionQueueInbound> transactionQueue = await _salesOrderManagementDataService.GetInboundTransactionQueue();
				foreach (TransactionQueueInbound transactionQueueItem in transactionQueue)
				{
					_salesOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

					int senderId = transactionQueueItem.SenderTransactionQueueId;
					string exchangeName = transactionQueueItem.ExchangeName;
					string transactionCode = transactionQueueItem.TransactionCode;

					TransactionQueueInboundHistory transactionHistory = await _salesOrderManagementDataService.GetInboundTransactionQueueHistoryBySender(senderId, exchangeName);
					if (transactionHistory != null)
					{ 
						await LogDuplicateMessage(transactionQueueItem);
						await _salesOrderManagementDataService.DeleteInboundTransactionQueueEntry(transactionQueueItem.TransactionQueueInboundId);
					}
					else if (transactionCode == TransactionQueueTypes.ProductUpdated)
					{
						await ProductUpdated(transactionQueueItem);
						await _salesOrderManagementDataService.DeleteInboundTransactionQueueEntry(transactionQueueItem.TransactionQueueInboundId);
					}

					await _salesOrderManagementDataService.UpdateDatabase();

					_salesOrderManagementDataService.CommitTransaction();

				}

				_salesOrderManagementDataService.CloseConnection();

			}
			catch (Exception ex)
			{
				_salesOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_salesOrderManagementDataService.CloseConnection();
			}

			return returnResponse;
		}

		/// <summary>
		/// Product Updated
		/// </summary>
		/// <param name="transaction"></param>
		private async Task ProductUpdated(TransactionQueueInbound transaction)
		{

			ProductUpdatePayload payload = JsonConvert.DeserializeObject<ProductUpdatePayload>(transaction.Payload);

			int productMasterId = payload.ProductId;

			Product product = await _salesOrderManagementDataService.GetProductInformationByProductMasterForUpdate(productMasterId);
			if (product != null)
			{
				product.ProductNumber = payload.ProductNumber;
				product.Description = payload.Description;
				product.UnitPrice = payload.UnitPrice;

				await _salesOrderManagementDataService.UpdateProduct(product);
			}
			else
			{
				product = new Product();
				product.ProductNumber = payload.ProductNumber;
				product.ProductMasterId = payload.ProductId;
				product.Description = payload.Description;
				product.UnitPrice = payload.UnitPrice;

				await _salesOrderManagementDataService.CreateProduct(product);

			}

			await LogSuccessfullyProcessed(transaction);
		}



		/// <summary>
		/// Log Successfully Processed
		/// </summary>
		/// <param name="transaction"></param>
		/// <returns></returns>
		private async Task LogSuccessfullyProcessed(TransactionQueueInbound transaction)
		{
			TransactionQueueInboundHistory transactionHistory = new TransactionQueueInboundHistory();
			transactionHistory.TransactionQueueInboundId = transaction.TransactionQueueInboundId;
			transactionHistory.SenderTransactionQueueId = transaction.SenderTransactionQueueId;
			transactionHistory.TransactionCode = transaction.TransactionCode;
			transactionHistory.Payload = transaction.Payload;
			transactionHistory.ExchangeName = transaction.ExchangeName;
			transactionHistory.ProcessedSuccessfully = true;
			transactionHistory.DuplicateMessage = false;
			transactionHistory.ErrorMessage = string.Empty;
			transactionHistory.DateCreatedInbound = transaction.DateCreated;

			await _salesOrderManagementDataService.CreateInboundTransactionQueueHistory(transactionHistory);
		}


		/// <summary>
		///  Log Duplicate Message
		/// </summary>
		/// <param name="transactionQueueItem"></param>
		/// <returns></returns>
		private async Task LogDuplicateMessage(TransactionQueueInbound transactionQueueItem)
		{
			// log history as duplicate
			TransactionQueueInboundHistory transactionHistory = new TransactionQueueInboundHistory();
			transactionHistory.TransactionQueueInboundId = transactionQueueItem.TransactionQueueInboundId;
			transactionHistory.SenderTransactionQueueId = transactionQueueItem.SenderTransactionQueueId;
			transactionHistory.TransactionCode = transactionQueueItem.TransactionCode;
			transactionHistory.Payload = transactionQueueItem.Payload;
			transactionHistory.ExchangeName = transactionQueueItem.ExchangeName;
			transactionHistory.ProcessedSuccessfully = false;
			transactionHistory.DuplicateMessage = true;
			transactionHistory.ErrorMessage = string.Empty;
			transactionHistory.DateCreatedInbound = transactionQueueItem.DateCreated;

			await _salesOrderManagementDataService.CreateInboundTransactionQueueHistory(transactionHistory);
			
		}
	}

}

