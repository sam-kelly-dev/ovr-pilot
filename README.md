ovr-pilot
=========

![ovr-pilot](http://i.imgur.com/jp1JYSS.png)
Barebones Unity3D project, works as a template for a seated VR experience. Implements basic keyboard, menu, and IK functionality.

#### Getting Started

This project requires Unity 4.60f3 and the Oculus SDK v0.4.4. It was built using Oculus' 0.4.3.1 Unity3D integration. This project is fully compatible with the Free version of Unity, there is no Pro requirement.

Once you have the prerequisite stuff installed, it's quite easy to get started.

    git clone https://github.com/sam-kelly-dev/ovr-pilot.git
    Open the ovr-pilot folder in Unity as a project.
    Open the scene "Test" from the ovr-pilot folder.
    Press Play.

#### Using This Template

The desired feature list of this is to have a drag and drop VR-ready character controller with inverse kinematics, default poses, Leap Motion and Hydra support, configurable avatars, menus, controller and mouse/keyboard support. Lofty goals, I know.

##### Structure

Aside from the IK folder (Thanks Dogzer), everything you need is in /Assets/Core/. All file references will be from there unless otherwise noted.

The /Prefabs folder has the main Pilot prefab, which is what you place into the scene. Place it on the ground, and the character will be sitting in the air with his feet on the ground. Put a cube under him or something, maybe a chair. The sitting animation is a Mecanim controller. We're going to override the sitting animation with IK from the waist up. The Pilot prefab is just a GameObject with a PilotScript script on it.

In /Prefabs/Avatars, you can find the different example characters. These models are either free Unity assets or created with widely available free software, imported with default settings. If you follow the patterns of the examples when creating your characters in external software, you should be fine. Each Avatar is simply the imported model with an Animator and an IKRigConfig script attached, which tells the IKLimb controllers which limbs to use. To modify a Pilot, drop one of these new Avatars into the DefaultCharacter variable in the Pilot Script.

In /Prefabs/Animation/Animator Controllers, you'll find PilotAnimatorController. This is a very simple Mecanim controller that drives the base animation for the character. If you disable any of the IKLimb GameObjects, it will revert those bones back to the current animation state of the Animator. The Animator uses Layers and Avatar Masks to isolate the different limbs, so you can apply the sitting animation, and then override the Arms Layer with the T-Pose instead of sitting down, or smoothly transition from one state to another.

The IKLimb script is not perfect. I don't understand it, I haven't even tried to yet. It's not perfect, and sometimes the mesh deforms in weird ways. You can see this with some of the arms. If you have a solution, submit a pull request or get in touch with me. 

#### Pilots

The Pilot prefab is really simple. The tricky part is setting up the Pilot's Avatar. This is a model, an IKRigConfig script, and a Mecanim animator. You can theoretically use any Mecanim-compatible model, but it might take some effort on your part to get the IK to work. 

The four example Avatars are all based on widely available assets. 

- DefaultAvatar ([Mecanim Example Scene from Unity Technologies](https://www.assetstore.unity3d.com/en/#!/content/7673))
- GlassesGuy ([Third Person Character from Sample Assets Beta (4.6) from Unity Technologies](https://www.assetstore.unity3d.com/en/#!/content/21064))
- MakeHumanBasicRig ([A simple model created using MakeHuman](http://www.makehuman.org/))
- Mixamo ([Mixamo Character Pack from Mixamo](https://www.assetstore.unity3d.com/en/#!/content/124))

The GlassesGuy and MakeHuman both need some work on the arms. They twist the wrong way. It's a known issue. If you can make them work, let me know!

**Creating an avatar**: 

1. Import your model, hopefully similar to one of the examples.
2. In the "Rig" menu of the imported object, Configure the avatar. Default settings should be good enough, but this might be a good thing to try tweaking later on. Click Apply, and then Done.
3. Create an instance of the model in the editor window. Drag it from the Heirarchy back into the Project folder to make a Prefab.
4. Add an Animator object to the prefab. Give it the PilotAnimatorController, and the Avatar that you created in step 2.
5. Find the Pilot object in your Heirarchy, or drop one into your scene if it's not already there, and select it. Drop the new prefab you've created onto the DefaultAvatar slot in the Inspector.
6. Press play. 

If your avatar is compatible, you should be good to go. If it's not, start messing with the bone assignments in the Prefab. 

If your avatar is not compatible, there's a ton of ways you can try to fix it. You can modify the IK code. You can change the Mecanim Avatar configuration in the Rig menu (Step 2). You can edit the IKRigConfig values on your prefab. You can script around it and manually assign IK targets and parents. I'm trying to support what appear to be the most common free options.

The Pilot object will create several child objects:

**YourModelName(Clone)**- This is the model- the mesh(es), and the bones that control it. 

**OVRHeadCamera(Clone)**- This is the Oculus headset. This drives the IKLimbTorso movements.

**IKLimbXX** - These are the actual IK objects that override the Animator. You can find them in PilotScript. The bones that the IKLimb is referencing is set when the Pilot is instantiated, and you can select the targets on the Avatar's Prefab that you created above. Running the game again will apply the new IKLimb references.

**Joint and Tip Targets**- These are the Transforms that direct the IK limbs. The Tip position is where the head or hands are trying to get to, and the Joint positions are where the meeting point of the two bones is supposed to be. The Tip overrides the Joint.

**Moving the Hands**- Just look for the LeftArmTipTarget and RightArmTipTarget and move them around. They're in the root of the Heirarchy by default. If it looks weird, try moving the JointTarget for the arm as well.

Support for the Autodesk Character Generator and UMA is coming soon.

#### The Controls

Right now only keyboard controls are implemented. Escape will immediately quit the application, and Spacebar will recenter the pose. 

#### Thanks

Thanks for checking out this project. Please feel free to offer feedback, or contribute!
