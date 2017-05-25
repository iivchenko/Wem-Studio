Task("Default")
  .Does(() =>
{
  MSBuild("..\\Src\\WEMMS.sln");
});

RunTarget("Default");