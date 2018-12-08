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
var purchase_order_inquiry_viewmodel_1 = require("../view-models/purchase-order-inquiry.viewmodel");
var http_service_1 = require("./../../shared-components-services/http.service");
var alert_service_1 = require("./../../shared-components-services/alert.service");
var operators_1 = require("rxjs/operators");
var operators_2 = require("rxjs/operators");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var PurchaseOrderInquiryComponent = /** @class */ (function () {
    function PurchaseOrderInquiryComponent(router, sessionService, httpService, alertService) {
        this.router = router;
        this.sessionService = sessionService;
        this.httpService = httpService;
        this.alertService = alertService;
        this.selectedRowIndex = -1;
        this.sessionService.moduleLoadedEvent.emit();
        this.purchaseOrderInquiryViewModel = new purchase_order_inquiry_viewmodel_1.PurchaseOrderInquiryViewModel();
        this.purchaseOrderInquiryViewModel.pageSize = 20;
        this.purchaseOrderInquiryViewModel.displayedColumns = ['purchaseOrderNumber', 'supplierName',
            'city', 'region', 'orderTotal', 'orderDate', 'purchaseOrderStatusDescription'];
        this.purchaseOrderInquiryViewModel.pageSizeOptions = [5, 10, 25, 100];
        this.initializeSearch();
    }
    PurchaseOrderInquiryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchForm.valueChanges.pipe(operators_1.debounceTime(1000), operators_2.distinctUntilChanged()).subscribe(function (changes) {
            _this.purchaseOrderInquiryViewModel.currentPageNumber = 1;
            _this.purchaseOrderInquiryViewModel.currentPageIndex = 0;
            if (_this.lastSearchValue !== _this.purchaseOrderInquiryViewModel.supplierName) {
                _this.executeSearch();
            }
        });
        this.executeSearch();
    };
    PurchaseOrderInquiryComponent.prototype.initializeSearch = function () {
        this.purchaseOrderInquiryViewModel.supplierName = '';
        this.purchaseOrderInquiryViewModel.currentPageNumber = 1;
        this.purchaseOrderInquiryViewModel.currentPageIndex = 0;
        this.purchaseOrderInquiryViewModel.totalPages = 0;
        this.purchaseOrderInquiryViewModel.totalPurchaseOrders = 0;
        this.purchaseOrderInquiryViewModel.sortDirection = 'DESC';
        this.purchaseOrderInquiryViewModel.sortExpression = 'PurchaseOrderNumber';
        this.purchaseOrderInquiryViewModel.purchaseOrders = new Array();
    };
    PurchaseOrderInquiryComponent.prototype.executeSearch = function () {
        var _this = this;
        var url = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'purchaseorder/purchaseorderinquiry';
        this.httpService.HttpPost(url, this.purchaseOrderInquiryViewModel).
            subscribe(function (response) {
            _this.purchaseOrderInquirySuccess(response);
        }, function (response) { return _this.purchaseOrderInquiryFailed(response); });
    };
    PurchaseOrderInquiryComponent.prototype.purchaseOrderInquirySuccess = function (response) {
        response.entity.forEach(function (element) {
            var orderDate = element.dateCreated.toString().substring(0, 10);
            element.orderDate = orderDate.substring(5, 7) + '/' + orderDate.substring(8, 10) + '/' + orderDate.substring(0, 4);
        });
        this.purchaseOrderInquiryViewModel.purchaseOrders = response.entity;
        this.purchaseOrderInquiryViewModel.totalPurchaseOrders = response.totalRows;
        this.purchaseOrderInquiryViewModel.totalPages = response.totalPages;
    };
    PurchaseOrderInquiryComponent.prototype.purchaseOrderInquiryFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    PurchaseOrderInquiryComponent.prototype.onPaginateChange = function (event) {
        this.purchaseOrderInquiryViewModel.currentPageNumber = event.pageIndex + 1;
        this.purchaseOrderInquiryViewModel.currentPageIndex = event.pageIndex;
        this.purchaseOrderInquiryViewModel.pageSize = event.pageSize;
        this.executeSearch();
    };
    PurchaseOrderInquiryComponent.prototype.sortData = function (sort) {
        this.purchaseOrderInquiryViewModel.currentPageNumber = 1;
        this.purchaseOrderInquiryViewModel.currentPageIndex = 0;
        this.purchaseOrderInquiryViewModel.sortDirection = sort.direction;
        this.purchaseOrderInquiryViewModel.sortExpression = sort.active;
        this.executeSearch();
    };
    PurchaseOrderInquiryComponent.prototype.resetSearch = function () {
        this.lastSearchValue = '';
        this.purchaseOrderInquiryViewModel.supplierName = '';
        this.initializeSearch();
        this.executeSearch();
    };
    PurchaseOrderInquiryComponent.prototype.selectPurchaseOrder = function (row) {
        var purchaseOrderId = this.purchaseOrderInquiryViewModel.purchaseOrders[row].purchaseOrderId;
        this.router.navigate(['/inventorymanagement/purchase-order-receiving'], { queryParams: { id: purchaseOrderId } });
    };
    __decorate([
        core_1.ViewChild('form'),
        __metadata("design:type", forms_1.NgForm)
    ], PurchaseOrderInquiryComponent.prototype, "searchForm", void 0);
    PurchaseOrderInquiryComponent = __decorate([
        core_1.Component({
            selector: 'app-purchase-order-inquiry',
            templateUrl: './purchase-order-inquiry.component.html',
            styleUrls: ['./purchase-order-inquiry.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router,
            session_service_1.SessionService,
            http_service_1.HttpService,
            alert_service_1.AlertService])
    ], PurchaseOrderInquiryComponent);
    return PurchaseOrderInquiryComponent;
}());
exports.PurchaseOrderInquiryComponent = PurchaseOrderInquiryComponent;
//# sourceMappingURL=purchase-order-inquiry.component.js.map