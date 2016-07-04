## Authorize.Fody

Add `[Authorize]` attribute if method has `[HttpGet]` or `[HttpPost]`.
** Target method must not return `Sysstem.Net.Http.HttpResponseMessage`

## Instsall

> Install-Package Authorize.Fody

## Build

> build.cmd -target build

## Publish nuget package

> build.cmd -target push
