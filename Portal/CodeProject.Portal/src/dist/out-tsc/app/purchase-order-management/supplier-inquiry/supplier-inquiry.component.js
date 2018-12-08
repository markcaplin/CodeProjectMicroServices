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
var supplier_inquiry_viewmodel_1 = require("../view-models/supplier-inquiry.viewmodel");
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var operators_1 = require("rxjs/operators");
var operators_2 = require("rxjs/operators");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var SupplierInquiryComponent = /** @class */ (function () {
    function SupplierInquiryComponent(router, sessionService, httpService, alertService) {
        this.router = router;
        this.sessionService = sessionService;
        this.httpService = httpService;
        this.alertService = alertService;
        this.selectedRowIndex = -1;
        this.sessionService.moduleLoadedEvent.emit();
        this.supplierInquiryViewModel = new supplier_inquiry_viewmodel_1.SupplierInquiryViewModel();
        this.supplierInquiryViewModel.pageSize = 20;
        this.supplierInquiryViewModel.displayedColumns = ['supplierName', 'addressLine1', 'addressLine2', 'city', 'region', 'postalCode'];
        this.supplierInquiryViewModel.pageSizeOptions = [5, 10, 25, 100];
        this.initializeSearch();
    }
    SupplierInquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchForm.valueChanges.pipe(operators_1.debounceTime(1000), operators_2.distinctUntilChanged()).subscribe(function (changes) {
            _this.supplierInquiryViewModel.currentPageNumber = 1;
            _this.supplierInquiryViewModel.currentPageIndex = 0;
            if (_this.lastSearchValue !== _this.supplierInquiryViewModel.supplierName) {
                _this.executeSearch();
            }
        });
        this.executeSearch();
    };
    SupplierInquiryComponent.prototype.initializeSearch = function () {
        this.supplierInquiryViewModel.supplierName = '';
        this.supplierInquiryViewModel.currentPageNumber = 1;
        this.supplierInquiryViewModel.currentPageIndex = 0;
        this.supplierInquiryViewModel.totalPages = 0;
        this.supplierInquiryViewModel.totalSuppliers = 0;
        this.supplierInquiryViewModel.sortDirection = '';
        this.supplierInquiryViewModel.sortExpression = '';
        this.supplierInquiryViewModel.suppliers = new Array();
    };
    SupplierInquiryComponent.prototype.executeSearch = function () {
        var _this = this;
        var url = this.sessionService.appSettings.purchaseOrderManagementWebApiUrl + 'supplier/supplierinquiry';
        this.httpService.HttpPost(url, this.supplierInquiryViewModel).
            subscribe(function (response) {
            _this.supplierInquirySuccess(response);
        }, function (response) { return _this.supplierInquiryFailed(response); });
    };
    SupplierInquiryComponent.prototype.supplierInquirySuccess = function (response) {
        this.supplierInquiryViewModel.suppliers = response.entity;
        this.supplierInquiryViewModel.totalSuppliers = response.totalRows;
        this.supplierInquiryViewModel.totalPages = response.totalPages;
    };
    SupplierInquiryComponent.prototype.supplierInquiryFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    SupplierInquiryComponent.prototype.onPaginateChange = function (event) {
        this.supplierInquiryViewModel.currentPageNumber = event.pageIndex + 1;
        this.supplierInquiryViewModel.currentPageIndex = event.pageIndex;
        this.supplierInquiryViewModel.pageSize = event.pageSize;
        this.executeSearch();
    };
    SupplierInquiryComponent.prototype.sortData = function (sort) {
        this.supplierInquiryViewModel.currentPageNumber = 1;
        this.supplierInquiryViewModel.currentPageIndex = 0;
        this.supplierInquiryViewModel.sortDirection = sort.direction;
        this.supplierInquiryViewModel.sortExpression = sort.active;
        this.executeSearch();
    };
    SupplierInquiryComponent.prototype.resetSearch = function () {
        this.lastSearchValue = '';
        this.supplierInquiryViewModel.supplierName = '';
        this.initializeSearch();
        this.executeSearch();
    };
    SupplierInquiryComponent.prototype.selectSupplier = function (row) {
        var supplierId = this.supplierInquiryViewModel.suppliers[row].supplierId;
        this.router.navigate(['/purchaseordermanagement/supplier-maintenance'], { queryParams: { id: supplierId } });
    };
    __decorate([
        core_1.ViewChild('form'),
        __metadata("design:type", forms_1.NgForm)
    ], SupplierInquiryComponent.prototype, "searchForm", void 0);
    SupplierInquiryComponent = __decorate([
        core_1.Component({
            selector: 'app-supplier-inquiry',
            templateUrl: './supplier-inquiry.component.html',
            styleUrls: ['./supplier-inquiry.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router, session_service_1.SessionService, http_service_1.HttpService,
            alert_service_1.AlertService])
    ], SupplierInquiryComponent);
    return SupplierInquiryComponent;
}());
exports.SupplierInquiryComponent = SupplierInquiryComponent;
//# sourceMappingURL=supplier-inquiry.component.js.map