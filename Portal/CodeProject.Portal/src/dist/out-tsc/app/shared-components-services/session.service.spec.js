"use strict";
/* tslint:disable:no-unused-variable */
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var session_service_1 = require("./session.service");
describe('Service: Session', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [session_service_1.SessionService]
        });
    });
    it('should ...', testing_1.inject([session_service_1.SessionService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=session.service.spec.js.map