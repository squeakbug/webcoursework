import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthService } from './services/auth.service';
import { ConverterService } from './services/converter.service';
import { AuthInterceptor } from './intercaptors/auth.interceptor';

@NgModule({
  imports: [
    
  ],
  providers: [
    AuthService,
    AuthInterceptor,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    ConverterService,
  ],
  declarations: []
})
export class CoreModule { }
