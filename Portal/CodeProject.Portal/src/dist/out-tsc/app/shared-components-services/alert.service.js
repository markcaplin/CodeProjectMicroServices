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
var material_1 = require("@angular/material");
var material_2 = require("@angular/material");
var AlertService = /** @class */ (function () {
    function AlertService(snackBar) {
        this.snackBar = snackBar;
        this.verticalPosition = 'bottom';
        this.horizontalPosition = 'right';
        this.duration = 5000;
        this.action = '';
        this.progressBarUIEvent = new core_1.EventEmitter();
    }
    AlertService.prototype.startProgressBar = function () {
        this.progressBarUIEvent.emit(true);
    };
    AlertService.prototype.stopProgressBar = function () {
        this.progressBarUIEvent.emit(false);
    };
    AlertService.prototype.ShowSuccessMessage = function (message) {
        var config = new material_2.MatSnackBarConfig();
        config.verticalPosition = this.verticalPosition;
        config.horizontalPosition = this.horizontalPosition;
        config.duration = this.duration;
        config.panelClass = ['successMessage'];
        this.snackBar.open(message, this.action, config);
        this.stopProgressBar();
    };
    AlertService.prototype.ShowErrorMessage = function (message) {
        var config = new material_2.MatSnackBarConfig();
        config.verticalPosition = this.verticalPosition;
        config.horizontalPosition = this.horizontalPosition;
        config.duration = this.duration;
        config.panelClass = ['errorMessage'];
        this.snackBar.open(message, this.action, config);
        this.stopProgressBar();
    };
    AlertService.prototype.ShowWarningMessage = function (message) {
        var config = new material_2.MatSnackBarConfig();
        config.verticalPosition = this.verticalPosition;
        config.horizontalPosition = this.horizontalPosition;
        config.duration = this.duration;
        config.panelClass = ['warningMessage'];
        this.snackBar.open(message, this.action, config);
        this.stopProgressBar();
    };
    AlertService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        }),
        __metadata("design:paramtypes", [material_1.MatSnackBar])
    ], AlertService);
    return AlertService;
}());
exports.AlertService = AlertService;
//# sourceMappingURL=alert.service.js.map