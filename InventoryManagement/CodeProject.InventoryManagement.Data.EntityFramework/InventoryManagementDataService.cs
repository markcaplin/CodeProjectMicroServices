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
		public async Task CreateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueue.DateCreated = dateCreated;

			await dbConnection.TransactionQueueOutbound.AddAsync(transactionQueue);

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
			Product product = await dbConnection.Products.Where(x => x.ProductNumber == productNumber && x.AccountId == accountId).FirstOrDefaultAsync();
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
				sqlStatement, sentToExchangeParameter).OrderBy(x=>x.TransactionQueueOutboundId).ToListAsync();

			return transactionQueue;

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
		/// Update Transaction Queue
		/// </summary>
		/// <param name="transactionQueue"></param>
		/// <returns></returns>
		public async Task UpdateOutboundTransactionQueue(TransactionQueueOutbound transactionQueue)
		{
			await Task.Delay(0);
		}

		/// <summary>
		/// Get Inbound Transaction Queue
		/// </summary>
		/// <returns></returns>
		public async Task<List<TransactionQueueInbound>> GetInboundTransactionQueue()
		{
			List<TransactionQueueInbound> transactionQueue = await dbConnection.TransactionQueueInbound.OrderBy(x => x.TransactionQueueInboundId).ToListAsync();
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
		/// Get Outbound Transaction Queue Item By Id
		/// </summary>
		/// <param name="transactionQueueId"></param>
		/// <returns></returns>
		public async Task<TransactionQueueOutbound> GetOutboundTransactionQueueItemById(int transactionQueueId)
		{
			TransactionQueueOutbound transactionQueueItem = await dbConnection.TransactionQueueOutbound.Where(x=>x.TransactionQueueOutboundId == transactionQueueId).FirstOrDefaultAsync();
			return transactionQueueItem;
		}
		/// <summary>
		/// Create Outbound Transaction Queue History
		/// </summary>
		/// <param name="transactionQueueItem"></param>
		/// <returns></returns>
		public async Task CreateOutboundTransactionQueueHistory(TransactionQueueOutboundHistory transactionQueueItem)
		{
			DateTime dateCreated = DateTime.UtcNow;
			transactionQueueItem.DateCreated = dateCreated;

			await dbConnection.TransactionQueueOutboundHistory.AddAsync(transactionQueueItem);
		}

		/// <summary>
		/// Delete Outbound Transaction Queue Entry
		/// </summary>
		/// <param name="transactionQueueId"></param>
		/// <returns></returns>
		public async Task DeleteOutboundTransactionQueueEntry(int transactionQueueId)
		{
			TransactionQueueOutbound transactionQueue = await dbConnection.TransactionQueueOutbound.Where(x => x.TransactionQueueOutboundId == transactionQueueId).FirstOrDefaultAsync();
			dbConnection.TransactionQueueOutbound.Remove(transactionQueue);
		}

	}
}
