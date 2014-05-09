---
layout: default
title: Contributing to the Facebook SDK for .NET
---

So you want to contribute to the Facebook SDK for .NET. Great! We can use the help and appreciate the help! Below you will find
our contribution guidelines which are nearly identical to the [NuGet Project contribution guidelines](http://docs.nuget.org/docs/contribute/contributing-to-nuget).

## Ways to Contribute

While writing code is certainly glamorous and gets all the attention, it's not the only way to contribute to the 
Facebook SDK for .NET. 
In many cases, it's not even the most **valuable** way to contribute. 
If you would like to contribute to the Facebook SDK for .NET to help the project along, consider these options 
(which are valid for any OSS project, not just the Facebook SDK for .NET):

* Improve our [documentation](/docs/contributing-documentation). Documentation often gets little love and attention in an Open Source project, but those who help with documentation receive tremendous love and kudos from the team. 
* Submit a <a title="Facebook SDK for .NET Issues" href="https://github.com/facebook-csharp-sdk/facebook-csharp-sdk/issues/new">bug report</a> (for an excellent guide on submitting good bug reports, read <a title="Painless Bug Tracking" href="http://www.joelonsoftware.com/articles/fog0000000029.html">Painless Bug Tracking</a>). 
* Submit a <a title="Facebook SDK for .NET Issues" href="https://github.com/facebook-csharp-sdk/facebook-csharp-sdk/issues/new">feature request</a>. 
* Help verify submitted fixes for bugs. 
* Help answer questions on [Stackoverflow](http://stackoverflow.com/questions/tagged/facebook-c%23-sdk)
* Submit a unit test to help provide code coverage. 
* Tell others about the project. 
* Tell the developers how much you appreciate the product! 

## Contributing Code

Contributing code refers to making a contribution to the source code for Facebook SDK for .NET itself.

Miguel de Icaza has a good post 
on <a href="http://tirania.org/blog/archive/2010/Dec-31.html">Open Source Contribution Etiquette</a> that is worth reading, 
as the guidance he gives applies well to the Facebook SDK for .NET.

### Getting Started

The first order of business is to get yourself familiar with the product (and depending on the type of contribution you wish to make, the code).

1. Download the latest release and try the product out.
2. Read up on the docs at [facebooksdk.net](http://facebooksdk.net).
3. Get your [development environment set up](/docs/setting-up-the-development-environment). 
4. Familiarize yourself with the source code. Make sure you can build it. 
5. Consider answering questions on Stackoverflow, to build on your understanding of the code. 
6. Familiarize yourself with the [project guidelines](/docs/project-guidelines) and our [coding conventions](/docs/coding-guidelines). Following the coding conventions is very important to us as we're very picky about them out of necessity in order to maintain consistency. We know that sometimes, people will make a small mistake here and there and if it's a really nitpicky thing, we'll accept the pull request anyways and just fix the code ourselves. 

### Contributing a Bug Fix or Feature

Now that you've read through our **Getting Started** section above and have set up your development environment accordingly, you're ready to start 
contributing code! Please follow the following steps each time you take on a feature or bug fix. 
Note that some of these steps are unnecessary for small changes.

1. Decide what feature or bug fix you plan to take on and start a discussion with the title of the bug so we know someone is already working on it. e.g. "I'm going to fix issue 59: Something's Broken." If you're just starting out, pick something small to fix such as:
    * Add a missing unit test
    * Search for a // TODO comment in the code and address one 
    * Fix a defect in the <a title="Issue List" href="https://github.com/facebook-csharp-sdk/facebook-csharp-sdk/issues?milestone=&sort=updated&state=open">issue list</a> or just find one with the &ldquo;Proposed&rdquo; status). Try something small first and work your way up to larger issues. 
2. Create a server-side Fork of the Facebook SDK for .NET project in Github.  
Navigate to the <a title="Facebook SDK for .NET Source Code" href="https://github.com/facebook-csharp-sdk/facebook-csharp-sdk">Source Code</a> and 
click **Fork** to create the remote clone of the main repository. The MvcContrib project has 
<a title="How to contribute to MvcContrib" href="http://mvccontrib.codeplex.com/wikipage?title=HowToContribute&amp;referringTitle=T4MVC_contrib">
a great write-up of this process.</a>
3. Clone the fork you created in the previous step to your machine. 
4. Run build.cmd from the command line to ensure packages are restored. 
5. Make the relevant changes in your local clone (potentially adding a unit test if this is a bug fix).
6. Run build.cmd from the command line and make sure that there are 0 errors. 
7. Commit your changes in your local clone. You may end up repeating steps 4-6 multiple times as you work. When you are finished and ready to have us accept 
your change, go to step 7. 
8. Pull from master and merge your changes with the latest from master (fix any merge conflicts you might have).
9. Send a pull request and make sure the summary contains **relevant bug numbers** and **a good description of your changes**.
10. If you need to revise your code, then do so locally and update the review. Repeat until we approve the review. 
11. Wait for your review to be approved. We'll try to get to it as soon as possible. 
12. Push to your server fork and send a pull request. Note, you can push to your server fork at any 
time if you want to backup your code on the server, but don't send the pull request until your review is approved.
 
> If you're contributing a new non-trivial feature, we will ask you to fill out a Contributor License Agreement 
form before we merge your change into the core. It doesn't take long and you can email us the form. 
There's no need to deal with stamps and sending anything over snail mail.

## General Guidelines

**Please only contribute code which you wrote or have the rights to contribute.** Do not contribute GPL code as our code is licensed under the Apache 2 license.