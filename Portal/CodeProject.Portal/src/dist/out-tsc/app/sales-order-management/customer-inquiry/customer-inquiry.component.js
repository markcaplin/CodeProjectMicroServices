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
var customer_inquiry_viewmodel_1 = require("../view-models/customer-inquiry.viewmodel");
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var operators_1 = require("rxjs/operators");
var operators_2 = require("rxjs/operators");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var CustomerInquiryComponent = /** @class */ (function () {
    function CustomerInquiryComponent(router, sessionService, httpService, alertService) {
        this.router = router;
        this.sessionService = sessionService;
        this.httpService = httpService;
        this.alertService = alertService;
        this.selectedRowIndex = -1;
        this.sessionService.moduleLoadedEvent.emit();
        this.customerInquiryViewModel = new customer_inquiry_viewmodel_1.CustomerInquiryViewModel();
        this.customerInquiryViewModel.pageSize = 20;
        this.customerInquiryViewModel.displayedColumns = ['customerName', 'addressLine1', 'addressLine2', 'city', 'region', 'postalCode'];
        this.customerInquiryViewModel.pageSizeOptions = [5, 10, 25, 100];
        this.initializeSearch();
    }
    CustomerInquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchForm.valueChanges.pipe(operators_1.debounceTime(1000), operators_2.distinctUntilChanged()).subscribe(function (changes) {
            _this.customerInquiryViewModel.currentPageNumber = 1;
            _this.customerInquiryViewModel.currentPageIndex = 0;
            if (_this.lastSearchValue !== _this.customerInquiryViewModel.customerName) {
                _this.executeSearch();
            }
        });
        this.executeSearch();
    };
    CustomerInquiryComponent.prototype.initializeSearch = function () {
        this.customerInquiryViewModel.customerName = '';
        this.customerInquiryViewModel.currentPageNumber = 1;
        this.customerInquiryViewModel.currentPageIndex = 0;
        this.customerInquiryViewModel.totalPages = 0;
        this.customerInquiryViewModel.totalCustomers = 0;
        this.customerInquiryViewModel.sortDirection = '';
        this.customerInquiryViewModel.sortExpression = '';
        this.customerInquiryViewModel.customers = new Array();
    };
    CustomerInquiryComponent.prototype.executeSearch = function () {
        var _this = this;
        var url = this.sessionService.appSettings.salesOrderManagementWebApiUrl + 'customer/customerinquiry';
        this.httpService.HttpPost(url, this.customerInquiryViewModel).
            subscribe(function (response) {
            _this.customerInquirySuccess(response);
        }, function (response) { return _this.customerInquiryFailed(response); });
    };
    CustomerInquiryComponent.prototype.customerInquirySuccess = function (response) {
        this.customerInquiryViewModel.customers = response.entity;
        this.customerInquiryViewModel.totalCustomers = response.totalRows;
        this.customerInquiryViewModel.totalPages = response.totalPages;
    };
    CustomerInquiryComponent.prototype.customerInquiryFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    CustomerInquiryComponent.prototype.onPaginateChange = function (event) {
        this.customerInquiryViewModel.currentPageNumber = event.pageIndex + 1;
        this.customerInquiryViewModel.currentPageIndex = event.pageIndex;
        this.customerInquiryViewModel.pageSize = event.pageSize;
        this.executeSearch();
    };
    CustomerInquiryComponent.prototype.sortData = function (sort) {
        this.customerInquiryViewModel.currentPageNumber = 1;
        this.customerInquiryViewModel.currentPageIndex = 0;
        this.customerInquiryViewModel.sortDirection = sort.direction;
        this.customerInquiryViewModel.sortExpression = sort.active;
        this.executeSearch();
    };
    CustomerInquiryComponent.prototype.resetSearch = function () {
        this.lastSearchValue = '';
        this.customerInquiryViewModel.customerName = '';
        this.initializeSearch();
        this.executeSearch();
    };
    CustomerInquiryComponent.prototype.selectCustomer = function (row) {
        var customerId = this.customerInquiryViewModel.customers[row].customerId;
        this.router.navigate(['/salesordermanagement/customer-maintenance'], { queryParams: { id: customerId } });
    };
    __decorate([
        core_1.ViewChild('form'),
        __metadata("design:type", forms_1.NgForm)
    ], CustomerInquiryComponent.prototype, "searchForm", void 0);
    CustomerInquiryComponent = __decorate([
        core_1.Component({
            selector: 'app-customer-inquiry',
            templateUrl: './customer-inquiry.component.html',
            styleUrls: ['./customer-inquiry.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router, session_service_1.SessionService, http_service_1.HttpService,
            alert_service_1.AlertService])
    ], CustomerInquiryComponent);
    return CustomerInquiryComponent;
}());
exports.CustomerInquiryComponent = CustomerInquiryComponent;
//# sourceMappingURL=customer-inquiry.component.js.map