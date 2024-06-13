$theProject = "VidiView.Api.DataModel.csproj"

# Load the .csproj file as an XML document
$xml = [Xml] (Get-Content $theProject)
if ($xml -eq $null) {
    Write-Host "Failed to load the .csproj file as an XML document"
    exit
}

# Find the Version node
$versionNode = $xml.SelectSingleNode('//Version')
if ($versionNode -eq $null) {
    Write-Host "Version tag not found in the .csproj file."
    exit
}

# Get the version as a string
$version = $versionNode.'#text'
Write-Host "Current version: $version"

# Split the version number into parts
$versionParts = $version.Split(".")
# Increment the build number (the third part of the version number)
$versionParts[2] = [int]$versionParts[2] + 1
# Join the version parts back together
$newVersion = $versionParts -join '.'
Write-Host "New version: $newVersion"
Write-Host "Data type of newVersion: $($newVersion.GetType().FullName)"

# Update the Version node
#$versionNode.InnerText = $newVersion
# Update the <Version> tag in the XML document
$xml.Project.PropertyGroup[0].Version = $newVersion

# Save the updated .csproj file
$xml.Save($theProject)
