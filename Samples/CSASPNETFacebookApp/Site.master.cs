using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook.Web;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        NavigationMenu.Items[0].NavigateUrl = this.CanvasUrlBuilder.BuildCanvasPageUrl("/Default.aspx").AbsoluteUri;
    }

    protected CanvasUrlBuilder CanvasUrlBuilder = new CanvasUrlBuilder();
}
