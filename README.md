ovr-pilot
=========

![ovr-pilot](http://i.imgur.com/jp1JYSS.png)
Barebones Unity3D project, works as a template for a seated VR experience. Implements basic keyboard, menu, and IK functionality.

#### Getting Started

This project requires Unity 4.6rc1 and the Oculus SDK v0.4.3. It was built using Oculus' 0.4.3.1 Unity3D integration. This project is fully compatible with the Free version of Unity, there is no Pro requirement.

Once you have the prerequisite stuff installed, it's quite easy to get started.

    git clone https://github.com/sam-kelly-dev/ovr-pilot.git
    Open the ovr-pilot folder in Unity as a project.
    Press Play.

#### Using This Template

The person wearing the headset in real life is referred to as the Pilot. The representation of that person in the world is called the Pilot Avatar. You can find all of the keyboard-based functionality in the PilotController script, attached to the PilotAvatar GameObject. It also houses the FPS calculation that's displayed in the debug screen.

The Inverse Kinematics are implemented using [Dogzer's free package](http://u3d.as/content/dogzer/inverse-kinematics/2fP). I made some slight modifications, and I use the IKLimb script instead of the IKControl script. You can find the IK controllers and targets in the IK GameObject attached to the PilotAvatar GameObject.

The GUI that appears for the Debug menu is housed in the DebugOverlay GameObject, which is attached to PilotAvatar->OVRCameraRig->CenterEyeAnchor. 

#### The Controls

Right now only keyboard controls are implemented. Escape quits the game, and "D" brings up a Debug Menu that displays the current FPS. Tab recalibrates the Rift's position.

#### Thanks

Thanks for checking out this project. Please feel free to offer feedback, or contribute!
