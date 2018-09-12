using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.InventoryManagement.Data.Entities;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Interfaces;

namespace CodeProject.InventoryManagement.Interfaces
{
    public interface IInventoryManagementDataService : IDataRepository, IDisposable
	{
		Task CreateProduct(Product product);
		Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task CreateOutboundTransactionQueueHistory(TransactionQueueOutboundHistory transactionQueueItem);
		Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue);
		Task<Product> GetProductInformation(int productId);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task<Product> GetProductInformationByProductNumber(string productNumber, int accountId);
		Task UpdateProduct(Product product);
		Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue();
		Task<TransactionQueueOutbound> GetOutboundTransactionQueueItemById(int transactionQueueId);
		Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task<List<TransactionQueueInbound>> GetInboundTransactionQueue();
		Task<TransactionQueueInboundHistory> GetInboundTransactionQueueHistoryBySender(int senderTransactionQueueId, string exchangeName);
		Task DeleteInboundTransactionQueueEntry(int transactionQueueId);
		Task DeleteOutboundTransactionQueueEntry(int transactionQueueId);
		Task<TransactionQueueSemaphore> GetTransactionQueueSemaphore(string semaphoreKey);
		Task UpdateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
		Task CreateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
	}
}
