/// <reference path="../../../../node_modules/angular2-in-memory-web-api/typings/browser.d.ts" />

import {bootstrap}    from '@angular/platform-browser-dynamic';
import {NewsFeedApp} from './NewsFeedApp';
import {enableProdMode} from '@angular/core';
import {HTTP_PROVIDERS} from '@angular/http';
import {HttpInterceptor} from '../Common/httpinterceptor';

enableProdMode();
bootstrap(NewsFeedApp, [HTTP_PROVIDERS, HttpInterceptor]);