# Rave Variable Usage Report Generator
This is an application that analyses rave modules to extract rave variables used in Jeppesen's Airline Optimization Softwares like Tracking, Planning, Manpower etc.

The report is generated as an HTML file that contains information about the variable such as how many times and in which files that variable is used.

# Usage
## UI
This application is provided by a user interface that needs .Net Framework 4.5 installed. Running application without a parameter opens an interface contains a button to select CARMUSR folder.

![screenshot.png](https://raw.githubusercontent.com/hasangok/jeppesen-rave-variable-usage-report-generator/master/images/screenshot.png)

## Command Line
You can use this application on command line providing path to CARMUSR.

```terminal
JeppesenRaveVariableUsageReportGenerator.exe "/path/to/carmusr/directory/"
```

# Example Report
![report.png](https://raw.githubusercontent.com/hasangok/jeppesen-rave-variable-usage-report-generator/master/images/report.png)
