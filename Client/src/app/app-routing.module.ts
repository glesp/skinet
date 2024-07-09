import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

const routes: Routes = [];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    TooltipModule.forRoot()
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
