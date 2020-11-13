import { Component, OnInit } from '@angular/core';
import { ActivationCodeCreationRequest } from '../../models/activationcodecreationrequest.model';
import { ActivationCodeService } from '../../services/activation-code.service';

@Component({
  selector: 'app-activation-code-add',
  templateUrl: './activation-code-add.component.html',
  styleUrls: ['./activation-code-add.component.css']
})
export class ActivationCodeAddComponent implements OnInit {

  creationRequest = new ActivationCodeCreationRequest();
  submitted = false;

  constructor(private activationCodeService:ActivationCodeService ) { }

  ngOnInit(): void {

  }

  generateCodes(): void{
    const data = {
      productcode: this.creationRequest.productcode,
      productname: this.creationRequest.productname,
      licensetype: this.creationRequest.licensetype,
      quantity: this.creationRequest.quantity,
      lifetime: this.creationRequest.lifetime
    }

    this.activationCodeService.create(data)
    .subscribe(
      response => {
        console.log(response);
        this.submitted = true;
      },
      error => {
        console.log(error)
      });
  }

  newRequest(): void{
    this.creationRequest = new ActivationCodeCreationRequest();
  }

}
