import { Component, OnInit } from '@angular/core';
import { SessionService } from '../../shared-components-services/session.service';

@Component({
  selector: 'app-product-inquiry',
  templateUrl: './product-inquiry.component.html',
  styleUrls: ['./product-inquiry.component.css']
})
export class ProductInquiryComponent implements OnInit {

  constructor(private sessionService: SessionService) {
      this.sessionService.moduleLoadedEvent.emit();
   }

  ngOnInit() {

  }

}
