import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpInterceptor, HttpRequest, HttpResponse, HttpErrorResponse, HttpHandler, HttpEvent } from '@angular/common/http';

import { Observable, throwError, tap } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authReq = req.clone({
        setHeaders: {
            Authorization: `Bearer ${this.auth.getToken()}`
        }
    });

    return next.handle(authReq).pipe(
      tap(
        event => {
          if (event instanceof HttpResponse)
            console.log('Server response')
        },
        err => {
          if (err instanceof HttpErrorResponse) {
            if (err.status == 401)
              console.log('Unauthorized')
          }
        }
      )
    )
  }
}