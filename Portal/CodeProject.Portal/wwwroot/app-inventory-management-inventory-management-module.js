(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["app-inventory-management-inventory-management-module"],{

/***/ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.css":
/*!************************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.css ***!
  \************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.html":
/*!*************************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.html ***!
  \*************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n<app-inventory-management-nav-bar></app-inventory-management-nav-bar>\n<p>\n  inventory-adjustments works!\n</p>\n"

/***/ }),

/***/ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.ts":
/*!***********************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.ts ***!
  \***********************************************************************************************/
/*! exports provided: InventoryAdjustmentsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InventoryAdjustmentsComponent", function() { return InventoryAdjustmentsComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var InventoryAdjustmentsComponent = /** @class */ (function () {
    function InventoryAdjustmentsComponent() {
    }
    InventoryAdjustmentsComponent.prototype.ngOnInit = function () {
    };
    InventoryAdjustmentsComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-inventory-adjustments',
            template: __webpack_require__(/*! ./inventory-adjustments.component.html */ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.html"),
            styles: [__webpack_require__(/*! ./inventory-adjustments.component.css */ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], InventoryAdjustmentsComponent);
    return InventoryAdjustmentsComponent;
}());



/***/ }),

/***/ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.css":
/*!**************************************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.css ***!
  \**************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.html":
/*!***************************************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.html ***!
  \***************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n<mat-toolbar color=\"warn\">\n  \n    <div style=\"margin-right:20px;\">\n      <a mat-list-item>\n        <mat-icon class=\"icon\">dashboard</mat-icon>\n        <span class=\"label\">Inventory Management</span>\n      </a>\n    </div>\n</mat-toolbar>\n\n<mat-toolbar color=\"accent\">\n\n  <div style=\"margin-right:20px;\">\n    <a mat-list-item [routerLink]=\"['/inventorymanagement/product-maintenance']\">\n      <mat-icon class=\"icon\">dashboard</mat-icon>\n      <span class=\"label\">Product Maintenance</span>\n    </a>\n  </div>\n\n  <div style=\"margin-right:20px;\">\n    <a mat-list-item  [routerLink]=\"['/inventorymanagement/product-inquiry']\">\n      <mat-icon class=\"icon\">dashboard</mat-icon>\n      <span class=\"label\">Product Inquiry</span>\n    </a>\n  </div>\n\n  <div style=\"margin-right:20px;\">\n    <a mat-list-item  [routerLink]=\"['/inventorymanagement/inventory-adjustments']\">\n      <mat-icon class=\"icon\">dashboard</mat-icon>\n      <span class=\"label\">Inventory Adjustments</span>\n    </a>\n  </div>\n\n  <div style=\"margin-right:20px;\">\n    <a mat-list-item  [routerLink]=\"['/inventorymanagement/order-shipments']\">\n      <mat-icon class=\"icon\">dashboard</mat-icon>\n      <span class=\"label\">Order Shipments</span>\n    </a>\n  </div>\n\n  <div style=\"margin-right:20px;\">\n    <a mat-list-item  [routerLink]=\"['/inventorymanagement/purchase-order-receiving']\">\n      <mat-icon class=\"icon\">dashboard</mat-icon>\n      <span class=\"label\">Purchase Order Receiving</span>\n    </a>\n  </div>\n\n</mat-toolbar>"

/***/ }),

/***/ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.ts":
/*!*************************************************************************************************************!*\
  !*** ./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.ts ***!
  \*************************************************************************************************************/
/*! exports provided: InventoryManagementNavBarComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InventoryManagementNavBarComponent", function() { return InventoryManagementNavBarComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var InventoryManagementNavBarComponent = /** @class */ (function () {
    function InventoryManagementNavBarComponent() {
    }
    InventoryManagementNavBarComponent.prototype.ngOnInit = function () {
    };
    InventoryManagementNavBarComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-inventory-management-nav-bar',
            template: __webpack_require__(/*! ./inventory-management-nav-bar.component.html */ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.html"),
            styles: [__webpack_require__(/*! ./inventory-management-nav-bar.component.css */ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], InventoryManagementNavBarComponent);
    return InventoryManagementNavBarComponent;
}());



/***/ }),

/***/ "./src/app/inventory-management/inventory-management.module.ts":
/*!*********************************************************************!*\
  !*** ./src/app/inventory-management/inventory-management.module.ts ***!
  \*********************************************************************/
