import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component'
import { ConfigurationsComponent } from './configurations/configurations.component';
import { RoutesConfig } from 'src/app/configs/routes.config';

const converterRoutes: Routes = [
  { path: `${RoutesConfig.routes.converter.configurations}/:id`, component: ConfigurationsComponent },
  { path: RoutesConfig.routes.converter.main, component: MainComponent },
  { path: '**', redirectTo: RoutesConfig.routes.error404 }
];

@NgModule({
  imports: [RouterModule.forChild(converterRoutes)],
  exports: [RouterModule]
})
export class ConverterRoutingModule { }