import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { FullRoutes } from 'src/app/configs/routes.config';
import { AuthService } from 'src/app/modules/core/services/auth.service'

export class Alert {

    constructor() { }

    public static alert(status: number) {
        alert(status);
    }
}