/*! exports provided: InventoryManagementModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InventoryManagementModule", function() { return InventoryManagementModule; });
/* harmony import */ var _inventory_management_nav_bar_inventory_management_nav_bar_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./inventory-management-nav-bar/inventory-management-nav-bar.component */ "./src/app/inventory-management/inventory-management-nav-bar/inventory-management-nav-bar.component.ts");
/* harmony import */ var _inventory_adjustments_inventory_adjustments_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./inventory-adjustments/inventory-adjustments.component */ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.ts");
/* harmony import */ var _purchase_order_receiving_purchase_order_receiving_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./purchase-order-receiving/purchase-order-receiving.component */ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.ts");
/* harmony import */ var _order_shipments_order_shipments_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./order-shipments/order-shipments.component */ "./src/app/inventory-management/order-shipments/order-shipments.component.ts");
/* harmony import */ var _product_maintenance_product_maintenance_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./product-maintenance/product-maintenance.component */ "./src/app/inventory-management/product-maintenance/product-maintenance.component.ts");
/* harmony import */ var _product_inquiry_product_inquiry_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./product-inquiry/product-inquiry.component */ "./src/app/inventory-management/product-inquiry/product-inquiry.component.ts");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _inventory_management_routing__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./inventory-management.routing */ "./src/app/inventory-management/inventory-management.routing.ts");
/* harmony import */ var _angular_material__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material */ "./node_modules/@angular/material/esm5/material.es5.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};











var InventoryManagementModule = /** @class */ (function () {
    function InventoryManagementModule() {
    }
    InventoryManagementModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_6__["NgModule"])({
            imports: [
                _inventory_management_routing__WEBPACK_IMPORTED_MODULE_9__["InventoryManagementRoutingModule"],
                _angular_common__WEBPACK_IMPORTED_MODULE_7__["CommonModule"],
                _angular_forms__WEBPACK_IMPORTED_MODULE_8__["FormsModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatTabsModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatInputModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatButtonModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatSelectModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatIconModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatListModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatIconModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatSidenavModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatToolbarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatSnackBarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatProgressBarModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatFormFieldModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatCardModule"],
                _angular_material__WEBPACK_IMPORTED_MODULE_10__["MatGridListModule"],
            ],
            declarations: [_product_inquiry_product_inquiry_component__WEBPACK_IMPORTED_MODULE_5__["ProductInquiryComponent"], _product_maintenance_product_maintenance_component__WEBPACK_IMPORTED_MODULE_4__["ProductMaintenanceComponent"], _inventory_adjustments_inventory_adjustments_component__WEBPACK_IMPORTED_MODULE_1__["InventoryAdjustmentsComponent"],
                _purchase_order_receiving_purchase_order_receiving_component__WEBPACK_IMPORTED_MODULE_2__["PurchaseOrderReceivingComponent"], _order_shipments_order_shipments_component__WEBPACK_IMPORTED_MODULE_3__["OrderShipmentsComponent"], _inventory_management_nav_bar_inventory_management_nav_bar_component__WEBPACK_IMPORTED_MODULE_0__["InventoryManagementNavBarComponent"]]
        })
    ], InventoryManagementModule);
    return InventoryManagementModule;
}());



/***/ }),

/***/ "./src/app/inventory-management/inventory-management.routing.ts":
/*!**********************************************************************!*\
  !*** ./src/app/inventory-management/inventory-management.routing.ts ***!
  \**********************************************************************/
/*! exports provided: InventoryManagementRoutingModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "InventoryManagementRoutingModule", function() { return InventoryManagementRoutingModule; });
/* harmony import */ var _inventory_adjustments_inventory_adjustments_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./inventory-adjustments/inventory-adjustments.component */ "./src/app/inventory-management/inventory-adjustments/inventory-adjustments.component.ts");
/* harmony import */ var _purchase_order_receiving_purchase_order_receiving_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./purchase-order-receiving/purchase-order-receiving.component */ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.ts");
/* harmony import */ var _order_shipments_order_shipments_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./order-shipments/order-shipments.component */ "./src/app/inventory-management/order-shipments/order-shipments.component.ts");
/* harmony import */ var _product_maintenance_product_maintenance_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./product-maintenance/product-maintenance.component */ "./src/app/inventory-management/product-maintenance/product-maintenance.component.ts");
/* harmony import */ var _product_inquiry_product_inquiry_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./product-inquiry/product-inquiry.component */ "./src/app/inventory-management/product-inquiry/product-inquiry.component.ts");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};







