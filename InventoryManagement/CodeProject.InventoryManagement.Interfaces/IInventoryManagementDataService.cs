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
		Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue);
		Task<Product> GetProductInformation(int productId);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task<Product> GetProductInformationByProductNumber(string productNumber, int accountId);
		Task UpdateProduct(Product product);
		Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue();
		Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue);


	}
}
