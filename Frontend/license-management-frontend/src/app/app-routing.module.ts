import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivationCodeListComponent } from './components/activation-code-list/activation-code-list.component';
import { ActivationRequestListComponent } from './components/activation-request-list/activation-request-list.component';
import { LicenseListComponent } from './components/license-list/license-list.component';
import { ValidationRequestListComponent } from './components/validation-request-list/validation-request-list.component';
import { ActivationCodeAddComponent } from './components/activation-code-add/activation-code-add.component';
import { LicenseCompareComponent } from './components/license-compare/license-compare.component';

const routes: Routes = [
  { path: 'activationcodes', component: ActivationCodeListComponent },
  { path: 'activationrequests', component: ActivationRequestListComponent },
  { path: 'licenses', component: LicenseListComponent },
  { path: 'validationrequests', component: ValidationRequestListComponent },
  { path: 'activationcodesadd', component: ActivationCodeAddComponent },
  { path: 'licensecompare', component: LicenseCompareComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
