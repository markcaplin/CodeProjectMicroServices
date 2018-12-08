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
var core_1 = require("@angular/core");
var session_service_1 = require("../../shared-components-services/session.service");
var product_inquiry_viewmodel_1 = require(".//../view-models/product-inquiry.viewmodel");
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var operators_1 = require("rxjs/operators");
var operators_2 = require("rxjs/operators");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var ProductInquiryComponent = /** @class */ (function () {
    function ProductInquiryComponent(router, sessionService, httpService, alertService) {
        this.router = router;
        this.sessionService = sessionService;
        this.httpService = httpService;
        this.alertService = alertService;
        this.selectedRowIndex = -1;
        this.sessionService.moduleLoadedEvent.emit();
        this.productInquiryViewModel = new product_inquiry_viewmodel_1.ProductInquiryViewModel();
        this.productInquiryViewModel.pageSize = 20;
        this.productInquiryViewModel.displayedColumns = ['productNumber', 'description', 'unitPrice'];
        this.productInquiryViewModel.pageSizeOptions = [5, 10, 25, 100];
        this.productInquiryViewModel.products = new Array();
        this.lastSearchValue = '';
        this.initializeSearch();
    }
    ProductInquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchForm.valueChanges.pipe(operators_1.debounceTime(1000), operators_2.distinctUntilChanged()).subscribe(function (changes) {
            _this.productInquiryViewModel.currentPageNumber = 1;
            _this.productInquiryViewModel.currentPageIndex = 0;
            if (_this.lastSearchValue !== _this.productInquiryViewModel.productNumber) {
                _this.executeSearch();
            }
        });
        this.executeSearch();
    };
    ProductInquiryComponent.prototype.initializeSearch = function () {
        this.productInquiryViewModel.productNumber = '';
        this.productInquiryViewModel.currentPageNumber = 1;
        this.productInquiryViewModel.currentPageIndex = 0;
        this.productInquiryViewModel.totalPages = 0;
        this.productInquiryViewModel.totalProducts = 0;
        this.productInquiryViewModel.sortDirection = 'ASC';
        this.productInquiryViewModel.sortExpression = 'ProductNumber';
        this.productInquiryViewModel.products = new Array();
    };
    ProductInquiryComponent.prototype.executeSearch = function () {
        var _this = this;
        this.lastSearchValue = this.productInquiryViewModel.productNumber;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/productinquiry';
        this.httpService.HttpPost(url, this.productInquiryViewModel).
            subscribe(function (response) {
            _this.productInquirySuccess(response);
        }, function (response) { return _this.productInquiryFailed(response); });
    };
    ProductInquiryComponent.prototype.productInquirySuccess = function (response) {
        this.productInquiryViewModel.products = response.entity;
        this.productInquiryViewModel.totalProducts = response.totalRows;
        this.productInquiryViewModel.totalPages = response.totalPages;
    };
    ProductInquiryComponent.prototype.productInquiryFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    ProductInquiryComponent.prototype.onPaginateChange = function (event) {
        this.productInquiryViewModel.currentPageNumber = event.pageIndex + 1;
        this.productInquiryViewModel.currentPageIndex = event.pageIndex;
        this.productInquiryViewModel.pageSize = event.pageSize;
        this.executeSearch();
    };
    ProductInquiryComponent.prototype.sortData = function (sort) {
        this.productInquiryViewModel.currentPageNumber = 1;
        this.productInquiryViewModel.currentPageIndex = 0;
        this.productInquiryViewModel.sortDirection = sort.direction;
        this.productInquiryViewModel.sortExpression = sort.active;
        this.executeSearch();
    };
    ProductInquiryComponent.prototype.resetSearch = function () {
        this.lastSearchValue = '';
        this.productInquiryViewModel.productNumber = '';
        this.initializeSearch();
        this.executeSearch();
    };
    ProductInquiryComponent.prototype.selectProduct = function (row) {
        var productId = this.productInquiryViewModel.products[row].productId;
        this.router.navigate(['/inventorymanagement/product-maintenance'], { queryParams: { id: productId } });
    };
    __decorate([
        core_1.ViewChild('form'),
        __metadata("design:type", forms_1.NgForm)
    ], ProductInquiryComponent.prototype, "searchForm", void 0);
    ProductInquiryComponent = __decorate([
        core_1.Component({
            selector: 'app-product-inquiry',
            templateUrl: './product-inquiry.component.html',
            styleUrls: ['./product-inquiry.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router,
            session_service_1.SessionService,
            http_service_1.HttpService,
            alert_service_1.AlertService])
    ], ProductInquiryComponent);
    return ProductInquiryComponent;
}());
exports.ProductInquiryComponent = ProductInquiryComponent;
//# sourceMappingURL=product-inquiry.component.js.map