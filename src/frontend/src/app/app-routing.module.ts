import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoutesConfig } from 'src/app/configs/routes.config';

const routes: Routes = [
  {
    path: RoutesConfig.routes.auth.root,
    loadChildren: () => import('./modules/auth/auth.module').then(m => m.AuthModule),
  },
  {
    path: RoutesConfig.routes.converter.root,
    loadChildren: () => import('./modules/converter/converter.module').then(m => m.ConverterModule),
  },
  { path: '**', redirectTo: RoutesConfig.routes.error404 }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
