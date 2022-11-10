import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignUpComponent } from './signup/signup.component';
import { LogInComponent } from './login/login.component';
import { RoutesConfig } from 'src/app/configs/routes.config';

const authenticationRoutes: Routes = [
  { path: RoutesConfig.routes.auth.signUp, component: SignUpComponent },
  { path: RoutesConfig.routes.auth.logIn, component: LogInComponent },
  { path: '**', redirectTo: RoutesConfig.routes.error404 }
];

@NgModule({
  imports: [RouterModule.forChild(authenticationRoutes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }