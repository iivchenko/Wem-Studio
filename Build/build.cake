var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("NuGet")    
    .Does(() =>
{
    NuGetRestore("../Src/WemStudio.sln");
});

Task("Build")
  .IsDependentOn("NuGet")
  .Does(() =>
{
  MSBuild("../Src/WemStudio.sln",  new MSBuildSettings().SetConfiguration(configuration));
});

Task("Zip")
  .IsDependentOn("Build")
  .Does(() =>
{
  var root = System.IO.Path.Combine("../Src/WemStudio.Tool.Wpf/bin/", configuration);
  var source = root;
  var destination = System.IO.Path.Combine(root, "wem-studio.zip");
  
  Zip(source, destination);
});

Task("Default")
  .IsDependentOn("Build")
  .Does(() =>
{
});

RunTarget(target);