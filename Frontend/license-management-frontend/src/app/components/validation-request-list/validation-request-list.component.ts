import { Component, OnInit } from '@angular/core';
import { ValidationRequestService } from 'src/app/services/validation-request.service';
import { ValidationRequest } from '../../models/validationrequest.model';

@Component({
  selector: 'app-validation-request-list',
  templateUrl: './validation-request-list.component.html',
  styleUrls: ['./validation-request-list.component.css']
})
export class ValidationRequestListComponent implements OnInit {

  _listElements : ValidationRequest[] = [];
  title = '';
  currentElement = new ValidationRequest();
  currentIndex = -1;

  constructor(private validationRequestService : ValidationRequestService) { }

  ngOnInit(): void {
    this.retrieveElements();
  }

  retrieveElements(): void{
    this.validationRequestService.getAll()
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
    this.currentElement = new ValidationRequest();
    this.currentIndex = -1;
  }

  setActiveActivationCode(_element:ValidationRequest, index:number): void {
    this.currentElement = _element;
    this.currentIndex = index;
  }

}
