using System;
using CodeProject.LoggingManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.LoggingManagement.Data.Entities;
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

namespace CodeProject.LoggingManagement.Business.MessageService
{
	public class MessageProcessing : IMessageQueueProcessing
	{
		ILoggingManagementDataService _loggingManagementDataService;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Inventory Management Message Processing
		/// </summary>
		/// <param name="inventoryManagementDataService"></param>
		public MessageProcessing(ILoggingManagementDataService loggingManagementDataService)
		{
			_loggingManagementDataService = loggingManagementDataService;
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
				_loggingManagementDataService.OpenConnection();
	
				List<AcknowledgementsQueue> acknowledgementsQueue = await _loggingManagementDataService.ProcessAcknowledgementsQueue();
				foreach (AcknowledgementsQueue transactionQueueItem in acknowledgementsQueue)
				{
					_loggingManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

					MessageQueue message = new MessageQueue();
					message.ExchangeName = transactionQueueItem.ExchangeName;
					message.TransactionQueueId = transactionQueueItem.SenderTransactionQueueId;
					message.TransactionCode = TransactionQueueTypes.Acknowledgement;

					ResponseModel<MessageQueue> messageQueueResponse = messageQueueing.SendMessage(message);
					if (messageQueueResponse.ReturnStatus == true)
					{
						await _loggingManagementDataService.DeleteAcknowledgementsQueue(transactionQueueItem.AcknowledgementsQueueId);
						returnResponse.Entity.Add(message);
					}

					await _loggingManagementDataService.UpdateDatabase();

					_loggingManagementDataService.CommitTransaction();
				}

				_loggingManagementDataService.CloseConnection();

			}
			catch (Exception ex)
			{
				_loggingManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_loggingManagementDataService.CloseConnection();
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
				_loggingManagementDataService.OpenConnection();
				_loggingManagementDataService.BeginTransaction((int)IsolationLevel.ReadCommitted);

				MessagesSent existingMessageSent = await _loggingManagementDataService.GetMessageSent(messageQueue.TransactionQueueId, messageQueue.ExchangeName, messageQueue.TransactionCode);

				if (messageQueue.QueueName != string.Empty && messageQueue.QueueName != null)
				{
					MessagesReceived existingMessageReceived = await _loggingManagementDataService.GetMessageReceived(messageQueue.TransactionQueueId, messageQueue.ExchangeName, messageQueue.TransactionCode, messageQueue.QueueName);
					if (existingMessageReceived != null)
					{
						returnResponse.ReturnStatus = true;
						return returnResponse;
					}

				}

				if (existingMessageSent == null)
				{
					MessagesSent messageSent = new MessagesSent();
					messageSent.ExchangeName = messageQueue.ExchangeName;
					messageSent.SenderTransactionQueueId = messageQueue.TransactionQueueId;
					messageSent.TransactionCode = messageQueue.TransactionCode;
					messageSent.Payload = messageQueue.Payload;

					if (messageSent.TransactionCode == "ProductUpdated")
					{
						messageSent.AcknowledgementsRequired = MessageExchangeFanouts.ProductUpdated;
						messageSent.AcknowledgementsReceived = 0;
					}

					if (messageQueue.QueueName != string.Empty && messageQueue.QueueName != null)
					{
						existingMessageSent.AcknowledgementsReceived = existingMessageSent.AcknowledgementsReceived + 1;
					}

					await _loggingManagementDataService.CreateMessagesSent(messageSent);
				}

				if (messageQueue.QueueName != string.Empty && messageQueue.QueueName != null)
				{
					if (existingMessageSent != null)
					{
						existingMessageSent.AcknowledgementsReceived = existingMessageSent.AcknowledgementsReceived + 1;
						await _loggingManagementDataService.UpdateMessagesSent(existingMessageSent);
					}

					MessagesReceived messageReceived = new MessagesReceived();
					messageReceived.ExchangeName = messageQueue.ExchangeName;
					messageReceived.SenderTransactionQueueId = messageQueue.TransactionQueueId;
					messageReceived.TransactionCode = messageQueue.TransactionCode;
					messageReceived.Payload = messageQueue.Payload;
					messageReceived.QueueName = messageQueue.QueueName;

					await _loggingManagementDataService.CreateMessagesReceived(messageReceived);

					if (existingMessageSent.AcknowledgementsReceived == existingMessageSent.AcknowledgementsReceived)
					{
						AcknowledgementsQueue acknowledgementsQueue = new AcknowledgementsQueue();
						acknowledgementsQueue.ExchangeName = messageQueue.ExchangeName;
						acknowledgementsQueue.SenderTransactionQueueId = messageQueue.TransactionQueueId;
						acknowledgementsQueue.TransactionCode = messageQueue.TransactionCode;

						await _loggingManagementDataService.CreateAcknowledgementsQueue(acknowledgementsQueue);

					}
				}

				await _loggingManagementDataService.UpdateDatabase();

				_loggingManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_loggingManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_loggingManagementDataService.CloseConnection();
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
			await Task.Delay(0);
			ResponseModel<List<MessageQueue>> returnResponse = new ResponseModel<List<MessageQueue>>();
			returnResponse.Entity = new List<MessageQueue>();
			return returnResponse;
		}


	}

}

