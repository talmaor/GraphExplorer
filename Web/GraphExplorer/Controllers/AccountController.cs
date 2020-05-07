/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/


using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace GraphExplorer.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().
                    Authentication.Challenge(new AuthenticationProperties 
                        {RedirectUri = Url.Action(nameof(HomeController.Index), "Home")}, 
                        OpenIdConnectAuthenticationDefaults.AuthenticationType);

            }
        }

        [HttpGet]
        public void SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignOut), "Account");

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        [HttpGet]
        public void About()
        {

        }
    }
}