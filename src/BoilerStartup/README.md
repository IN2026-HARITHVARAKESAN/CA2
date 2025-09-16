# Boiler Controller Console Application

- Implements a comprehensive boiler startup controller using C#. This application will simulate the operations of a boiler system, reflecting real-world scenarios with detailed state transitions, timers, and logging capabilities.

## Implementation Details
## Design
### View
- UiManager - Handles UI and display functions to user.

### Model
- Enums :
	- SystemStatus
		- Lockout
		- Ready
		- Pre-Purge
		- Ignition
		- Operational
	- InterlockSwitchStatus
		- Open
		- Closed

### Controller
 - BoilerStartUpManager - switch cases and methods for the operations
 - FileHandler - Handles file for logging
 - Logger - Handles logging events

### Services
- InputGetter - Gets input from the user
- InputValidator - Validates input that are given by the user.

### Testing
Did manual testing :
- Reset Lockout can be chose when interlock switch is closed.
- Boiler can be started when System status is ready.
- Error simulation can be done only when system status is operational.
- Stopping Boiler can be done between ready and operation state any time.