# corecover
Light-weight cross platform code coverage tool for .Net Core applications.

The goal is to provide an easy to use command line for linux:

```
./run.sh TestProject/BinFolder/ coverage.xml **CURRENT**
./run.sh MyTestProject.csproj myReport.xml **FUTURE**
```
And windows:
```
run TestProject\BinFolder\ coverage.xml **CURRENT**
run MyTestProject.csproj myReport.xml **FUTURE**
```

The reports generated are currently based on [OpenCover](https://github.com/OpenCover/opencover) format.

# Status

* Only methods are being covered.
* Coverage is not yet accurate.
* Dynamically generated classes are not removed from report.
* Coverage report only shows first line of the branch.