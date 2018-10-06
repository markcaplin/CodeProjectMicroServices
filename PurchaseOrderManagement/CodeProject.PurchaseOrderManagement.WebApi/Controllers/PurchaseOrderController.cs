

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
	public class PurchaseOrderController : ControllerBase
	{
		private readonly IPurchaseOrderManagementBusinessService _purchaseOrderManagementBusinessService;

		private IHubContext<MessageQueueHub> _messageQueueContext;

		public IConfiguration configuration { get; }

		/// <summary>
		/// Purchase Controller
		/// </summary>
		public PurchaseOrderController(IPurchaseOrderManagementBusinessService purchaseOrderManagementBusinessService, IHubContext<MessageQueueHub> messageQueueContext)
		{
			_purchaseOrderManagementBusinessService = purchaseOrderManagementBusinessService;
			_messageQueueContext = messageQueueContext;
		}

		/// <summary>
		/// Create Purchase Order
		/// </summary>
		/// <param name="purchaseOrderDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("CreatePurchaseOrder")]
		public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderDataTransformation purchaseOrderDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;

			purchaseOrderDataTransformation.AccountId = accountId;

			ResponseModel<PurchaseOrderDataTransformation> returnResponse = new ResponseModel<PurchaseOrderDataTransformation>();

			try
			{
				returnResponse = await _purchaseOrderManagementBusinessService.CreatePurchaseOrder(purchaseOrderDataTransformation);
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
		/// Get Purchase Order
		/// </summary>
		/// <param name="purchaseOrderDataTransformation"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("GetPurchaseOrder")]
		public async Task<IActionResult> GetPurchaseOrder([FromBody] PurchaseOrderDataTransformation purchaseOrderDataTransformation)
		{

			SecurityModel securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);

			int accountId = securityModel.AccountId;
			int purchaseOrderId = purchaseOrderDataTransformation.PurchaseOrderId;

			ResponseModel<PurchaseOrderDataTransformation> returnResponse = new ResponseModel<PurchaseOrderDataTransformation>();

			try
			{
				returnResponse = await _purchaseOrderManagementBusinessService.GetPurchaseOrder(accountId, purchaseOrderId);
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