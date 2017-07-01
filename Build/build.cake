Task("NuGet")    
    .Does(() =>
{
    NuGetRestore("../Src/WemStudio.sln");
});

Task("Default")
  .IsDependentOn("NuGet")
  .Does(() =>
{
  MSBuild("../Src/WemStudio.sln");
});

RunTarget("Default");