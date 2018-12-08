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
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
var purchase_order_detail_viewmodel_1 = require("./../view-models/purchase-order-detail.viewmodel");
var product_viewmodel_1 = require("./../view-models/product.viewmodel");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var core_1 = require("@angular/core");
var purchase_order_viewmodel_1 = require("../view-models/purchase-order.viewmodel");
var session_service_1 = require("../../shared-components-services/session.service");
var router_1 = require("@angular/router");
var http_service_1 = require("../../shared-components-services/http.service");
var table_1 = require("@angular/material/table");
var material_1 = require("@angular/material");
var PurchaseOrderMaintenanceComponent = /** @class */ (function () {
    function PurchaseOrderMaintenanceComponent(dialog, router, httpService, route, sessionService, alertService) {
        this.dialog = dialog;
        this.router = router;
        this.httpService = httpService;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.detailDataSource = new table_1.MatTableDataSource();
        this.disableSubmitButton = true;
        this.purchaseOrderViewModel = new purchase_order_viewmodel_1.PurchaseOrderViewModel();
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.productDescription = '';
        purchaseOrderDetailViewModel.productNumber = ' ';
        purchaseOrderDetailViewModel.unitPriceFormatted = '';
        purchaseOrderDetailViewModel.orderQuantityFormatted = '';
        purchaseOrderDetailViewModel.editProductNumber = true;
        purchaseOrderDetailViewModel.editQuantity = false;
        purchaseOrderDetailViewModel.editUnitPrice = false;
        purchaseOrderDetailViewModel.editMode = false;
        purchaseOrderDetailViewModel.disableAddButton = true;
        purchaseOrderDetailViewModel.disableCancelButton = true;
        purchaseOrderDetailViewModel.disableDeleteButton = true;
        purchaseOrderDetailViewModel.disableEditButton = true;
        purchaseOrderDetailViewModel.disableSaveButton = true;
        var purchaseOrderDetailViewModelOriginalValues = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
        this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues.push(purchaseOrderDetailViewModelOriginalValues);
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
    }
    PurchaseOrderMaintenanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route.queryParams.subscribe(function (params) {
            _this.purchaseOrderViewModel.purchaseOrderId = +params['id'] || 0;
            if (_this.purchaseOrderViewModel.purchaseOrderId > 0) {
                _this.getPurchaseOrder();
            }
        });
    };
    PurchaseOrderMaintenanceComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    PurchaseOrderMaintenanceComponent.prototype.getPurchaseOrder = function () {
        var _this = this;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/getpurchaseorder';
        this.httpService.HttpPost(url, this.purchaseOrderViewModel)
            .subscribe(function (response) {
            _this.getPurchaseOrderSuccess(response);
        }, function (response) { return _this.getPurchaseOrderFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.getPurchaseOrderSuccess = function (response) {
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
            purchaseOrderDetailViewModel.unitPriceFormatted = element.unitPrice.toFixed(2);
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
            purchaseOrderDetailViewModelOriginalValues.unitPriceFormatted = element.unitPrice.toFixed(2);
            _this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
            _this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues.push(purchaseOrderDetailViewModelOriginalValues);
        });
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
        this.purchaseOrderViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice', 'orderQuantity', 'actions'];
        if (this.purchaseOrderViewModel.purchaseOrderStatusDescription !== 'Open') {
            this.disablePurchaseOrderButtons();
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.getPurchaseOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.getProduct = function () {
        var _this = this;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber
            = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber.trim();
        if (this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber.length === 0) {
            return;
        }
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/getproduct';
        var productViewModel = new product_viewmodel_1.ProductViewModel();
        productViewModel.productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber;
        this.httpService.HttpPost(url, productViewModel)
            .subscribe(function (response) {
            _this.getProductSuccess(response);
        }, function (response) { return _this.getProductFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.getProductSuccess = function (response) {
        this.purchaseOrderViewModel.purchaseOrderDetails[0].productDescription = response.entity.description;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].productId = response.entity.productId;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editQuantity = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editUnitPrice = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editMode = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableAddButton = false;
    };
    PurchaseOrderMaintenanceComponent.prototype.getProductFailed = function (error) {
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableAddButton = true;
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.addLineItem = function () {
        var _this = this;
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.purchaseOrderDetailId = 0;
        purchaseOrderDetailViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderId;
        purchaseOrderDetailViewModel.productId = this.purchaseOrderViewModel.purchaseOrderDetails[0].productId;
        purchaseOrderDetailViewModel.productMasterId = this.purchaseOrderViewModel.purchaseOrderDetails[0].productMasterId;
        purchaseOrderDetailViewModel.productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber;
        purchaseOrderDetailViewModel.unitPrice = parseFloat(this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPriceFormatted);
        purchaseOrderDetailViewModel.orderQuantity = parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantityFormatted, 0);
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/createpurchaseorderdetail';
        this.httpService.HttpPost(url, purchaseOrderDetailViewModel)
            .subscribe(function (response) {
            _this.addLineItemSuccess(response);
        }, function (response) { return _this.addLineItemFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.addLineItemSuccess = function (response) {
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.purchaseOrderDetailId = response.entity.purchaseOrderDetailId;
        purchaseOrderDetailViewModel.purchaseOrderId = response.entity.purchaseOrderId;
        purchaseOrderDetailViewModel.productId = response.entity.productId;
        purchaseOrderDetailViewModel.productDescription = response.entity.productDescription;
        purchaseOrderDetailViewModel.productMasterId = response.entity.productMasterId;
        purchaseOrderDetailViewModel.productNumber = response.entity.productNumber;
        purchaseOrderDetailViewModel.unitPrice = response.entity.unitPrice;
        purchaseOrderDetailViewModel.orderQuantity = response.entity.orderQuantity;
        purchaseOrderDetailViewModel.orderQuantityFormatted = response.entity.orderQuantity.toFixed(0);
        purchaseOrderDetailViewModel.unitPriceFormatted = response.entity.unitPrice.toFixed(2);
        this.purchaseOrderViewModel.orderTotal = response.entity.orderTotal;
        this.purchaseOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
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
        purchaseOrderDetailViewModelOriginalValues.unitPrice = response.entity.unitPrice;
        purchaseOrderDetailViewModelOriginalValues.orderQuantity = response.entity.orderQuantity;
        purchaseOrderDetailViewModelOriginalValues.orderQuantityFormatted = response.entity.orderQuantity.toFixed(0);
        purchaseOrderDetailViewModelOriginalValues.unitPriceFormatted = response.entity.unitPrice.toFixed(2);
        this.purchaseOrderViewModel.purchaseOrderDetails.push(purchaseOrderDetailViewModel);
        this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues.push(purchaseOrderDetailViewModelOriginalValues);
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].productNumber = ' ';
        this.purchaseOrderViewModel.purchaseOrderDetails[0].productDescription = ' ';
        this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPrice = 0.0;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].unitPriceFormatted = '';
        this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantity = 0;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].orderQuantityFormatted = '';
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editQuantity = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editUnitPrice = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editProductNumber = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].editMode = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableCancelButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableDeleteButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableEditButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[0].disableSaveButton = true;
        var message = 'Line Item successfully saved.';
        this.alertService.ShowSuccessMessage(message);
    };
    PurchaseOrderMaintenanceComponent.prototype.addLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.editLineItem = function (i) {
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editQuantity = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editUnitPrice = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editMode = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableSaveButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableCancelButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableDeleteButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableEditButton = true;
        var purchaseOrderDetailViewModelOriginalValues = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModelOriginalValues.unitPrice =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice;
        purchaseOrderDetailViewModelOriginalValues.orderQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity;
        purchaseOrderDetailViewModelOriginalValues.orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity.toFixed(0);
        purchaseOrderDetailViewModelOriginalValues.unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice.toFixed(2);
        this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i] =
            purchaseOrderDetailViewModelOriginalValues;
        this.setDisableSubmitButton();
    };
    PurchaseOrderMaintenanceComponent.prototype.updateLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.purchaseOrderDetailId = this.purchaseOrderViewModel.purchaseOrderDetails[i].purchaseOrderDetailId;
        purchaseOrderDetailViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderId;
        purchaseOrderDetailViewModel.productId = this.purchaseOrderViewModel.purchaseOrderDetails[i].productId;
        purchaseOrderDetailViewModel.productMasterId = this.purchaseOrderViewModel.purchaseOrderDetails[i].productMasterId;
        purchaseOrderDetailViewModel.productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[i].productNumber;
        purchaseOrderDetailViewModel.unitPrice = parseFloat(this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPriceFormatted);
        purchaseOrderDetailViewModel.orderQuantity = parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantityFormatted, 0);
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantity =
            parseInt(this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantityFormatted, 0);
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPrice =
            parseFloat(this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPriceFormatted);
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/updatepurchaseorderdetail';
        this.httpService.HttpPost(url, purchaseOrderDetailViewModel)
            .subscribe(function (response) {
            _this.updateLineItemSuccess(response);
        }, function (response) { return _this.updateLineItemFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.deleteLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var purchaseOrderDetailViewModel = new purchase_order_detail_viewmodel_1.PurchaseOrderDetailViewModel();
        purchaseOrderDetailViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderDetails[i].purchaseOrderId;
        purchaseOrderDetailViewModel.purchaseOrderDetailId = this.purchaseOrderViewModel.purchaseOrderDetails[i].purchaseOrderDetailId;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/deletepurchaseorderdetail';
        this.httpService.HttpPost(url, purchaseOrderDetailViewModel)
            .subscribe(function (response) {
            _this.deleteLineItemSuccess(response);
        }, function (response) { return _this.deleteLineItemFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.cancelEdit = function (i) {
        this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPrice =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].unitPrice;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].unitPriceFormatted;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantity =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].orderQuantity;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues[i].orderQuantityFormatted;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editQuantity = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editUnitPrice = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].editMode = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableSaveButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableCancelButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableDeleteButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[i].disableEditButton = false;
        this.setEnableSubmitButton();
    };
    PurchaseOrderMaintenanceComponent.prototype.updateLineItemSuccess = function (response) {
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].editQuantity = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].editUnitPrice = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].editMode = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableAddButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableSaveButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableCancelButton = true;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableDeleteButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].disableEditButton = false;
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantityFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].orderQuantity.toFixed(0);
        this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPriceFormatted =
            this.purchaseOrderViewModel.purchaseOrderDetails[this.currentLineItem].unitPrice.toFixed(2);
        this.purchaseOrderViewModel.orderTotal = response.entity.orderTotal;
        this.purchaseOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully updated.';
        this.alertService.ShowSuccessMessage(message);
    };
    PurchaseOrderMaintenanceComponent.prototype.deleteLineItemSuccess = function (response) {
        this.purchaseOrderViewModel.purchaseOrderDetails.splice(this.currentLineItem, 1);
        this.purchaseOrderViewModel.purchaseOrderDetailsOriginalValues.splice(this.currentLineItem, 1);
        this.detailDataSource.data = this.purchaseOrderViewModel.purchaseOrderDetails;
        this.purchaseOrderViewModel.orderTotal = response.entity.orderTotal;
        this.purchaseOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully deleted.';
        this.alertService.ShowSuccessMessage(message);
    };
    PurchaseOrderMaintenanceComponent.prototype.deleteLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.updateLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.setDisableSubmitButton = function () {
        this.disableSubmitButton = true;
    };
    PurchaseOrderMaintenanceComponent.prototype.setEnableSubmitButton = function () {
        this.disableSubmitButton = true;
        if (this.purchaseOrderViewModel.purchaseOrderDetails.length > 0 && this.purchaseOrderViewModel.purchaseOrderStatusDescription === 'Open') {
            this.disableSubmitButton = false;
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.submitPurchaseOrder = function () {
        var _this = this;
        var purchaseOrderViewModel = new purchase_order_viewmodel_1.PurchaseOrderViewModel();
        purchaseOrderViewModel.purchaseOrderId = this.purchaseOrderViewModel.purchaseOrderId;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/submitpurchaseorder';
        this.httpService.HttpPost(url, purchaseOrderViewModel)
            .subscribe(function (response) {
            _this.submitPurchaseOrderSuccess(response);
        }, function (response) { return _this.submitPurchaseOrderFailed(response); });
    };
    PurchaseOrderMaintenanceComponent.prototype.submitPurchaseOrderSuccess = function (response) {
        this.purchaseOrderViewModel.purchaseOrderStatusDescription = response.entity.purchaseOrderStatusDescription;
        this.setDisableSubmitButton();
        var message = 'Purchase Order Submitted.';
        this.alertService.ShowSuccessMessage(message);
    };
    PurchaseOrderMaintenanceComponent.prototype.submitPurchaseOrderFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderMaintenanceComponent.prototype.disablePurchaseOrderButtons = function () {
    };
    PurchaseOrderMaintenanceComponent.prototype.deleteLineItemDialog = function (i) {
        var _this = this;
        var productNumber = this.purchaseOrderViewModel.purchaseOrderDetails[i].productNumber;
        var orderQuantity = this.purchaseOrderViewModel.purchaseOrderDetails[i].orderQuantityFormatted;
        var unitPrice = this.purchaseOrderViewModel.purchaseOrderDetails[i].unitPriceFormatted;
        var index = i;
        var dialogRef = this.dialog.open(DeletePurchaseOrderLineItemDialogComponent, {
            width: '50%',
            data: {
                title: 'Delete Purchase Order Line Item',
                index: index,
                orderQuantity: orderQuantity,
                unitPrice: unitPrice,
                productNumber: productNumber
            }
        });
        dialogRef.afterClosed().subscribe(function (result) {
            var returnedIndex = parseInt(result, 0);
            if (returnedIndex > 0) {
                _this.deleteLineItem(returnedIndex);
            }
        });
    };
    PurchaseOrderMaintenanceComponent.prototype.submitPurchaseOrderDialog = function () {
        var _this = this;
        var purchaseOrderNumber = this.purchaseOrderViewModel.purchaseOrderNumber;
        var orderTotal = this.purchaseOrderViewModel.orderTotalFormatted;
        var dialogRef = this.dialog.open(SubmitPurchaseOrderDialogComponent, {
            width: '50%',
            data: {
                title: 'Submit Purchase Order',
                orderTotal: orderTotal,
                purchaseOrderNumber: purchaseOrderNumber
            }
        });
        dialogRef.afterClosed().subscribe(function (result) {
            var returnedResponse = parseInt(result, 0);
            if (returnedResponse > 0) {
                _this.submitPurchaseOrder();
            }
        });
    };
    PurchaseOrderMaintenanceComponent = __decorate([
        core_1.Component({
            selector: 'app-purchase-order-maintenance',
            templateUrl: './purchase-order-maintenance.component.html',
            styleUrls: ['./purchase-order-maintenance.component.css']
        }),
        __metadata("design:paramtypes", [material_1.MatDialog, router_1.Router, http_service_1.HttpService, router_1.ActivatedRoute,
            session_service_1.SessionService, alert_service_1.AlertService])
    ], PurchaseOrderMaintenanceComponent);
    return PurchaseOrderMaintenanceComponent;
}());
exports.PurchaseOrderMaintenanceComponent = PurchaseOrderMaintenanceComponent;
var DeletePurchaseOrderLineItemDialogComponent = /** @class */ (function () {
    function DeletePurchaseOrderLineItemDialogComponent(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    DeletePurchaseOrderLineItemDialogComponent.prototype.onNoClick = function () {
        this.dialogRef.close();
    };
    DeletePurchaseOrderLineItemDialogComponent = __decorate([
        core_1.Component({
            selector: 'app-delete-purchase-order-lineitem-dialog',
            templateUrl: 'delete-purchase-order-lineitem-dialog.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, Object])
    ], DeletePurchaseOrderLineItemDialogComponent);
    return DeletePurchaseOrderLineItemDialogComponent;
}());
exports.DeletePurchaseOrderLineItemDialogComponent = DeletePurchaseOrderLineItemDialogComponent;
var SubmitPurchaseOrderDialogComponent = /** @class */ (function () {
    function SubmitPurchaseOrderDialogComponent(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    SubmitPurchaseOrderDialogComponent.prototype.onNoClick = function () {
        this.dialogRef.close();
    };
    SubmitPurchaseOrderDialogComponent = __decorate([
        core_1.Component({
            selector: 'app-submit-purchase-order-dialog',
            templateUrl: 'submit-purchase-order-dialog.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, Object])
    ], SubmitPurchaseOrderDialogComponent);
    return SubmitPurchaseOrderDialogComponent;
}());
exports.SubmitPurchaseOrderDialogComponent = SubmitPurchaseOrderDialogComponent;
//# sourceMappingURL=purchase-order-maintenance.component.js.map