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
var customer_viewmodel_1 = require("../view-models/customer.viewmodel");
var router_1 = require("@angular/router");
var sales_order_viewmodel_1 = require("../view-models/sales-order.viewmodel");
var CustomerMaintenanceComponent = /** @class */ (function () {
    function CustomerMaintenanceComponent(router, route, sessionService, alertService, httpService) {
        this.router = router;
        this.route = route;
        this.sessionService = sessionService;
        this.alertService = alertService;
        this.httpService = httpService;
        this.customerViewModel = new customer_viewmodel_1.CustomerViewModel();
        this.customerViewModel.customerName = '';
        this.customerViewModel.addressLine1 = '';
        this.customerViewModel.addressLine2 = '';
        this.customerViewModel.city = '';
        this.customerViewModel.region = '';
        this.customerViewModel.postalCode = '';
        this.customerViewModel.customerId = 0;
        this.createMode = true;
        this.readonlyMode = false;
    }
    CustomerMaintenanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route
            .queryParams
            .subscribe(function (params) {
            _this.customerViewModel.customerId = +params['id'] || 0;
            if (_this.customerViewModel.customerId > 0) {
                _this.createMode = false;
                _this.readonlyMode = true;
                _this.getCustomerInformation();
            }
        });
    };
    CustomerMaintenanceComponent.prototype.ngOnDestroy = function () {
        this.routerSubscription.unsubscribe();
    };
    CustomerMaintenanceComponent.prototype.getCustomerInformation = function () {
        var _this = this;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'customer/getcustomer';
        this.httpService.HttpPost(url, this.customerViewModel).subscribe(function (response) {
            _this.getCustomerSuccess(response);
        }, function (response) { return _this.getCustomerFailed(response); });
    };
    CustomerMaintenanceComponent.prototype.initializeCustomer = function () {
        this.customerViewModel = new customer_viewmodel_1.CustomerViewModel();
        this.customerViewModel.customerName = '';
        this.customerViewModel.addressLine1 = '';
        this.customerViewModel.addressLine2 = '';
        this.customerViewModel.city = '';
        this.customerViewModel.region = '';
        this.customerViewModel.postalCode = '';
        this.customerViewModel.customerId = 0;
    };
    CustomerMaintenanceComponent.prototype.createNewCustomer = function () {
        this.readonlyMode = false;
        this.initializeCustomer();
    };
    CustomerMaintenanceComponent.prototype.getCustomerSuccess = function (response) {
        this.customerViewModel = response.entity;
    };
    CustomerMaintenanceComponent.prototype.getCustomerFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    CustomerMaintenanceComponent.prototype.createOrUpdateCustomer = function () {
        var _this = this;
        var customer = new customer_viewmodel_1.CustomerViewModel();
        customer = this.customerViewModel;
        var url = '';
        if (customer.customerId === 0) {
            url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'customer/createcustomer';
        }
        else {
            url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'customer/updatecustomer';
        }
        this.httpService.HttpPost(url, customer).subscribe(function (response) {
            _this.createOrUpdateCustomerSuccess(response);
        }, function (response) { return _this.createOrUpdateCustomerFailed(response); });
    };
    CustomerMaintenanceComponent.prototype.createOrUpdateCustomerSuccess = function (response) {
        var customerViewModel = response.entity;
        this.customerViewModel.customerId = customerViewModel.customerId;
        var message = 'Customer successfully saved.';
        this.alertService.ShowSuccessMessage(message);
        this.createMode = false;
        this.readonlyMode = true;
    };
    CustomerMaintenanceComponent.prototype.createOrUpdateCustomerFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    CustomerMaintenanceComponent.prototype.editCustomer = function () {
        this.createMode = false;
        this.readonlyMode = false;
    };
    CustomerMaintenanceComponent.prototype.createSalesOrder = function () {
        var _this = this;
        var salesOrderViewModel = new sales_order_viewmodel_1.SalesOrderViewModel();
        salesOrderViewModel.customerId = this.customerViewModel.customerId;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'salesorder/createsalesorder';
        this.httpService.HttpPost(url, salesOrderViewModel)
            .subscribe(function (response) {
            _this.createSalesOrderSuccess(response);
        }, function (response) { return _this.createSalesOrderFailed(response); });
    };
    CustomerMaintenanceComponent.prototype.createSalesOrderSuccess = function (response) {
        var salesOrderId = response.entity.salesOrderId;
        this.router.navigate(['/salesordermanagement/sales-order-maintenance'], { queryParams: { id: salesOrderId } });
    };
    CustomerMaintenanceComponent.prototype.createSalesOrderFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    CustomerMaintenanceComponent = __decorate([
        core_1.Component({
            selector: 'app-customer-maintenance',
            templateUrl: './customer-maintenance.component.html',
            styleUrls: ['./customer-maintenance.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router, router_1.ActivatedRoute, session_service_1.SessionService,
            alert_service_1.AlertService, http_service_1.HttpService])
    ], CustomerMaintenanceComponent);
    return CustomerMaintenanceComponent;
}());
exports.CustomerMaintenanceComponent = CustomerMaintenanceComponent;
//# sourceMappingURL=customer-maintenance.component.js.map