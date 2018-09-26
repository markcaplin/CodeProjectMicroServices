
import { HttpErrorResponse } from '@angular/common/http';
import { HttpService } from './../../shared-components-services/http.service';
import { AlertService } from './../../shared-components-services/alert.service';
import { SessionService } from './../../shared-components-services/session.service';
import { Component, OnInit } from '@angular/core';
import { SupplierViewModel } from '../view-models/supplier.viewmodel';
import { SupplierViewModelResponse } from '../view-models/supplier-response.viewmodel';

@Component({
  selector: 'app-supplier-maintenance',
  templateUrl: './supplier-maintenance.component.html',
  styleUrls: ['./supplier-maintenance.component.css']
})
export class SupplierMaintenanceComponent implements OnInit {

  public supplierViewModel: SupplierViewModel;

  constructor(private sessionService: SessionService, private alertService: AlertService, private httpService: HttpService) {

    this.supplierViewModel = new SupplierViewModel();
    this.supplierViewModel.name = 'Caplin Systems, Inc.';
    this.supplierViewModel.addressLine1 = '17615 SW 6 Street';
    this.supplierViewModel.addressLine2 = 'Silverlakes';
    this.supplierViewModel.city = 'Pembroke Pines';
    this.supplierViewModel.region = 'FL';
    this.supplierViewModel.postalCode = '33029';
    this.supplierViewModel.supplierId = 0;

  }

  ngOnInit() {

  }

  public createOrUpdateSupplier() {

    let supplier = new SupplierViewModel();
    supplier = this.supplierViewModel;

    let url = '';
    if (supplier.supplierId === 0) {
      url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl +   'supplier/createsupplier';
    } else {
      url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl +   'supplier/updatesupplier';
    }

    this.httpService.HttpPost<SupplierViewModelResponse>(url, supplier).subscribe((response: SupplierViewModelResponse) => {
      this.createOrUpdateSupplierSuccess(response);
    }, response => this.createOrUpdateSupplierFailed(response));

  }

  private createOrUpdateSupplierSuccess(response: SupplierViewModelResponse) {
    let supplierViewModel: SupplierViewModel = response.entity;
    this.supplierViewModel.supplierId = supplierViewModel.supplierId;
    const message = 'Supplier successfully saved.';
    this.alertService.ShowSuccessMessage(message);
  }

  private createOrUpdateSupplierFailed(error: HttpErrorResponse) {
    let errorResponse: SupplierViewModelResponse = error.error;
    if (error.status > 0) {
      this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
    } else {
      this.alertService.ShowErrorMessage(error.message);
    }
  }

}