var InventoryManagementRoutes = [
    { path: '', component: _product_maintenance_product_maintenance_component__WEBPACK_IMPORTED_MODULE_3__["ProductMaintenanceComponent"] },
    { path: 'product-maintenance', component: _product_maintenance_product_maintenance_component__WEBPACK_IMPORTED_MODULE_3__["ProductMaintenanceComponent"] },
    { path: 'product-inquiry', component: _product_inquiry_product_inquiry_component__WEBPACK_IMPORTED_MODULE_4__["ProductInquiryComponent"] },
    { path: 'inventory-adjustments', component: _inventory_adjustments_inventory_adjustments_component__WEBPACK_IMPORTED_MODULE_0__["InventoryAdjustmentsComponent"] },
    { path: 'order-shipments', component: _order_shipments_order_shipments_component__WEBPACK_IMPORTED_MODULE_2__["OrderShipmentsComponent"] },
    { path: 'purchase-order-receiving', component: _purchase_order_receiving_purchase_order_receiving_component__WEBPACK_IMPORTED_MODULE_1__["PurchaseOrderReceivingComponent"] }
];
var InventoryManagementRoutingModule = /** @class */ (function () {
    function InventoryManagementRoutingModule() {
    }
    InventoryManagementRoutingModule = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_5__["NgModule"])({
            imports: [
                _angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"].forChild(InventoryManagementRoutes)
            ],
            exports: [_angular_router__WEBPACK_IMPORTED_MODULE_6__["RouterModule"]]
        })
    ], InventoryManagementRoutingModule);
    return InventoryManagementRoutingModule;
}());



/***/ }),

/***/ "./src/app/inventory-management/order-shipments/order-shipments.component.css":
/*!************************************************************************************!*\
  !*** ./src/app/inventory-management/order-shipments/order-shipments.component.css ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/order-shipments/order-shipments.component.html":
/*!*************************************************************************************!*\
  !*** ./src/app/inventory-management/order-shipments/order-shipments.component.html ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<app-inventory-management-nav-bar></app-inventory-management-nav-bar>\n<p>\n  order-shipments works!\n</p>\n"

/***/ }),

/***/ "./src/app/inventory-management/order-shipments/order-shipments.component.ts":
/*!***********************************************************************************!*\
  !*** ./src/app/inventory-management/order-shipments/order-shipments.component.ts ***!
  \***********************************************************************************/
/*! exports provided: OrderShipmentsComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OrderShipmentsComponent", function() { return OrderShipmentsComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var OrderShipmentsComponent = /** @class */ (function () {
    function OrderShipmentsComponent() {
    }
    OrderShipmentsComponent.prototype.ngOnInit = function () {
    };
    OrderShipmentsComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-order-shipments',
            template: __webpack_require__(/*! ./order-shipments.component.html */ "./src/app/inventory-management/order-shipments/order-shipments.component.html"),
            styles: [__webpack_require__(/*! ./order-shipments.component.css */ "./src/app/inventory-management/order-shipments/order-shipments.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], OrderShipmentsComponent);
    return OrderShipmentsComponent;
}());



/***/ }),

/***/ "./src/app/inventory-management/product-inquiry/product-inquiry.component.css":
/*!************************************************************************************!*\
  !*** ./src/app/inventory-management/product-inquiry/product-inquiry.component.css ***!
  \************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/product-inquiry/product-inquiry.component.html":
/*!*************************************************************************************!*\
  !*** ./src/app/inventory-management/product-inquiry/product-inquiry.component.html ***!
  \*************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<app-inventory-management-nav-bar></app-inventory-management-nav-bar>\n<p>\n  product-inquiry works!\n</p>\n"

/***/ }),

/***/ "./src/app/inventory-management/product-inquiry/product-inquiry.component.ts":
/*!***********************************************************************************!*\
  !*** ./src/app/inventory-management/product-inquiry/product-inquiry.component.ts ***!
  \***********************************************************************************/
