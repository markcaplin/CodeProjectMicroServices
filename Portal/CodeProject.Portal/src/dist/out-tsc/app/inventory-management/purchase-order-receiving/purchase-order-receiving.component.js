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
var purchase_order_detail_viewmodel_1 = require("./../view-models/purchase-order-detail.viewmodel");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var core_1 = require("@angular/core");
var purchase_order_viewmodel_1 = require("../view-models/purchase-order.viewmodel");
var session_service_1 = require("../../shared-components-services/session.service");
var router_1 = require("@angular/router");
var http_service_1 = require("../../shared-components-services/http.service");
var table_1 = require("@angular/material/table");
var material_1 = require("@angular/material");
var PurchaseOrderReceivingComponent = /** @class */ (function () {
    function PurchaseOrderReceivingComponent(dialog, router, httpService, route, sessionService, alertService) {
        this.dialog = dialog;
        this.router = router;
        this.httpService = httpService;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.detailDataSource = new table_1.MatTableDataSource();
        this.disableSubmitButton = true;
        this.purchaseOrderViewModel = new purchase_order_viewmodel_1.PurchaseOrderViewModel();
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
    }
    PurchaseOrderReceivingComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route.queryParams.subscribe(function (params) {
            _this.purchaseOrderViewModel.purchaseOrderId = +params['id'] || 0;
            if (_this.purchaseOrderViewModel.purchaseOrderId > 0) {
                _this.getPurchaseOrder();
            }
        });
    };
    PurchaseOrderReceivingComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    PurchaseOrderReceivingComponent.prototype.getPurchaseOrder = function () {
        var _this = this;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'purchaseorder/getpurchaseorder';
        this.httpService.HttpPost(url, this.purchaseOrderViewModel)
            .subscribe(function (response) {
            _this.getPurchaseOrderSuccess(response);
        }, function (response) { return _this.getPurchaseOrderFailed(response); });
    };
    PurchaseOrderReceivingComponent.prototype.getPurchaseOrderSuccess = function (response) {
        var _this = this;
        response.entity.orderDate = response.entity.dateCreated.toString().substring(0, 10);
        this.purchaseOrderViewModel.supplierName = response.entity.supplierName;
        this.purchaseOrderViewModel.accountId = response.entity.accountId;
        this.purchaseOrderViewModel.addressLine1 = response.entity.addressLine1;
        this.purchaseOrderViewModel.addressLine2 = response.entity.addressLine2;
        this.purchaseOrderViewModel.city = response.entity.city;
        this.purchaseOrderViewModel.region = response.entity.region;
        this.purchaseOrderViewModel.postalCode = response.entity.postalCode;
        this.purchaseOrderViewModel.purchaseOrderId = response.entity.purchaseOrderId;
        this.purchaseOrderViewModel.purchaseOrderNumber = response.entity.purchaseOrderNumber;
        this.purchaseOrderViewModel.orderDate = response.entity.orderDate;
        this.purchaseOrderViewModel.orderTotal = response.entity.orderTotal;
        this.purchaseOrderViewModel.purchaseOrderStatusDescription = response.entity.purchaseOrderStatusDescription;
        this.purchaseOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        response.entity.purchaseOrderDetails.forEach(function (element) {
            var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
            purchaseOrderDetailViewModel.purchaseOrderDetailId = element.purchaseOrderDetailId;
            purchaseOrderDetailViewModel.purchaseOrderId = element.purchaseOrderId;
            purchaseOrderDetailViewModel.productId = element.productId;
            purchaseOrderDetailViewModel.productDescription = element.productDescription;
            purchaseOrderDetailViewModel.productMasterId = element.productMasterId;
            purchaseOrderDetailViewModel.productNumber = element.productNumber;
            purchaseOrderDetailViewModel.unitPrice = element.unitPrice;
            purchaseOrderDetailViewModel.orderQuantity = element.orderQuantity;
            purchaseOrderDetailViewModel.orderQuantityFormatted = element.orderQuantity.toFixed(0);
            purchaseOrderDetailViewModel.receivedQuantityFormatted = element.receivedQuantity.toFixed(0);
            purchaseOrderDetailViewModel.unitPriceFormatted = element.unitPrice.toFixed(2);
            purchaseOrderDetailViewModel.editCurrentReceivedQuantity = false;
            purchaseOrderDetailViewModel.editQuantity = false;
            purchaseOrderDetailViewModel.editUnitPrice = false;
            purchaseOrderDetailViewModel.editProductNumber = false;
            purchaseOrderDetailViewModel.editMode = false;
            purchaseOrderDetailViewModel.disableAddButton = true;
            purchaseOrderDetailViewModel.disableSaveButton = true;
            purchaseOrderDetailViewModel.disableCancelButton = true;
            purchaseOrderDetailViewModel.disableDeleteButton = false;
            purchaseOrderDetailViewModel.disableEditButton = false;
            var purchaseOrderDetailViewModelOriginalValues = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
            purchaseOrderDetailViewModelOriginalValues.unitPrice = element.unitPrice;
            purchaseOrderDetailViewModelOriginalValues.orderQuantity = element.orderQuantity;
            purchaseOrderDetailViewModelOriginalValues.orderQuantityFormatted = element.orderQuantity.toFixed(0);
            purchaseOrderDetailViewModelOriginalValues.receivedQuantityFormatted = element.receivedQuantity.toFixed(0);
            purchaseOrderDetailViewModelOriginalValues.unitPriceFormatted = element.unitPrice.toFixed(2);
            _this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
            _this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues.push(purchaseOrderDetailViewModelOriginalValues);
        });
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
        this.purchaseOrderViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice',
            'orderQuantity', 'receivedQuantity', 'currentReceivedQuantity', 'actions'];
        if (this.purchaseOrderViewModel.purchaseOrderStatusDescription !== 'Open') {
            this.disablePurchaseOrderButtons();
        }
    };
    PurchaseOrderReceivingComponent.prototype.getPurchaseOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderReceivingComponent.prototype.getProductFailed = function (error) {
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableAddButton = true;
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderReceivingComponent.prototype.editLineItem = function (i) {
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editCurrentReceivedQuantity = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableSaveButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableCancelButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableDeleteButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableEditButton = true;
        var purchaseOrderDetailViewModelOriginalValues = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModelOriginalValues.unitPrice =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice;
        purchaseOrderDetailViewModelOriginalValues.orderQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity;
        purchaseOrderDetailViewModelOriginalValues.receivedQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].receivedQuantity;
        purchaseOrderDetailViewModelOriginalValues.orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity.toFixed(0);
        purchaseOrderDetailViewModelOriginalValues.receivedQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].receivedQuantity.toFixed(0);
        purchaseOrderDetailViewModelOriginalValues.unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice.toFixed(2);
        this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i] =
            purchaseOrderDetailViewModelOriginalValues;
        this.setDisableSubmitButton();
    };
    PurchaseOrderReceivingComponent.prototype.updateLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.purchaseOrderDetailId = this.purchaseOrderViewModel.purchaseOrderDetails[i].purchaseOrderDetailId;
        purchaseOrderDetailViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderId;
        purchaseOrderDetailViewModel.currentReceivedQuantity =
            parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].currentReceivedQuantityFormatted, 0);
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'purchaseorder/updatepurchaseorderdetail';
        this.httpService.HttpPost(url, purchaseOrderDetailViewModel)
            .subscribe(function (response) {
            _this.updateLineItemSuccess(response);
        }, function (response) { return _this.updateLineItemFailed(response); });
    };
    PurchaseOrderReceivingComponent.prototype.cancelEdit = function (i) {
        this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].unitPrice;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].unitPriceFormatted;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].orderQuantity;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].receivedQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].receivedQuantity;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].currentReceivedQuantity = 0;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].currentReceivedQuantityFormatted = '';
        this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].orderQuantityFormatted;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].receivedQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].receivedQuantityFormatted;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editCurrentReceivedQuantity = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableSaveButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableCancelButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableDeleteButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableEditButton = false;
        this.setEnableSubmitButton();
    };
    PurchaseOrderReceivingComponent.prototype.updateLineItemSuccess = function (response) {
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].editCurrentReceivedQuantity = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableSaveButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableCancelButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableDeleteButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableEditButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantity.toFixed(0);
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].receivedQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].receivedQuantity.toFixed(0);
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].currentReceivedQuantityFormatted = '';
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPrice.toFixed(2);
        this.purchaseOrderViewModel.orderTotal = response.entity.orderTotal;
        this.purchaseOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully updated.';
        this.alertService.ShowSuccessMessage(message);
    };
    PurchaseOrderReceivingComponent.prototype.updateLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderReceivingComponent.prototype.setDisableSubmitButton = function () {
        this.disableSubmitButton = true;
    };
    PurchaseOrderReceivingComponent.prototype.setEnableSubmitButton = function () {
        this.disableSubmitButton = true;
        if (this.purchaseOrderViewModel.orderTotal > 0 && this.purchaseOrderViewModel.purchaseOrderStatusDescription === 'Open') {
            this.disableSubmitButton = false;
        }
    };
    PurchaseOrderReceivingComponent.prototype.disablePurchaseOrderButtons = function () {
    };
    PurchaseOrderReceivingComponent = __decorate([
        core_1.Component({
            selector: 'app-purchase-order-receiving',
            templateUrl: './purchase-order-receiving.component.html',
            styleUrls: ['./purchase-order-receiving.component.css']
        }),
        __metadata("design:paramtypes", [material_1.MatDialog, router_1.Router, http_service_1.HttpService, router_1.ActivatedRoute,
            session_service_1.SessionService, alert_service_1.AlertService])
    ], PurchaseOrderReceivingComponent);
    return PurchaseOrderReceivingComponent;
}());
exports.PurchaseOrderReceivingComponent = PurchaseOrderReceivingComponent;
//# sourceMappingURL=purchase-order-receiving.component.js.map