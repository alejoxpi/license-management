import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { ActivationRequestService } from 'src/app/services/activation-request.service';
import { ActivationRequest } from '../../models/activationrequest.model';


@Component({
  selector: 'app-activation-request-list',
  templateUrl: './activation-request-list.component.html',
  styleUrls: ['./activation-request-list.component.css']
})
export class ActivationRequestListComponent implements OnInit {

  _listElements : ActivationRequest[] = [];
  title = '';
  currentElement = new ActivationRequest();
  currentIndex = -1;

  constructor(private activationrequestService : ActivationRequestService) { }

  ngOnInit(): void {
    this.retrieveElements();

  }

  retrieveElements(): void{
    this.activationrequestService.getAll()
    .subscribe(
      data=>{
        this._listElements = data;
        console.log(this._listElements);
      },
      error => {
        console.log(error)
      });
      
  }

  refreshList(): void {
    this.retrieveElements();
    this.currentElement = new ActivationRequest();
    this.currentIndex = -1;
  }

  setActiveActivationCode(_element:ActivationRequest, index:number): void {
    this.currentElement = _element;
    this.currentIndex = index;
  }

}
