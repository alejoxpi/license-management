import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { ActivationCodeService } from 'src/app/services/activation-code.service';
import { ActivationCode } from '../../models/activationcode.model';

@Component({
  selector: 'app-activation-code-list',
  templateUrl: './activation-code-list.component.html',
  styleUrls: ['./activation-code-list.component.css']
})
export class ActivationCodeListComponent implements OnInit {

  activationcodes : ActivationCode[] = [];
  title = '';
  currentActivationCode = new ActivationCode();
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
    this.currentActivationCode = new ActivationCode();
    this.currentIndex = -1;
  }

  setActiveActivationCode(activationcode:ActivationCode, index:number): void {
    this.currentActivationCode = activationcode;
    this.currentIndex = index;
  }

}
