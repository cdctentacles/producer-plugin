[CmdletBinding(PositionalBinding = $false)]
param (
    [Parameter(Mandatory=$false)]
    [Switch] $RestoreOnly,
    [Parameter(Mandatory=$false)]
    [string] $outputPath
)

if($RestoreOnly)
{
dotnet restore --packages "..\packages"
}
else
{
# First restore then, build
if([string]::IsNullOrEmpty($outputPath))
{
dotnet restore --packages "..\packages"
dotnet build "ProducerPlugin.csproj"
}
else
{
dotnet restore --packages "..\packages"
dotnet build "ProducerPlugin.csproj" --output $outputPath
}
}