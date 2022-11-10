import { NgModule } from '@angular/core';

import { Error404Component } from './error404/error404.component';
import { RootRoutingModule } from './root-routing.module';

@NgModule({
  declarations: [
    Error404Component
  ],
  imports: [ 
    RootRoutingModule
  ],
  exports: [
    Error404Component
  ]
})
export class RootModule {}
