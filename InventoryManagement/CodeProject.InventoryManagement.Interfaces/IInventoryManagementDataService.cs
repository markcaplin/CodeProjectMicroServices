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
		Task CreateTransactionQueue(TransactionQueue transactionQueue);
		Task<Product> GetProductInformation(int productId);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task<Product> GetProductInformationByProductNumber(string productNumber, int accountId);
		Task UpdateProduct(Product product);
	
	}
}
