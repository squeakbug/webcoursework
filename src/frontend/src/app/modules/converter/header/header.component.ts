import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { FullRoutes } from 'src/app/configs/routes.config';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-converter-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    providers: [ AuthService ]
})
export class HeaderComponent implements OnInit {

    @Input() IsMain: boolean = false;
    userName: string = "";
    routes = FullRoutes;
    constructor(private router: Router, private authService: AuthService) {

    }

    ngOnInit(): void {
        let storageName = localStorage.getItem('currentUser');
        this.userName = storageName == null ? "" : storageName;
    }

    onBackClicked() {
        this.router.navigate([`${this.routes.main}`]);
    }

    onExitClicked() {
        this.authService.logout().subscribe({
            next: (data: any) => {
                this.router.navigate([`${this.routes.login}`]);  
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