import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ActivationCodeListComponent } from './components/activation-code-list/activation-code-list.component';
import { ActivationCodeDetailsComponent } from './components/activation-code-details/activation-code-details.component';
import { ActivationRequestListComponent } from './components/activation-request-list/activation-request-list.component';
import { LicenseListComponent } from './components/license-list/license-list.component';
import { ValidationRequestListComponent } from './components/validation-request-list/validation-request-list.component';
import { ActivationCodeAddComponent } from './components/activation-code-add/activation-code-add.component';
import { LicenseCompareComponent } from './components/license-compare/license-compare.component';


@NgModule({
  declarations: [
    AppComponent,
    ActivationCodeListComponent,
    ActivationCodeDetailsComponent,
    ActivationRequestListComponent,
    LicenseListComponent,
    ValidationRequestListComponent,
    ActivationCodeAddComponent,
    LicenseCompareComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
