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
		Task CreateTransactionQueue(TransactionQueue transactionQueue);
		Task CreateCustomer(Customer customer);
		Task CreateProduct(Product product);
		Task<Product> GetProductInformationForUpdate(int productId);
		Task UpdateProduct(Product product);
	}
}
