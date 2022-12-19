import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from './main/main.component'
import { ConfigurationsComponent } from './configurations/configurations.component';
import { RoutesConfig } from 'src/app/configs/routes.config';
import { ConverterGuard } from './converter.guard';

const converterRoutes: Routes = [
  { path: `${RoutesConfig.routes.converter.configurations}/:id`, component: ConfigurationsComponent, canActivate: [ConverterGuard] },
  { path: RoutesConfig.routes.converter.main, component: MainComponent, canActivate: [ConverterGuard] },
  { path: '**', redirectTo: RoutesConfig.routes.error404 }
];

@NgModule({
  imports: [RouterModule.forChild(converterRoutes)],
  exports: [RouterModule],
  providers: [ConverterGuard]
})
export class ConverterRoutingModule { }