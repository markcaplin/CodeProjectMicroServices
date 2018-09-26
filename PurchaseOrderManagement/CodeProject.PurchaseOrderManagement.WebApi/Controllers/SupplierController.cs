using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using CodeProject.PurchaseOrderManagement.Interfaces;
using CodeProject.PurchaseOrderManagement.Data.Transformations;
using CodeProject.PurchaseOrderManagement.WebApi.ActionFilters;
using CodeProject.Shared.Common.Models;
using CodeProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using CodeProject.Shared.Common.Interfaces;
using Microsoft.Extensions.Options;
using CodeProject.PurchaseOrderManagement.WebApi.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace CodeProject.PurchaseOrderManagement.WebApi.Controllers
{
	[ServiceFilter(typeof(SecurityFilter))]
	[Authorize]
	[Route("api/[controller]")]
	[EnableCors("SiteCorsPolicy")]
	[ApiController]
	public class SupplierController : ControllerBase
	{
		private readonly IPurchaseOrderManagementBusinessService _purchaseOrderManagementBusinessService;

		private IHubContext<MessageQueueHub> _messageQueueContext;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Supplier Controller
		/// </summary>
		public SupplierController(IPurchaseOrderManagementBusinessService purchaseOrderManagementBusinessService, IHubContext<MessageQueueHub> messageQueueContext)
		{
			_purchaseOrderManagementBusinessService = purchaseOrderManagementBusinessService;
			_messageQueueContext = messageQueueContext;
		}

		/// <summary>
		/// Register User
		/// </summary>
		/// <param name="supplierDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("CreateSupplier")]
		public async Task<IActionResult> CreateSupplier([FromBody] SupplierDataTransformation supplierDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;

			supplierDataTransformation.AccountId = accountId;

			ResponseModel<SupplierDataTransformation> returnResponse = new ResponseModel<SupplierDataTransformation>();
			try
			{
				returnResponse = await _purchaseOrderManagementBusinessService.CreateSupplier(supplierDataTransformation);
				returnResponse.Token = securityModel.Token;
				if (returnResponse.ReturnStatus == false)
				{
					return BadRequest(returnResponse);
				}

				return Ok(returnResponse);
				
			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
				return BadRequest(returnResponse);
			}

		}

		/// <summary>
		/// Update Supplier
		/// </summary>
		/// <param name="supplierDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("UpdateSupplier")]
		public async Task<IActionResult> UpdateSupplier([FromBody] SupplierDataTransformation supplierDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;

			supplierDataTransformation.AccountId = accountId;

			ResponseModel<SupplierDataTransformation> returnResponse = new ResponseModel<SupplierDataTransformation>();
			try
			{
				returnResponse = await _purchaseOrderManagementBusinessService.UpdateSupplier(supplierDataTransformation);
				returnResponse.Token = securityModel.Token;
				if (returnResponse.ReturnStatus == false)
				{
					return BadRequest(returnResponse);
				}

				return Ok(returnResponse);
				
			}
			catch (Exception ex)
			{
				returnResponse.ReturnStatus = false;
				returnResponse.ReturnMessage.Add(ex.Message);
				return BadRequest(returnResponse);
			}

		}


	}
}