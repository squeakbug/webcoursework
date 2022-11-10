import { Component } from '@angular/core';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LogInComponent {

    routes = FullRoutes;
    constructor() {
        
    }
}