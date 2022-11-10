import { Component } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { Subscription } from 'rxjs';

import { FullRoutes } from 'src/app/configs/routes.config';

@Component({
    selector: 'app-converter-configurations',
    templateUrl: './configurations.component.html',
    styleUrls: ['./configurations.component.css']
})
export class ConfigurationsComponent {

    routes = FullRoutes;
    id: number | undefined;
    private subscription: Subscription;
    constructor(private activateRoute: ActivatedRoute) {

        this.subscription = activateRoute.params.subscribe(params => this.id=params['id']);
    }
}