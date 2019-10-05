param (
    [string]$version = $( Read-Host "Please provide package version" ),
    [string]$build = "Release",
    [string]$CertificateSubjectName = "Kephas Software SRL",
    [string]$Timestamper = "http://timestamp.digicert.com"
)

# The symbol source is changed since Nov. 2018
# https://blog.nuget.org/20181116/Improved-debugging-experience-with-the-NuGet-org-symbol-server-and-snupkg.html
# however, a bug is still present - until it is fixed stay with the old symbols


function get-packagename([string]$pathname) {
    return $pathname.Replace("..\", "").Replace("TestingFramework\", "")
}

$paths = @(
    "..\Kephas.SharePoint.Core"
)

foreach ($path in $paths) {
    $packagename = get-packagename $path
    $packagepath = "$path\bin\$build\$packagename.$version.nupkg"
    .\NuGet.exe sign "$packagepath" -CertificateSubjectName "$CertificateSubjectName" -Timestamper "$Timestamper"
}
