# Facebook C# SDK Sample

    Install-Package Facebook.Sample.VB

## Overview
This sample contains simple usage on how to make requests to Facebook 
using the [Facebook C# SDK](http://facebooksdk.codeplex.com) in VB.
(For C# equivalent please checkout "Facebook.Sample" nuget package instead.)

Facebook.Sample.VB nuget package contains variety of codes on helping you 
get started with the new graph api, old legacy rest api, fql and batch
requests.

## Getting Started
There are several source files included in this sample. They have been
categoried according to the type of api.

Each sample file contains a public static method called **RunSamples** 
which allows you to easily start the samples in a particular file. 
You can continue to run individual sample by calling any particular methods.

Note: 
* Most of the samples requires an access_token. If you would like to get
  an access_token you can try installing Facebook.Sample.Winforms.Login.VB nuget
  package.
* You need user_about_me and publish_stream to run most of the samples.
* Certain samples writes on your facebook wall.

	Samples.Facebook.VB.GraphApiSamples.RunSamples(accessToken);
	Samples.Facebook.VB.LegacyRestApiSamples.RunSamples(accessToken);
	Samples.Facebook.VB.FQLSamples.RunSamples(accessToken);
	Samples.Facebook.VB.BatchRequestsSamples.RunSamples(accessToken);

You can also run the samples individually by calling the approprite methods.

	Samples.Facebook.VB.GraphApiSamples.GetSampleWithoutAccessToken();
	Samples.Facebook.VB.GraphApiSamples.GetSampleWithAccessToken(accessToken);
	Samples.Facebook.VB.LegacyRestApiSamples.PostToMyWall(accessToken, "hello world");

Incase your are using .net 4.0 or any compiler that supports *dynamic* keyword,
you might want to try out *Facebook.Sample.Dynamic.VB* nuget package instead.

## Facebook C# SDK related nuget sample packages

	Install-Package Facebook.Sample.Dynamic.VB
	Install-Package Facebook.Sample.Winforms.Login.VB

## License
All source codes included in this sample are distributed under 
[Microsoft Public License (Ms-PL)](http://facebooksdk.codeplex.com/license).
