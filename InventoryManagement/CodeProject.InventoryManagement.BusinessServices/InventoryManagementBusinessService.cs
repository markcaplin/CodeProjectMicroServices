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
		IInventoryManagementDataService _inventoryManagementDataService;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Acount Business Service
		/// </summary>
		/// <param name="accountDataService"></param>
		public InventoryManagementBusinessService(IInventoryManagementDataService inventoryManagementDataService)
		{
			_inventoryManagementDataService = inventoryManagementDataService;
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
				_inventoryManagementDataService.OpenConnection();
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
				_inventoryManagementDataService.OpenConnection();
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



	}

}

