"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/* tslint:disable:no-unused-variable */
var testing_1 = require("@angular/core/testing");
var purchase_order_management_nav_bar_component_1 = require("./purchase-order-management-nav-bar.component");
describe('PurchaseOrderManagementNavBarComponent', function () {
    var component;
    var fixture;
    beforeEach(testing_1.async(function () {
        testing_1.TestBed.configureTestingModule({
            declarations: [purchase_order_management_nav_bar_component_1.PurchaseOrderManagementNavBarComponent]
        })
            .compileComponents();
    }));
    beforeEach(function () {
        fixture = testing_1.TestBed.createComponent(purchase_order_management_nav_bar_component_1.PurchaseOrderManagementNavBarComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=purchase-order-management-nav-bar.component.spec.js.map