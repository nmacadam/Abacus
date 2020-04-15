<p align="center">
  <img src="https://raw.githubusercontent.com/nmacadam/Abacus/master/Abacus/Assets/Examples/abacus.png?token=AJA7SOMOZKBOEUGFL5O5OCC6UDJ5S" width="50%" height="50%">
</p>
<p align="center">
  playtesting tools for unity
</p>
<h1></h1>

### Todo:
- Finish testing
- Create Unity Package

## Features
Abacus is a toolkit for defining and recording game metrics during playtesting sessions.  It exports a JSON file after a playtesting session containing all of the recorded data, which can be visualized with Abacus Board.  Abacus can be integrated into a Unity project with little to no coupling and removed just as easily for production builds.

### 🧾 Generic Value Recording
Abacus can periodically record generic type values throughout gameplay.  This is accomplished completely nonintrusively with a Metric component.  No calls like 'Abacus.RecordValue(myValue)' required--it's handled automatically!  The default time step for recording values can be globally adjusted, or a custom time step can be assigned to each Metric.  Creating a new Metric recorder is simple, either inherit from FieldMetric<> to read from fields, or PropertyMetric<> to read from properties.

### ⏱ Temporal Recording
Abacus provides several means to record temporal data about gameplay.  These components do require a call to record but are built to easily integrate into your project's event system.
- Timestamps (mark a time with a string and save it)
- Stopwatch (single event, multiple durations for comparison)
- Splitwatch (seperate, sequential events for an event timeline)

## Roadmap
- Intermediate value dumping (for minimizing necessary memory)
- Cumulative metrics for multiple sessions
- Screenshot support
- Raw data output types (.csv, etc.)

### Dependencies
- Json.NET for Unity
- NSubstitute
