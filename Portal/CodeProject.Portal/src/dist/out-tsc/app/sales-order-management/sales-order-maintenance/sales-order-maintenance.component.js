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
var sales_order_detail_viewmodel_1 = require("./../view-models/sales-order-detail.viewmodel");
var product_viewmodel_1 = require("./../view-models/product.viewmodel");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var core_1 = require("@angular/core");
var sales_order_viewmodel_1 = require("../view-models/sales-order.viewmodel");
var session_service_1 = require("../../shared-components-services/session.service");
var router_1 = require("@angular/router");
var http_service_1 = require("../../shared-components-services/http.service");
var table_1 = require("@angular/material/table");
var material_1 = require("@angular/material");
var SalesOrderMaintenanceComponent = /** @class */ (function () {
    function SalesOrderMaintenanceComponent(dialog, router, httpService, route, sessionService, alertService) {
        this.dialog = dialog;
        this.router = router;
        this.httpService = httpService;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.detailDataSource = new table_1.MatTableDataSource();
        this.disableSubmitButton = true;
        this.salesOrderViewModel = new sales_order_viewmodel_1.SalesOrderViewModel();
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.productDescription = '';
        salesOrderDetailViewModel.productNumber = ' ';
        salesOrderDetailViewModel.unitPriceFormatted = '';
        salesOrderDetailViewModel.orderQuantityFormatted = '';
        salesOrderDetailViewModel.editProductNumber = true;
        salesOrderDetailViewModel.editQuantity = false;
        salesOrderDetailViewModel.editUnitPrice = false;
        salesOrderDetailViewModel.editMode = false;
        salesOrderDetailViewModel.disableAddButton = true;
        salesOrderDetailViewModel.disableCancelButton = true;
        salesOrderDetailViewModel.disableDeleteButton = true;
        salesOrderDetailViewModel.disableEditButton = true;
        salesOrderDetailViewModel.disableSaveButton = true;
        var salesOrderDetailViewModelOriginalValues = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        this.salesOrderViewModel.salesOrderDetails.push(salesOrderDetailViewModel);
        this.salesOrderViewModel.salesOrderDetailsOriginalValues.push(salesOrderDetailViewModelOriginalValues);
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
    }
    SalesOrderMaintenanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route.queryParams.subscribe(function (params) {
            _this.salesOrderViewModel.salesOrderId = +params['id'] || 0;
            if (_this.salesOrderViewModel.salesOrderId > 0) {
                _this.getSalesOrder();
            }
        });
    };
    SalesOrderMaintenanceComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    SalesOrderMaintenanceComponent.prototype.getSalesOrder = function () {
        var _this = this;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/getsalesorder';
        this.httpService.HttpPost(url, this.salesOrderViewModel)
            .subscribe(function (response) {
            _this.getSalesOrderSuccess(response);
        }, function (response) { return _this.getSalesOrderFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.getSalesOrderSuccess = function (response) {
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
            salesOrderDetailViewModel.unitPriceFormatted = element.unitPrice.toFixed(2);
            salesOrderDetailViewModel.shippedQuantityFormatted = element.shippedQuantity.toFixed(0);
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
            salesOrderDetailViewModelOriginalValues.unitPriceFormatted = element.unitPrice.toFixed(2);
            _this.salesOrderViewModel.salesOrderDetails.push(salesOrderDetailViewModel);
            _this.salesOrderViewModel.salesOrderDetailsOriginalValues.push(salesOrderDetailViewModelOriginalValues);
        });
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
        this.salesOrderViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice', 'orderQuantity', 'shippedQuantity', 'actions'];
        if (this.salesOrderViewModel.salesOrderStatusDescription !== 'Open') {
            this.disableSalesOrderButtons();
        }
    };
    SalesOrderMaintenanceComponent.prototype.getSalesOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.getProduct = function () {
        var _this = this;
        this.salesOrderViewModel.salesOrderDetails[0].productNumber
            = this.salesOrderViewModel.salesOrderDetails[0].productNumber.trim();
        if (this.salesOrderViewModel.salesOrderDetails[0].productNumber.length === 0) {
            return;
        }
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/getproduct';
        var productViewModel = new product_viewmodel_1.ProductViewModel();
        productViewModel.productNumber = this.salesOrderViewModel.salesOrderDetails[0].productNumber;
        this.httpService.HttpPost(url, productViewModel)
            .subscribe(function (response) {
            _this.getProductSuccess(response);
        }, function (response) { return _this.getProductFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.getProductSuccess = function (response) {
        this.salesOrderViewModel.salesOrderDetails[0].productDescription = response.entity.description;
        this.salesOrderViewModel.salesOrderDetails[0].productId = response.entity.productId;
        this.salesOrderViewModel.salesOrderDetails[0].editQuantity = true;
        this.salesOrderViewModel.salesOrderDetails[0].editUnitPrice = true;
        this.salesOrderViewModel.salesOrderDetails[0].editMode = true;
        this.salesOrderViewModel.salesOrderDetails[0].disableAddButton = false;
    };
    SalesOrderMaintenanceComponent.prototype.getProductFailed = function (error) {
        this.salesOrderViewModel.salesOrderDetails[0].disableAddButton = true;
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.addLineItem = function () {
        var _this = this;
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.salesOrderDetailId = 0;
        salesOrderDetailViewModel.salesOrderId = this.salesOrderViewModel.salesOrderId;
        salesOrderDetailViewModel.productId = this.salesOrderViewModel.salesOrderDetails[0].productId;
        salesOrderDetailViewModel.productMasterId = this.salesOrderViewModel.salesOrderDetails[0].productMasterId;
        salesOrderDetailViewModel.productNumber = this.salesOrderViewModel.salesOrderDetails[0].productNumber;
        salesOrderDetailViewModel.unitPrice = parseFloat(this.salesOrderViewModel.salesOrderDetails[0].unitPriceFormatted);
        salesOrderDetailViewModel.orderQuantity = parseInt(this.salesOrderViewModel.salesOrderDetails[0].orderQuantityFormatted, 0);
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/createsalesorderdetail';
        this.httpService.HttpPost(url, salesOrderDetailViewModel)
            .subscribe(function (response) {
            _this.addLineItemSuccess(response);
        }, function (response) { return _this.addLineItemFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.addLineItemSuccess = function (response) {
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.salesOrderDetailId = response.entity.salesOrderDetailId;
        salesOrderDetailViewModel.salesOrderId = response.entity.salesOrderId;
        salesOrderDetailViewModel.productId = response.entity.productId;
        salesOrderDetailViewModel.productDescription = response.entity.productDescription;
        salesOrderDetailViewModel.productMasterId = response.entity.productMasterId;
        salesOrderDetailViewModel.productNumber = response.entity.productNumber;
        salesOrderDetailViewModel.unitPrice = response.entity.unitPrice;
        salesOrderDetailViewModel.orderQuantity = response.entity.orderQuantity;
        salesOrderDetailViewModel.orderQuantityFormatted = response.entity.orderQuantity.toFixed(0);
        salesOrderDetailViewModel.unitPriceFormatted = response.entity.unitPrice.toFixed(2);
        this.salesOrderViewModel.orderTotal = response.entity.orderTotal;
        this.salesOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
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
        salesOrderDetailViewModelOriginalValues.unitPrice = response.entity.unitPrice;
        salesOrderDetailViewModelOriginalValues.orderQuantity = response.entity.orderQuantity;
        salesOrderDetailViewModelOriginalValues.orderQuantityFormatted = response.entity.orderQuantity.toFixed(0);
        salesOrderDetailViewModelOriginalValues.unitPriceFormatted = response.entity.unitPrice.toFixed(2);
        this.salesOrderViewModel.salesOrderDetails.push(salesOrderDetailViewModel);
        this.salesOrderViewModel.salesOrderDetailsOriginalValues.push(salesOrderDetailViewModelOriginalValues);
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
        this.salesOrderViewModel.salesOrderDetails[0].productNumber = ' ';
        this.salesOrderViewModel.salesOrderDetails[0].productDescription = ' ';
        this.salesOrderViewModel.salesOrderDetails[0].unitPrice = 0.0;
        this.salesOrderViewModel.salesOrderDetails[0].unitPriceFormatted = '';
        this.salesOrderViewModel.salesOrderDetails[0].orderQuantity = 0;
        this.salesOrderViewModel.salesOrderDetails[0].orderQuantityFormatted = '';
        this.salesOrderViewModel.salesOrderDetails[0].editQuantity = false;
        this.salesOrderViewModel.salesOrderDetails[0].editUnitPrice = false;
        this.salesOrderViewModel.salesOrderDetails[0].editProductNumber = true;
        this.salesOrderViewModel.salesOrderDetails[0].editMode = false;
        this.salesOrderViewModel.salesOrderDetails[0].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[0].disableCancelButton = true;
        this.salesOrderViewModel.salesOrderDetails[0].disableDeleteButton = true;
        this.salesOrderViewModel.salesOrderDetails[0].disableEditButton = true;
        this.salesOrderViewModel.salesOrderDetails[0].disableSaveButton = true;
        var message = 'Line Item successfully saved.';
        this.alertService.ShowSuccessMessage(message);
    };
    SalesOrderMaintenanceComponent.prototype.addLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.editLineItem = function (i) {
        this.salesOrderViewModel.salesOrderDetails[i].editQuantity = true;
        this.salesOrderViewModel.salesOrderDetails[i].editUnitPrice = true;
        this.salesOrderViewModel.salesOrderDetails[i].editMode = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableSaveButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableCancelButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableDeleteButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableEditButton = true;
        var salesOrderDetailViewModelOriginalValues = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModelOriginalValues.unitPrice =
            this.salesOrderViewModel.salesOrderDetails[i].unitPrice;
        salesOrderDetailViewModelOriginalValues.orderQuantity =
            this.salesOrderViewModel.salesOrderDetails[i].orderQuantity;
        salesOrderDetailViewModelOriginalValues.orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[i].orderQuantity.toFixed(0);
        salesOrderDetailViewModelOriginalValues.unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetails[i].unitPrice.toFixed(2);
        this.salesOrderViewModel.salesOrderDetailsOriginalValues[i] =
            salesOrderDetailViewModelOriginalValues;
        this.setDisableSubmitButton();
    };
    SalesOrderMaintenanceComponent.prototype.updateLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.salesOrderDetailId = this.salesOrderViewModel.salesOrderDetails[i].salesOrderDetailId;
        salesOrderDetailViewModel.salesOrderId = this.salesOrderViewModel.salesOrderId;
        salesOrderDetailViewModel.productId = this.salesOrderViewModel.salesOrderDetails[i].productId;
        salesOrderDetailViewModel.productMasterId = this.salesOrderViewModel.salesOrderDetails[i].productMasterId;
        salesOrderDetailViewModel.productNumber = this.salesOrderViewModel.salesOrderDetails[i].productNumber;
        salesOrderDetailViewModel.unitPrice = parseFloat(this.salesOrderViewModel.salesOrderDetails[i].unitPriceFormatted);
        salesOrderDetailViewModel.orderQuantity = parseInt(this.salesOrderViewModel.salesOrderDetails[i].orderQuantityFormatted, 0);
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantity =
            parseInt(this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantityFormatted, 0);
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPrice =
            parseFloat(this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPriceFormatted);
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/updatesalesorderdetail';
        this.httpService.HttpPost(url, salesOrderDetailViewModel)
            .subscribe(function (response) {
            _this.updateLineItemSuccess(response);
        }, function (response) { return _this.updateLineItemFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.deleteLineItem = function (i) {
        var _this = this;
        this.currentLineItem = i;
        var salesOrderDetailViewModel = new sales_order_detail_viewmodel_1.SalesOrderDetailViewModel();
        salesOrderDetailViewModel.salesOrderId = this.salesOrderViewModel.salesOrderDetails[i].salesOrderId;
        salesOrderDetailViewModel.salesOrderDetailId = this.salesOrderViewModel.salesOrderDetails[i].salesOrderDetailId;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/deletesalesorderdetail';
        this.httpService.HttpPost(url, salesOrderDetailViewModel)
            .subscribe(function (response) {
            _this.deleteLineItemSuccess(response);
        }, function (response) { return _this.deleteLineItemFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.cancelEdit = function (i) {
        this.salesOrderViewModel.salesOrderDetails[i].unitPrice =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].unitPrice;
        this.salesOrderViewModel.salesOrderDetails[i].unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].unitPriceFormatted;
        this.salesOrderViewModel.salesOrderDetails[i].orderQuantity =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].orderQuantity;
        this.salesOrderViewModel.salesOrderDetails[i].orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetailsOriginalValues[i].orderQuantityFormatted;
        this.salesOrderViewModel.salesOrderDetails[i].editQuantity = false;
        this.salesOrderViewModel.salesOrderDetails[i].editUnitPrice = false;
        this.salesOrderViewModel.salesOrderDetails[i].editMode = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableSaveButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableCancelButton = true;
        this.salesOrderViewModel.salesOrderDetails[i].disableDeleteButton = false;
        this.salesOrderViewModel.salesOrderDetails[i].disableEditButton = false;
        this.setEnableSubmitButton();
    };
    SalesOrderMaintenanceComponent.prototype.updateLineItemSuccess = function (response) {
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].editQuantity = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].editUnitPrice = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].editMode = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableAddButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableSaveButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableCancelButton = true;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableDeleteButton = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].disableEditButton = false;
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantityFormatted =
            this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].orderQuantity.toFixed(0);
        this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPriceFormatted =
            this.salesOrderViewModel.salesOrderDetails[this.currentLineItem].unitPrice.toFixed(2);
        this.salesOrderViewModel.orderTotal = response.entity.orderTotal;
        this.salesOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully updated.';
        this.alertService.ShowSuccessMessage(message);
    };
    SalesOrderMaintenanceComponent.prototype.deleteLineItemSuccess = function (response) {
        this.salesOrderViewModel.salesOrderDetails.splice(this.currentLineItem, 1);
        this.salesOrderViewModel.salesOrderDetailsOriginalValues.splice(this.currentLineItem, 1);
        this.detailDataSource.data = this.salesOrderViewModel.salesOrderDetails;
        this.salesOrderViewModel.orderTotal = response.entity.orderTotal;
        this.salesOrderViewModel.orderTotalFormatted = response.entity.orderTotal.toFixed(2);
        this.setEnableSubmitButton();
        var message = 'Line Item successfully deleted.';
        this.alertService.ShowSuccessMessage(message);
    };
    SalesOrderMaintenanceComponent.prototype.deleteLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.updateLineItemFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.setDisableSubmitButton = function () {
        this.disableSubmitButton = true;
    };
    SalesOrderMaintenanceComponent.prototype.setEnableSubmitButton = function () {
        this.disableSubmitButton = true;
        if (this.salesOrderViewModel.salesOrderDetails.length > 0 && this.salesOrderViewModel.salesOrderStatusDescription === 'Open') {
            this.disableSubmitButton = false;
        }
    };
    SalesOrderMaintenanceComponent.prototype.submitSalesOrder = function () {
        var _this = this;
        var salesOrderViewModel = new sales_order_viewmodel_1.SalesOrderViewModel();
        salesOrderViewModel.salesOrderId = this.salesOrderViewModel.salesOrderId;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/submitsalesorder';
        this.httpService.HttpPost(url, salesOrderViewModel)
            .subscribe(function (response) {
            _this.submitSalesOrderSuccess(response);
        }, function (response) { return _this.submitSalesOrderFailed(response); });
    };
    SalesOrderMaintenanceComponent.prototype.submitSalesOrderSuccess = function (response) {
        this.salesOrderViewModel.salesOrderStatusDescription = response.entity.salesOrderStatusDescription;
        this.setDisableSubmitButton();
        var message = 'Sales Order Submitted.';
        this.alertService.ShowSuccessMessage(message);
    };
    SalesOrderMaintenanceComponent.prototype.submitSalesOrderFailed = function (error) {
        var errorResponse = error.error;
        console.log(error.status + ' error status');
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderMaintenanceComponent.prototype.disableSalesOrderButtons = function () {
    };
    SalesOrderMaintenanceComponent.prototype.deleteLineItemDialog = function (i) {
        var _this = this;
        var productNumber = this.salesOrderViewModel.salesOrderDetails[i].productNumber;
        var orderQuantity = this.salesOrderViewModel.salesOrderDetails[i].orderQuantityFormatted;
        var unitPrice = this.salesOrderViewModel.salesOrderDetails[i].unitPriceFormatted;
        var index = i;
        var dialogRef = this.dialog.open(DeleteSalesOrderLineItemDialogComponent, {
            width: '50%',
            data: {
                title: 'Delete Sales Order Line Item',
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
    SalesOrderMaintenanceComponent.prototype.submitSalesOrderDialog = function () {
        var _this = this;
        var salesOrderNumber = this.salesOrderViewModel.salesOrderNumber;
        var orderTotal = this.salesOrderViewModel.orderTotalFormatted;
        var dialogRef = this.dialog.open(SubmitSalesOrderDialogComponent, {
            width: '50%',
            data: {
                title: 'Submit Sales Order',
                orderTotal: orderTotal,
                salesOrderNumber: salesOrderNumber
            }
        });
        dialogRef.afterClosed().subscribe(function (result) {
            var returnedResponse = parseInt(result, 0);
            if (returnedResponse > 0) {
                _this.submitSalesOrder();
            }
        });
    };
    SalesOrderMaintenanceComponent = __decorate([
        core_1.Component({
            selector: 'app-sales-order-maintenance',
            templateUrl: './sales-order-maintenance.component.html',
            styleUrls: ['./sales-order-maintenance.component.css']
        }),
        __metadata("design:paramtypes", [material_1.MatDialog, router_1.Router, http_service_1.HttpService, router_1.ActivatedRoute,
            session_service_1.SessionService, alert_service_1.AlertService])
    ], SalesOrderMaintenanceComponent);
    return SalesOrderMaintenanceComponent;
}());
exports.SalesOrderMaintenanceComponent = SalesOrderMaintenanceComponent;
var DeleteSalesOrderLineItemDialogComponent = /** @class */ (function () {
    function DeleteSalesOrderLineItemDialogComponent(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    DeleteSalesOrderLineItemDialogComponent.prototype.onNoClick = function () {
        this.dialogRef.close();
    };
    DeleteSalesOrderLineItemDialogComponent = __decorate([
        core_1.Component({
            selector: 'app-delete-sales-order-lineitem-dialog',
            templateUrl: 'delete-sales-order-lineitem-dialog.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, Object])
    ], DeleteSalesOrderLineItemDialogComponent);
    return DeleteSalesOrderLineItemDialogComponent;
}());
exports.DeleteSalesOrderLineItemDialogComponent = DeleteSalesOrderLineItemDialogComponent;
var SubmitSalesOrderDialogComponent = /** @class */ (function () {
    function SubmitSalesOrderDialogComponent(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    SubmitSalesOrderDialogComponent.prototype.onNoClick = function () {
        this.dialogRef.close();
    };
    SubmitSalesOrderDialogComponent = __decorate([
        core_1.Component({
            selector: 'app-submit-sales-order-dialog',
            templateUrl: 'submit-sales-order-dialog.html',
        }),
        __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
        __metadata("design:paramtypes", [material_1.MatDialogRef, Object])
    ], SubmitSalesOrderDialogComponent);
    return SubmitSalesOrderDialogComponent;
}());
exports.SubmitSalesOrderDialogComponent = SubmitSalesOrderDialogComponent;
//# sourceMappingURL=sales-order-maintenance.component.js.map