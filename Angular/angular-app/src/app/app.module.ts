import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthModule, LogLevel, OidcConfigService } from 'angular-auth-oidc-client';
import { HttpClientModule } from '@angular/common/http';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
    oidcConfigService.withConfig({
      clientId: 'angular',
      //stsServer: 'http://localhost:5000',
      //stsServer: 'https://localhost:44305',
      stsServer: 'http://localhost:81/identityserver',
      responseType: 'code',
      redirectUrl: window.location.origin +'/angularclient',
      postLogoutRedirectUri: window.location.origin + '/angularclient',
      scope: 'openid ApiOne',
      logLevel: LogLevel.Debug,
    });
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule.forRoot(),
    HttpClientModule
  ],
  providers: [
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService],
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
