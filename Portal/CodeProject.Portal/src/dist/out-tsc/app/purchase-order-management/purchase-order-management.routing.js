"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var purchase_order_maintenance_component_1 = require("./purchase-order-maintenance/purchase-order-maintenance.component");
var purchase_order_inquiry_component_1 = require("./purchase-order-inquiry/purchase-order-inquiry.component");
var supplier_maintenance_component_1 = require("./supplier-maintenance/supplier-maintenance.component");
var supplier_inquiry_component_1 = require("./supplier-inquiry/supplier-inquiry.component");
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var PurchaseOrderManagementRoutes = [
    { path: '', component: purchase_order_inquiry_component_1.PurchaseOrderInquiryComponent },
    { path: 'supplier-maintenance', component: supplier_maintenance_component_1.SupplierMaintenanceComponent },
    { path: 'supplier-maintenance/:id', component: supplier_maintenance_component_1.SupplierMaintenanceComponent },
    { path: 'supplier-inquiry', component: supplier_inquiry_component_1.SupplierInquiryComponent },
    { path: 'purchase-order-maintenance', component: purchase_order_maintenance_component_1.PurchaseOrderMaintenanceComponent },
    { path: 'purchase-order-maintenance/:id', component: purchase_order_maintenance_component_1.PurchaseOrderMaintenanceComponent },
    { path: 'purchase-order-inquiry', component: purchase_order_inquiry_component_1.PurchaseOrderInquiryComponent },
];
var PurchaseOrderManagementRoutingModule = /** @class */ (function () {
    function PurchaseOrderManagementRoutingModule() {
    }
    PurchaseOrderManagementRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forChild(PurchaseOrderManagementRoutes)
            ],
            exports: [router_1.RouterModule]
        })
    ], PurchaseOrderManagementRoutingModule);
    return PurchaseOrderManagementRoutingModule;
}());
exports.PurchaseOrderManagementRoutingModule = PurchaseOrderManagementRoutingModule;
//# sourceMappingURL=purchase-order-management.routing.js.map