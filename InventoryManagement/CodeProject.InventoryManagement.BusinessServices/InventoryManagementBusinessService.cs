using System;
using CodeProject.InventoryManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.InventoryManagement.BusinessRules;
using CodeProject.InventoryManagement.Data.Entities;
using CodeProject.InventoryManagement.Data.Transformations;
using CodeProject.Shared.Common.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using CodeProject.Shared.Common.Utilities;
using CodeProject.Shared.Common.Models.MessageQueuePayloads;
using CodeProject.Shared.Common.Utilties;
using CodeProject.Shared.Common.Interfaces;
using Newtonsoft.Json;

namespace CodeProject.InventoryManagement.BusinessServices
{
	public class InventoryManagementBusinessService : IInventoryManagementBusinessService
	{
		private readonly IInventoryManagementDataService _inventoryManagementDataService;
		private readonly ConnectionStrings _connectionStrings;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Inventory Management
		/// </summary>
		/// <param name="inventoryManagementDataService"></param>
		/// <param name="connectionStrings"></param>
		public InventoryManagementBusinessService(IInventoryManagementDataService inventoryManagementDataService, ConnectionStrings connectionStrings)
		{
			_inventoryManagementDataService = inventoryManagementDataService;
			_connectionStrings = connectionStrings;
		}
		/// <summary>
		/// Create Product
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<ProductDataTransformation>> CreateProduct(ProductDataTransformation productDataTransformation)
		{

			ResponseModel<ProductDataTransformation> returnResponse = new ResponseModel<ProductDataTransformation>();

			Product product = new Product();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				ProductBusinessRules<ProductDataTransformation> productBusinessRules = new ProductBusinessRules<ProductDataTransformation>(productDataTransformation, _inventoryManagementDataService);
				ValidationResult validationResult = await productBusinessRules.Validate();
				if (validationResult.ValidationStatus == false)
				{
					_inventoryManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage = validationResult.ValidationMessages;
					returnResponse.ReturnStatus = validationResult.ValidationStatus;

					return returnResponse;
				}

				product.AccountId = productDataTransformation.AccountId;
				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
				product.BinLocation = productDataTransformation.BinLocation;

				await _inventoryManagementDataService.CreateProduct(product);

				await _inventoryManagementDataService.UpdateDatabase();

				TransactionQueueOutbound transactionQueue = new TransactionQueueOutbound();
				transactionQueue.Payload = GenerateProductUpdatePayload(product);
				transactionQueue.TransactionCode = TransactionQueueTypes.ProductUpdated;
				transactionQueue.ExchangeName = MessageQueueExchanges.InventoryManagement;

				await _inventoryManagementDataService.CreateOutboundTransactionQueue(transactionQueue);

				await _inventoryManagementDataService.UpdateDatabase();

				_inventoryManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			productDataTransformation.ProductId = product.ProductId;

			returnResponse.Entity = productDataTransformation;

			return returnResponse;

		}

		/// <summary>
		/// Update Product
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<ProductDataTransformation>> UpdateProduct(ProductDataTransformation productDataTransformation)
		{

			ResponseModel<ProductDataTransformation> returnResponse = new ResponseModel<ProductDataTransformation>();

			Product product = new Product();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				ProductBusinessRules<ProductDataTransformation> productBusinessRules = new ProductBusinessRules<ProductDataTransformation>(productDataTransformation, _inventoryManagementDataService);
				ValidationResult validationResult = await productBusinessRules.Validate();
				if (validationResult.ValidationStatus == false)
				{
					_inventoryManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage = validationResult.ValidationMessages;
					returnResponse.ReturnStatus = validationResult.ValidationStatus;

					return returnResponse;
				}

				int productId = productDataTransformation.ProductId;

				product = await _inventoryManagementDataService.GetProductInformationForUpdate(productId);

				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
				product.BinLocation = productDataTransformation.BinLocation;

				await _inventoryManagementDataService.UpdateProduct(product);

				TransactionQueueOutbound transactionQueue = new TransactionQueueOutbound();
				transactionQueue.Payload = GenerateProductUpdatePayload(product);
				transactionQueue.TransactionCode = TransactionQueueTypes.ProductUpdated;
				transactionQueue.ExchangeName = MessageQueueExchanges.InventoryManagement;

				await _inventoryManagementDataService.CreateOutboundTransactionQueue(transactionQueue);

				await _inventoryManagementDataService.UpdateDatabase();

				_inventoryManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			returnResponse.Entity = productDataTransformation;

			return returnResponse;

		}
		/// <summary>
		/// Generate Product Update Payload
		/// </summary>
		/// <param name="product"></param>
		/// <returns></returns>
		private string GenerateProductUpdatePayload(Product product)
		{
			ProductUpdatePayload productUpdatePayload = new ProductUpdatePayload();
			productUpdatePayload.AccountId = product.AccountId;
			productUpdatePayload.ProductId = product.ProductId;
			productUpdatePayload.BinLocation = product.BinLocation;
			productUpdatePayload.Description = product.Description;
			productUpdatePayload.ProductNumber = product.ProductNumber;
			productUpdatePayload.UnitPrice = product.UnitPrice;

			string payload = SerializationFunction<ProductUpdatePayload>.ReturnStringFromObject(productUpdatePayload);

			return payload;

		}

