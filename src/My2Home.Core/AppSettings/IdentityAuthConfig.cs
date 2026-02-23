using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Core.AppSettings
{
    public interface IIdentityAuthConfig
    {

    }
    public class IdentityAuthConfig : IIdentityAuthConfig
    {
        [JsonProperty("tokenEndpoint")]
        public string TokenEndpoint { get; set; }
        public string Env { get; set; }
        public string BaseUrl { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("requireHttps")]
        public bool RequireHttps { get; set; }
        [JsonProperty("redirectUri")]
        public string RedirectUri { get; set; }
        [JsonProperty("silentRefreshRedirectUri")]
        public string SilentRefreshRedirectUri { get; set; }
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("showDebugInformation")]
        public bool ShowDebugInformation { get; set; }
        [JsonProperty("sessionChecksEnabled")]
        public bool SessionChecksEnabled { get; set; }
        //public AuthConfigSettings AuthConfigSettings { get; set; }
    }

    //public class AuthConfigSettings
    //{
    //    [JsonProperty("issuer")]
    //    public string Issuer { get; set; }
    //    [JsonProperty("requireHttps")]
    //    public bool RequireHttps { get; set; }
    //    [JsonProperty("redirectUri")]
    //    public string RedirectUri { get; set; }
    //    [JsonProperty("silentRefreshRedirectUri")]
    //    public string SilentRefreshRedirectUri { get; set; }
    //    [JsonProperty("clientId")]
    //    public string ClientId { get; set; }
    //    [JsonProperty("scope")]
    //    public string Scope { get; set; }
    //    [JsonProperty("showDebugInformation")]
    //    public bool ShowDebugInformation { get; set; }
    //    [JsonProperty("sessionChecksEnabled")]
    //    public bool SessionChecksEnabled { get; set; }

    //}
}
