# GameMonitorV2
Monitors the running time of a selected game or program. User click "Browse" to find a game or program to monitor. Application will monitor the elapsed run time of the selected game or program. Once the game or program has exceeded the pre-determined limit (3 hours), it will give a warning and allow the user to shut down the game or program. 
 
Program was developed in a topdown manner in the MVVM (Model-View-View-Model) pattern with focus on test-driven development environment. Testing was done using NUnit and Moq. Autofac was used to handle dependency injection, and Log4Net was used to handle logging.
 
Program Flow:
Main Application form: GameMonitorForm.cs, which creates a new GameMonitorDisplay for each program monitored. Each GameMonitorDisplayViewModel creates a new PollWatcher object to monitor the selected game.
