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
var session_service_1 = require("../../shared-components-services/session.service");
var http_1 = require("@angular/common/http");
var UploadProductMasterComponent = /** @class */ (function () {
    function UploadProductMasterComponent(sessionService) {
        /* this.httpHeaders = new HttpHeaders();
        const securityToken: string = localStorage.getItem('token');
        if (securityToken != null) {
            let tokenString = `Bearer ${securityToken}`;
            this.httpHeaders = new HttpHeaders()
              .set('authorization', tokenString)
              .set('Content-Type', 'multipart/form-data');
        }*/
        this.sessionService = sessionService;
        this.httpHeaders = new http_1.HttpHeaders();
        var securityToken = localStorage.getItem('token');
        if (securityToken != null) {
            var tokenString = "Bearer " + securityToken;
            this.httpHeaders = new http_1.HttpHeaders()
                .set('authorization', tokenString);
        }
        this.uploadUrl = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/uploadproductmasterfile';
    }
    UploadProductMasterComponent.prototype.ngOnInit = function () {
    };
    UploadProductMasterComponent = __decorate([
        core_1.Component({
            selector: 'app-upload-product-master',
            templateUrl: './upload-product-master.component.html',
            styleUrls: ['./upload-product-master.component.css']
        }),
        __metadata("design:paramtypes", [session_service_1.SessionService])
    ], UploadProductMasterComponent);
    return UploadProductMasterComponent;
}());
exports.UploadProductMasterComponent = UploadProductMasterComponent;
//# sourceMappingURL=upload-product-master.component.js.map