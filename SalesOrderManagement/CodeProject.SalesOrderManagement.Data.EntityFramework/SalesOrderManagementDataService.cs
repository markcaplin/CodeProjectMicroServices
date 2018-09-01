using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.SalesOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.SalesOrderManagement.Data.Entities;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;

namespace CodeProject.SalesOrderManagement.Data.EntityFramework
{
	public class SalesOrderManagementDataService : EntityFrameworkRepository, ISalesOrderManagementDataService
	{
		/// <summary>
		/// Create Sales Order
		/// </summary>
		/// <param name="salesOrder"></param>
		/// <returns></returns>
		public async Task CreateSalesOrder(SalesOrder salesOrder)
		{
			DateTime dateCreated = DateTime.UtcNow;
			salesOrder.DateCreated = dateCreated;
			salesOrder.DateUpdated = dateCreated;

			await dbConnection.SalesOrders.AddAsync(salesOrder);
		}

		/// <summary>
		/// Create Sales Order Detail
		/// </summary>
		/// <param name="salesOrderDetail"></param>
		/// <returns></returns>
		public async Task CreateSalesOrderDetail(SalesOrderDetail salesOrderDetail)
		{
			DateTime dateCreated = DateTime.UtcNow;
			salesOrderDetail.DateCreated = dateCreated;
			salesOrderDetail.DateUpdated = dateCreated;

			await dbConnection.SalesOrderDetails.AddAsync(salesOrderDetail);

		}

		/// <summary>
		/// Create Transaction Queue
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task CreateTransactionQueue(TransactionQueue transactionQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueue.DateCreated = dateCreated;

			await dbConnection.TransactionQueue.AddAsync(transactionQueue);
		}

		/// <summary>
		/// Create Customer
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		public async Task CreateCustomer(Customer customer)
		{
			DateTime dateCreated = DateTime.UtcNow;
			customer.DateCreated = dateCreated;
			customer.DateUpdated = dateCreated;

			await dbConnection.Customers.AddAsync(customer);

		}

		/// <summary>
		/// Create Product
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		public async Task CreateProduct(Product product)
		{
			DateTime dateCreated = DateTime.UtcNow;
			product.DateCreated = dateCreated;
			product.DateUpdated = dateCreated;

			await dbConnection.Products.AddAsync(product);
		}

		/// <summary>
		/// Get Product Information For Update
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		public async Task<Product> GetProductInformationForUpdate(int productId)
		{
			string sqlStatement = "SELECT * FROM PRODUCTS WITH (UPDLOCK) WHERE PRODUCTID = @ProductId";

			DbParameter productIdParameter = new SqlParameter("ProductId", productId);

			Product product = await dbConnection.Products.FromSql(sqlStatement, productIdParameter).FirstOrDefaultAsync();
			return product;
		}
		/// <summary>
		/// Update Product
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		public async Task UpdateProduct(Product product)
		{
			await Task.Delay(0);
			DateTime dateUpdated = DateTime.UtcNow;
			product.DateUpdated = dateUpdated;

		}
	}
}
