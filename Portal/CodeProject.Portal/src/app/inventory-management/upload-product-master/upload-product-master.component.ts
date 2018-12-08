
import { Component, OnInit } from '@angular/core';
import { SessionService } from '../../shared-components-services/session.service';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { FileUploader } from 'ng2-file-upload';



@Component({
  selector: 'app-upload-product-master',
  templateUrl: './upload-product-master.component.html',
  styleUrls: ['./upload-product-master.component.css']
})
export class UploadProductMasterComponent implements OnInit {

  public httpHeaders: HttpHeaders;
  public hasBaseDropZoneOver: boolean = false;
  public hasAnotherDropZoneOver: boolean = false;
  public uploader: FileUploader;

  constructor(private sessionService: SessionService) {

    /* this.httpHeaders = new HttpHeaders();
    const securityToken: string = localStorage.getItem('token');
    if (securityToken != null) {
        let tokenString = `Bearer ${securityToken}`;
        this.httpHeaders = new HttpHeaders()
          .set('authorization', tokenString)
          .set('Content-Type', 'multipart/form-data');
    }*/

    //this.httpHeaders = new HttpHeaders();
    const securityToken: string = localStorage.getItem('token');

    /*if (securityToken != null) {
        let tokenString = `Bearer ${securityToken}`;
        this.httpHeaders = new HttpHeaders()
          .set('authorization', tokenString);
    }*/

    const uploadUrl = this.sessionService.appSettings.inventoryManagementWebApiUrl + 'product/uploadproductmasterfile';
    this.uploader = new FileUploader({ url: uploadUrl });

    if (securityToken != null) {
      let tokenString = `Bearer ${securityToken}`;
      this.uploader.authToken = tokenString;
    }
   
  }

  ngOnInit() {

  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

}
