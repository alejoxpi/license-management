import { Component, OnInit } from '@angular/core';
import { License } from '../../models/license.model';
import { LicenseService  } from 'src/app/services/license.service';
import { ValidationRequest } from '../../models/validationrequest.model';
import { ValidationRequestService } from 'src/app/services/validation-request.service';
import { ActivatedRoute, Router } from '@angular/router';


@Component({
  selector: 'app-license-compare',
  templateUrl: './license-compare.component.html',
  styleUrls: ['./license-compare.component.css']
})
export class LicenseCompareComponent implements OnInit {

  license = new License();
  validationRequest = new ValidationRequest();

  constructor(
    private licenseService:LicenseService,
    private validationRequestService:ValidationRequestService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.compareLicense_ValidationRequest(this.route.snapshot.paramMap.get('id'));
  }

  compareLicense_ValidationRequest(id:any): void{

    this.validationRequestService.get(id)
    .subscribe(
      data => {
        this.validationRequest = data;
        console.log(data);
        console.log(this.validationRequest.licenseCode);

        this.licenseService.get(this.validationRequest.licenseCode)
          .subscribe(
            data => {
              this.license = data;
              console.log(data)
            },
            error =>{
              console.log(error);
            });



      },
      error =>{
        console.log(error);
      });

  }

}
