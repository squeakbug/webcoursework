import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { ConverterRoutingModule } from './converter-routing.module';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { MainComponent } from './main/main.component';
import { ConfigurationsComponent } from './configurations/configurations.component';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    MainComponent,
    ConfigurationsComponent
  ],
  imports: [
    ConverterRoutingModule,
    FormsModule,
    HttpClientModule,
    CommonModule
  ],
  providers: [],
})
export class ConverterModule { }
