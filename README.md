# corecover
[![Linux Build text](https://travis-ci.org/pjbgf/corecover.svg)](https://travis-ci.org/pjbgf/corecover)
[![Windows Build status](https://ci.appveyor.com/api/projects/status/lqs8r879krlnkba8?svg=true)](https://ci.appveyor.com/project/pjbgf/corecover)  
[![Coverage Status](https://coveralls.io/repos/github/pjbgf/corecover/badge.svg?branch=master)](https://coveralls.io/github/pjbgf/corecover?branch=master) [![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  
Light-weight cross platform code coverage tool for .Net Core applications.

# Usage
Implemented as a dotnet cli extension, which allows for an easy to use command line:

```
dotnet cover TestProject/BinFolder/ coverage.xml
```


# Install

1. Install the CoreCover nuget in your project.
2. Add the following entry to the .csproj of your test project:

```
<ItemGroup>
  <DotNetCliToolReference Include="CoreCover" Version="*" />
</ItemGroup>
```

# Status

* Only methods are being covered.
* Coverage accuracy is not **reliable** yet.
* Dynamically generated classes are not removed from report.
* Last line of a method is never marked as covered.
* Only currently supported format for report is [OpenCover](https://github.com/OpenCover/opencover).
 