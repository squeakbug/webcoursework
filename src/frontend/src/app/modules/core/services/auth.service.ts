import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';
  
@Injectable()
export class AuthService {

    errorMessage: String = "";
    currentUser: Observable<User> | undefined;
    constructor(private http: HttpClient){ }

    public getToken(): string | null {
        return localStorage.getItem('token');
    } 
    
    public isAuthenticated(): boolean {
        const token = this.getToken();
        return token == null;
    }

    getUsers() : Observable<User[]> {
        return this.http.get(`${environment.api_url}/users`).pipe(map((data:any) => {
            return data.map(function(user: User): User {
                return user;
            });
        }));
    };

    getUserById(id: number) : Observable<User> {
        return this.http.get(`${environment.api_url}/users/${id}`).pipe(map((data:any) => {
            return data;
        }));
    };

    login(login: string, password: string) {
        let uri = `${environment.api_url}/users/login?login=` + login + "&password=" + password;
        return this.http.get(uri).pipe(map((data: any) => {
            let x = this.http.get<User>(`${environment.api_url}/users/` + data).pipe(map((userData: any): User => {
                return userData;
            }));
            return x;
        }));
    }

    registrate(login: string, password: string, repPassword: string) {
        const body = { login: login, password: password, repPassword: repPassword };
        return this.http.post(`${environment.api_url}/users/registration`, body);
    }

    logout() {
        return this.http.get(`${environment.api_url}/users/logout`);
    }
}