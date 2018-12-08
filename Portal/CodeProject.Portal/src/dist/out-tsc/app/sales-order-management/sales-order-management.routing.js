"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var sales_order_maintenance_component_1 = require("./sales-order-maintenance/sales-order-maintenance.component");
var sales_order_inquiry_component_1 = require("./sales-order-inquiry/sales-order-inquiry.component");
var customer_maintenance_component_1 = require("./customer-maintenance/customer-maintenance.component");
var customer_inquiry_component_1 = require("./customer-inquiry/customer-inquiry.component");
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var SalesOrderManagementRoutes = [
    { path: '', component: sales_order_inquiry_component_1.SalesOrderInquiryComponent },
    { path: 'customer-maintenance', component: customer_maintenance_component_1.CustomerMaintenanceComponent },
    { path: 'customer-maintenance/:id', component: customer_maintenance_component_1.CustomerMaintenanceComponent },
    { path: 'customer-inquiry', component: customer_inquiry_component_1.CustomerInquiryComponent },
    { path: 'sales-order-maintenance', component: sales_order_maintenance_component_1.SalesOrderMaintenanceComponent },
    { path: 'sales-order-maintenance/:id', component: sales_order_maintenance_component_1.SalesOrderMaintenanceComponent },
    { path: 'sales-order-inquiry', component: sales_order_inquiry_component_1.SalesOrderInquiryComponent },
];
var SalesOrderManagementRoutingModule = /** @class */ (function () {
    function SalesOrderManagementRoutingModule() {
    }
    SalesOrderManagementRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forChild(SalesOrderManagementRoutes)
            ],
            exports: [router_1.RouterModule]
        })
    ], SalesOrderManagementRoutingModule);
    return SalesOrderManagementRoutingModule;
}());
exports.SalesOrderManagementRoutingModule = SalesOrderManagementRoutingModule;
//# sourceMappingURL=sales-order-management.routing.js.map