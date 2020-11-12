import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { ActivationCodeService } from 'src/app/services/activation-code.service';

@Component({
  selector: 'app-activation-code-list',
  templateUrl: './activation-code-list.component.html',
  styleUrls: ['./activation-code-list.component.css']
})
export class ActivationCodeListComponent implements OnInit {

  activationcodes : any;
  title = '';
  currentActivationCode = null;
  currentIndex = -1;

  constructor(private activationcodeService : ActivationCodeService) { }

  ngOnInit(): void {
    this.retrieveActivationCodes();
  }

  retrieveActivationCodes(): void{
    this.activationcodeService.getAll()
    .subscribe(
      data=>{
        this.activationcodes = data;     
        console.log(data);   
      },
      error => {
        console.log(error);
      });
  }

  refreshList(): void {
    this.retrieveActivationCodes();
    this.currentActivationCode = null;
    this.currentIndex = -1;
  }

  setActiveActivationCode(activationcode:any, index:number): void {
    this.currentActivationCode = activationcode;
    this.currentIndex = index;
  }

}
