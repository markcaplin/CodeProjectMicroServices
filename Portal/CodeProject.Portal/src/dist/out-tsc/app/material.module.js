"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var material_1 = require("@angular/material");
var sort_1 = require("@angular/material/sort");
var menu_1 = require("@angular/material/menu");
var progress_spinner_1 = require("@angular/material/progress-spinner");
var angular_material_fileupload_1 = require("angular-material-fileupload");
var MaterialModule = /** @class */ (function () {
    function MaterialModule() {
    }
    MaterialModule = __decorate([
        core_1.NgModule({
            imports: [
                material_1.MatButtonModule,
                material_1.MatIconModule,
                material_1.MatFormFieldModule,
                material_1.MatInputModule,
                material_1.MatDatepickerModule,
                material_1.MatNativeDateModule,
                material_1.MatCheckboxModule,
                material_1.MatSidenavModule,
                material_1.MatToolbarModule,
                material_1.MatSidenavModule,
                material_1.MatToolbarModule,
                material_1.MatSnackBarModule,
                material_1.MatProgressBarModule,
                material_1.MatListModule,
                material_1.MatCardModule,
                material_1.MatTableModule,
                material_1.MatPaginatorModule,
                sort_1.MatSortModule,
                menu_1.MatMenuModule,
                progress_spinner_1.MatProgressSpinnerModule,
                material_1.MatDialogModule,
                angular_material_fileupload_1.MatFileUploadModule
            ],
            exports: [
                material_1.MatButtonModule,
                material_1.MatIconModule,
                material_1.MatFormFieldModule,
                material_1.MatInputModule,
                material_1.MatDatepickerModule,
                material_1.MatNativeDateModule,
                material_1.MatCheckboxModule,
                material_1.MatSidenavModule,
                material_1.MatToolbarModule,
                material_1.MatSidenavModule,
                material_1.MatToolbarModule,
                material_1.MatSnackBarModule,
                material_1.MatProgressBarModule,
                material_1.MatListModule,
                material_1.MatCardModule,
                material_1.MatTableModule,
                material_1.MatPaginatorModule,
                sort_1.MatSortModule,
                menu_1.MatMenuModule,
                progress_spinner_1.MatProgressSpinnerModule,
                material_1.MatDialogModule,
                angular_material_fileupload_1.MatFileUploadModule
            ]
        })
    ], MaterialModule);
    return MaterialModule;
}());
exports.MaterialModule = MaterialModule;
//# sourceMappingURL=material.module.js.map