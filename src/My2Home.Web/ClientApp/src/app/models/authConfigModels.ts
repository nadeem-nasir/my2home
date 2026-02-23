export interface IAuthConfigSettings
{
  issuer: string;
  requireHttps: boolean;
  redirectUri: string;
  silentRefreshRedirectUri: string;
  clientId: string;
  scope: string;
  showDebugInformation: boolean;
  sessionChecksEnabled: boolean;
  tokenEndpoint: string;
}

/* User model*/
export interface User {
  displayName: string;
  roles: string[];
}

/*end user model */

/* Register user model*/

export interface RegisterModel {
  //userName: string;
  password: string;
  confirmPassword: string;
  email: string;
  fullName: string;
  countryId: number;
}

export interface StaffRegisterModel
{


}
