import { NgModule } from '@angular/core';

import { AuthRoutingModule } from './auth-routing.module';
import { SignUpComponent } from './signup/signup.component';
import { LogInComponent } from './login/login.component';

@NgModule({
  imports: [
    AuthRoutingModule
  ],
  declarations: [
    SignUpComponent,
    LogInComponent
  ],
})
export class AuthModule {}
