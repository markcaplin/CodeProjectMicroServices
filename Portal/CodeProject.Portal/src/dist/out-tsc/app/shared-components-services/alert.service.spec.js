"use strict";
/* tslint:disable:no-unused-variable */
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var alert_service_1 = require("./alert.service");
describe('Service: Alert', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [alert_service_1.AlertService]
        });
    });
    it('should ...', testing_1.inject([alert_service_1.AlertService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=alert.service.spec.js.map