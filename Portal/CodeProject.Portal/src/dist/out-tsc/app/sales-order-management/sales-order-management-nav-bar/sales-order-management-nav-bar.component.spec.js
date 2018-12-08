"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/* tslint:disable:no-unused-variable */
var testing_1 = require("@angular/core/testing");
var sales_order_management_nav_bar_component_1 = require("./sales-order-management-nav-bar.component");
describe('SalesOrderManagementNavBarComponent', function () {
    var component;
    var fixture;
    beforeEach(testing_1.async(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [sales_order_management_nav_bar_component_1.SalesOrderManagementNavBarComponent]
        })
            .compileComponents();
    }));
    beforeEach(function () {
        fixture = testing_1.TestBed.createComponent(sales_order_management_nav_bar_component_1.SalesOrderManagementNavBarComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=sales-order-management-nav-bar.component.spec.js.map