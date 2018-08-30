using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Models;
using CodeProject.InventoryManagement.Data.Entities;
using CodeProject.InventoryManagement.Data.Transformations;

namespace CodeProject.InventoryManagement.Interfaces
{
    public interface IInventoryManagementBusinessService
	{ 
		Task<ResponseModel<ProductDataTransformation>> CreateProduct(ProductDataTransformation productDataTransformation);
		Task<ResponseModel<ProductDataTransformation>> UpdateProduct(ProductDataTransformation productDataTransformation);

		//Task<ResponseModel<AccountDataTransformation>> Login(AccountDataTransformation accountDataTransformation);
		//Task<ResponseModel<AccountDataTransformation>> UpdateUser(AccountDataTransformation accountDataTransformation);
		//Task<ResponseModel<User>> UpdateUser(int userId);


	}
}
