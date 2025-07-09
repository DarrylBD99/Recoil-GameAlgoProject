# Recoil
Built using Unity 6000.0.30f1

<img src="https://i.imgur.com/9VraYQo.png" />

An endless roguelike top-down shooter, where each enemy type has a different pathfinding and attack pattern.
The goal is to survive as long as possible.

You may refer to the YouTUbe Demonstration video below

[![Alt text](https://img.youtube.com/vi/HX5VtzFBR38/0.jpg)](https://www.youtube.com/watch?v=HX5VtzFBR38)

## Unity Editor
The main scene is located in "Scenes/SampleScene". If you want to visualise pathfinding, make sure to turn on Gizmo in the Game Window.

## Build

Note: To build this project, make sure you delete GridDebug and NodeGraph_Editor (as well as any reference to these classes to prevent errors) cause Unity doesn't seem to like building a project while UnityEditor is used.

When selecting a build profile, make sure to select the build profile "New Windows Profile" as it is already tested.

If there is no such Build Profile, create a new Windows Build Profile and make sure to check "override global scene list" and include "Scenes/SampleScene" in the scene list. 
