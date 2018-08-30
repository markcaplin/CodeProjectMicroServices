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

import {
    MatSidenavModule,
    MatToolbarModule,
    MatInputModule,
    MatButtonModule,
    MatTabsModule,
    MatSelectModule,
    MatIconModule,
    MatListModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatFormFieldModule,
    MatCardModule,
    MatGridListModule,
} from '@angular/material';


@NgModule({
    imports: [
        InventoryManagementRoutingModule,
        CommonModule,
        FormsModule,
        MatTabsModule,
        MatInputModule,
        MatButtonModule,
        MatSelectModule,
        MatIconModule,
        MatListModule,
        MatIconModule,
        MatSidenavModule,
        MatToolbarModule,
        MatSnackBarModule,
        MatProgressBarModule,
        MatFormFieldModule,
        MatCardModule,
        MatGridListModule,
    ],
    declarations: [ProductInquiryComponent, ProductMaintenanceComponent, InventoryAdjustmentsComponent,
        PurchaseOrderReceivingComponent, OrderShipmentsComponent, InventoryManagementNavBarComponent]
})
export class InventoryManagementModule { }

