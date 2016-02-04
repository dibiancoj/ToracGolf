import {bootstrap}    from 'angular2/platform/browser'
import {NewsFeedApp} from './NewsFeedApp'
import {enableProdMode} from 'angular2/core';
import {HTTP_PROVIDERS} from 'angular2/http';
import { HttpInterceptor } from '../Common/httpinterceptor';


enableProdMode();
bootstrap(NewsFeedApp, [HTTP_PROVIDERS, HttpInterceptor]);