"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var customer_maintenance_component_1 = require("./customer-maintenance/customer-maintenance.component");
var customer_inquiry_component_1 = require("./customer-inquiry/customer-inquiry.component");
var sales_order_maintenance_component_1 = require("./sales-order-maintenance/sales-order-maintenance.component");
var sales_order_inquiry_component_1 = require("./sales-order-inquiry/sales-order-inquiry.component");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var material_module_1 = require("../material.module");
var sales_order_management_routing_1 = require("./sales-order-management.routing");
var sales_order_management_nav_bar_component_1 = require("./sales-order-management-nav-bar/sales-order-management-nav-bar.component");
var sales_order_maintenance_component_2 = require("./sales-order-maintenance/sales-order-maintenance.component");
var sales_order_maintenance_component_3 = require("./sales-order-maintenance/sales-order-maintenance.component");
var SalesOrderManagementModule = /** @class */ (function () {
    function SalesOrderManagementModule() {
    }
    SalesOrderManagementModule = __decorate([
        core_1.NgModule({
            imports: [
                sales_order_management_routing_1.SalesOrderManagementRoutingModule,
                common_1.CommonModule,
                forms_1.FormsModule,
                material_module_1.MaterialModule
            ],
            entryComponents: [sales_order_maintenance_component_2.DeleteSalesOrderLineItemDialogComponent, sales_order_maintenance_component_3.SubmitSalesOrderDialogComponent],
            declarations: [customer_inquiry_component_1.CustomerInquiryComponent, customer_maintenance_component_1.CustomerMaintenanceComponent,
                sales_order_inquiry_component_1.SalesOrderInquiryComponent, sales_order_maintenance_component_1.SalesOrderMaintenanceComponent,
                sales_order_management_nav_bar_component_1.SalesOrderManagementNavBarComponent, sales_order_maintenance_component_2.DeleteSalesOrderLineItemDialogComponent, sales_order_maintenance_component_3.SubmitSalesOrderDialogComponent]
        })
    ], SalesOrderManagementModule);
    return SalesOrderManagementModule;
}());
exports.SalesOrderManagementModule = SalesOrderManagementModule;
//# sourceMappingURL=sales-order-management.module.js.map