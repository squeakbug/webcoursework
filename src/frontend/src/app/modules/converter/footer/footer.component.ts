import { Component } from '@angular/core';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-converter-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.css']
})
export class FooterComponent {

    routes = FullRoutes;
    constructor() {
        
    }
}