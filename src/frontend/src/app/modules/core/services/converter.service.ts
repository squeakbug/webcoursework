import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, throwError, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../models/user.model';
import { Configuration } from '../models/configuration.model';
import { Font } from '../models/font.model';
import { Convertion } from '../models/convertion.model';
import { DoConvertionResponse } from '../models/do-convertion-response.model';
  
@Injectable()
export class ConverterService {
     
    constructor(private http: HttpClient){ }

    convert(fontId: number, configId: number, template: string): Observable<DoConvertionResponse> {
        let uri = `${environment.api_url}/convertions/from-template?fontId=` + fontId
            + "&configId=" + configId + "&template=" + template;
        return this.http.get(uri).pipe(map((data: any) => {
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    getConfigurationById(id: number):  Observable<Configuration> {
        return this.http.get(`${environment.api_url}/configurations/` + id).pipe(map((data: any) => {
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    getFontById(id: number) {
        return this.http.get(`${environment.api_url}/fonts/` + id).pipe(map((data: any)=>{
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    getConfigurations():  Observable<Configuration[]> {
        return this.http.get(`${environment.api_url}/configurations`).pipe(map((data:any)=>{
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    createConfig(config: Configuration) {
        console.log("createConfig", config);
        return this.http.post(`${environment.api_url}/configurations`, config);
    }

    deleteConfigById(id: number) {
        return this.http.delete(`${environment.api_url}/configurations/` + id);
    }

    updateConfigById(config: Configuration) {
        console.log("updateConfifById", config);
        return this.http.put(`${environment.api_url}/configurations/` + config.id, config);
    }

    getFonts(): Observable<Font[]> {
        return this.http.get(`${environment.api_url}/fonts`).pipe(map((data:any)=>{
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    getConvertations(): Observable<Convertion[]> {
        return this.http.get(`${environment.api_url}/convertions`).pipe(map((data:any)=>{
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    getConvertionById(id: number): Observable<DoConvertionResponse> {
        return this.http.get(`${environment.api_url}/convertions/` + id).pipe(map((data: any)=>{
            return data;
        }),
        catchError(err => {
            console.log(err);
            return [];
        }));
    }

    createConvertion(cvt: Convertion) {
        return this.http.post(`${environment.api_url}/convertions`, cvt);
    }

    public isSignedIn(): Observable<boolean> {
        return this.http.get(`${environment.api_url}/fonts`).pipe(map((data: any) => {
            return true;
        }),
        catchError(error => {
            return of(false);
        }));
    }
}