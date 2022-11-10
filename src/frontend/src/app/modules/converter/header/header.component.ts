import { Component, Input } from '@angular/core';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-converter-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css']
})
export class HeaderComponent {

    routes = FullRoutes;
    constructor() {
        
    }
}