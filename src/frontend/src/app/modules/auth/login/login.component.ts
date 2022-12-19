import { NgIfContext } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { FullRoutes } from 'src/app/configs/routes.config';
import { AuthService } from 'src/app/modules/core/services/auth.service'

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [AuthService]
})
export class LogInComponent {

    routes = FullRoutes;
    login = "";
    password = "";
    constructor(private router: Router, private authService: AuthService) {
        
    }

    onLoginClicked() {
        this.authService.login(this.login, this.password).subscribe({
            next: (data: any) => {
                data.subscribe({
                    next: (userData: any) => {
                        localStorage.setItem('currentUser', userData.name)
                        this.router.navigate([`${this.routes.main}`]);
                    },
                })
            },
            error: error => { 
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
    }

    onRegistrateClicked() {
        this.router.navigate([`${this.routes.signup}`]);
    }
}