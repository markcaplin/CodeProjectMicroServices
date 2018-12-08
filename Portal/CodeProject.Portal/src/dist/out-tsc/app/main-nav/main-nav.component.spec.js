"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var sidenav_1 = require("@angular/material/sidenav");
var main_nav_component_1 = require("./main-nav.component");
describe('MainNavComponent', function () {
    var component;
    var fixture;
    beforeEach(testing_1.fakeAsync(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [sidenav_1.MatSidenavModule],
            declarations: [main_nav_component_1.MainNavComponent]
        })
            .compileComponents();
        fixture = testing_1.TestBed.createComponent(main_nav_component_1.MainNavComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    }));
    it('should compile', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=main-nav.component.spec.js.map