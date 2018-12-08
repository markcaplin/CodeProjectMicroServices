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
var sales_order_inquiry_viewmodel_1 = require("../view-models/sales-order-inquiry.viewmodel");
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var operators_1 = require("rxjs/operators");
var operators_2 = require("rxjs/operators");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var SalesOrderInquiryComponent = /** @class */ (function () {
    function SalesOrderInquiryComponent(router, sessionService, httpService, alertService) {
        this.router = router;
        this.sessionService = sessionService;
        this.httpService = httpService;
        this.alertService = alertService;
        this.selectedRowIndex = -1;
        this.sessionService.moduleLoadedEvent.emit();
        this.salesOrderInquiryViewModel = new sales_order_inquiry_viewmodel_1.SalesOrderInquiryViewModel();
        this.salesOrderInquiryViewModel.pageSize = 20;
        this.salesOrderInquiryViewModel.displayedColumns = ['salesOrderNumber', 'customerName',
            'city', 'region', 'orderTotal', 'orderDate', 'salesOrderStatusDescription'];
        this.salesOrderInquiryViewModel.pageSizeOptions = [5, 10, 25, 100];
        this.initializeSearch();
    }
    SalesOrderInquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchForm.valueChanges.pipe(operators_1.debounceTime(1000), operators_2.distinctUntilChanged()).subscribe(function (changes) {
            _this.salesOrderInquiryViewModel.currentPageNumber = 1;
            _this.salesOrderInquiryViewModel.currentPageIndex = 0;
            if (_this.lastSearchValue !== _this.salesOrderInquiryViewModel.customerName) {
                _this.executeSearch();
            }
        });
        this.executeSearch();
    };
    SalesOrderInquiryComponent.prototype.initializeSearch = function () {
        this.salesOrderInquiryViewModel.customerName = '';
        this.salesOrderInquiryViewModel.currentPageNumber = 1;
        this.salesOrderInquiryViewModel.currentPageIndex = 0;
        this.salesOrderInquiryViewModel.totalPages = 0;
        this.salesOrderInquiryViewModel.totalSalesOrders = 0;
        this.salesOrderInquiryViewModel.sortDirection = 'DESC';
        this.salesOrderInquiryViewModel.sortExpression = 'SalesOrderNumber';
        this.salesOrderInquiryViewModel.salesOrders = new Array();
    };
    SalesOrderInquiryComponent.prototype.executeSearch = function () {
        var _this = this;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'salesorder/salesorderinquiry';
        this.httpService.HttpPost(url, this.salesOrderInquiryViewModel).
            subscribe(function (response) {
            _this.salesOrderInquirySuccess(response);
        }, function (response) { return _this.salesOrderInquiryFailed(response); });
    };
    SalesOrderInquiryComponent.prototype.salesOrderInquirySuccess = function (response) {
        response.entity.forEach(function (element) {
            var orderDate = element.dateCreated.toString().substring(0, 10);
            element.orderDate = orderDate.substring(5, 7) + '/' + orderDate.substring(8, 10) + '/' + orderDate.substring(0, 4);
        });
        this.salesOrderInquiryViewModel.salesOrders = response.entity;
        this.salesOrderInquiryViewModel.totalSalesOrders = response.totalRows;
        this.salesOrderInquiryViewModel.totalPages = response.totalPages;
    };
    SalesOrderInquiryComponent.prototype.salesOrderInquiryFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SalesOrderInquiryComponent.prototype.onPaginateChange = function (event) {
        this.salesOrderInquiryViewModel.currentPageNumber = event.pageIndex + 1;
        this.salesOrderInquiryViewModel.currentPageIndex = event.pageIndex;
        this.salesOrderInquiryViewModel.pageSize = event.pageSize;
        this.executeSearch();
    };
    SalesOrderInquiryComponent.prototype.sortData = function (sort) {
        this.salesOrderInquiryViewModel.currentPageNumber = 1;
        this.salesOrderInquiryViewModel.currentPageIndex = 0;
        this.salesOrderInquiryViewModel.sortDirection = sort.direction;
        this.salesOrderInquiryViewModel.sortExpression = sort.active;
        this.executeSearch();
    };
    SalesOrderInquiryComponent.prototype.resetSearch = function () {
        this.lastSearchValue = '';
        this.salesOrderInquiryViewModel.customerName = '';
        this.initializeSearch();
        this.executeSearch();
    };
    SalesOrderInquiryComponent.prototype.selectSalesOrder = function (row) {
        var salesOrderId = this.salesOrderInquiryViewModel.salesOrders[row].salesOrderId;
        this.router.navigate(['/inventorymanagement/sales-order-shipping'], { queryParams: { id: salesOrderId } });
    };
    __decorate([
        core_1.ViewChild('form'),
        __metadata("design:type", forms_1.NgForm)
    ], SalesOrderInquiryComponent.prototype, "searchForm", void 0);
    SalesOrderInquiryComponent = __decorate([
        core_1.Component({
            selector: 'app-sales-order-inquiry',
            templateUrl: './sales-order-inquiry.component.html',
            styleUrls: ['./sales-order-inquiry.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router,
            session_service_1.SessionService,
            http_service_1.HttpService,
            alert_service_1.AlertService])
    ], SalesOrderInquiryComponent);
    return SalesOrderInquiryComponent;
}());
exports.SalesOrderInquiryComponent = SalesOrderInquiryComponent;
//# sourceMappingURL=sales-order-inquiry.component.js.map