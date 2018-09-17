using System;
using CodeProject.PurchaseOrderManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.PurchaseOrderManagement.Data.Entities;
using CodeProject.PurchaseOrderManagement.Data.Transformations;
using CodeProject.Shared.Common.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using CodeProject.Shared.Common.Utilities;
using CodeProject.Shared.Common.Models.MessageQueuePayloads;

namespace CodeProject.PurchaseOrderManagement.BusinessServices
{
	public class PurchaseOrderManagementBusinessService : IPurchaseOrderManagementBusinessService
	{
		private readonly IPurchaseOrderManagementDataService _purchaseOrderManagementDataService;
		private readonly ConnectionStrings _connectionStrings;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Acount Business Service
		/// </summary>
		/// <param name="accountDataService"></param>
		public PurchaseOrderManagementBusinessService(IPurchaseOrderManagementDataService purchaseOrderManagementDataService, ConnectionStrings connectionStrings)
		{
			_purchaseOrderManagementDataService = purchaseOrderManagementDataService;
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
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_purchaseOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				product.AccountId = productDataTransformation.AccountId;
				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
			
				await _purchaseOrderManagementDataService.CreateProduct(product);

				await _purchaseOrderManagementDataService.UpdateDatabase();

				_purchaseOrderManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_purchaseOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_purchaseOrderManagementDataService.CloseConnection();
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
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_purchaseOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				int productId = productDataTransformation.ProductId;

				product = await _purchaseOrderManagementDataService.GetProductInformationForUpdate(productId);

				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
			
				await _purchaseOrderManagementDataService.UpdateProduct(product);

				await _purchaseOrderManagementDataService.UpdateDatabase();

				_purchaseOrderManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_purchaseOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_purchaseOrderManagementDataService.CloseConnection();
			}

			returnResponse.Entity = productDataTransformation;

			return returnResponse;

		}
	
	}

}

