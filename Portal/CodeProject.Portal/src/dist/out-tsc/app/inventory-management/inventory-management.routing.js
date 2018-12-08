"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var upload_product_master_component_1 = require("./upload-product-master/upload-product-master.component");
var purchase_order_receiving_component_1 = require("./purchase-order-receiving/purchase-order-receiving.component");
var product_maintenance_component_1 = require("./product-maintenance/product-maintenance.component");
var product_inquiry_component_1 = require("./product-inquiry/product-inquiry.component");
var purchase_order_inquiry_component_1 = require("./purchase-order-inquiry/purchase-order-inquiry.component");
var sales_order_inquiry_component_1 = require("./sales-order-inquiry/sales-order-inquiry.component");
var sales_order_shipments_component_1 = require("./sales-order-shipments/sales-order-shipments.component");
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var InventoryManagementRoutes = [
    { path: '', component: product_maintenance_component_1.ProductMaintenanceComponent },
    { path: 'product-maintenance', component: product_maintenance_component_1.ProductMaintenanceComponent },
    { path: 'product-inquiry', component: product_inquiry_component_1.ProductInquiryComponent },
    { path: 'upload-product-master', component: upload_product_master_component_1.UploadProductMasterComponent },
    { path: 'purchase-order-receiving', component: purchase_order_receiving_component_1.PurchaseOrderReceivingComponent },
    { path: 'purchase-order-inquiry', component: purchase_order_inquiry_component_1.PurchaseOrderInquiryComponent },
    { path: 'sales-order-inquiry', component: sales_order_inquiry_component_1.SalesOrderInquiryComponent },
    { path: 'sales-order-shipping', component: sales_order_shipments_component_1.SalesOrderShipmentsComponent }
];
var InventoryManagementRoutingModule = /** @class */ (function () {
    function InventoryManagementRoutingModule() {
    }
    InventoryManagementRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forChild(InventoryManagementRoutes)
            ],
            exports: [router_1.RouterModule]
        })
    ], InventoryManagementRoutingModule);
    return InventoryManagementRoutingModule;
}());
exports.InventoryManagementRoutingModule = InventoryManagementRoutingModule;
//# sourceMappingURL=inventory-management.routing.js.map