[ This is currently work in process, therefore not fully functional. ]

# corecover
Light-weight code coverage command-line application running over .Net core to support cross-platform applications.

The goal is to provide an easy to use command line for linux:

```
./run.sh MyTestProject.csproj myReport.xml
```
And windows:
```
run.cmd MyTestProject.csproj myReport.xml
```

The reports generated will be based on OpenCover format.
