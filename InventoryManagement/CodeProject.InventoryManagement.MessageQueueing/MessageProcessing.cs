using System;
using CodeProject.InventoryManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.InventoryManagement.BusinessRules;
using CodeProject.InventoryManagement.Data.Entities;
using CodeProject.InventoryManagement.Data.Transformations;
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

namespace CodeProject.InventoryManagement.Business.MessageService
{
	public class MessageProcessing : IMessageQueueProcessing
	{
		IInventoryManagementDataService _inventoryManagementDataService;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Inventory Management Message Processing
		/// </summary>
		/// <param name="inventoryManagementDataService"></param>
		public MessageProcessing(IInventoryManagementDataService inventoryManagementDataService)
		{
			_inventoryManagementDataService = inventoryManagementDataService;
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
				_inventoryManagementDataService.OpenConnection();
				_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				List<TransactionQueueOutbound> transactionQueue = await _inventoryManagementDataService.GetOutboundTransactionQueue();
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
						await _inventoryManagementDataService.UpdateOutboundTransactionQueue(transactionQueueItem);

						returnResponse.Entity.Add(message);
					}
					else
					{
						break;
					}
					
				}

				await _inventoryManagementDataService.UpdateDatabase();

				_inventoryManagementDataService.CommitTransaction();
				_inventoryManagementDataService.CloseConnection();

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
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
				_inventoryManagementDataService.OpenConnection();
				_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.ReadCommitted);

				TransactionQueueInbound transactionQueue = new TransactionQueueInbound();
				transactionQueue.ExchangeName = messageQueue.ExchangeName;
				transactionQueue.SenderTransactionQueueId = messageQueue.TransactionQueueId;
				transactionQueue.TransactionCode = messageQueue.TransactionCode;
				transactionQueue.Payload = messageQueue.Payload;

				await _inventoryManagementDataService.CreateInboundTransactionQueue(transactionQueue);

				await _inventoryManagementDataService.UpdateDatabase();

				_inventoryManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
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
				_inventoryManagementDataService.OpenConnection();

				List<TransactionQueueInbound> transactionQueue = await _inventoryManagementDataService.GetInboundTransactionQueue();
				foreach (TransactionQueueInbound transactionQueueItem in transactionQueue)
				{
					_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

					int senderId = transactionQueueItem.SenderTransactionQueueId;
					string exchangeName = transactionQueueItem.ExchangeName;
					string transactionCode = transactionQueueItem.TransactionCode;

					if (transactionCode == TransactionQueueTypes.Acknowledgement)
					{
						await ProcessAcknowledgement(transactionQueueItem);
						await _inventoryManagementDataService.DeleteInboundTransactionQueueEntry(transactionQueueItem.TransactionQueueInboundId);
					}
					else
					{
						//TransactionQueueInboundHistory transactionHistory = await _inventoryManagementDataService.GetInboundTransactionQueueHistoryBySender(senderId, exchangeName);
						// if (transactionHistory != null)
						///{
						//	await LogDuplicateMessage(transactionQueueItem);
						//	await _inventoryManagementDataService.DeleteInboundTransactionQueueEntry(transactionQueueItem.TransactionQueueInboundId);
						//}
					}

					await _inventoryManagementDataService.UpdateDatabase();

					_inventoryManagementDataService.CommitTransaction();

				}

				_inventoryManagementDataService.CloseConnection();

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			return returnResponse;
		}

		/// <summary>
		/// Process Acknowledgement
		/// </summary>
		/// <param name="transaction"></param>
		private async Task ProcessAcknowledgement(TransactionQueueInbound transaction)
		{

			int transactionId = transaction.SenderTransactionQueueId;

			TransactionQueueOutbound transactionQueueItem = await _inventoryManagementDataService.GetOutboundTransactionQueueItemById(transactionId);
			if (transactionQueueItem != null)
			{
				await LogOutboundTransactionToHistory(transactionQueueItem);

			}

		}

		/// <summary>
		/// Log Outbound Transaction To History
		/// </summary>
		/// <param name="transactionQueueItem"></param>
		/// <returns></returns>
		private async Task LogOutboundTransactionToHistory(TransactionQueueOutbound transactionQueueItem)
		{
			TransactionQueueOutboundHistory transactionHistory = new TransactionQueueOutboundHistory();
			transactionHistory.TransactionQueueOutboundId = transactionQueueItem.TransactionQueueOutboundId;
			transactionHistory.TransactionCode = transactionQueueItem.TransactionCode;
			transactionHistory.Payload = transactionQueueItem.Payload;
			transactionHistory.ExchangeName = transactionQueueItem.ExchangeName;
			transactionHistory.SentToExchange = transactionQueueItem.SentToExchange;
			transactionHistory.DateOutboundTransactionCreated = transactionQueueItem.DateCreated;
			transactionHistory.DateSentToExchange = transactionQueueItem.DateSentToExchange;
			transactionHistory.DateToResendToExchange = transactionQueueItem.DateToResendToExchange;

			await _inventoryManagementDataService.CreateOutboundTransactionQueueHistory(transactionHistory);
			await _inventoryManagementDataService.DeleteOutboundTransactionQueueEntry(transactionQueueItem.TransactionQueueOutboundId);
			
		}
	}

}

