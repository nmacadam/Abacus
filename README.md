# 🧮 Abacus
 Playtesting metrics toolkit for Unity

### Todo:
- Inline documentation
- Remove unused & deprecated code
- Generate doxyfile
- Finish testing
- Create Unity Package

## Features
### 🧾 Generic Value Recording
Abacus can periodically record generic type values throughout gameplay.  This is accomplished completely nonintrusively with a Metric component.  No calls like 'Abacus.RecordValue(myValue)' required!  The default time step for recording values can be globally adjusted, or a custom time step can be assigned to each Metric.

### ⏱ Temporal Recording
Abacus provides several means to record temporal data about gameplay.
- Timestamps (mark a time with a string and save it)
- Stopwatch (single event, multiple durations for comparison)
- Splitwatch (seperate, sequential events for an event timeline)

## Upcoming Features
- Intermediate value dumping (for minimizing necessary memory)
- Cumulative metrics for multiple sessions
- Screenshot support
