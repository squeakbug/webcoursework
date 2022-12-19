import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AuthRoutingModule } from './auth-routing.module';
import { SignUpComponent } from './signup/signup.component';
import { LogInComponent } from './login/login.component';

@NgModule({
  imports: [
    AuthRoutingModule,
    FormsModule
  ],
  declarations: [
    SignUpComponent,
    LogInComponent
  ],
})
export class AuthModule {}
