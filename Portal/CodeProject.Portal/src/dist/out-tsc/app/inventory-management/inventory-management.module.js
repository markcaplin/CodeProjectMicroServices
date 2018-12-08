"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var upload_product_master_component_1 = require("./upload-product-master/upload-product-master.component");
var purchase_order_inquiry_component_1 = require("./purchase-order-inquiry/purchase-order-inquiry.component");
var sales_order_inquiry_component_1 = require("./sales-order-inquiry/sales-order-inquiry.component");
var inventory_management_nav_bar_component_1 = require("./inventory-management-nav-bar/inventory-management-nav-bar.component");
var purchase_order_receiving_component_1 = require("./purchase-order-receiving/purchase-order-receiving.component");
var sales_order_shipments_component_1 = require("./sales-order-shipments/sales-order-shipments.component");
var product_maintenance_component_1 = require("./product-maintenance/product-maintenance.component");
var product_inquiry_component_1 = require("./product-inquiry/product-inquiry.component");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var inventory_management_routing_1 = require("./inventory-management.routing");
var material_module_1 = require("../material.module");
var InventoryManagementModule = /** @class */ (function () {
    function InventoryManagementModule() {
    }
    InventoryManagementModule = __decorate([
        core_1.NgModule({
            imports: [
                inventory_management_routing_1.InventoryManagementRoutingModule,
                common_1.CommonModule,
                forms_1.FormsModule,
                material_module_1.MaterialModule
            ],
            declarations: [product_inquiry_component_1.ProductInquiryComponent, purchase_order_inquiry_component_1.PurchaseOrderInquiryComponent, product_maintenance_component_1.ProductMaintenanceComponent,
                upload_product_master_component_1.UploadProductMasterComponent, purchase_order_receiving_component_1.PurchaseOrderReceivingComponent, sales_order_inquiry_component_1.SalesOrderInquiryComponent,
                sales_order_shipments_component_1.SalesOrderShipmentsComponent, inventory_management_nav_bar_component_1.InventoryManagementNavBarComponent]
        })
    ], InventoryManagementModule);
    return InventoryManagementModule;
}());
exports.InventoryManagementModule = InventoryManagementModule;
//# sourceMappingURL=inventory-management.module.js.map