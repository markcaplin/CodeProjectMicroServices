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
var router_1 = require("@angular/router");
var core_1 = require("@angular/core");
var appsettings_model_1 = require("./shared-models/appsettings.model");
var session_service_1 = require("./shared-components-services/session.service");
var alert_service_1 = require("./shared-components-services/alert.service");
var AppComponent = /** @class */ (function () {
    function AppComponent(elementRef, router, alertService, sessionService) {
        var _this = this;
        this.elementRef = elementRef;
        this.router = router;
        this.alertService = alertService;
        this.sessionService = sessionService;
        this.title = 'Caplin Systems, Incorporated.';
        this.iterations = 0;
        this.showProgressBar = false;
        this.sessionId = 0;
        this.runningMonitor = false;
        this.hideMenuBar = true;
        this.alertService.progressBarUIEvent.subscribe(function (event) { return _this.updateProgressBar(event); });
        this.sessionService.authenicationEvent.subscribe(function (event) { return _this.authenicationEvent(event); });
        this.sessionService.moduleLoadedEvent.subscribe(function (event) { return _this.moduleLoadedEvent(event); });
        // const native = this.elementRef.nativeElement;
        // const settings = native.getAttribute('settings');
        var appSettings = new appsettings_model_1.AppSettings();
        appSettings.accountManagementWebApiUrl = 'https://localhost:44303/api/';
        appSettings.inventoryManagementWebApiUrl = 'https://localhost:44340/api/';
        appSettings.purchaseOrderManagementWebApiUrl = 'https://localhost:44327/api/';
        appSettings.salesOrderManagementWebApiUrl = 'https://localhost:44396/api/';
        // appSettings = JSON.parse(settings);
        sessionService.setAppSettings(appSettings);
        this.isAuthenicated = sessionService.isAuthenicated;
        sessionService.startSession();
    }
    AppComponent.prototype.moduleLoadedEvent = function (event) {
        this.hideMenuBar = true;
    };
    AppComponent.prototype.updateProgressBar = function (event) {
        this.showProgressBar = event;
    };
    AppComponent.prototype.authenicationEvent = function (userViewModel) {
        var _this = this;
        this.isAuthenicated = userViewModel.isAuthenicated;
        this.firstName = userViewModel.firstName;
        this.lastName = userViewModel.lastName;
        this.tokenExpirationDate = userViewModel.tokenExpirationDate;
        if (this.isAuthenicated === true && this.runningMonitor === false) {
            this.runningMonitor = true;
            this.monitorSession();
            this.sessionId = setInterval(function () {
                _this.monitorSession();
            }, 5000);
        }
        else {
            if (this.isAuthenicated === false && this.runningMonitor === true) {
                this.clearSessionInterval();
            }
        }
    };
    AppComponent.prototype.toggleNavBar = function () {
        if (this.hideMenuBar === false) {
            this.hideMenuBar = true;
        }
        else {
            this.hideMenuBar = false;
        }
    };
    AppComponent.prototype.monitorSession = function () {
        var isExpiredSession = this.sessionService.isExpiredSession();
        if (isExpiredSession) {
            this.isAuthenicated = false;
            this.clearSessionInterval();
            this.logout();
        }
        else {
            this.isAuthenicated = true;
        }
        this.iterations++;
    };
    AppComponent.prototype.logout = function () {
        this.sessionService.endSession();
        this.router.navigate(['/home/home']);
    };
    AppComponent.prototype.clearSessionInterval = function () {
        if (this.sessionId !== 0) {
            clearInterval(this.sessionId);
            this.sessionId = 0;
        }
        this.runningMonitor = false;
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: 'app-root',
            templateUrl: './app.component.html',
            styleUrls: ['./app.component.css']
        }),
        __metadata("design:paramtypes", [core_1.ElementRef,
            router_1.Router, alert_service_1.AlertService, session_service_1.SessionService])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map