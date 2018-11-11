import { PurchaseOrderInquiryComponent } from './purchase-order-inquiry/purchase-order-inquiry.component';
import { InventoryManagementNavBarComponent } from './inventory-management-nav-bar/inventory-management-nav-bar.component';
import { InventoryAdjustmentsComponent } from './inventory-adjustments/inventory-adjustments.component';
import { PurchaseOrderReceivingComponent } from './purchase-order-receiving/purchase-order-receiving.component';
import { OrderShipmentsComponent } from './order-shipments/order-shipments.component';
import { ProductMaintenanceComponent } from './product-maintenance/product-maintenance.component';
import { ProductInquiryComponent } from './product-inquiry/product-inquiry.component';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { InventoryManagementRoutingModule } from './inventory-management.routing';

import { MaterialModule } from '../material.module';

@NgModule({
    imports: [
        InventoryManagementRoutingModule,
        CommonModule,
        FormsModule,
        MaterialModule
    ],
    declarations: [ProductInquiryComponent, PurchaseOrderInquiryComponent, ProductMaintenanceComponent, InventoryAdjustmentsComponent,
        PurchaseOrderReceivingComponent, OrderShipmentsComponent, InventoryManagementNavBarComponent]
})
export class InventoryManagementModule { }

