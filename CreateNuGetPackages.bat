SETLOCAL

set "module=Fellow.Epi.ImagePlaceholder"
set output="../ModulePackages/%module%"

rmdir /s /q %output%
mkdir %output%

nuget pack "Fellow.Epi.ImagePlaceholder\Fellow.Epi.ImagePlaceholder.csproj" -Build -OutputDirectory %output% -Prop Configuration=Release;Platform=AnyCPU