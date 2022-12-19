import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-converter-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.css']
})
export class FooterComponent {

    @Input() MainAddr: string = "";
    @Input() FragmentName: string = "";
    routes = FullRoutes;
    constructor(private router: Router) {
        
    }

    onUpClicked() {
        let element = document.getElementById(this.FragmentName);
        element?.scrollIntoView();
    }
}