import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { FullRoutes } from 'src/app/configs/routes.config';
import { AuthService } from 'src/app/modules/core/services/auth.service'

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.css'],
    providers: [AuthService]
})
export class SignUpComponent {
    routes = FullRoutes;
    login:string = "";
    password = "";
    repPassword = "";
    constructor(private router: Router, private authService: AuthService) {
        
    }

    onRegistrateClicked() {
        this.authService.registrate(this.login, this.password, this.repPassword).subscribe({
            next:(data: any) => { 
                this.router.navigate([`${this.routes.login}`]);
                alert("Успешная регистрация")
            },
            error: error => {
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
    }

    onGoBackClicked() {
        this.router.navigate([`${this.routes.login}`]);
    }
}