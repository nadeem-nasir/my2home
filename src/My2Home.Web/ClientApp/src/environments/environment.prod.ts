/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
export const environment =
{
  production: true,
  baseApiUrl: "http://www.hostelzhub.com/api/"
};

/* AuthConfig using angular-oauth2-oidc by Manfred Steyer*/
/*https://github.com/manfredsteyer/angular-oauth2-oidc */

import { AuthConfig } from "angular-oauth2-oidc";
export const authConfig: AuthConfig = {

  tokenEndpoint: "http://www.hostelzhub.com/connect/token",
  // Url of the Identity Provider
  issuer: "http://www.hostelzhub.com/",
  requireHttps: false,

  // URL of the SPA to redirect the user to after login
  redirectUri: "http://www.hostelzhub.com/signin-oidc",

  // URL of the SPA to redirect the user after silent refresh
  silentRefreshRedirectUri: "http://www.hostelzhub.com/silent-refresh.html",

  // The SPA's id. The SPA is registerd with this id at the auth-server
  clientId: "My2Homespaappcorecore",

  // options: {},

  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: "openid profile email offline_access roles",

  showDebugInformation: true,

  sessionChecksEnabled: true,
  oidc: false
};
