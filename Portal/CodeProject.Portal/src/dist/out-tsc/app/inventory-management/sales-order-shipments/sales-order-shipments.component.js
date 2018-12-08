"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var sales_order_detail_viewmodel_1 = require("./../view-models/sales-order-detail.viewmodel");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var core_1 = require("@angular/core");
var sales_order_viewmodel_1 = require("../view-models/sales-order.viewmodel");
var session_service_1 = require("../../shared-components-services/session.service");
var router_1 = require("@angular/router");
var http_service_1 = require("../../shared-components-services/http.service");
var table_1 = require("@angular/material/table");
var material_1 = require("@angular/material");
var SalesOrderShipmentsComponent = /** @class */ (function () {
    function SalesOrderShipmentsComponent(dialog, router, httpService, route, sessionService, alertService) {
        this.dialog = dialog;
        this.router = router;
        this.httpService = httpService;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.detailDataSource = new table_1.MatTableDataSource();
        this.disableSubmitButton = true;
        this.salesOrderViewModel = new sales_order_viewmodel_1.SalesOrderViewModel();
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
    }
    SalesOrderShipmentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route.queryParams.subscribe(function (params) {
            _this.salesOrderViewModel.salesOrderId = +params['id'] || 0;
            if (_this.salesOrderViewModel.salesOrderId > 0) {
                _this.getSalesOrder();
            }
        });
    };
    SalesOrderShipmentsComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    SalesOrderShipmentsComponent.prototype.getSalesOrder = function () {
        var _this = this;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'salesorder/getsalesorder';
        this.httpService.HttpPost(url, this.salesOrderViewModel)
            .subscribe(function (response) {
            _this.getSalesOrderSuccess(response);
        }, function (response) { return _this.getSalesOrderFailed(response); });
    };
    SalesOrderShipmentsComponent.prototype.getSalesOrderSuccess = function (response) {
        var _this = this;
        response.entity.orderDate = response.entity.dateCreated.toString().substring(0, 10);
        this.salesOrderViewModel.customerName = response.entity.customerName;
        this.salesOrderViewModel.accountId = response.entity.accountId;
        this.salesOrderViewModel.addressLine1 = response.entity.addressLine1;
        this.salesOrderViewModel.addressLine2 = response.entity.addressLine2;
        this.salesOrderViewModel.city = response.entity.city;
        this.salesOrderViewModel.region = response.entity.region;
        this.salesOrderViewModel.postalCode = response.entity.postalCode;
        this.salesOrderViewModel.salesOrderId = response.entity.salesOrderId;
        this.salesOrderViewModel.salesOrderNumber = response.entity.salesOrderNumber;
        this.salesOrderViewModel.orderDate = response.entity.orderDate;
        this.salesOrderViewModel.orderTotal = response.entity.orderTotal;
        this.salesOrderViewModel.salesOrderStatusDescription = response.entity.salesOrderStatusDescription;
        this.salesOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        response.entity.salesOrderDetails.forEach(function (element) {
            var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
            salesOrderDetailViewModel.salesOrderDetailId = element.salesOrderDetailId;
            salesOrderDetailViewModel.salesOrderId = element.salesOrderId;
            salesOrderDetailViewModel.productId = element.productId;
            salesOrderDetailViewModel.productDescription = element.productDescription;
            salesOrderDetailViewModel.productMasterId = element.productMasterId;
            salesOrderDetailViewModel.productNumber = element.productNumber;
            salesOrderDetailViewModel.unitPrice = element.unitPrice;
            salesOrderDetailViewModel.orderQuantity = element.orderQuantity;
            salesOrderDetailViewModel.orderQuantityFormatted = element.orderQuantity.toFixed(0);
            salesOrderDetailViewModel.shippedQuantityFormatted = element.shippedQuantity.toFixed(0);
            salesOrderDetailViewModel.unitPriceFormatted = element.unitPrice.toFixed(2);
            salesOrderDetailViewModel.editCurrentShippedQuantity = false;
            salesOrderDetailViewModel.editQuantity = false;
            salesOrderDetailViewModel.editUnitPrice = false;
            salesOrderDetailViewModel.editProductNumber = false;
            salesOrderDetailViewModel.editMode = false;
            salesOrderDetailViewModel.disableAddButton = true;
            salesOrderDetailViewModel.disableSaveButton = true;
            salesOrderDetailViewModel.disableCancelButton = true;
            salesOrderDetailViewModel.disableDeleteButton = false;
            salesOrderDetailViewModel.disableEditButton = false;
            var salesOrderDetailViewModelOriginalValues = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
            salesOrderDetailViewModelOriginalValues.unitPrice = element.unitPrice;
            salesOrderDetailViewModelOriginalValues.orderQuantity = element.orderQuantity;
            salesOrderDetailViewModelOriginalValues.orderQuantityFormatted = element.orderQuantity.toFixed(0);
            salesOrderDetailViewModelOriginalValues.shippedQuantityFormatted = element.shippedQuantity.toFixed(0);
            salesOrderDetailViewModelOriginalValues.unitPriceFormatted = element.unitPrice.toFixed(2);
            _this.salesOrderViewModel.salesOrderDetails.push(salesOrderDetailViewModel);
            _this.salesOrderViewModel.salesOrderDetailsOriginalValues.push(salesOrderDetailViewModelOriginalValues);
        });
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
        this.salesOrderViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice',
            'orderQuantity', 'shippedQuantity', 'currentShippedQuantity', 'actions'];
        if (this.salesOrderViewModel.salesOrderStatusDescription !== 'Open') {
            this.disableSalesOrderButtons();
        }
    };
    SalesOrderShipmentsComponent.prototype.getSalesOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderShipmentsComponent.prototype.getProductFailed = function (error) {
        this.salesOrderViewModel.salesOrderDetails[0].disableAddButton = true;
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderShipmentsComponent.prototype.editLineItem = function (i) {
        this.salesOrderViewModel.salesOrderDetails[i].editCurrentShippedQuantity = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableSaveButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableCancelButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableDeleteButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableEditButton = true;
        var salesOrderDetailViewModelOriginalValues = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModelOriginalValues.unitPrice =
            this.salesOrderViewModel.salesOrderDetails[i].unitPrice;
        salesOrderDetailViewModelOriginalValues.orderQuantity =
            this.salesOrderViewModel.salesOrderDetails[i].orderQuantity;
        salesOrderDetailViewModelOriginalValues.shippedQuantity =
            this.salesOrderViewModel.salesOrderDetails[i].shippedQuantity;
        salesOrderDetailViewModelOriginalValues.orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[i].orderQuantity.toFixed(0);
        salesOrderDetailViewModelOriginalValues.shippedQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[i].shippedQuantity.toFixed(0);
        salesOrderDetailViewModelOriginalValues.unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetails[i].unitPrice.toFixed(2);
        this.salesOrderViewModel.salesOrderDetailsOriginalValues[i] =
            salesOrderDetailViewModelOriginalValues;
        this.setDisableSubmitButton();
    };
    SalesOrderShipmentsComponent.prototype.updateLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.salesOrderDetailId = this.salesOrderViewModel.salesOrderDetails[i].salesOrderDetailId;
        salesOrderDetailViewModel.salesOrderId = this.salesOrderViewModel.salesOrderId;
        salesOrderDetailViewModel.currentShippedQuantity =
            parseInt(this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].currentShippedQuantityFormatted, 0);
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'salesorder/updatesalesorderdetail';
        this.httpService.HttpPost(url, salesOrderDetailViewModel)
            .subscribe(function (response) {
            _this.updateLineItemSuccess(response);
        }, function (response) { return _this.updateLineItemFailed(response); });
    };
    SalesOrderShipmentsComponent.prototype.cancelEdit = function (i) {
        this.salesOrderViewModel.salesOrderDetails[i].unitPrice =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].unitPrice;
        this.salesOrderViewModel.salesOrderDetails[i].unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].unitPriceFormatted;
        this.salesOrderViewModel.salesOrderDetails[i].orderQuantity =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].orderQuantity;
        this.salesOrderViewModel.salesOrderDetails[i].shippedQuantity =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].shippedQuantity;
        this.salesOrderViewModel.salesOrderDetails[i].currentShippedQuantity = 0;
        this.salesOrderViewModel.salesOrderDetails[i].currentShippedQuantityFormatted = '';
        this.salesOrderViewModel.salesOrderDetails[i].orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].orderQuantityFormatted;
        this.salesOrderViewModel.salesOrderDetails[i].shippedQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].shippedQuantityFormatted;
        this.salesOrderViewModel.salesOrderDetails[i].editCurrentShippedQuantity = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableSaveButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableCancelButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableDeleteButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableEditButton = false;
        this.setEnableSubmitButton();
    };
    SalesOrderShipmentsComponent.prototype.updateLineItemSuccess = function (response) {
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].editCurrentShippedQuantity = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableSaveButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableCancelButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableDeleteButton = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableEditButton = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantity.toFixed(0);
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].shippedQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].shippedQuantity.toFixed(0);
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].currentShippedQuantityFormatted = '';
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPrice.toFixed(2);
        this.salesOrderViewModel.orderTotal = response.entity.orderTotal;
        this.salesOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully updated.';
        this.alertService.ShowSuccessMessage(message);
    };
    SalesOrderShipmentsComponent.prototype.updateLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderShipmentsComponent.prototype.setDisableSubmitButton = function () {
        this.disableSubmitButton = true;
    };
    SalesOrderShipmentsComponent.prototype.setEnableSubmitButton = function () {
        this.disableSubmitButton = true;
        if (this.salesOrderViewModel.orderTotal > 0 && this.salesOrderViewModel.salesOrderStatusDescription === 'Open') {
            this.disableSubmitButton = false;
        }
    };
    SalesOrderShipmentsComponent.prototype.disableSalesOrderButtons = function () {
    };
    SalesOrderShipmentsComponent = __decorate([
        core_1.Component({
            selector: 'app-sales-order-shipments',
            templateUrl: './sales-order-shipments.component.html',
            styleUrls: ['./sales-order-shipments.component.css']
        }),
        __metadata("design:paramtypes", [material_1.MatDialog, router_1.Router, http_service_1.HttpService, router_1.ActivatedRoute,
            session_service_1.SessionService, alert_service_1.AlertService])
    ], SalesOrderShipmentsComponent);
    return SalesOrderShipmentsComponent;
}());
exports.SalesOrderShipmentsComponent = SalesOrderShipmentsComponent;
//# sourceMappingURL=sales-order-shipments.component.js.map