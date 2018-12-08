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
var session_service_1 = require("./session.service");
var alert_service_1 = require("./alert.service");
var core_1 = require("@angular/core");
var http_1 = require("@angular/common/http");
var operators_1 = require("rxjs/operators");
var rxjs_1 = require("rxjs");
var HttpService = /** @class */ (function () {
    function HttpService(httpClient, alertService, sessionService) {
        this.httpClient = httpClient;
        this.alertService = alertService;
        this.sessionService = sessionService;
    }
    HttpService.prototype.HttpGet = function (url) {
        var _this = this;
        this.alertService.startProgressBar();
        var tokenString = '';
        var httpHeaders = new http_1.HttpHeaders();
        var securityToken = localStorage.getItem('token');
        if (securityToken != null) {
            tokenString = "Bearer " + securityToken;
            httpHeaders = new http_1.HttpHeaders()
                .set('authorization', tokenString)
                .set('Content-Type', 'application/json');
        }
        else {
            httpHeaders = new http_1.HttpHeaders()
                .set('Content-Type', 'application/json');
        }
        // const headers = new HttpHeaders();
        // console.log('token ' + tokenString);
        // headers.append('Authorization', tokenString);
        // headers.append('Content-Type', 'application/json; charset=utf-8');
        // headers.append('Content-Type', 'application/json');
        // headers.append('Accept', 'q=0.8;application/json;q=0.9');
        // console.log('token=' + tokenString);
        console.log('url=' + url);
        return this.httpClient.get(url, { headers: httpHeaders })
            .pipe(operators_1.catchError(function (err) { return _this.handleError(err); }), operators_1.finalize(function () {
            _this.alertService.stopProgressBar();
        }));
    };
    HttpService.prototype.HttpPost = function (url, data) {
        var _this = this;
        this.alertService.startProgressBar();
        // const securityToken: string = this.sessionService.userViewModel.token;
        var tokenString = '';
        var securityToken = localStorage.getItem('token');
        var httpHeaders = new http_1.HttpHeaders();
        if (securityToken != null) {
            tokenString = "Bearer " + securityToken;
            httpHeaders = new http_1.HttpHeaders()
                .set('authorization', tokenString)
                .set('Content-Type', 'application/json');
        }
        else {
            httpHeaders = new http_1.HttpHeaders()
                .set('Content-Type', 'application/json');
        }
        // const headers = new HttpHeaders();
        // headers.append('Authorization', tokenString);
        // headers.append('Content-Type', 'application/json; charset=utf-8');
        // headers.append('Content-Type', 'application/json');
        // headers.append('Accept', 'q=0.8;application/json;q=0.9');
        console.log('token=' + tokenString);
        // const basePath = this.sessionService.appSettings.webApiUrl;
        console.log('url=' + url);
        return this.httpClient.post(url, data, { headers: httpHeaders })
            .pipe(operators_1.catchError(function (err) { return _this.handleError(err); }), operators_1.finalize(function () {
            _this.alertService.stopProgressBar();
        }));
    };
    HttpService.prototype.handleError = function (error) {
        console.log('handle error');
        if (error.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            console.error('An error occurred:', error.error.message);
        }
        else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            console.error("Backend returned code " + error.status + ", body was: " + error.error);
        }
        // const body = error.error;
        // console.log(body);
        // const errorStatusText = error.statusText;
        // const errorMessage = error.message;
        // console.log(errorStatusText + ' *** ' + errorMessage + '///');
        return rxjs_1.throwError(error);
    };
    HttpService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [http_1.HttpClient, alert_service_1.AlertService, session_service_1.SessionService])
    ], HttpService);
    return HttpService;
}());
exports.HttpService = HttpService;
//# sourceMappingURL=http.service.js.map