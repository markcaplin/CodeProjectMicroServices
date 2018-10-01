using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Models;
using CodeProject.PurchaseOrderManagement.Data.Entities;
using CodeProject.PurchaseOrderManagement.Data.Transformations;

namespace CodeProject.PurchaseOrderManagement.Interfaces
{
    public interface IPurchaseOrderManagementBusinessService
	{ 
		Task<ResponseModel<SupplierDataTransformation>> CreateSupplier(SupplierDataTransformation supplierDataTransformation);
		Task<ResponseModel<SupplierDataTransformation>> UpdateSupplier(SupplierDataTransformation supplierDataTransformation);
		Task<ResponseModel<SupplierDataTransformation>> GetSupplierInformation(int accountId, int supplierId);
		Task<ResponseModel<List<SupplierDataTransformation>>> SupplierInquiry(int accountId, string supplierName, int currentPageNumber, int pageSize, string sortExpression, string sortDirection);
	}
}
