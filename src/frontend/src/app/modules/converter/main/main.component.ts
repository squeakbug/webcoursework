import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { FullRoutes } from 'src/app/configs/routes.config';
import { Configuration } from '../../core/models/configuration.model';
import { Convertion } from '../../core/models/convertion.model';
import { Font } from '../../core/models/font.model';
import { ConverterService } from '../../core/services/converter.service';

@Component({
    selector: 'app-converter-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.css'],
    providers: [ ConverterService ]
})
export class MainComponent implements OnInit {

    routes = FullRoutes;
    isMain = true;
    selectedFont = new Font();
    fonts: Font[] = [];
    template: string = "";
    selectedConfig = new Configuration();
    configs: Configuration[] = [];
    selectedCvt = new Convertion();
    cvts: Convertion[] = [];
    header: string = "";
    body: string = "";
    radio: string = "1";
    saveCvtName = "";
    headerFragmentName = "header";
    mainAddr = this.routes.main;
    constructor(private router: Router, public cvtService: ConverterService) {
        
    }

    ngOnInit(): void {
        this.cvtService.getFonts().subscribe({
            next: (data: any) => { 
                this.fonts = data;
                if (this.fonts.length > 0) {
                    this.selectedFont = this.fonts[0];
                }
            },
            error: error => { 
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
        
        this.cvtService.getConfigurations().subscribe({
            next: (data: any) => {
                this.configs = data;
                if (this.configs.length > 0) {
                    this.selectedConfig = this.configs[0];
                }
            },
            error: error => { 
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                } 
            }
        });

        this.cvtService.getConvertations().subscribe({
            next: (data: any) => { 
                this.cvts = data;
                if (this.cvts.length > 0) {
                    this.selectedCvt = this.cvts[0];
                }
            },
            error: error => { 
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
    }

    onConvertClicked(): void {
        if (this.radio == "1") {
            this.cvtService.convert(this.selectedFont.id, this.selectedConfig.id, this.template).subscribe({
                next: (data: any) => { 
                    this.body = data.body;
                    this.header = data.head;
                },
                error: error => { 
                    console.log(error);
                    if (error instanceof HttpErrorResponse) {
                        alert(error.statusText);
                    }
                }
            });
        } else {
            console.log(this.selectedCvt);
            this.cvtService.getConvertionById(this.selectedCvt.id).subscribe({
                next: (data: any) => { 
                    this.body = data.body;
                    this.header = data.head;
                },
                error: error => {
                    console.log(error);
                    if (error instanceof HttpErrorResponse) {
                        alert(error.statusText);
                    }
                }
            });
        }
    }

    onChangeConfigClicked() {
        this.router.navigate([`${this.routes.configurations}/` + this.selectedConfig.id]);
    }

    onSaveConvertionClicked() {
        let cvt = new Convertion();
        cvt.head = this.header;
        cvt.body = this.body;
        cvt.name = this.saveCvtName;
        this.cvtService.createConvertion(cvt).subscribe({
            next: (data: any) => {
                // Запрос отправляется для получения идентификтора конвертации
                this.cvtService.getConvertations().subscribe({
                    next: (data: any) => { 
                        this.cvts = data;
                    },
                    error: error => { 
                        console.log(error);
                        if (error instanceof HttpErrorResponse) {
                            alert(error.statusText);
                        }
                    }
                });
            }
        });
    }
}