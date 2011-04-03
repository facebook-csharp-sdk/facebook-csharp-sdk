# Facebook C# SDK Sample (Winforms Login)

    Install-Package Facebook.Sample.Winforms.Login

## Overview
This sample contains simple usage on how to authorize Facebook Application
using [Facebook C# SDK](http://facebooksdk.codeplex.com) in WinForms app.

*Note: This sample internally makes use of 
[Facebook OAuth Dialog](https://developers.facebook.com/docs/reference/dialogs/oauth/)*

## Getting Started

To run this sample, you will require a Facebook Application Id (AppId) which
is also referred to as ClientId in OAuth2 terminology. If you don't have one,
you can register a new application at 
[https://www.facebook.com/developers/createapp.php](https://www.facebook.com/developers/createapp.php)

You can then run the sample using the following code.

	Samples.Facebook.Winforms.SampleRunner.RunSample("app_id", null);

*Make sure to replace the app_id with your appropriate Facebook Application ID.*

Incase you are running from console application make sure to add the STAThread attribute in
the Main method.

	[STAThread]
	static void Main(string[] args)
	{
	}

## Extended Permissions
If you would like to ask for extended permissions (also reffered to as scope
in OAuth2), make sure to pass an array of strings as the second parameter.

	Samples.Facebook.Winforms.SampleRunner.RunSample("app_id", 
			new[] { "user_about_me", "publish_stream", "offline_access" });

You can learn more about extended permissions at 
[https://developers.facebook.com/docs/authentication/permissions/](https://developers.facebook.com/docs/authentication/permissions/)

## Facebook C# SDK related nuget sample packages

	Install-Package Facebook.Sample
	Install-Package Facebook.Sample.Dynamic

## License
All source codes included in this sample are distributed under 
[Microsoft Public License (Ms-PL)](http://facebooksdk.codeplex.com/license).