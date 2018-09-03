using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.SalesOrderManagement.Data.Entities;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Interfaces;

namespace CodeProject.SalesOrderManagement.Interfaces
{
    public interface ISalesOrderManagementDataService : IDataRepository, IDisposable
	{
		Task CreateSalesOrder(SalesOrder salesOrder);
		Task CreateSalesOrderDetail(SalesOrderDetail salesOrderDetail);
		Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue);
		Task CreateInboundTransactionQueueHistory(TransactionQueueInboundHistory transactionQueueHistory);
		Task<TransactionQueueInboundHistory> GetInboundTransactionQueueHistoryBySender(int senderTransactionQueueId, string exchangeName);
		Task<Product> GetProductInformationByProductMasterForUpdate(int productMasterId);
		Task DeleteInboundTransactionQueueEntry(int transactionQueueId);
		Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task<List<TransactionQueueInbound>> GetInboundTransactionQueue();
		Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue();
		Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);
		Task CreateCustomer(Customer customer);
		Task CreateProduct(Product product);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task UpdateProduct(Product product);
	}
}
