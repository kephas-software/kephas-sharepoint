param (
    [string]$version = $( Read-Host "Please provide package version" ),
    [string]$build = "Release",
)

$packages = @(
    "Kephas.SharePoint.Core"
)

foreach ($package in $packages) {
    $packagepath = "..\$package\bin\$build\$packagename.$version.nupkg"
    .\NuGet.exe push "$package.$version.nupkg" -Source https://api.nuget.org/v3/index.json 
    .\NuGet.exe push "$package.$version.symbols.nupkg" -Source https://nuget.smbsrc.net
}