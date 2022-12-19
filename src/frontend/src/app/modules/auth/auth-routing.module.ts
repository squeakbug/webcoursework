import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignUpComponent } from './signup/signup.component';
import { LogInComponent } from './login/login.component';
import { RoutesConfig } from 'src/app/configs/routes.config';
import { AuthGuard } from './auth.guard';

const authenticationRoutes: Routes = [
  { path: RoutesConfig.routes.auth.signUp, component: SignUpComponent, canActivate: [AuthGuard], pathMatch: 'full' },
  { path: RoutesConfig.routes.auth.logIn, component: LogInComponent, canActivate: [AuthGuard], pathMatch: 'full' },
  { path: '**', redirectTo: RoutesConfig.routes.error404 }
];

@NgModule({
  imports: [RouterModule.forChild(authenticationRoutes)],
  exports: [RouterModule],
  providers: [AuthGuard]
})
export class AuthRoutingModule { }