/*! exports provided: ProductInquiryComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ProductInquiryComponent", function() { return ProductInquiryComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _shared_components_services_session_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../shared-components-services/session.service */ "./src/app/shared-components-services/session.service.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ProductInquiryComponent = /** @class */ (function () {
    function ProductInquiryComponent(sessionService) {
        this.sessionService = sessionService;
        this.sessionService.moduleLoadedEvent.emit();
    }
    ProductInquiryComponent.prototype.ngOnInit = function () {
    };
    ProductInquiryComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-product-inquiry',
            template: __webpack_require__(/*! ./product-inquiry.component.html */ "./src/app/inventory-management/product-inquiry/product-inquiry.component.html"),
            styles: [__webpack_require__(/*! ./product-inquiry.component.css */ "./src/app/inventory-management/product-inquiry/product-inquiry.component.css")]
        }),
        __metadata("design:paramtypes", [_shared_components_services_session_service__WEBPACK_IMPORTED_MODULE_1__["SessionService"]])
    ], ProductInquiryComponent);
    return ProductInquiryComponent;
}());



/***/ }),

/***/ "./src/app/inventory-management/product-maintenance/product-maintenance.component.css":
/*!********************************************************************************************!*\
  !*** ./src/app/inventory-management/product-maintenance/product-maintenance.component.css ***!
  \********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/product-maintenance/product-maintenance.component.html":
/*!*********************************************************************************************!*\
  !*** ./src/app/inventory-management/product-maintenance/product-maintenance.component.html ***!
  \*********************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<app-inventory-management-nav-bar></app-inventory-management-nav-bar>\n\n<mat-card>\n  <mat-card-header>\n    <mat-card-title>\n      <h2>Product Maintenance</h2>\n    </mat-card-title>\n  </mat-card-header>\n  <mat-card-content>\n\n    <form>\n\n      <div>\n        <mat-form-field style=\"width:300px\">\n          <input matInput name=\"ProductNumber\" value=\"{{productViewModel.productNumber}}\" \n          [(ngModel)]=\"productViewModel.productNumber\" placeholder=\"Product Number\">\n        </mat-form-field>\n      </div>\n\n      <div>\n        <mat-form-field style=\"width:300px\">\n          <input matInput name=\"Description\" placeholder=\"Description\" \n          value=\"{{productViewModel.description}}\" [(ngModel)]=\"productViewModel.description\">\n        </mat-form-field>\n      </div>\n\n    \n      <div>\n        <mat-form-field style=\"width:300px\">\n          <input matInput name=\"BinLocation\" placeholder=\"Bin Location\" \n          value=\"{{productViewModel.binLocation}}\" [(ngModel)]=\"productViewModel.binLocation\">\n        </mat-form-field>\n      </div>\n\n      <div>\n        <mat-form-field style=\"width:300px\">\n          <input matInput name=\"UnitPrice\" placeholder=\"Unit Price\" value=\"{{productViewModel.unitPrice}}\" \n          [(ngModel)]=\"productViewModel.unitPrice\">\n        </mat-form-field>\n      </div>\n\n    </form>\n  </mat-card-content>\n  <mat-card-actions>\n    <button mat-flat-button color=\"primary\" (click)=\"createOrUpdateProduct()\">Save Product</button>\n  </mat-card-actions>\n</mat-card>\n"

/***/ }),

/***/ "./src/app/inventory-management/product-maintenance/product-maintenance.component.ts":
/*!*******************************************************************************************!*\
  !*** ./src/app/inventory-management/product-maintenance/product-maintenance.component.ts ***!
  \*******************************************************************************************/
