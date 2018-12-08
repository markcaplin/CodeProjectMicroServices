"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var supplier_maintenance_component_1 = require("./supplier-maintenance/supplier-maintenance.component");
var supplier_inquiry_component_1 = require("./supplier-inquiry/supplier-inquiry.component");
var purchase_order_maintenance_component_1 = require("./purchase-order-maintenance/purchase-order-maintenance.component");
var purchase_order_inquiry_component_1 = require("./purchase-order-inquiry/purchase-order-inquiry.component");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var material_module_1 = require("../material.module");
var purchase_order_management_routing_1 = require("./purchase-order-management.routing");
var purchase_order_management_nav_bar_component_1 = require("./purchase-order-management-nav-bar/purchase-order-management-nav-bar.component");
var purchase_order_maintenance_component_2 = require("./purchase-order-maintenance/purchase-order-maintenance.component");
var purchase_order_maintenance_component_3 = require("./purchase-order-maintenance/purchase-order-maintenance.component");
var PurchaseOrderManagementModule = /** @class */ (function () {
    function PurchaseOrderManagementModule() {
    }
    PurchaseOrderManagementModule = __decorate([
        core_1.NgModule({
            imports: [
                purchase_order_management_routing_1.PurchaseOrderManagementRoutingModule,
                common_1.CommonModule,
                forms_1.FormsModule,
                material_module_1.MaterialModule
            ],
            entryComponents: [purchase_order_maintenance_component_2.DeletePurchaseOrderLineItemDialogComponent, purchase_order_maintenance_component_3.SubmitPurchaseOrderDialogComponent],
            declarations: [supplier_inquiry_component_1.SupplierInquiryComponent, supplier_maintenance_component_1.SupplierMaintenanceComponent,
                purchase_order_inquiry_component_1.PurchaseOrderInquiryComponent, purchase_order_maintenance_component_1.PurchaseOrderMaintenanceComponent,
                purchase_order_management_nav_bar_component_1.PurchaseOrderManagementNavBarComponent, purchase_order_maintenance_component_2.DeletePurchaseOrderLineItemDialogComponent, purchase_order_maintenance_component_3.SubmitPurchaseOrderDialogComponent]
        })
    ], PurchaseOrderManagementModule);
    return PurchaseOrderManagementModule;
}());
exports.PurchaseOrderManagementModule = PurchaseOrderManagementModule;
//# sourceMappingURL=purchase-order-management.module.js.map