		/// <summary>
		/// Generate Inventory Transaction Payload
		/// </summary>
		/// <param name="inventoryTransaction"></param>
		/// <returns></returns>
		private string GenerateInventoryTransactionPayload(InventoryTransaction inventoryTransaction)
		{
			InventoryTransactionPayload inventoryTransactionPayload = new InventoryTransactionPayload();

			inventoryTransactionPayload.ProductId = inventoryTransaction.ProductId;
			inventoryTransactionPayload.Quantity = inventoryTransaction.Quantity;
			inventoryTransactionPayload.UnitCost = inventoryTransaction.UnitCost;
			inventoryTransactionPayload.EntityId = inventoryTransaction.EntityId;
			inventoryTransactionPayload.MasterEntityId = inventoryTransaction.MasterEntityId;
			inventoryTransactionPayload.TransactionDate = inventoryTransaction.TransactionDate;

			string payload = SerializationFunction<InventoryTransactionPayload>.ReturnStringFromObject(inventoryTransactionPayload);

			return payload;


		}

		/// <summary>
		/// Purchase Order Inquiry
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="supplierName"></param>
		/// <param name="currentPageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public async Task<ResponseModel<List<PurchaseOrderDataTransformation>>> PurchaseOrderInquiry(int accountId, string supplierName, int currentPageNumber, int pageSize, string sortExpression, string sortDirection)
		{

			ResponseModel<List<PurchaseOrderDataTransformation>> returnResponse = new ResponseModel<List<PurchaseOrderDataTransformation>>();

			List<PurchaseOrderDataTransformation> purchaseOrders = new List<PurchaseOrderDataTransformation>();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				DataGridPagingInformation dataGridPagingInformation = new DataGridPagingInformation();
				dataGridPagingInformation.CurrentPageNumber = currentPageNumber;
				dataGridPagingInformation.PageSize = pageSize;
				dataGridPagingInformation.SortDirection = sortDirection;
				dataGridPagingInformation.SortExpression = sortExpression;

				List<PurchaseOrder> purchaseOrderList = await _inventoryManagementDataService.PurchaseOrderInquiry(accountId, supplierName, dataGridPagingInformation);
				foreach (PurchaseOrder purchaseOrder in purchaseOrderList)
				{
					PurchaseOrderDataTransformation purchaseOrderDataTransformation = new PurchaseOrderDataTransformation();
					purchaseOrderDataTransformation.AddressLine1 = purchaseOrder.AddressLine1;
					purchaseOrderDataTransformation.AddressLine2 = purchaseOrder.AddressLine2;
					purchaseOrderDataTransformation.City = purchaseOrder.City;
					purchaseOrderDataTransformation.Region = purchaseOrder.Region;
					purchaseOrderDataTransformation.PostalCode = purchaseOrder.PostalCode;
					purchaseOrderDataTransformation.SupplierName = purchaseOrder.SupplierName;
					purchaseOrderDataTransformation.DateCreated = purchaseOrder.DateCreated;
					purchaseOrderDataTransformation.OrderTotal = purchaseOrder.OrderTotal;
					purchaseOrderDataTransformation.AccountId = purchaseOrder.AccountId;
					purchaseOrderDataTransformation.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
					purchaseOrderDataTransformation.PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber;
					purchaseOrderDataTransformation.PurchaseOrderStatusId = purchaseOrder.PurchaseOrderStatusId;
					purchaseOrderDataTransformation.PurchaseOrderStatusDescription = purchaseOrder.PurchaseOrderStatus.Description;
					purchaseOrders.Add(purchaseOrderDataTransformation);
				}

				returnResponse.Entity = purchaseOrders;
				returnResponse.TotalRows = dataGridPagingInformation.TotalRows;
				returnResponse.TotalPages = dataGridPagingInformation.TotalPages;

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{

				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			return returnResponse;

		}

		/// <summary>
		/// Update Purchase Order Detail
		/// </summary>
		/// <param name="purchaseOrderDetailDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<PurchaseOrderDetailDataTransformation>> UpdatePurchaseOrderDetail(PurchaseOrderDetailDataTransformation purchaseOrderDetailDataTransformation)
		{

			ResponseModel<PurchaseOrderDetailDataTransformation> returnResponse = new ResponseModel<PurchaseOrderDetailDataTransformation>();

			PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();

			try
			{
				int accountId = purchaseOrderDetailDataTransformation.AccountId;
				int purchaseOrderId = purchaseOrderDetailDataTransformation.PurchaseOrderId;
				int purchaseOrderDetailId = purchaseOrderDetailDataTransformation.PurchaseOrderDetailId;

				if (purchaseOrderDetailDataTransformation.CurrentReceivedQuantity == 0)
				{
					returnResponse.ReturnMessage.Add("Invalid Received Quantity");
					returnResponse.ReturnStatus = false;

					return returnResponse;
				}

				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_inventoryManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				PurchaseOrder purchaseOrder = await _inventoryManagementDataService.GetPurchaseOrderHeader(accountId, purchaseOrderId);
				if (purchaseOrder == null)
				{
					_inventoryManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage.Add("Purchase Order not found");
					returnResponse.ReturnStatus = false;

					return returnResponse;
				}

				purchaseOrderDetail = await _inventoryManagementDataService.GetPurchaseOrderDetailForUpdate(purchaseOrderDetailId);
				if (purchaseOrderDetail == null)
				{
					_inventoryManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage.Add("Purchase Order Detail not found");
					returnResponse.ReturnStatus = false;

					return returnResponse;
				}

				purchaseOrderDetail.ReceivedQuantity = purchaseOrderDetail.ReceivedQuantity + purchaseOrderDetailDataTransformation.CurrentReceivedQuantity;

				await _inventoryManagementDataService.UpdatePurchaseOrderDetail(purchaseOrderDetail);

				Product product = await _inventoryManagementDataService.GetProductInformationForUpdate(purchaseOrderDetail.ProductId);
				if (product == null)
				{
					_inventoryManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage.Add("Product not found");
					returnResponse.ReturnStatus = false;

					return returnResponse;
				}

				double newAverageCost = CalculateAverageCost(product.OnHandQuantity, product.AverageCost, purchaseOrderDetailDataTransformation.CurrentReceivedQuantity, purchaseOrderDetail.UnitPrice);
				if (newAverageCost != 0) {
					product.AverageCost = newAverageCost;
				}

				product.OnHandQuantity = product.OnHandQuantity + purchaseOrderDetailDataTransformation.CurrentReceivedQuantity;

				await _inventoryManagementDataService.UpdateProduct(product);

				InventoryTransaction inventoryTransaction = new InventoryTransaction();
				inventoryTransaction.EntityId = purchaseOrderDetail.PurchaseOrderDetailId;
				inventoryTransaction.MasterEntityId = purchaseOrderDetail.MasterPurchaseOrderDetailId;
				inventoryTransaction.ProductId = purchaseOrderDetail.ProductId;
				inventoryTransaction.UnitCost = purchaseOrderDetail.UnitPrice;
				inventoryTransaction.Quantity = purchaseOrderDetailDataTransformation.CurrentReceivedQuantity;
				inventoryTransaction.TransactionDate = DateTime.UtcNow;

				await _inventoryManagementDataService.CreateInventoryTransaction(inventoryTransaction);

				TransactionQueueOutbound transactionQueue = new TransactionQueueOutbound();
				transactionQueue.Payload = GenerateInventoryTransactionPayload(inventoryTransaction);
				transactionQueue.TransactionCode = TransactionQueueTypes.InventoryReceived;
				transactionQueue.ExchangeName = MessageQueueExchanges.InventoryManagement;

				await _inventoryManagementDataService.CreateOutboundTransactionQueue(transactionQueue);

				await _inventoryManagementDataService.UpdateDatabase();

				_inventoryManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			returnResponse.Entity = purchaseOrderDetailDataTransformation;

			return returnResponse;

		}

		/// <summary>
		/// Calculate Average Cost
		/// </summary>
		/// <param name="onHandQuantity"></param>
		/// <param name="averageCost"></param>
		/// <param name="lineItemQuantity"></param>
		/// <param name="lineItemCost"></param>
		/// <returns></returns>
		private double CalculateAverageCost(int onHandQuantity, double averageCost, int lineItemQuantity, double lineItemCost)
		{
			double currentCost = onHandQuantity * averageCost;
			double totalCost = currentCost + (lineItemCost * lineItemQuantity);
			int totalQuantity = onHandQuantity + lineItemQuantity;
			if (totalQuantity == 0)
			{
				return 0.0;
			}
			double newAverageCost = totalCost / totalQuantity;
			return newAverageCost;
		}


		/// <summary>
		/// Get Purchase Order
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="purchaseOrderId"></param>
		/// <returns></returns>
		public async Task<ResponseModel<PurchaseOrderDataTransformation>> GetPurchaseOrder(int accountId, int purchaseOrderId)
		{
			ResponseModel<PurchaseOrderDataTransformation> returnResponse = new ResponseModel<PurchaseOrderDataTransformation>();
			PurchaseOrderDataTransformation purchaseOrderDataTransformation = new PurchaseOrderDataTransformation();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				PurchaseOrder purchaseOrder = await _inventoryManagementDataService.GetPurchaseOrder(accountId, purchaseOrderId);

				purchaseOrderDataTransformation.PurchaseOrderId = purchaseOrderId;
				purchaseOrderDataTransformation.PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber;
				purchaseOrderDataTransformation.PurchaseOrderStatusId = purchaseOrder.PurchaseOrderStatusId;
				purchaseOrderDataTransformation.SupplierName = purchaseOrder.SupplierName;
				purchaseOrderDataTransformation.AddressLine1 = purchaseOrder.AddressLine1;
				purchaseOrderDataTransformation.AddressLine2 = purchaseOrder.AddressLine2;
				purchaseOrderDataTransformation.City = purchaseOrder.City;
				purchaseOrderDataTransformation.Region = purchaseOrder.Region;
				purchaseOrderDataTransformation.PostalCode = purchaseOrder.PostalCode;
				purchaseOrderDataTransformation.OrderTotal = purchaseOrder.OrderTotal;
				purchaseOrderDataTransformation.PurchaseOrderStatusDescription = purchaseOrder.PurchaseOrderStatus.Description;
				purchaseOrderDataTransformation.DateCreated = purchaseOrder.DateCreated;
				purchaseOrderDataTransformation.DateUpdated = purchaseOrder.DateUpdated;
				purchaseOrderDataTransformation.PurchaseOrderDetails = new List<PurchaseOrderDetailDataTransformation>();

				foreach (PurchaseOrderDetail purchaseOrderDetail in purchaseOrder.PurchaseOrderDetails)
				{
					PurchaseOrderDetailDataTransformation purchaseOrderDetailDataTransformation = new PurchaseOrderDetailDataTransformation();
					purchaseOrderDetailDataTransformation.PurchaseOrderDetailId = purchaseOrderDetail.PurchaseOrderDetailId;
					purchaseOrderDetailDataTransformation.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
					purchaseOrderDetailDataTransformation.ProductId = purchaseOrderDetail.ProductId;
					purchaseOrderDetailDataTransformation.ProductNumber = purchaseOrderDetail.Product.ProductNumber;
					purchaseOrderDetailDataTransformation.ProductDescription = purchaseOrderDetail.Product.Description;
					purchaseOrderDetailDataTransformation.UnitPrice = purchaseOrderDetail.UnitPrice;
					purchaseOrderDetailDataTransformation.OrderQuantity = purchaseOrderDetail.OrderQuantity;
					purchaseOrderDetailDataTransformation.ReceivedQuantity = purchaseOrderDetail.ReceivedQuantity;
					purchaseOrderDetailDataTransformation.DateCreated = purchaseOrderDetail.DateCreated;
					purchaseOrderDetailDataTransformation.DateUpdated = purchaseOrderDetail.DateUpdated;

					purchaseOrderDataTransformation.PurchaseOrderDetails.Add(purchaseOrderDetailDataTransformation);
				}

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			returnResponse.Entity = purchaseOrderDataTransformation;

			return returnResponse;

		}

		/// <summary>
		/// Product Inquiry
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="productNumber"></param>
		/// <param name="currentPageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public async Task<ResponseModel<List<ProductDataTransformation>>> ProductInquiry(int accountId, string productNumber, int currentPageNumber, int pageSize, string sortExpression, string sortDirection)
		{

			ResponseModel<List<ProductDataTransformation>> returnResponse = new ResponseModel<List<ProductDataTransformation>>();

			List<ProductDataTransformation> products = new List<ProductDataTransformation>();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				DataGridPagingInformation dataGridPagingInformation = new DataGridPagingInformation();
				dataGridPagingInformation.CurrentPageNumber = currentPageNumber;
				dataGridPagingInformation.PageSize = pageSize;
				dataGridPagingInformation.SortDirection = sortDirection;
				dataGridPagingInformation.SortExpression = sortExpression;

				List<Product> productList = await _inventoryManagementDataService.ProductInquiry(accountId, productNumber, dataGridPagingInformation);
				foreach (Product product in productList)
				{
					ProductDataTransformation productDataTransformation = new ProductDataTransformation();
					productDataTransformation.ProductId = product.ProductId;
					productDataTransformation.AverageCost = product.AverageCost;
					productDataTransformation.BinLocation = product.BinLocation;
					productDataTransformation.CommittedQuantity = product.CommittedQuantity;
					productDataTransformation.DateCreated = product.DateCreated;
					productDataTransformation.Description = product.Description;
					productDataTransformation.OnHandQuantity = product.OnHandQuantity;
					productDataTransformation.OnOrderQuantity = product.OnOrderQuantity;
					productDataTransformation.ProductNumber = product.ProductNumber;
					productDataTransformation.UnitPrice = product.UnitPrice;
				
					products.Add(productDataTransformation);
				}

				returnResponse.Entity = products;
				returnResponse.TotalRows = dataGridPagingInformation.TotalRows;
				returnResponse.TotalPages = dataGridPagingInformation.TotalPages;

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_inventoryManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			return returnResponse;

		}

		/// <summary>
		/// Get Product
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="productId"></param>
		/// <returns></returns>
		public async Task<ResponseModel<ProductDataTransformation>> GetProduct(int accountId, int productId)
		{
			ResponseModel<ProductDataTransformation> returnResponse = new ResponseModel<ProductDataTransformation>();
			ProductDataTransformation productDataTransformation = new ProductDataTransformation();

			try
			{
				_inventoryManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				Product product = await _inventoryManagementDataService.GetProduct(accountId, productId);
				if (product == null)
				{
					returnResponse.ReturnStatus = false;
					returnResponse.ReturnMessage.Add("Product Id not found.");
				}
				else
				{
					productDataTransformation.AccountId = product.AccountId;
					productDataTransformation.ProductId = product.ProductId;
					productDataTransformation.ProductNumber = product.ProductNumber;
					productDataTransformation.Description = product.Description;
					productDataTransformation.AverageCost = product.AverageCost;
					productDataTransformation.BinLocation = product.BinLocation;
					productDataTransformation.CommittedQuantity = product.CommittedQuantity;
					productDataTransformation.OnHandQuantity = product.OnHandQuantity;
					productDataTransformation.OnOrderQuantity = product.OnOrderQuantity;
					productDataTransformation.UnitPrice = product.UnitPrice;
				
					returnResponse.ReturnStatus = true;
				}

			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_inventoryManagementDataService.CloseConnection();
			}

			returnResponse.Entity = productDataTransformation;

			return returnResponse;

		}

	}

}

