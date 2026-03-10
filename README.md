# MetaverseVR-Asset-Test-2026

This is a Unity based asset test for Metaverse VR. This project includes a fully interactive boat with realistic movement, simple buoyancy and a docking mechanic.
It also includes obstacle ships that travel a circuit made of Bezier curves, these ships feature colliders that can interact with the player boat.

# Features
* Powerboat Movement: Realistic movement with acceleration, deceleration, reversing functionality and basic buoyancy.
* Docking Area: The player can dock the boat in a green zone next to the dock.
* Docking UI: When the player docks portside, a timer will appear counting down how long they need to stay in the zone. It will also tell them if they have docked in the wrong orientation, and it will show the mission complete when fully docked.
* User Interface: Displays current speed, reversing state, a reset button, and a mission timer.
* Sunseeker Movement: The sunseeker boats follow a circuit made up of Bezier curves to simulate realistic movement.

## Technical Features

* **Input Abstraction Layer**: Uses Unity's Input System with an `IPlayerInput` interface and an `InputProvider` implementation. This decouples gameplay logic from the input system, making it easy to swap input sources or add new control schemes without modifying the player movement code.

* **Event-Driven HUD Architecture**: Gameplay systems communicate with the HUD via C# events rather than direct references. This keeps UI and gameplay systems loosely coupled, improving modularity, maintainability, and reducing the risk of cross-system bugs.

* **State-Based Docking System**: Docking logic is implemented using an enum-driven state system (`DockingState`), allowing the HUD to react to docking progress without embedding UI logic inside gameplay scripts.

# Controls
* W Key: Accelerate the boat.
* S Key: Decelerate the boat.
* R Key: Toggle between forward and reverse (when the boat is stationary).
* A/D Keys: Steer the boat left and right.

# Gameplay
Navigate the powerboat towards the island and dock the boat in the green zone. The boat must be portside and inside the zone for 5 seconds to complete the game. Avoid obstacle ships along the way.

# Requirements
* Unity version: 2021.3.45f2
* Platform: Windows (Standalone Executable)
