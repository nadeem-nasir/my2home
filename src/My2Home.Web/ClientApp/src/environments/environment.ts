/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  baseApiUrl:"http://localhost:57678/api/"
};


/* AuthConfig using angular-oauth2-oidc by Manfred Steyer*/
/*https://github.com/manfredsteyer/angular-oauth2-oidc */

import { AuthConfig } from "angular-oauth2-oidc";
export const authConfig: AuthConfig = {
  // Url of the Identity Provider
  tokenEndpoint: "http://localhost:57678/connect/token",
  
  issuer: "http://localhost:57678/",
  requireHttps: false,

  // URL of the SPA to redirect the user to after login
  redirectUri: "http://localhost:57678/signin-oidc",

  // URL of the SPA to redirect the user after silent refresh
  silentRefreshRedirectUri: "http://localhost:57678/silent-refresh.html",

  // The SPA's id. The SPA is registerd with this id at the auth-server
  clientId: "My2Homespaappcorecore",

  // options: {},

  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: "openid profile email offline_access roles",

  showDebugInformation: true,

  sessionChecksEnabled: true,
  oidc:false
};
