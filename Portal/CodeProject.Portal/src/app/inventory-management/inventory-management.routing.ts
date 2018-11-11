import { InventoryAdjustmentsComponent } from './inventory-adjustments/inventory-adjustments.component';
import { PurchaseOrderReceivingComponent } from './purchase-order-receiving/purchase-order-receiving.component';
import { OrderShipmentsComponent } from './order-shipments/order-shipments.component';
import { ProductMaintenanceComponent } from './product-maintenance/product-maintenance.component';
import { ProductInquiryComponent } from './product-inquiry/product-inquiry.component';
import { PurchaseOrderInquiryComponent } from './purchase-order-inquiry/purchase-order-inquiry.component';

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const InventoryManagementRoutes: Routes = [
    { path: '', component: ProductMaintenanceComponent },
    { path: 'product-maintenance', component: ProductMaintenanceComponent},
    { path: 'product-inquiry', component: ProductInquiryComponent },
    { path: 'inventory-adjustments', component: InventoryAdjustmentsComponent },
    { path: 'order-shipments', component: OrderShipmentsComponent },
    { path: 'purchase-order-receiving', component: PurchaseOrderReceivingComponent },
    { path: 'purchase-order-inquiry', component: PurchaseOrderInquiryComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(InventoryManagementRoutes)
    ],
    exports: [RouterModule]
})
export class InventoryManagementRoutingModule { }
