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

The desired feature list of this is to have a drag and drop VR-ready character controller with inverse kinematics, default poses, Leap Motion and Hydra support, configurable avatars, menus, controller and mouse/keyboard support. Lofty goals, I know.

##### Structure

Aside from the IK folder (Thanks Dogzer), everything you need is in /Assets/Core/. All file references will be from there unless otherwise noted.

The /Prefabs folder has the main Pilot prefab, which is what you place into the scene. Place it on the ground, and the character will be sitting in the air with his feet on the ground. Put a cube under him or something, maybe a chair. The sitting animation is a Mecanim controller. We're going to override the sitting animation with IK from the waist up.

In /Prefabs/Avatars, you can find the different example characters. These models are either free Unity assets or created with widely available free software, imported with default settings. If you follow the patterns of the examples when creating your characters in external software, you should be fine. Each Avatar is simply the imported model with an Animator and an IKRigConfig script attached, which tells the IKLimb controllers which limbs to use. To modify a Pilot, drop one of these new Avatars into the DefaultCharacter variable in the Pilot Script.

In /Prefabs/Animation/Animator Controllers, you'll find PilotAnimatorController. This is a very simple Mecanim controller that drives the base animation for the character. If you disable any of the IKLimb GameObjects, it will revert those bones back to the current animation state of the Animator. The Animator uses Layers and Avatar Masks to isolate the different limbs, so you can apply the sitting animation, and then override the Arms Layer with the T-Pose instead of sitting down, or smoothly transition from one state to another.

The IKLimb script is not perfect. I don't understand it, I haven't even tried to yet. It's not perfect, and sometimes the mesh deforms in weird ways. You can see this with the DefaultAvatar's neck. If you have a solution, submit a pull request or get in touch with me. 

The rig setup on the example Avatar should select the best bones for everything to work as long as your model conforms to the same setup, but you might need to tweak things a bit. Look for the Target GameObjects to move the body and hands around. The Elbow Target GameObjects control the rotation of the joint. You can also adjust the bones in the Editor if you select the IKRigConfig script on the example Avatars.

#### The Controls

Right now only keyboard controls are implemented. Escape quits the game, and "D" brings up a Debug Menu that displays the current FPS. Tab recalibrates the Rift's position. Tap "C" to toggle between holding an Xbox controller and not holding an Xbox controller.

#### Thanks

Thanks for checking out this project. Please feel free to offer feedback, or contribute!
