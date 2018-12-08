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
var appsettings_model_1 = require("../shared-models/appsettings.model");
var user_viewmodel_1 = require("../shared-view-models/user.viewmodel");
var angular_jwt_1 = require("@auth0/angular-jwt");
var SessionService = /** @class */ (function () {
    function SessionService() {
        this.appSettings = new appsettings_model_1.AppSettings();
        this.userViewModel = new user_viewmodel_1.UserViewModel();
        this.isAuthenicated = false;
        this.authenicationEvent = new core_1.EventEmitter();
        this.moduleLoadedEvent = new core_1.EventEmitter();
        this.jwtHelperService = new angular_jwt_1.JwtHelperService();
    }
    SessionService.prototype.setAppSettings = function (appSettings) {
        this.appSettings = appSettings;
    };
    SessionService.prototype.setUserViewModel = function (userViewModel) {
        this.userViewModel = userViewModel;
        var token = userViewModel.token;
        this.userViewModel.tokenExpirationDate = this.jwtHelperService.getTokenExpirationDate(token);
        this.isAuthenicated = userViewModel.isAuthenicated;
        this.authenicationEvent.emit(userViewModel);
    };
    SessionService.prototype.isExpiredSession = function () {
        if (this.userViewModel.token === null || this.userViewModel.token === undefined) {
            return true;
        }
        var isExpired = this.jwtHelperService.isTokenExpired(this.userViewModel.token);
        return isExpired;
    };
    SessionService.prototype.endSession = function () {
        localStorage.removeItem('token');
        this.userViewModel = new user_viewmodel_1.UserViewModel();
        this.userViewModel.isAuthenicated = false;
        this.authenicationEvent.emit(this.userViewModel);
    };
    SessionService.prototype.startSession = function () {
        var token = localStorage.getItem('token');
        if (token != null && token !== undefined) {
            var jwtHelperService = new angular_jwt_1.JwtHelperService();
            var decodedToken = jwtHelperService.decodeToken(token);
            this.userViewModel = new user_viewmodel_1.UserViewModel();
            this.userViewModel.token = token;
            this.userViewModel.firstName = decodedToken.given_name;
            this.userViewModel.lastName = decodedToken.nameid;
            this.userViewModel.emailAddress = decodedToken.email;
            this.userViewModel.companyName = decodedToken.name;
            this.userViewModel.isAuthenicated = true;
            this.userViewModel.tokenExpirationDate = jwtHelperService.getTokenExpirationDate(token);
            this.authenicationEvent.emit(this.userViewModel);
        }
    };
    SessionService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [])
    ], SessionService);
    return SessionService;
}());
exports.SessionService = SessionService;
//# sourceMappingURL=session.service.js.map