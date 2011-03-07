using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Facebook.Web
{
    [Obsolete("Use Facebook.Web.FacebookCanvasAuthorizer")]
    [TypeForwardedFrom("Facebook.Web, Version=4.2.1.0, Culture=neutral, PublicKeyToken=58cb4f2111d1e6de")]
    public class CanvasAuthorizer : Authorizer
    {

        private ICanvasSettings canvasSettings;

        public CanvasAuthorizer(FacebookAppBase facebookApp)
            : base(facebookApp)
        {
            this.canvasSettings = CanvasSettings.Current;
        }

        public CanvasAuthorizer(FacebookAppBase facebookApp, ICanvasSettings canvasSettings)
            : base(facebookApp)
        {
            Contract.Requires(canvasSettings != null);

            this.canvasSettings = canvasSettings;
        }

        public override void HandleUnauthorizedRequest(HttpContextBase httpContext)
        {
            throw new NotFiniteNumberException();
        }

    }
}
