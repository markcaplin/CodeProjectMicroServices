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
using CodeProject.InventoryManagement.BusinessRules;

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
		/// Create Supplier
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<SupplierDataTransformation>> CreateSupplier(SupplierDataTransformation supplierDataTransformation)
		{

			ResponseModel<SupplierDataTransformation> returnResponse = new ResponseModel<SupplierDataTransformation>();

			Supplier supplier = new Supplier();
			
			try
			{
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_purchaseOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				SupplierBusinessRules<SupplierDataTransformation> supplierBusinessRules = new SupplierBusinessRules<SupplierDataTransformation>(supplierDataTransformation, _purchaseOrderManagementDataService);
				ValidationResult validationResult = await supplierBusinessRules.Validate();
				if (validationResult.ValidationStatus == false)
				{
					_purchaseOrderManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage = validationResult.ValidationMessages;
					returnResponse.ReturnStatus = validationResult.ValidationStatus;

					return returnResponse;
				}

				supplier.AccountId = supplierDataTransformation.AccountId;
				supplier.Name = supplierDataTransformation.Name;
				supplier.AddressLine1 = supplierDataTransformation.AddressLine1;
				supplier.AddressLine2 = supplierDataTransformation.AddressLine2;
				supplier.City = supplierDataTransformation.City;
				supplier.Region = supplierDataTransformation.Region;
				supplier.PostalCode = supplierDataTransformation.PostalCode;

				await _purchaseOrderManagementDataService.CreateSupplier(supplier);

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

			supplierDataTransformation.SupplierId = supplier.SupplierId;
		
			returnResponse.Entity = supplierDataTransformation;

			return returnResponse;

		}

		/// <summary>
		/// Update Supplier
		/// </summary>
		/// <param name="productDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<SupplierDataTransformation>> UpdateSupplier(SupplierDataTransformation supplierDataTransformation)
		{

			ResponseModel<SupplierDataTransformation> returnResponse = new ResponseModel<SupplierDataTransformation>();

			Supplier supplier = new Supplier();
	
			try
			{
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_purchaseOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				SupplierBusinessRules<SupplierDataTransformation> supplierBusinessRules = new SupplierBusinessRules<SupplierDataTransformation>(supplierDataTransformation, _purchaseOrderManagementDataService);
				ValidationResult validationResult = await supplierBusinessRules.Validate();
				if (validationResult.ValidationStatus == false)
				{
					_purchaseOrderManagementDataService.RollbackTransaction();

					returnResponse.ReturnMessage = validationResult.ValidationMessages;
					returnResponse.ReturnStatus = validationResult.ValidationStatus;

					return returnResponse;
				}

				int supplierId = supplierDataTransformation.SupplierId;

				supplier = await _purchaseOrderManagementDataService.GetSupplierInformationForUpdate(supplierId);

				supplier.Name = supplierDataTransformation.Name;
				supplier.AddressLine1 = supplierDataTransformation.AddressLine1;
				supplier.AddressLine2 = supplierDataTransformation.AddressLine2;
				supplier.City = supplierDataTransformation.City;
				supplier.Region = supplierDataTransformation.Region;
				supplier.PostalCode = supplierDataTransformation.PostalCode;

				await _purchaseOrderManagementDataService.UpdateSupplier(supplier);

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

			returnResponse.Entity = supplierDataTransformation;

			return returnResponse;

		}
	
	}

}

