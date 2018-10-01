using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.PurchaseOrderManagement.Data.Entities;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.Shared.Common.Models;

namespace CodeProject.PurchaseOrderManagement.Interfaces
{
    public interface IPurchaseOrderManagementDataService : IDataRepository, IDisposable
	{
		Task<Supplier> GetSupplierInformationForUpdate(int accountId, int supplierId);
		Task<Supplier> GetSupplierInformation(int accountId, int supplierId);
		Task CreatePurchaseOrder(PurchaseOrder purchaseOrder);
		Task CreatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail);
		Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue);
		Task CreateInboundTransactionQueueHistory(TransactionQueueInboundHistory transactionQueueHistory);
		Task<TransactionQueueInboundHistory> GetInboundTransactionQueueHistoryBySender(int senderTransactionQueueId, string exchangeName);
		Task<Supplier> GetSupplierInformationBySupplierName(string supplierName, int accountId);
		Task<Product> GetProductInformationByProductMasterForUpdate(int productMasterId);
		Task DeleteInboundTransactionQueueEntry(int transactionQueueId);
		Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task<List<TransactionQueueInbound>> GetInboundTransactionQueue();
		Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue();
		Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task CreateSupplier(Supplier supplier);
		Task UpdateSupplier(Supplier supplier);
		Task<TransactionQueueSemaphore> GetTransactionQueueSemaphore(string semaphoreKey);
		Task UpdateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
		Task CreateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore);
		Task<List<Supplier>> SupplierInquiry(int accountID, string supplierName, DataGridPagingInformation paging);


	}
}
