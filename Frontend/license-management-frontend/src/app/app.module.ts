import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ActivationCodeListComponent } from './components/activation-code-list/activation-code-list.component';
import { ActivationCodeDetailsComponent } from './components/activation-code-details/activation-code-details.component';


@NgModule({
  declarations: [
    AppComponent,
    ActivationCodeListComponent,
    ActivationCodeDetailsComponent
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
