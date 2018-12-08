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
var product_viewmodel_1 = require("../view-models/product.viewmodel");
var router_1 = require("@angular/router");
var ProductMaintenanceComponent = /** @class */ (function () {
    function ProductMaintenanceComponent(sessionService, router, route, alertService, httpService) {
        this.sessionService = sessionService;
        this.router = router;
        this.route = route;
        this.alertService = alertService;
        this.httpService = httpService;
        this.productViewModel = new product_viewmodel_1.ProductViewModel();
        this.productViewModel.productNumber = '';
        this.productViewModel.description = '';
        this.productViewModel.binLocation = '';
        this.productViewModel.productId = 0;
    }
    ProductMaintenanceComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerSubscription = this.route.queryParams.subscribe(function (params) {
            _this.productViewModel.productId = +params['id'] || 0;
            if (_this.productViewModel.productId > 0) {
                _this.getProduct();
            }
        });
    };
    ProductMaintenanceComponent.prototype.getProduct = function () {
        var _this = this;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/getproduct';
        this.httpService.HttpPost(url, this.productViewModel)
            .subscribe(function (response) {
            _this.getProductSuccess(response);
        }, function (response) { return _this.getProductFailed(response); });
    };
    ProductMaintenanceComponent.prototype.getProductSuccess = function (response) {
        var productViewModel = response.entity;
        this.productViewModel = productViewModel;
    };
    ProductMaintenanceComponent.prototype.getProductFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    ProductMaintenanceComponent.prototype.createOrUpdateProduct = function () {
        var _this = this;
        var product = new product_viewmodel_1.ProductViewModel();
        product = this.productViewModel;
        var url = '';
        if (product.productId === 0) {
            url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/createproduct';
        }
        else {
            url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/updateproduct';
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
        core_1.Component({
            selector: 'app-product-maintenance',
            templateUrl: './product-maintenance.component.html',
            styleUrls: ['./product-maintenance.component.css']
        }),
        __metadata("design:paramtypes", [session_service_1.SessionService, router_1.Router, router_1.ActivatedRoute,
            alert_service_1.AlertService, http_service_1.HttpService])
    ], ProductMaintenanceComponent);
    return ProductMaintenanceComponent;
}());
exports.ProductMaintenanceComponent = ProductMaintenanceComponent;
//# sourceMappingURL=product-maintenance.component.js.map