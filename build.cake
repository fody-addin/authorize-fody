var target = Argument("target", "Default");
var npi = EnvironmentVariable("npi");

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