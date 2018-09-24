import { Component, OnInit } from '@angular/core';
import { SessionService } from '../../shared-components-services/session.service';

@Component({
  selector: 'app-supplier-inquiry',
  templateUrl: './supplier-inquiry.component.html',
  styleUrls: ['./supplier-inquiry.component.css']
})
export class SupplierInquiryComponent implements OnInit {

  constructor(private sessionService: SessionService) {
    this.sessionService.moduleLoadedEvent.emit();
 }

  ngOnInit() {
  }

}
