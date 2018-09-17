using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.PurchaseOrderManagement.Data.Entities;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Interfaces;

namespace CodeProject.PurchaseOrderManagement.Interfaces
{
    public interface IPurchaseOrderManagementDataService : IDataRepository, IDisposable
	{
		Task CreatePurchaseOrder(PurchaseOrder purchaseOrder);
		Task CreatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail);
		Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue);
		Task CreateInboundTransactionQueueHistory(TransactionQueueInboundHistory transactionQueueHistory);
		Task<TransactionQueueInboundHistory> GetInboundTransactionQueueHistoryBySender(int senderTransactionQueueId, string exchangeName);
		Task<Product> GetProductInformationByProductMasterForUpdate(int productMasterId);
		Task DeleteInboundTransactionQueueEntry(int transactionQueueId);
		Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task<List<TransactionQueueInbound>> GetInboundTransactionQueue();
		Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue();
		Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task CreateSupplier(Supplier customer);
		Task CreateProduct(Product product);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task UpdateProduct(Product product);
		Task<TransactionQueueSemaphore> GetTransactionQueueSemaphore(string semaphoreKey);
		Task UpdateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
		Task CreateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
	}
}
