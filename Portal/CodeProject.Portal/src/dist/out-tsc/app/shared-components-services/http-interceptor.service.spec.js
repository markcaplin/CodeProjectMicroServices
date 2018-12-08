"use strict";
/* tslint:disable:no-unused-variable */
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var http_interceptor_service_1 = require("./http-interceptor.service");
describe('Service: HttpInterceptor', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [http_interceptor_service_1.HttpInterceptorService]
        });
    });
    it('should ...', testing_1.inject([http_interceptor_service_1.HttpInterceptorService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=http-interceptor.service.spec.js.map