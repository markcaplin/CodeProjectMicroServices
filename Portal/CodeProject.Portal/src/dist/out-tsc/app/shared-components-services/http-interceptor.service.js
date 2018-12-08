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
var http_1 = require("@angular/common/http");
var operators_1 = require("rxjs/operators");
var session_service_1 = require("./session.service");
var HttpInterceptorService = /** @class */ (function () {
    function HttpInterceptorService(sessionService, router) {
        this.sessionService = sessionService;
        this.router = router;
    }
    HttpInterceptorService.prototype.intercept = function (request, next) {
        // modify request
        var _this = this;
        request = request.clone({
        // setHeaders: {
        //  Authorization: `Bearer ${localStorage.getItem('MY_TOKEN')}`
        // }
        });
        // console.log('----request----');
        // console.log(request);
        // console.log('--- end of request---');
        return next.handle(request).pipe(operators_1.tap(function (event) {
            if (event instanceof http_1.HttpResponse) {
                // console.log(event);
                // console.log(' all looks good');
                // http response status code
                // console.log(event.status);
                var token = event.body.token;
                if (token !== '' && token !== null && token !== undefined) {
                    localStorage.setItem('token', token);
                    _this.sessionService.startSession();
                }
                // console.log('token=' + token);
                // const header = event.headers.get('content-type');
                // console.log('header=' + header);
            }
        }, function (error) {
            // http response status code
            // console.log('----response----');
            // console.error('status code:');
            // console.error(error.status);
            // console.error(error.message);
            // console.log('--- end of response---');
            if (error.status === 401) {
                console.log('unauthorized');
                _this.router.navigate(['/home/login']);
            }
        }));
    };
    HttpInterceptorService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [session_service_1.SessionService, router_1.Router])
    ], HttpInterceptorService);
    return HttpInterceptorService;
}());
exports.HttpInterceptorService = HttpInterceptorService;
//# sourceMappingURL=http-interceptor.service.js.map