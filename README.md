# Multiplayer local coop
A simple implementation of a couch multiplayer game using Unity.
The `GameControllerManager` script handles the mapping between the connected controllers with the Player GameObjects.
The `ControllerPlugDetection` detects if a new controller has been plugged or if an old has been unplugged.
Starting the simulation with connected controllers will automatically generate the players.
Plugging a new controller will generate a new Player GameObject that will be associated to that controller only if there are no already-spawned players waiting for a controller.

<img src="https://raw.githubusercontent.com/endosama/unity-couch-multiplayer-base/master/preview.png" alt="Preview" width="654" height="387"/>

Works only with game controller!
Created with Unity 2019.2.5f1!
Tested with PS3/PS4 controller!

# Setup controller with InControl
Create a folder InControl and clone:
```
git clone https://github.com/pbhogan/InControl.git
```

Follow this getting started:
```
http://www.gallantgames.com/pages/incontrol-getting-started
```


# Setup additional Packages

```
Unity > Window > Package Manager
```
Enable those packages:
 1. Lightweight Rendering Pipeline.
 2. Postprocessing
 
If some Material is not shown correctly use:
```
Edit > Rendering Pipeline > Lightweight Rendering Pipeline > Upgrade project materials...
```
