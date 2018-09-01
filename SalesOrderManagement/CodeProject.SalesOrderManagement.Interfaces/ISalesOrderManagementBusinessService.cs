using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeProject.Shared.Common.Models;
using CodeProject.SalesOrderManagement.Data.Entities;
using CodeProject.SalesOrderManagement.Data.Transformations;

namespace CodeProject.SalesOrderManagement.Interfaces
{
    public interface ISalesOrderManagementBusinessService
	{ 
		Task<ResponseModel<ProductDataTransformation>> CreateProduct(ProductDataTransformation productDataTransformation);
	}
}
