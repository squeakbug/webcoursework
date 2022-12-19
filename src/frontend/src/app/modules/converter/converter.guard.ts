import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Route } from "@angular/router";
import { catchError, Observable, map, of } from "rxjs";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { AuthService } from "../core/services/auth.service";
import { FullRoutes } from "src/app/configs/routes.config";
import { ConverterService } from "../core/services/converter.service";

@Injectable()
export class ConverterGuard implements CanActivate {
 
    routes = FullRoutes;
    constructor(private cvtService: ConverterService, private router: Router) {

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) : Observable<boolean> | boolean {
        return this.cvtService.isSignedIn().pipe(
            map((isSignedIn) => {
                if (!isSignedIn) {
                    this.router.navigate([`${this.routes.login}`]);
                    return false;
                }
                return true;
            })
        );
    }
}