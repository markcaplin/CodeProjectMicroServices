"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var platform_browser_1 = require("@angular/platform-browser");
var router_1 = require("@angular/router");
var http_1 = require("@angular/common/http");
var forms_1 = require("@angular/forms");
var core_1 = require("@angular/core");
var app_component_1 = require("./app.component");
var animations_1 = require("@angular/platform-browser/animations");
var about_component_1 = require("./home-directory/about/about.component");
var contact_component_1 = require("./home-directory/contact/contact.component");
var home_component_1 = require("./home-directory/home/home.component");
var application_routing_1 = require("./application-routing");
var session_service_1 = require("./shared-components-services/session.service");
var http_service_1 = require("./shared-components-services/http.service");
var alert_service_1 = require("./shared-components-services/alert.service");
var http_interceptor_service_1 = require("./shared-components-services/http-interceptor.service");
var http_2 = require("@angular/common/http");
var main_nav_component_1 = require("./main-nav/main-nav.component");
var layout_1 = require("@angular/cdk/layout");
var material_module_1 = require("./material.module");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            declarations: [
                app_component_1.AppComponent,
                home_component_1.HomeComponent,
                about_component_1.AboutComponent,
                contact_component_1.ContactComponent,
                main_nav_component_1.MainNavComponent
            ],
            imports: [
                platform_browser_1.BrowserModule,
                animations_1.BrowserAnimationsModule,
                material_module_1.MaterialModule,
                platform_browser_1.BrowserModule,
                router_1.RouterModule.forRoot(application_routing_1.ApplicationRoutes),
                animations_1.BrowserAnimationsModule,
                forms_1.FormsModule,
                layout_1.LayoutModule
            ],
            exports: [router_1.RouterModule, http_1.HttpClientModule, animations_1.BrowserAnimationsModule],
            providers: [session_service_1.SessionService, http_service_1.HttpService, alert_service_1.AlertService,
                http_interceptor_service_1.HttpInterceptorService,
                {
                    provide: http_2.HTTP_INTERCEPTORS,
                    useClass: http_interceptor_service_1.HttpInterceptorService,
                    multi: true
                }
            ],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map