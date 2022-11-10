import { Component } from '@angular/core';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-converter-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.css']
})
export class MainComponent {

    routes = FullRoutes;
    constructor() {
        
    }
}