/*! exports provided: ProductMaintenanceComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ProductMaintenanceComponent", function() { return ProductMaintenanceComponent; });
/* harmony import */ var _shared_components_services_http_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./../../shared-components-services/http.service */ "./src/app/shared-components-services/http.service.ts");
/* harmony import */ var _shared_components_services_alert_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./../../shared-components-services/alert.service */ "./src/app/shared-components-services/alert.service.ts");
/* harmony import */ var _shared_components_services_session_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./../../shared-components-services/session.service */ "./src/app/shared-components-services/session.service.ts");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _product_view_models_product_viewmodel__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../product-view-models/product.viewmodel */ "./src/app/inventory-management/product-view-models/product.viewmodel.ts");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ProductMaintenanceComponent = /** @class */ (function () {
    function ProductMaintenanceComponent(sessionService, alertService, httpService) {
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.httpService = httpService;
        this.productViewModel = new _product_view_models_product_viewmodel__WEBPACK_IMPORTED_MODULE_4__["ProductViewModel"]();
        this.productViewModel.productNumber = 'Product Number';
        this.productViewModel.description = 'Description';
        this.productViewModel.binLocation = 'Bin Location';
        this.productViewModel.unitPrice = 100.00;
        this.productViewModel.productId = 0;
    }
    ProductMaintenanceComponent.prototype.ngOnInit = function () {
    };
    ProductMaintenanceComponent.prototype.createOrUpdateProduct = function () {
        var _this = this;
        var product = new _product_view_models_product_viewmodel__WEBPACK_IMPORTED_MODULE_4__["ProductViewModel"]();
        product = this.productViewModel;
        var url = '';
        if (product.productId === 0) {
            url = 'https://localhost:44340/api/product/createproduct';
        }
        else {
            url = 'https://localhost:44340/api/product/updateproduct';
        }
        this.httpService.HttpPost(url, product).subscribe(function (response) {
            _this.createOrUpdateProductSuccess(response);
        }, function (response) { return _this.createOrUpdateProductFailed(response); });
    };
    ProductMaintenanceComponent.prototype.createOrUpdateProductSuccess = function (response) {
        var productViewModel = response.entity;
        this.productViewModel.productId = productViewModel.productId;
        var message = 'Product successfully saved.';
        this.alertService.ShowSuccessMessage(message);
    };
    ProductMaintenanceComponent.prototype.createOrUpdateProductFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    ProductMaintenanceComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_3__["Component"])({
            selector: 'app-product-maintenance',
            template: __webpack_require__(/*! ./product-maintenance.component.html */ "./src/app/inventory-management/product-maintenance/product-maintenance.component.html"),
            styles: [__webpack_require__(/*! ./product-maintenance.component.css */ "./src/app/inventory-management/product-maintenance/product-maintenance.component.css")]
        }),
        __metadata("design:paramtypes", [_shared_components_services_session_service__WEBPACK_IMPORTED_MODULE_2__["SessionService"], _shared_components_services_alert_service__WEBPACK_IMPORTED_MODULE_1__["AlertService"], _shared_components_services_http_service__WEBPACK_IMPORTED_MODULE_0__["HttpService"]])
    ], ProductMaintenanceComponent);
    return ProductMaintenanceComponent;
}());



/***/ }),

/***/ "./src/app/inventory-management/product-view-models/product.viewmodel.ts":
/*!*******************************************************************************!*\
  !*** ./src/app/inventory-management/product-view-models/product.viewmodel.ts ***!
  \*******************************************************************************/
/*! exports provided: ProductViewModel */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ProductViewModel", function() { return ProductViewModel; });
var ProductViewModel = /** @class */ (function () {
    function ProductViewModel() {
    }
    return ProductViewModel;
}());



/***/ }),

/***/ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.css":
/*!******************************************************************************************************!*\
  !*** ./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.css ***!
  \******************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ""

/***/ }),

/***/ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.html":
/*!*******************************************************************************************************!*\
  !*** ./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.html ***!
  \*******************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<app-inventory-management-nav-bar></app-inventory-management-nav-bar>\n<p>\n  purchase-order-receiving works!\n</p>\n"

/***/ }),

/***/ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.ts":
/*!*****************************************************************************************************!*\
  !*** ./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.ts ***!
  \*****************************************************************************************************/
/*! exports provided: PurchaseOrderReceivingComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "PurchaseOrderReceivingComponent", function() { return PurchaseOrderReceivingComponent; });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (undefined && undefined.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var PurchaseOrderReceivingComponent = /** @class */ (function () {
    function PurchaseOrderReceivingComponent() {
    }
    PurchaseOrderReceivingComponent.prototype.ngOnInit = function () {
    };
    PurchaseOrderReceivingComponent = __decorate([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_0__["Component"])({
            selector: 'app-purchase-order-receiving',
            template: __webpack_require__(/*! ./purchase-order-receiving.component.html */ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.html"),
            styles: [__webpack_require__(/*! ./purchase-order-receiving.component.css */ "./src/app/inventory-management/purchase-order-receiving/purchase-order-receiving.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], PurchaseOrderReceivingComponent);
    return PurchaseOrderReceivingComponent;
}());



/***/ })

}]);
//# sourceMappingURL=app-inventory-management-inventory-management-module.js.map