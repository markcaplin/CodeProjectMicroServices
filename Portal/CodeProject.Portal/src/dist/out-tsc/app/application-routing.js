"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var about_component_1 = require("./home-directory/about/about.component");
var contact_component_1 = require("./home-directory/contact/contact.component");
var home_component_1 = require("./home-directory/home/home.component");
exports.ApplicationRoutes = [
    { path: '', component: home_component_1.HomeComponent, pathMatch: 'full' },
    { path: 'home/home', component: home_component_1.HomeComponent },
    { path: 'home/contact', component: contact_component_1.ContactComponent },
    { path: 'home/about', component: about_component_1.AboutComponent },
    {
        path: 'accountmanagement', loadChildren: '../app/account-management/account-management.module#AccountManagementModule'
    },
    {
        path: 'inventorymanagement', loadChildren: '../app/inventory-management/inventory-management.module#InventoryManagementModule'
    },
    {
        path: 'purchaseordermanagement', loadChildren: '../app/purchase-order-management/purchase-order-management.module#PurchaseOrderManagementModule'
    },
    {
        path: 'salesordermanagement', loadChildren: '../app/sales-order-management/sales-order-management.module#SalesOrderManagementModule'
    }
];
//# sourceMappingURL=application-routing.js.map