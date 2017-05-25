Task("NuGet")    
    .Does(() =>
{
    NuGetRestore("../Src/WEMMS.sln");
});

Task("Default")
  .IsDependentOn("NuGet")
  .Does(() =>
{
  MSBuild("../Src/WEMMS.sln");
});

RunTarget("Default");