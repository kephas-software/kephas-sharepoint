param (
    [string]$version = $( Read-Host "Please provide package version" ),
    [string]$build = "Release",
    [string]$apiKey = ""
)

$packages = @(
    "Kephas.SharePoint.Core",
    "Kephas.SharePoint.Documents",
    "Kephas.SharePoint.Data"
)

foreach ($package in $packages) {
    $packagepath = "..\$package\bin\$build\$package.$version.nupkg"
    if ($apiKey -eq "") {
        .\NuGet.exe push -Source https://api.nuget.org/v3/index.json $packagepath 
    }
    else {
        .\NuGet.exe push -ApiKey $apiKey -Source https://api.nuget.org/v3/index.json $packagepath 
    }
}