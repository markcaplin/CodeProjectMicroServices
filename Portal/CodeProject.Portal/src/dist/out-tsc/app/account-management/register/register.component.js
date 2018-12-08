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
var alert_service_1 = require("../../shared-components-services/alert.service");
var http_service_1 = require("../../shared-components-services/http.service");
var user_viewmodel_1 = require("../../shared-view-models/user.viewmodel");
var session_service_1 = require("../../shared-components-services/session.service");
var router_1 = require("@angular/router");
var RegisterComponent = /** @class */ (function () {
    function RegisterComponent(router, httpService, alertService, sessionService) {
        this.router = router;
        this.httpService = httpService;
        this.alertService = alertService;
        this.sessionService = sessionService;
        this.userViewModel = new user_viewmodel_1.UserViewModel();
        this.userViewModel.firstName = '';
        this.userViewModel.lastName = '';
        this.userViewModel.password = '';
        this.userViewModel.passwordConfirmation = '';
        this.userViewModel.emailAddress = '';
        this.userViewModel.companyName = '';
    }
    RegisterComponent.prototype.ngOnInit = function () {
    };
    RegisterComponent.prototype.register = function () {
        var _this = this;
        var user = new user_viewmodel_1.UserViewModel();
        user = this.userViewModel;
        var url = this.sessionService.appSettings.accountManagementWebApiUrl + 'authorization/register';
        this.httpService.HttpPost(url, user).subscribe(function (response) {
            _this.registerSuccess(response);
        }, function (response) { return _this.registerFailed(response); });
    };
    RegisterComponent.prototype.registerSuccess = function (response) {
        var message = 'Registration Successful.';
        this.alertService.ShowSuccessMessage(message);
        localStorage.setItem('token', response.entity.token);
        this.sessionService.setUserViewModel(response.entity);
        this.router.navigate(['/home/home']);
    };
    RegisterComponent.prototype.registerFailed = function (error) {
        var errorResponse = error.error;
        if (error.status > 0) {
            this.alertService.ShowErrorMessage(errorResponse.returnMessage[0]);
        }
        else {
            this.alertService.ShowErrorMessage(error.message);
        }
    };
    RegisterComponent = __decorate([
        core_1.Component({
            selector: 'app-register',
            templateUrl: './register.component.html',
            styleUrls: ['./register.component.css']
        }),
        __metadata("design:paramtypes", [router_1.Router, http_service_1.HttpService,
            alert_service_1.AlertService, session_service_1.SessionService])
    ], RegisterComponent);
    return RegisterComponent;
}());
exports.RegisterComponent = RegisterComponent;
//# sourceMappingURL=register.component.js.map