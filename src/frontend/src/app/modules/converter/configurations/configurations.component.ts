import { Component } from '@angular/core';
import { NgSwitchDefault } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

import { FullRoutes } from 'src/app/configs/routes.config';
import { ConverterService } from '../../core/services/converter.service';
import { Configuration } from '../../core/models/configuration.model';

@Component({
    selector: 'app-converter-configurations',
    templateUrl: './configurations.component.html',
    styleUrls: ['./configurations.component.css']
})
export class ConfigurationsComponent implements OnInit {

    routes = FullRoutes;
    isMain = false;
    mainAddr = this.routes.configurations;
    headerFragmentName = "header";

    id: number = 0;
    commentVariableName: string = "false";
    commentCharVisualizer: string = "false";
    commentCharDescriptor: string = "false";
    commentStyle: string = "";
    bmpVisualizerChar: string = "";
    rotation: string = "";
    flipHorizontal: string = "false";
    flipVertical: string = "false";
    paddingRemovalHorizontal: string = "";
    paddingRemovalVertical: string = "";
    bitLayout: string = "";
    byteOrder: string = "";
    byteFormat: string = "";
    generateLookupArray: string = "false";
    generateSpaceCharacterBitmap: string = "false";
    spaceGenerationPixels: string = "0";
    minHeight: string = "0";
    varNfBitmaps: string = "";
    varNfCharInfo: string = "";
    varNfFontInfo: string = "";
    varNfWidth: string = "";
    varNfHeight: string = "";
    displayName: string = "";
    commentStartString: string = "";
    commentBlockEndString: string = "";
    commentBlockMiddleString: string = "";
    commentEndString: string = "";

    private routeSubscription: Subscription;
    private querySubscription: Subscription;
    constructor(private router: Router, private cvtService: ConverterService, private route: ActivatedRoute) {       
        this.routeSubscription = route.params.subscribe(params => this.id = Number(params['id']));
        this.querySubscription = route.queryParams.subscribe(
            (queryParam: any) => {
                this.commentVariableName = queryParam['commentVariableName'] ?? false;
                this.commentCharVisualizer = queryParam['commentCharVisualizer'] ?? false;
                this.commentCharDescriptor = queryParam['commentCharDescriptor'] ?? false;
                this.commentStyle = queryParam['commentStyle'] ?? "";
                this.bmpVisualizerChar = queryParam['bmpVisualizerChar'] ?? "";
                this.rotation = queryParam['rotation'] ?? "";
                this.flipHorizontal = queryParam['flipHorizontal'] ?? false;
                this.flipVertical = queryParam['flipVertical'] ?? false;
                this.paddingRemovalHorizontal = queryParam['paddingRemovalHorizontal'] ?? "";
                this.paddingRemovalVertical = queryParam['paddingRemovalVertical'] ?? "";
                this.bitLayout = queryParam['bitLayout'] ?? "";
                this.byteOrder = queryParam['byteOrder'] ?? "";
                this.byteFormat = queryParam['byteFormat'] ?? "";
                this.generateLookupArray = queryParam['generateLookupArray'] ?? false;
                this.generateSpaceCharacterBitmap = queryParam['generateSpaceCharacterBitmap'] ?? false;
                this.spaceGenerationPixels = queryParam['spaceGenerationPixels'] ?? 0;
                this.minHeight = queryParam['minHeight'] ?? 0;
                this.varNfBitmaps = queryParam['varNfBitmaps'] ?? "";
                this.varNfCharInfo = queryParam['varNfCharInfo'] ?? "";
                this.varNfFontInfo = queryParam['varNfFontInfo'] ?? "";
                this.varNfWidth = queryParam['varNfWidth'] ?? "";
                this.varNfHeight = queryParam['varNfHeight'] ?? "";
                this.displayName = queryParam['displayName'] ?? "";
                this.commentStartString = queryParam['commentStartString'] ?? "";
                this.commentBlockEndString = queryParam['commentBlockEndString'] ?? "";
                this.commentBlockMiddleString = queryParam['commentBlockMiddleString'] ?? "";
                this.commentEndString = queryParam['commentEndString'] ?? "";
            }
        );
    }

