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
var core_1 = require("@angular/core");
var layout_1 = require("@angular/cdk/layout");
var operators_1 = require("rxjs/operators");
var MainNavComponent = /** @class */ (function () {
    function MainNavComponent(breakpointObserver) {
        this.breakpointObserver = breakpointObserver;
        this.isHandset$ = this.breakpointObserver.observe(layout_1.Breakpoints.Handset)
            .pipe(operators_1.map(function (result) { return result.matches; }));
    }
    MainNavComponent = __decorate([
        core_1.Component({
            selector: 'app-main-nav',
            templateUrl: './main-nav.component.html',
            styleUrls: ['./main-nav.component.css']
        }),
        __metadata("design:paramtypes", [layout_1.BreakpointObserver])
    ], MainNavComponent);
    return MainNavComponent;
}());
exports.MainNavComponent = MainNavComponent;
//# sourceMappingURL=main-nav.component.js.map