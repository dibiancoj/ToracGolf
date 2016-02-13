/// <reference path="../../../../node_modules/angular2/typings/browser.d.ts" />

import {bootstrap}    from 'angular2/platform/browser'
import {NewsFeedApp} from './NewsFeedApp'
import {enableProdMode} from 'angular2/core';
import {HTTP_PROVIDERS} from 'angular2/http';
import { HttpInterceptor } from '../Common/httpinterceptor';

enableProdMode();
bootstrap(NewsFeedApp, [HTTP_PROVIDERS, HttpInterceptor]);