# corecover
![Alt text](https://travis-ci.org/pjbgf/corecover.svg?branch=master "Master branch status") [![Coverage Status](https://coveralls.io/repos/github/pjbgf/corecover/badge.svg?branch=master)](https://coveralls.io/github/pjbgf/corecover?branch=master) [![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  
Light-weight cross platform code coverage tool for .Net Core applications.

The goal is to provide an easy to use command line for linux:

```
./run.sh TestProject/BinFolder/ coverage.xml
```
And windows:
```
run TestProject\BinFolder\ coverage.xml
```

The reports generated are currently based on [OpenCover](https://github.com/OpenCover/opencover) format.

# Status

* Only methods are being covered.
* Coverage is not yet accurate.
* Dynamically generated classes are not removed from report.
* Coverage report only shows first line of the branch.