import { HttpErrorResponse } from '@angular/common/http';
import { HttpService } from './../../shared-components-services/http.service';
import { AlertService } from './../../shared-components-services/alert.service';
import { SessionService } from './../../shared-components-services/session.service';
import { Component, OnInit } from '@angular/core';
import { ProductViewModel } from '../product-view-models/product.viewmodel';
import { ProductViewModelResponse } from '../product-view-models/product-response.viewmodel';

@Component({
  selector: 'app-product-maintenance',
  templateUrl: './product-maintenance.component.html',
  styleUrls: ['./product-maintenance.component.css']
})
export class ProductMaintenanceComponent implements OnInit {

  public productViewModel: ProductViewModel;

  constructor(private sessionService: SessionService, private alertService: AlertService, private httpService: HttpService) {
    this.productViewModel = new ProductViewModel();
    this.productViewModel.productNumber = 'Product Number';
    this.productViewModel.description = 'Description';
    this.productViewModel.binLocation = 'Bin Location';
    this.productViewModel.unitPrice = 100.00;
    this.productViewModel.productId = 0;

  }

  ngOnInit() {

  }

  public createOrUpdateProduct() {
    let product = new ProductViewModel();
    product = this.productViewModel;

    let url = '';
    if (product.productId === 0) {
      url = this.sessionService.appSettings.inventoryManagementWebApiUrl +  'product/createproduct';
    } else {
      url = this.sessionService.appSettings.inventoryManagementWebApiUrl +  'product/updateproduct';
    }

    this.httpService.HttpPost<ProductViewModelResponse>(url, product).subscribe((response: ProductViewModelResponse) => {
      this.createOrUpdateProductSuccess(response);
    }, response => this.createOrUpdateProductFailed(response));

  }

  private createOrUpdateProductSuccess(response: ProductViewModelResponse) {
    let productViewModel: ProductViewModel = response.entity;
    this.productViewModel.productId = productViewModel.productId;
    const message = 'Product successfully saved.';
    this.alertService.ShowSuccessMessage(message);
  }

  private createOrUpdateProductFailed(error: HttpErrorResponse) {
    let errorResponse: ProductViewModelResponse = error.error;
    if (error.status > 0) {
      this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
    } else {
      this.alertService.ShowErrorMessage(error.message);
    }
  }

}
