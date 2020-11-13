import { Component, OnInit } from '@angular/core';
import { LicenseService  } from 'src/app/services/license.service';
import { License } from '../../models/license.model';

@Component({
  selector: 'app-license-list',
  templateUrl: './license-list.component.html',
  styleUrls: ['./license-list.component.css']
})
export class LicenseListComponent implements OnInit {

  _listElements : License[] = [];
  title = '';
  currentElement = new License();
  currentIndex = -1;

  constructor(private licenseService : LicenseService) { }

  ngOnInit(): void {
    this.retrieveElements();
  }

  retrieveElements(): void{
    this.licenseService.getAll()
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
    this.currentElement = new License();
    this.currentIndex = -1;
  }

  setActiveActivationCode(_element:License, index:number): void {
    this.currentElement = _element;
    this.currentIndex = index;
  }

}
