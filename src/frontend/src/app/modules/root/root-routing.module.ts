import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoutesConfig } from 'src/app/configs/routes.config';
import { Error404Component } from './error404/error404.component';

const rootRoutes: Routes = [
  { path: RoutesConfig.routes.error404, component: Error404Component }
];

@NgModule({
  imports: [RouterModule.forChild(rootRoutes)],
  exports: [RouterModule]
})
export class RootRoutingModule {}
