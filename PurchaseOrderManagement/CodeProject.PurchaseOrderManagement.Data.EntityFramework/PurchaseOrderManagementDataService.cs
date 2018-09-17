using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.PurchaseOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.PurchaseOrderManagement.Data.Entities;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;

namespace CodeProject.PurchaseOrderManagement.Data.EntityFramework
{
	public class PurchaseOrderManagementDataService : EntityFrameworkRepository, IPurchaseOrderManagementDataService
	{
		/// <summary>
		/// Create Purchase Order
		/// </summary>
		/// <param name="purchaseOrder"></param>
		/// <returns></returns>
		public async Task CreatePurchaseOrder(PurchaseOrder purchaseOrder)
		{
			DateTime dateCreated = DateTime.UtcNow;
			purchaseOrder.DateCreated = dateCreated;
			purchaseOrder.DateUpdated = dateCreated;

			await dbConnection.PurchaseOrders.AddAsync(purchaseOrder);
		}

		/// <summary>
		/// Create Sales Order Detail
		/// </summary>
		/// <param name="purchaseOrderDetail"></param>
		/// <returns></returns>
		public async Task CreatePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
		{
			DateTime dateCreated = DateTime.UtcNow;
			purchaseOrderDetail.DateCreated = dateCreated;
			purchaseOrderDetail.DateUpdated = dateCreated;

			await dbConnection.PurchaseOrderDetails.AddAsync(purchaseOrderDetail);

		}

		/// <summary>
		/// Create Inbound Transaction Queue
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task CreateInboundTransactionQueue(TransactionQueueInbound transactionQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueue.DateCreated = dateCreated;

			await dbConnection.TransactionQueueInbound.AddAsync(transactionQueue);
		}

		/// <summary>
		///  Create Inbound Transaction Queue History
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task CreateInboundTransactionQueueHistory(TransactionQueueInboundHistory transactionQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueue.DateCreated = dateCreated;

			await dbConnection.TransactionQueueInboundHistory.AddAsync(transactionQueue);
		}

		/// <summary>
		/// Create Inbound Transaction Queue
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueue.DateCreated = dateCreated;

			await dbConnection.TransactionQueueOutbound.AddAsync(transactionQueue);
		}

		/// <summary>
		/// Create Supplier
		/// </summary>
		/// <param name="customer"></param>
		/// <returns></returns>
		public async Task CreateSupplier(Supplier supplier)
		{
			DateTime dateCreated = DateTime.UtcNow;
			supplier.DateCreated = dateCreated;
			supplier.DateUpdated = dateCreated;

			await dbConnection.Suppliers.AddAsync(supplier);

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
		/// Get Product Information B yProduct Master For Update
		/// </summary>
		/// <param name="productMasterId"></param>
		/// <returns></returns>
		public async Task<Product> GetProductInformationByProductMasterForUpdate(int productMasterId)
		{
			string sqlStatement = "SELECT * FROM Products WITH (UPDLOCK) WHERE ProductMasterId = @ProductMasterId";

			DbParameter productIdParameter = new SqlParameter("ProductMasterId", productMasterId);

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

		/// <summary>
		/// Get Outbound Transaction Queue
		/// </summary>
		/// <returns></returns>
		public async Task<List<TransactionQueueOutbound>> GetOutboundTransactionQueue()
		{
			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.AppendLine(" SELECT * FROM TransactionQueueOutbound WITH (UPDLOCK) WHERE ");
			sqlBuilder.AppendLine(" SentToExchange = @SentToExchange ");

			string sqlStatement = sqlBuilder.ToString();

			SqlParameter sentToExchangeParameter = new SqlParameter("SentToExchange", false);

			List<TransactionQueueOutbound> transactionQueue = await dbConnection.TransactionQueueOutbound.FromSql(
				sqlStatement, sentToExchangeParameter).ToListAsync();

			return transactionQueue;

		}

		/// <summary>
		/// Get Inbound Transaction Queue History By Sender
		/// </summary>
		/// <param name="senderTransactionQueueId"></param>
		/// <returns></returns>
		public async Task<TransactionQueueInboundHistory> GetInboundTransactionQueueHistoryBySender(int senderTransactionQueueId, string exchangeName)
		{
			TransactionQueueInboundHistory transactionQueue = await dbConnection.TransactionQueueInboundHistory.Where(x => x.ExchangeName == exchangeName && x.SenderTransactionQueueId == senderTransactionQueueId).FirstOrDefaultAsync();
			return transactionQueue;
		}

		/// <summary>
		/// Get Inbound Transaction Queue
		/// </summary>
		/// <returns></returns>
		public async Task<List<TransactionQueueInbound>> GetInboundTransactionQueue()
		{
			List<TransactionQueueInbound> transactionQueue = await dbConnection.TransactionQueueInbound.OrderBy(x=>x.TransactionQueueInboundId).ToListAsync();
			return transactionQueue;
		}

		/// <summary>
		/// Delete Inbound Transaction Queue Entry
		/// </summary>
		/// <param name="transactionQueueId"></param>
		/// <returns></returns>
		public async Task DeleteInboundTransactionQueueEntry(int transactionQueueId)
		{
			TransactionQueueInbound transactionQueue = await dbConnection.TransactionQueueInbound.Where(x => x.TransactionQueueInboundId == transactionQueueId).FirstOrDefaultAsync();
			dbConnection.TransactionQueueInbound.Remove(transactionQueue);
		}

		/// <summary>
		/// Update Transaction Queue
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue)
		{
			await Task.Delay(0);
		}

		/// <summary>
		/// Update Transaction Queue Semaphore
		/// </summary>
		/// <param name="transactionQueueSemaphore"></param>
		/// <returns></returns>
		public async Task UpdateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore)
		{
			await Task.Delay(0);
			DateTime dateUpdated = DateTime.UtcNow;
			transactionQueueSemaphore.DateUpdated = dateUpdated;
		}

		/// <summary>
		/// Create Transaction Queue Semaphore
		/// </summary>
		/// <param name="transactionQueueSemaphore"></param>
		/// <returns></returns>
		public async Task CreateTransactionQueueSemaphore(TransactionQueueSemaphore transactionQueueSemaphore)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueueSemaphore.DateCreated = dateCreated;
			transactionQueueSemaphore.DateUpdated = dateCreated;

			await dbConnection.TransactionQueueSemaphores.AddAsync(transactionQueueSemaphore);
		}

		/// <summary>
		/// Get Transaction Queue Semaphore
		/// </summary>
		/// <param name="semaphoreKey"></param>
		/// <returns></returns>
		public async Task<TransactionQueueSemaphore> GetTransactionQueueSemaphore(string semaphoreKey)
		{
			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.AppendLine(" SELECT * FROM TransactionQueueSemaphores WITH (UPDLOCK) WHERE ");
			sqlBuilder.AppendLine(" SemaphoreKey = @SemaphoreKey ");

			string sqlStatement = sqlBuilder.ToString();

			SqlParameter semaphoreKeyParameter = new SqlParameter("SemaphoreKey", semaphoreKey);

			TransactionQueueSemaphore transactionQueue = await dbConnection.TransactionQueueSemaphores.FromSql(sqlStatement, semaphoreKeyParameter).FirstOrDefaultAsync();

			return transactionQueue;

		}
	}
}
