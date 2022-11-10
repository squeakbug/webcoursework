import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import {Observable, throwError} from 'rxjs';
import {map, catchError} from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';
  
@Injectable()
export class AuthService {
     
    constructor(private http: HttpClient){ }
         
    getUsers() : Observable<User[]> {
        return this.http.get(`${environment.api_url}/users`).pipe(map((data: any) => data),
        catchError(err => { console.log(err); return []; }));
    };
}