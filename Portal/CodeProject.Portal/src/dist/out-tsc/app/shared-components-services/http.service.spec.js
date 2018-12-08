"use strict";
/* tslint:disable:no-unused-variable */
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var http_service_1 = require("./http.service");
describe('Service: Http', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [http_service_1.HttpService]
        });
    });
    it('should ...', testing_1.inject([http_service_1.HttpService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=http.service.spec.js.map