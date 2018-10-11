
import { PurchaseOrderDetailViewModel } from './../view-models/purchase-order-detail.viewmodel';
import { ProductViewModel } from './../view-models/product.viewmodel';
import { AlertService } from './../../shared-components-services/alert.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { PurchaseOrderViewModel } from '../view-models/purchase-order.viewmodel';
import { PurchaseOrderViewModelResponse } from '../view-models/purchase-order-response.viewmodel';
import { SessionService } from '../../shared-components-services/session.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpService } from '../../shared-components-services/http.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ProductViewModelResponse } from '../view-models/product-response.viewmodel';
import { PurchaseOrderDetailViewModelResponse } from '../view-models/purchase-order-detail-response.viewmodel';

import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-purchase-order-maintenance',
  templateUrl: './purchase-order-maintenance.component.html',
  styleUrls: ['./purchase-order-maintenance.component.css']
})
export class PurchaseOrderMaintenanceComponent implements OnInit, OnDestroy {

  public purchaseOrderViewModel: PurchaseOrderViewModel;
  private routerSubscription: Subscription;
  public detailDataSource = new MatTableDataSource<PurchaseOrderDetailViewModel>();

  constructor(private router: Router, private httpService: HttpService, private route: ActivatedRoute,
    private sessionService: SessionService, private alertService: AlertService) {

    this.purchaseOrderViewModel = new PurchaseOrderViewModel();

    let purchaseOrderDetailViewModel = new PurchaseOrderDetailViewModel();

    purchaseOrderDetailViewModel.productDescription = '';
    purchaseOrderDetailViewModel.productNumber = ' ';
    purchaseOrderDetailViewModel.unitPriceFormatted = '';
    purchaseOrderDetailViewModel.orderQuantityFormatted = '';
    purchaseOrderDetailViewModel.editProductNumber = true;
    purchaseOrderDetailViewModel.editQuantity = false;
    purchaseOrderDetailViewModel.editUnitPrice = false;
    purchaseOrderDetailViewModel.editMode = false;

    this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
    this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;

  }

