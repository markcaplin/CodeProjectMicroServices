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
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var session_service_1 = require("./../../shared-components-services/session.service");
var core_1 = require("@angular/core");
var supplier_viewmodel_1 = require("../view-models/supplier.viewmodel");
var router_1 = require("@angular/router");
var purchase_order_viewmodel_1 = require("../view-models/purchase-order.viewmodel");
var SupplierMaintenanceComponent = /** @class */ (function () {
    function SupplierMaintenanceComponent(router, route, sessionService, alertService, httpService) {
        this.router = router;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.httpService = httpService;
        this.supplierViewModel = new supplier_viewmodel_1.SupplierViewModel();
        this.supplierViewModel.supplierName = '';
        this.supplierViewModel.addressLine1 = '';
        this.supplierViewModel.addressLine2 = '';
        this.supplierViewModel.city = '';
        this.supplierViewModel.region = '';
        this.supplierViewModel.postalCode = '';
        this.supplierViewModel.supplierId = 0;
        this.createMode = true;
        this.readonlyMode = false;
    }
    SupplierMaintenanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route
            .queryParams
            .subscribe(function (params) {
            _this.supplierViewModel.supplierId = +params['id'] || 0;
            if (_this.supplierViewModel.supplierId > 0) {
                _this.createMode = false;
                _this.readonlyMode = true;
                _this.getSupplierInformation();
            }
        });
    };
    SupplierMaintenanceComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    SupplierMaintenanceComponent.prototype.getSupplierInformation = function () {
        var _this = this;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'supplier/getsupplier';
        this.httpService.HttpPost(url, this.supplierViewModel).subscribe(function (response) {
            _this.getSupplierSuccess(response);
        }, function (response) { return _this.getSupplierFailed(response); });
    };
    SupplierMaintenanceComponent.prototype.initializeSupplier = function () {
        this.supplierViewModel = new supplier_viewmodel_1.SupplierViewModel();
        this.supplierViewModel.supplierName = '';
        this.supplierViewModel.addressLine1 = '';
        this.supplierViewModel.addressLine2 = '';
        this.supplierViewModel.city = '';
        this.supplierViewModel.region = '';
        this.supplierViewModel.postalCode = '';
        this.supplierViewModel.supplierId = 0;
    };
    SupplierMaintenanceComponent.prototype.createNewSupplier = function () {
        this.readonlyMode = false;
        this.initializeSupplier();
    };
    SupplierMaintenanceComponent.prototype.getSupplierSuccess = function (response) {
        this.supplierViewModel = response.entity;
    };
    SupplierMaintenanceComponent.prototype.getSupplierFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SupplierMaintenanceComponent.prototype.createOrUpdateSupplier = function () {
        var _this = this;
        var supplier = new supplier_viewmodel_1.SupplierViewModel();
        supplier = this.supplierViewModel;
        var url = '';
        if (supplier.supplierId === 0) {
            url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'supplier/createsupplier';
        }
        else {
            url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'supplier/updatesupplier';
        }
        this.httpService.HttpPost(url, supplier).subscribe(function (response) {
            _this.createOrUpdateSupplierSuccess(response);
        }, function (response) { return _this.createOrUpdateSupplierFailed(response); });
    };
    SupplierMaintenanceComponent.prototype.createOrUpdateSupplierSuccess = function (response) {
        var supplierViewModel = response.entity;
        this.supplierViewModel.supplierId = supplierViewModel.supplierId;
        var message = 'Supplier successfully saved.';
        this.alertService.ShowSuccessMessage(message);
        this.createMode = false;
        this.readonlyMode = true;
    };
    SupplierMaintenanceComponent.prototype.createOrUpdateSupplierFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SupplierMaintenanceComponent.prototype.editSupplier = function () {
        this.createMode = false;
        this.readonlyMode = false;
    };
    SupplierMaintenanceComponent.prototype.createPurchaseOrder = function () {
        var _this = this;
        var purchaseOrderViewModel = new purchase_order_viewmodel_1.PurchaseOrderViewModel();
        purchaseOrderViewModel.supplierId = this.supplierViewModel.supplierId;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'purchaseorder/createpurchaseorder';
        this.httpService.HttpPost(url, purchaseOrderViewModel)
            .subscribe(function (response) {
            _this.createPurchaseOrderSuccess(response);
        }, function (response) { return _this.createPurchaseOrderFailed(response); });
    };
    SupplierMaintenanceComponent.prototype.createPurchaseOrderSuccess = function (response) {
        var purchaseOrderId = response.entity.purchaseOrderId;
        this.router.navigate(['/purchaseordermanagement/purchase-order-maintenance'], { queryParams: { id: purchaseOrderId } });
    };
    SupplierMaintenanceComponent.prototype.createPurchaseOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SupplierMaintenanceComponent = __decorate([
        core_1.Component({
            selector: 'app-supplier-maintenance',
            templateUrl: './supplier-maintenance.component.html',
            styleUrls: ['./supplier-maintenance.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router, router_1.ActivatedRoute, session_service_1.SessionService,
            alert_service_1.AlertService, http_service_1.HttpService])
    ], SupplierMaintenanceComponent);
    return SupplierMaintenanceComponent;
}());
exports.SupplierMaintenanceComponent = SupplierMaintenanceComponent;
//# sourceMappingURL=supplier-maintenance.component.js.map