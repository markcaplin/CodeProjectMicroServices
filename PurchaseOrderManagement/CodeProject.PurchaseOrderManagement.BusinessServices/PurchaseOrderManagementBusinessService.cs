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
				supplier.Name = supplierDataTransformation.SupplierName;
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
				int accountId = supplierDataTransformation.AccountId;

				supplier = await _purchaseOrderManagementDataService.GetSupplierInformationForUpdate(accountId, supplierId);

				supplier.Name = supplierDataTransformation.SupplierName;
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

		/// <summary>
		/// Supplier Inquiry
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="supplierName"></param>
		/// <param name="currentPageNumber"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortExpression"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		public async Task<ResponseModel<List<SupplierDataTransformation>>> SupplierInquiry(int accountId, string supplierName, int currentPageNumber, int pageSize, string sortExpression, string sortDirection)
		{

			ResponseModel<List<SupplierDataTransformation>> returnResponse = new ResponseModel<List<SupplierDataTransformation>>();

			List<SupplierDataTransformation> suppliers = new List<SupplierDataTransformation>();

			try
			{
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				DataGridPagingInformation dataGridPagingInformation = new DataGridPagingInformation();
				dataGridPagingInformation.CurrentPageNumber = currentPageNumber;
				dataGridPagingInformation.PageSize = pageSize;
				dataGridPagingInformation.SortDirection = sortDirection;
				dataGridPagingInformation.SortExpression = sortExpression;

				List<Supplier> supplierList = await _purchaseOrderManagementDataService.SupplierInquiry(accountId, supplierName, dataGridPagingInformation);
				foreach(Supplier supplier in supplierList)
				{
					SupplierDataTransformation supplierDataTransformation = new SupplierDataTransformation();
					supplierDataTransformation.SupplierId = supplier.SupplierId;
					supplierDataTransformation.AddressLine1 = supplier.AddressLine1;
					supplierDataTransformation.AddressLine2 = supplier.AddressLine2;
					supplierDataTransformation.City = supplier.City;
					supplierDataTransformation.Region = supplier.Region;
					supplierDataTransformation.PostalCode = supplier.PostalCode;
					supplierDataTransformation.SupplierName = supplier.Name;
					suppliers.Add(supplierDataTransformation);

				}

				returnResponse.Entity = suppliers;
				returnResponse.TotalRows = dataGridPagingInformation.TotalRows;
				returnResponse.TotalPages = dataGridPagingInformation.TotalPages;

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

			return returnResponse;

		}

		/// <summary>
		/// Get Supplier Information
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="supplierId"></param>
		/// <returns></returns>
		public async Task<ResponseModel<SupplierDataTransformation>> GetSupplierInformation(int accountId, int supplierId)
		{

			ResponseModel<SupplierDataTransformation> returnResponse = new ResponseModel<SupplierDataTransformation>();
			SupplierDataTransformation supplierDataTransformation = new SupplierDataTransformation();

			Supplier supplier = new Supplier();

			try
			{
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				supplier = await _purchaseOrderManagementDataService.GetSupplierInformation(accountId, supplierId);

				supplierDataTransformation = new SupplierDataTransformation();
				supplierDataTransformation.SupplierId = supplier.SupplierId;
				supplierDataTransformation.AddressLine1 = supplier.AddressLine1;
				supplierDataTransformation.AddressLine2 = supplier.AddressLine2;
				supplierDataTransformation.City = supplier.City;
				supplierDataTransformation.Region = supplier.Region;
				supplierDataTransformation.PostalCode = supplier.PostalCode;
				supplierDataTransformation.SupplierName = supplier.Name;
		
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

