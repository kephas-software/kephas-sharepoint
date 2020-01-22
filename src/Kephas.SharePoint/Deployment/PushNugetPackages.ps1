param (
    [string]$version = $( Read-Host "Please provide package version" ),
    [string]$build = "Release"
)

$packages = @(
    "Kephas.SharePoint.Core",
    "Kephas.SharePoint.Documents",
    "Kephas.SharePoint.Data"
)

foreach ($package in $packages) {
    $packagepath = "..\$package\bin\$build\$package.$version.nupkg"
    .\NuGet.exe push $packagepath -Source https://api.nuget.org/v3/index.json 
}