  ngOnInit() {

    this.routerSubscription = this.route.queryParams.subscribe(params => {
      this.purchaseOrderViewModel.purchaseOrderId = +params['id'] || 0;
      if (this.purchaseOrderViewModel.purchaseOrderId > 0) {
        this.getPurchaseOrder();
      }
    });

  }

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }

  private getPurchaseOrder() {
    let url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/getpurchaseorder';
    this.httpService.HttpPost<PurchaseOrderViewModelResponse>(url, this.purchaseOrderViewModel)
      .subscribe((response: PurchaseOrderViewModelResponse) => {
        this.getPurchaseOrderSuccess(response);
      }, response => this.getPurchaseOrderFailed(response));
  }

  private getPurchaseOrderSuccess(response: PurchaseOrderViewModelResponse) {

    response.entity.orderDate = response.entity.dateCreated.toString().substring(0, 10);

    this.purchaseOrderViewModel.supplierName = response.entity.supplierName;
    this.purchaseOrderViewModel.accountId = response.entity.accountId;
    this.purchaseOrderViewModel.addressLine1 = response.entity.addressLine1;
    this.purchaseOrderViewModel.addressLine2 = response.entity.addressLine2;
    this.purchaseOrderViewModel.city = response.entity.city;
    this.purchaseOrderViewModel.region = response.entity.region;
    this.purchaseOrderViewModel.postalCode = response.entity.postalCode;
    this.purchaseOrderViewModel.purchaseOrderId = response.entity.purchaseOrderId;
    this.purchaseOrderViewModel.purchaseOrderNumber = response.entity.purchaseOrderNumber;
    this.purchaseOrderViewModel.orderDate = response.entity.orderDate;
    this.purchaseOrderViewModel.purchaseOrderStatusDescription = response.entity.purchaseOrderStatusDescription;

    response.entity.purchaseOrderDetails.forEach(element => {

      let purchaseOrderDetailViewModel = new PurchaseOrderDetailViewModel();
      purchaseOrderDetailViewModel.purchaseOrderDetailId = element.purchaseOrderDetailId;
      purchaseOrderDetailViewModel.purchaseOrderId = element.purchaseOrderId;
      purchaseOrderDetailViewModel.productId = element.productId;
      purchaseOrderDetailViewModel.productDescription = element.productDescription;
      purchaseOrderDetailViewModel.productMasterId = element.productMasterId;
      purchaseOrderDetailViewModel.productNumber = element.productNumber;
      purchaseOrderDetailViewModel.unitPrice = element.unitPrice;
      purchaseOrderDetailViewModel.orderQuantity = element.orderQuantity;
      purchaseOrderDetailViewModel.orderQuantityFormatted = element.orderQuantity.toFixed(0);
      purchaseOrderDetailViewModel.unitPriceFormatted = element.unitPrice.toFixed(2);
      purchaseOrderDetailViewModel.editQuantity = false;
      purchaseOrderDetailViewModel.editUnitPrice = false;
      purchaseOrderDetailViewModel.editProductNumber = false;
      purchaseOrderDetailViewModel.editMode = false;

      this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
    });

    this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;

    this.purchaseOrderViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice', 'orderQuantity', 'actions'];
  }

  private getPurchaseOrderFailed(error: HttpErrorResponse) {

    let errorResponse: PurchaseOrderViewModelResponse = error.error;
    if (error.status > 0) {
      this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
    } else {
      this.alertService.ShowErrorMessage(error.message);
    }

  }

  public getProduct() {

    this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber
      = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber.trim();

    if (this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber.length === 0) {
      return;
    }

    let url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/getproduct';
    let productViewModel = new ProductViewModel();
    productViewModel.productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber;
    this.httpService.HttpPost<ProductViewModelResponse>(url, productViewModel)
      .subscribe((response: ProductViewModelResponse) => {
        this.getProductSuccess(response);
      }, response => this.getProductFailed(response));

  }

  private getProductSuccess(response: ProductViewModelResponse) {
    this.purchaseOrderViewModel.purchaseOrderDetails[0].productDescription = response.entity.description;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].productId = response.entity.productId;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editQuantity = true;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editUnitPrice = true;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editMode = true;
  }

  private getProductFailed(error: HttpErrorResponse) {

    let errorResponse: PurchaseOrderViewModelResponse = error.error;
    console.log(error.status + ' error status');
    if (error.status > 0) {
      this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
    } else {
      this.alertService.ShowErrorMessage(error.message);
    }

  }

  public saveLineItem(i: number) {

    let purchaseOrderDetailViewModel = new PurchaseOrderDetailViewModel();

    purchaseOrderDetailViewModel.purchaseOrderDetailId = 0;
    purchaseOrderDetailViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderId;
    purchaseOrderDetailViewModel.productId = this.purchaseOrderViewModel.purchaseOrderDetails[0].productId;
    purchaseOrderDetailViewModel.productMasterId = this.purchaseOrderViewModel.purchaseOrderDetails[0].productMasterId;
    purchaseOrderDetailViewModel.productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber;
    purchaseOrderDetailViewModel.unitPrice = parseFloat(this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPriceFormatted);
    purchaseOrderDetailViewModel.orderQuantity = parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantityFormatted, 0);

    let url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/createpurchaseorderdetail';
    this.httpService.HttpPost<PurchaseOrderDetailViewModelResponse>(url, purchaseOrderDetailViewModel)
      .subscribe((response: PurchaseOrderDetailViewModelResponse) => {
        this.addLineItemSuccess(response);
      }, response => this.addLineItemFailed(response));

  }

  private addLineItemSuccess(response: PurchaseOrderDetailViewModelResponse) {

    let purchaseOrderDetailViewModel = new PurchaseOrderDetailViewModel();
    purchaseOrderDetailViewModel.purchaseOrderDetailId = response.entity.purchaseOrderDetailId;
    purchaseOrderDetailViewModel.purchaseOrderId = response.entity.purchaseOrderId;
    purchaseOrderDetailViewModel.productId = response.entity.productId;
    purchaseOrderDetailViewModel.productDescription = response.entity.productDescription;
    purchaseOrderDetailViewModel.productMasterId = response.entity.productMasterId;
    purchaseOrderDetailViewModel.productNumber = response.entity.productNumber;
    purchaseOrderDetailViewModel.unitPrice = response.entity.unitPrice;
    purchaseOrderDetailViewModel.orderQuantity = response.entity.orderQuantity;
    purchaseOrderDetailViewModel.orderQuantityFormatted = response.entity.orderQuantity.toFixed(0);
    purchaseOrderDetailViewModel.unitPriceFormatted = response.entity.unitPrice.toFixed(2);
    purchaseOrderDetailViewModel.editQuantity = false;
    purchaseOrderDetailViewModel.editUnitPrice = false;
    purchaseOrderDetailViewModel.editProductNumber = false;
    purchaseOrderDetailViewModel.editMode = false;

    this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
    this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;

    this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber = ' ';
    this.purchaseOrderViewModel.purchaseOrderDetails[0].productDescription = ' ';
    this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPrice = 0.0;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPriceFormatted = '';
    this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantity = 0;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantityFormatted = '';
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editQuantity = false;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editUnitPrice = false;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editProductNumber = true;
    this.purchaseOrderViewModel.purchaseOrderDetails[0].editMode = false;

    const message = 'Line Item successfully saved.';
    this.alertService.ShowSuccessMessage(message);

  }

  private addLineItemFailed(error: HttpErrorResponse) {

    let errorResponse: PurchaseOrderDetailViewModelResponse = error.error;
    console.log(error.status + ' error status');
    if (error.status > 0) {
      this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
    } else {
      this.alertService.ShowErrorMessage(error.message);
    }

  }

  public editLineItem(i: number) {
    this.purchaseOrderViewModel.purchaseOrderDetails[i].editQuantity = true;
    this.purchaseOrderViewModel.purchaseOrderDetails[i].editUnitPrice = true;
    this.purchaseOrderViewModel.purchaseOrderDetails[i].editMode = true;
  }

  /*public saveLineItem(i: number) {
    this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice =
       parseFloat(this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPriceFormatted);
    this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity =
       parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantityFormatted, 0);

  }*/

}
