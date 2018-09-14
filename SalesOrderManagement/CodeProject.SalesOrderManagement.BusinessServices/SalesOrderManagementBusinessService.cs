using System;
using CodeProject.SalesOrderManagement.Interfaces;
using System.Collections.Generic;
using CodeProject.SalesOrderManagement.BusinessRules;
using CodeProject.SalesOrderManagement.Data.Entities;
using CodeProject.SalesOrderManagement.Data.Transformations;
using CodeProject.Shared.Common.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using CodeProject.Shared.Common.Utilities;
using CodeProject.Shared.Common.Models.MessageQueuePayloads;

namespace CodeProject.SalesOrderManagement.BusinessServices
{
	public class SalesOrderManagementBusinessService : ISalesOrderManagementBusinessService
	{
		private readonly ISalesOrderManagementDataService _salesOrderManagementDataService;
		private readonly ConnectionStrings _connectionStrings;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Acount Business Service
		/// </summary>
		/// <param name="accountDataService"></param>
		public SalesOrderManagementBusinessService(ISalesOrderManagementDataService salesOrderManagementDataService, ConnectionStrings connectionStrings)
		{
			_salesOrderManagementDataService = salesOrderManagementDataService;
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
				_salesOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_salesOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				product.AccountId = productDataTransformation.AccountId;
				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
			
				await _salesOrderManagementDataService.CreateProduct(product);

				await _salesOrderManagementDataService.UpdateDatabase();

				_salesOrderManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_salesOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_salesOrderManagementDataService.CloseConnection();
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
				_salesOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_salesOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				int productId = productDataTransformation.ProductId;

				product = await _salesOrderManagementDataService.GetProductInformationForUpdate(productId);

				product.ProductNumber = productDataTransformation.ProductNumber;
				product.Description = productDataTransformation.Description;
				product.UnitPrice = productDataTransformation.UnitPrice;
			
				await _salesOrderManagementDataService.UpdateProduct(product);

				await _salesOrderManagementDataService.UpdateDatabase();

				_salesOrderManagementDataService.CommitTransaction();

				returnResponse.ReturnStatus = true;

			}
			catch (Exception ex)
			{
				_salesOrderManagementDataService.RollbackTransaction();
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
			}
			finally
			{
				_salesOrderManagementDataService.CloseConnection();
			}

			returnResponse.Entity = productDataTransformation;

			return returnResponse;

		}
	
	}

}

