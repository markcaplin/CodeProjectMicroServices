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
		/// Create Purchase Order
		/// </summary>
		/// <param name="purchaseOrderDataTransformation"></param>
		/// <returns></returns>
		public async Task<ResponseModel<PurchaseOrderDataTransformation>> CreatePurchaseOrder(PurchaseOrderDataTransformation purchaseOrderDataTransformation)
		{

			ResponseModel<PurchaseOrderDataTransformation> returnResponse = new ResponseModel<PurchaseOrderDataTransformation>();

			PurchaseOrder purchaseOrder = new PurchaseOrder();

			try
			{
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);
				_purchaseOrderManagementDataService.BeginTransaction((int)IsolationLevel.Serializable);

				PurchaseOrderNumberSequence purchaseOrderNumberSequence = await _purchaseOrderManagementDataService.GetPurchaseOrderNumberSequence(purchaseOrderDataTransformation.AccountId);
				if (purchaseOrderNumberSequence == null)
				{
					purchaseOrderNumberSequence = new PurchaseOrderNumberSequence();
					purchaseOrderNumberSequence.AccountId = purchaseOrderDataTransformation.AccountId;
					purchaseOrderNumberSequence.PurchaseOrderNumber = 100000;
					await _purchaseOrderManagementDataService.CreatePurchaseOrderNumberSequence(purchaseOrderNumberSequence);
				}
				else
				{
					purchaseOrderNumberSequence.PurchaseOrderNumber = purchaseOrderNumberSequence.PurchaseOrderNumber + 1;
					await _purchaseOrderManagementDataService.UpdatePurchaseOrderNumberSequence(purchaseOrderNumberSequence);
				}

				purchaseOrder.PurchaseOrderNumber = purchaseOrderNumberSequence.PurchaseOrderNumber;
				purchaseOrder.AccountId = purchaseOrderDataTransformation.AccountId;
				purchaseOrder.SupplierId = purchaseOrderDataTransformation.SupplierId;
				purchaseOrder.PurchaseOrderStatusId = PurchaseOrderStatuses.Open;
				purchaseOrder.OrderTotal = 0.0;

				await _purchaseOrderManagementDataService.CreatePurchaseOrder(purchaseOrder);

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

			purchaseOrderDataTransformation.PurchaseOrderId = purchaseOrder.PurchaseOrderId;

			returnResponse.Entity = purchaseOrderDataTransformation;

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
				_purchaseOrderManagementDataService.OpenConnection(_connectionStrings.PrimaryDatabaseConnectionString);

				PurchaseOrder purchaseOrder = await _purchaseOrderManagementDataService.GetPurchaseOrder(accountId, purchaseOrderId);

				purchaseOrderDataTransformation.PurchaseOrderId = purchaseOrderId;
				purchaseOrderDataTransformation.PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber;
				purchaseOrderDataTransformation.PurchaseOrderStatusId = purchaseOrder.PurchaseOrderStatusId;
				purchaseOrderDataTransformation.SupplierId = purchaseOrder.Supplier.SupplierId;
				purchaseOrderDataTransformation.SupplierName = purchaseOrder.Supplier.Name;
				purchaseOrderDataTransformation.AddressLine1 = purchaseOrder.Supplier.AddressLine1;
				purchaseOrderDataTransformation.AddressLine2 = purchaseOrder.Supplier.AddressLine2;
				purchaseOrderDataTransformation.City = purchaseOrder.Supplier.City;
				purchaseOrderDataTransformation.Region = purchaseOrder.Supplier.Region;
				purchaseOrderDataTransformation.PostalCode = purchaseOrder.Supplier.PostalCode;
				purchaseOrderDataTransformation.OrderTotal = purchaseOrder.OrderTotal;
				purchaseOrderDataTransformation.PurchaseOrderStatusDescription = purchaseOrder.PurchaseOrderStatus.Description;
				purchaseOrderDataTransformation.DateCreated = purchaseOrder.DateCreated;
				purchaseOrderDataTransformation.DateUpdated = purchaseOrder.DateUpdated;
				purchaseOrderDataTransformation.PurchaseOrderDetails = new List<PurchaseOrderDetailDataTransformation>();

				foreach(PurchaseOrderDetail purchaseOrderDetail in purchaseOrder.PurchaseOrderDetails)
				{
					PurchaseOrderDetailDataTransformation purchaseOrderDetailDataTransformation = new PurchaseOrderDetailDataTransformation();
					purchaseOrderDetailDataTransformation.PurchaseOrderDetailId = purchaseOrderDetail.PurchaseOrderDetailId;
					purchaseOrderDetailDataTransformation.PurchaseOrderId = purchaseOrderDetail.PurchaseOrderId;
					purchaseOrderDetailDataTransformation.ProductId = purchaseOrderDetail.ProductId;
					purchaseOrderDetailDataTransformation.ProductMasterId = purchaseOrderDetail.Product.ProductMasterId;
					purchaseOrderDetailDataTransformation.ProductNumber = purchaseOrderDetail.Product.ProductNumber;
					purchaseOrderDetailDataTransformation.Description = purchaseOrderDetail.Product.Description;
					purchaseOrderDetailDataTransformation.UnitPrice = purchaseOrderDetail.Product.UnitPrice;
					purchaseOrderDetailDataTransformation.OrderQuantity = purchaseOrderDetail.OrderQuantity;
					purchaseOrderDetailDataTransformation.DateCreated = purchaseOrderDetail.DateCreated;
					purchaseOrderDetailDataTransformation.DateUpdated = purchaseOrderDetail.DateUpdated;

					purchaseOrderDataTransformation.PurchaseOrderDetails.Add(purchaseOrderDetailDataTransformation);
				}

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

			returnResponse.Entity = purchaseOrderDataTransformation;

			return returnResponse;

		}




	}

}

