import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivationCodeListComponent } from './components/activation-code-list/activation-code-list.component';
import { ActivationCodeDetailsComponent } from './components/activation-code-details/activation-code-details.component';

const routes: Routes = [
  { path: 'activationcodes', component: ActivationCodeListComponent },
  { path: 'activationcodes/:id', component: ActivationCodeDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
