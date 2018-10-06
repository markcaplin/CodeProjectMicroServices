import { Component, OnInit } from '@angular/core';
import { PurchaseOrderViewModel } from '../view-models/purchase-order.viewmodel';
import { PurchaseOrderViewModelResponse } from '../view-models/purchase-order-response.viewmodel';



@Component({
  selector: 'app-purchase-order-maintenance',
  templateUrl: './purchase-order-maintenance.component.html',
  styleUrls: ['./purchase-order-maintenance.component.css']
})
export class PurchaseOrderMaintenanceComponent implements OnInit {

  public purchaseOrderViewModel: PurchaseOrderViewModel;

  constructor() { }

  ngOnInit() {

    this.routerSubscription = this.route
    .queryParams
    .subscribe(params => {
      this.purchaseOrderViewModel.purchaseOrderId = +params['id'] || 0;
      if (this.purchaseOrderViewModel.purchaseOrderId > 0) {
          this.getPurchaseOrder();
      }
    });

  }

}