    ngOnInit(): void {
        this.cvtService.getConfigurationById(this.id).subscribe({
            next: (data: any) => {
                this.commentVariableName = data['commentVariableName'] ?? "false";
                this.commentCharVisualizer = data['commentCharVisualizer'] ?? "false";
                this.commentCharDescriptor = data['commentCharDescriptor'] ?? "false";
                this.commentStyle = data['commentStyle'] ?? "";
                this.bmpVisualizerChar = data['bmpVisualizerChar'] ?? "";
                this.rotation = data['rotation'] ?? "";
                this.flipHorizontal = data['flipHorizontal'] ?? "false";
                this.flipVertical = data['flipVertical'] ?? "false";
                this.paddingRemovalHorizontal = data['paddingRemovalHorizontal'] ?? "";
                this.paddingRemovalVertical = data['paddingRemovalVertical'] ?? "";
                this.bitLayout = data['bitLayout'] ?? "";
                this.byteOrder = data['byteOrder'] ?? "";
                this.byteFormat = data['byteFormat'] ?? "";
                this.generateLookupArray = data['generateLookupArray'] ?? false;
                this.generateSpaceCharacterBitmap = data['generateSpaceCharacterBitmap'] ?? false;
                this.spaceGenerationPixels = data['spaceGenerationPixels'] ?? "0";
                this.minHeight = data['minHeight'] ?? "0";
                this.varNfBitmaps = data['varNfBitmaps'] ?? "";
                this.varNfCharInfo = data['varNfCharInfo'] ?? "";
                this.varNfFontInfo = data['varNfFontInfo'] ?? "";
                this.varNfWidth = data['varNfWidth'] ?? "";
                this.varNfHeight = data['varNfHeight'] ?? "";
                this.displayName = data['displayName'] ?? "";
                this.commentStartString = data['commentStartString'] ?? "";
                this.commentBlockEndString = data['commentBlockEndString'] ?? "";
                this.commentBlockMiddleString = data['commentBlockMiddleString'] ?? "";
                this.commentEndString = data['commentEndString'] ?? "";
            },
            error: err => console.log(err)
        })
        this.mainAddr = this.mainAddr + `/${this.id}`;
        console.log(this.mainAddr)
    }

    onDeleteConfig() {
        this.cvtService.deleteConfigById(this.id).subscribe({});
        this.router.navigate([`${this.routes.main}`]);
    }

    getConfig(): Configuration {
        let config = new Configuration();
        config.commentVariableName = Boolean(this.commentVariableName);
        config.commentCharVisualizer = Boolean(this.commentCharVisualizer);
        config.commentCharDescriptor = Boolean(this.commentCharDescriptor);
        config.minHeight = Number(this.minHeight);
        config.spaceGenerationPixels = Number(this.spaceGenerationPixels);
        config.generateLookupArray = Boolean(this.generateLookupArray);
        config.generateSpaceCharacterBitmap = Boolean(this.generateSpaceCharacterBitmap);
        config.flipHorizontal = Boolean(this.flipHorizontal);
        config.flipVertical = Boolean(this.flipVertical)
        config.bitLayout = this.bitLayout;
        config.bmpVisualizerChar = this.bmpVisualizerChar;
        config.byteFormat = this.byteFormat;
        config.byteOrder = this.byteOrder;
        config.commentBlockEndString = this.commentBlockEndString;
        config.commentBlockMiddleString = this.commentBlockMiddleString;
        config.commentEndString = this.commentEndString;
        config.commentStartString = this.commentStartString;
        config.commentStyle = this.commentStyle;
        config.displayName = this.displayName;
        config.id = this.id;
        config.paddingRemovalHorizontal = this.paddingRemovalHorizontal;
        config.paddingRemovalVertical = this.paddingRemovalVertical;
        config.rotation = this.rotation;
        config.varNfBitmaps = this.varNfBitmaps;
        config.varNfCharInfo = this.varNfCharInfo;
        config.varNfFontInfo = this.varNfFontInfo;
        config.varNfHeight = this.varNfHeight;
        config.varNfWidth = this.varNfWidth;
        return config;
    }

    onCreateConfig() {
        let config = this.getConfig()
        this.cvtService.createConfig(config).subscribe({
            error: error => {
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
    }

    onSaveClicked() {
        let config = this.getConfig();
        this.cvtService.updateConfigById(config).subscribe({
            error: error => {
                console.log(error);
                if (error instanceof HttpErrorResponse) {
                    alert(error.statusText);
                }
            }
        });
        this.router.navigate([`${this.routes.main}`]);
    }
}