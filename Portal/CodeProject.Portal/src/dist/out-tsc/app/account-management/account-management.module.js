"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var login_component_1 = require("./login/login.component");
var register_component_1 = require("./register/register.component");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var account_management_routing_1 = require("./account-management.routing");
var material_1 = require("@angular/material");
var AccountManagementModule = /** @class */ (function () {
    function AccountManagementModule() {
    }
    AccountManagementModule = __decorate([
        core_1.NgModule({
            imports: [
                account_management_routing_1.AccountManagementRoutingModule,
                common_1.CommonModule,
                forms_1.FormsModule,
                material_1.MatTabsModule,
                material_1.MatInputModule,
                material_1.MatButtonModule,
                material_1.MatSelectModule,
                material_1.MatIconModule,
                material_1.MatListModule,
                material_1.MatIconModule,
                material_1.MatSidenavModule,
                material_1.MatToolbarModule,
                material_1.MatSnackBarModule,
                material_1.MatProgressBarModule,
                material_1.MatFormFieldModule,
                material_1.MatCardModule,
                material_1.MatGridListModule,
            ],
            declarations: [register_component_1.RegisterComponent, login_component_1.LoginComponent]
        })
    ], AccountManagementModule);
    return AccountManagementModule;
}());
exports.AccountManagementModule = AccountManagementModule;
//# sourceMappingURL=account-management.module.js.map