# corecover
[![Linux Build text](https://travis-ci.org/pjbgf/corecover.svg)](https://travis-ci.org/pjbgf/corecover)
[![Windows Build status](https://ci.appveyor.com/api/projects/status/lqs8r879krlnkba8?svg=true)](https://ci.appveyor.com/project/pjbgf/corecover)  
[![Coverage Status](https://coveralls.io/repos/github/pjbgf/corecover/badge.svg?branch=master)](https://coveralls.io/github/pjbgf/corecover?branch=master) [![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  
Light-weight cross platform code coverage tool for .Net Core applications.

The goal is to provide an easy to use command line for linux:

```
./run.sh TestProject/BinFolder/ coverage.xml **CURRENT**
```
And windows:
```
run TestProject\BinFolder\ coverage.xml **CURRENT**
```

The reports generated are currently based on [OpenCover](https://github.com/OpenCover/opencover) format.

# Status

* Only methods are being covered.
* Coverage is not yet accurate.
* Dynamically generated classes are not removed from report.
* Coverage report only shows first line of the branch.