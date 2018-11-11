using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Models;
using CodeProject.InventoryManagement.Data.Entities;
using CodeProject.InventoryManagement.Data.Transformations;
using CodeProject.Shared.Common.Interfaces;

namespace CodeProject.InventoryManagement.Interfaces
{
    public interface IInventoryManagementBusinessService
	{ 
		Task<ResponseModel<ProductDataTransformation>> CreateProduct(ProductDataTransformation productDataTransformation);
		Task<ResponseModel<ProductDataTransformation>> UpdateProduct(ProductDataTransformation productDataTransformation);
		Task<ResponseModel<List<PurchaseOrderDataTransformation>>> PurchaseOrderInquiry(int accountId, string supplierName, int currentPageNumber, int pageSize, string sortExpression, string sortDirection);
		Task<ResponseModel<PurchaseOrderDataTransformation>> GetPurchaseOrder(int accountId, int purchaseOrderId);
		Task<ResponseModel<PurchaseOrderDetailDataTransformation>> UpdatePurchaseOrderDetail(PurchaseOrderDetailDataTransformation purchaseOrderDetailDataTransformation);
		Task<ResponseModel<List<ProductDataTransformation>>> ProductInquiry(int accountId, string productNumber, int currentPageNumber, int pageSize, string sortExpression, string sortDirection);
		Task<ResponseModel<ProductDataTransformation>> GetProduct(int accountId, int productId);


	}
}
