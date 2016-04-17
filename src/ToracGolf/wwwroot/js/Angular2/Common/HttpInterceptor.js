System.register(['angular2/core', 'angular2/http', 'rxjs/add/operator/map'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, http_1;
    var HttpInterceptor;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (_1) {}],
        execute: function() {
            HttpInterceptor = (function () {
                function HttpInterceptor(http) {
                    this.HttpModule = http;
                }
                HttpInterceptor.prototype.Post = function (url, body) {
                    var _this = this;
                    //show the spinner
                    this.ShowAjaxSpinner(true);
                    //grab the query so we can fork this (i'm hooking into the map to hide the show dialog)...using multiple subscribe caused 2 ajax calls to be made.
                    //now sure if this is the best way to do this. It works though. Couldn't find much documentation on how to do this. Basically fork and run multiple methods
                    //off an observable. 
                    return this.HttpModule.post(url, body, { headers: this.CustomHeaderSelect() })
                        .map(function (res) { return res.json(); })
                        .map(function (x) {
                        _this.ShowAjaxSpinner(false);
                        return x;
                    });
                };
                //private ErrorHandler(err) {
                //    alert('Ajax Angular 2 Error');
                //    alert(JSON.stringify(err));
                //}
                HttpInterceptor.prototype.ShowAjaxSpinner = function (showSpinner) {
                    if (showSpinner) {
                        $("#loadingDiv").show();
                    }
                    else {
                        $("#loadingDiv").hide();
                    }
                };
                HttpInterceptor.prototype.CustomHeaderSelect = function () {
                    var customHeaders = new http_1.Headers();
                    customHeaders.append('RequestVerificationToken', $('#__RequestVerificationToken').val());
                    customHeaders.append('Content-Type', 'application/json');
                    return customHeaders;
                };
                HttpInterceptor = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [http_1.Http])
                ], HttpInterceptor);
                return HttpInterceptor;
            }());
            exports_1("HttpInterceptor", HttpInterceptor);
        }
    }
});
//# sourceMappingURL=httpinterceptor.js.map