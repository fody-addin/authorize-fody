## Authorize.Fody

Add `[Authorize]` attribute if method has `[HttpGet]` or `[HttpPost]` and method not return `System.Net.Http.HttpResponseMessage`.

## Install

> Install-Package Authorize.Fody

## Build

> build.cmd -target build

## Test

> build.cmd -target test

## Publish nuget package

> build.cmd -target push
