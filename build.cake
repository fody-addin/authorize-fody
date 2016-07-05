var target = Argument("target", "Default");
var npi = EnvironmentVariable("npi");

#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=xunit.runner.console&version=2.1.0"
// #tool "nuget:?package=xunit.runners&version=1.9.2"

Task("test")
    .Does(() => {
        //XBuild("Authorize.Fody.sln");
        var settings = new XUnit2Settings {
            ToolPath = @"tools\xunit.runner.console\tools\xunit.console.x86.exe"
        };
        XUnit2("./Authorize.Fody.Tests/bin/Debug/Authorize.Fody.Tests.dll", settings);
});

Task("push")
    .Description("Push nuget")
    .Does(() => {
        var nupkg = new DirectoryInfo("./NugetBuild").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
    });

Task("build")
    .Does(() => {
        DotNetBuild("Authorize.Fody.sln", settings =>  {
            settings.SetConfiguration("Release")
                .WithTarget("Rebuild");
        });
    });

RunTarget(target);