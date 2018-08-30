using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.InventoryManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.InventoryManagement.Data.Entities;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;

namespace CodeProject.InventoryManagement.Data.EntityFramework
{
	public class InventoryManagementDataService : EntityFrameworkRepository, IInventoryManagementDataService
	{
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
		/// Get Product Information
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		public async Task<Product> GetProductInformation(int productId)
		{
			Product product = await dbConnection.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
			return product;
		}
		/// <summary>
		/// Get Product Information By ProductNumber
		/// </summary>
		/// <param name="productNumber"></param>
		/// <param name="accountId"></param>
		/// <returns></returns>
		public async Task<Product> GetProductInformationByProductNumber(string productNumber, int accountId)
		{
			Product product = await dbConnection.Products.Where(x =>
			x.ProductNumber == productNumber && x.AccountId == accountId)
			.FirstOrDefaultAsync();
			return product;
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
