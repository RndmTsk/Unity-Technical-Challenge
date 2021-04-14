# Unity Technical Challenge

Your task is to create an interactive 3D visualizer in Unity. This well-crafted 3D visualizer will let you showcase and manipulate stunning 3D models on your phone or tablet. In addition to displaying photorealistic 3D models, this visualizer will let you experiment with different lighting conditions and post processing effects in realtime. It should be intuitive and pleasant to use.

Remember, this is your chance to show us your skills! We're focused on functionality and elegance in the implementation - artistic skills don't count towards the final points. We'll walk through your implementation during the interview.

## Primary Objectives

1. 3D Mesh Visualizer for Apple devices (iPhone / iPad) with the ability to rotate, scale, and translate the model
2. Add basic materials, textures, cameras, & lighting to the visualizer
3. Do all of this using our Universal Rendering Pipeline
4. Implement a simple and intuitive UI along with the 3D visualizer
5. Deploy to any iOS device with focus on performance

## Bonus Objectives

1. Ability to pick and choose different meshes, materials, textures in the visualizer
2. Impress us with additional Post Processing, Lights, and Cameras that you can squeeze in the App, while keeping stable performance on the device,

## Duration

1. It usually takes around 1-2 days to complete this but feel free to take your time and submit it before the end of the week

## Deliverables

1. Access to a Git repository with well documented instructions and code for your solution
2. Writeup describing the challenges you faced during development and how you solved them

## Criteria

1. Clean and readable code [30 Points]
2. Mobile rendering and UI [20 Points]
3. Version control and documentation [10 Points]
4. Performance optimization [20 Points]
5. Follow up interview [20 Points]

## Solution

This solution is a self-contained project which provides touch controls for interacting with the displayed models.

### Controls

A number of different elements can be controled by various gestures, and UI elements in the App

#### Gestures

- A single finger swipe will rotate the model in the desired direction
- Dragging with two fingers around the screen will translate the model
- Pinching with two fingers will scale the model down, but no smaller than the arbitrary value of `3`
- Stretching with two fingers will scale the model up, but no bigger than the arbitrary value of `8`

#### Models

- Different models can be selected by the buttons down the left-hand side of the view
- Several are primitive shapes
- A couple are more detailed, and complicated shapes, borrowed from existing Unity projects

#### URP Toggles

- Did not get a chance to add controls for this unfortunately. It would be fairly simple to expand on my UI to allow for various Volume "dials" and "knobs" now that I have the UI and touch controls out of the way.

## Challenges

It had been quite a while since I worked with anything complicated in Unity. Much of my experience was prior to uGUI and the new Input system. I have developed movement, AI, and event scripts, so I drew upon that experience.

### User Input

My initial work was done with the older Input system where you needed to use `GetAxis` for keyboard and mouse input, and touches for iOS devices. I knew that if I was able to translate whatever input I received into an intermediary `struct`, I could build my App logic separately from whatever input system I settled on.

Intuitive controls on mobile are a definitely a topic of debate, but I drew upon standard practices for the OS as well as other good examples like Google Maps to help inform my choices for App controls.

Rotation via multiple fingers is always a bad feel, so I decided to devote single-finger swipes to rotation based on the starting point of the touch.

Pinching for zooming is a common control which lent itself well to scaling, it feels intuitive. Similarly, stretching your fingers apart gives you a feel that the object should be getting larger. I decided to restrict the scale of the object to avoid having the object become so large it would take up the entire screen.

### uGUI

I had never used uGUI before, I had successfully integrated Scaleform into a previous project, so there was a lot to learn in a hurry. I started building with the legacy `OnGUI` just to get something in working order, I know it's not super performant on mobile devices, and it can take significant work to get it lined up, and looking decent, but any UI was a better than no UI. < WIP > ...

### Universal Render Pipeline

Having worked with the legacy rendering system, there was quite a bit to get up to speed on with the new system. I reviewed a number of articles (linked below), and started with the sample project. I still don't have a great handle on all of the pieces of the pipeline, but I've tried to stick with elements that give a bit more appeal, but are still on the list of recommended for use in mobile Apps.

### Unity on iOS

Working with Unity on iOS introduces a large amount of latency in a developer's cycle time. Much of what I built, was purposely done in the Editor as the cost of re-building the Xcode project, building to a simulator, and waiting for the simulator to load the App was orders of magnitude larger than building and running in the editor.

The other challenge, is that you can't really fake touch events that use more than one touch input with only a mouse or keyboard. This meant I was able to get the rotation gesture done fairly quickly, but other gestures took much longer.

## References

A log of articles I reviewed while getting back up to speed on the various elements of building Apps with Unity.

### Unity Legacy GUI

- [OnGUI Example](https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html)

### Unity UI

- [uGUI Manual](https://docs.unity3d.com/Manual/com.unity.ugui.html)
- [uGUI Tutorial](https://learn.unity.com/tutorial/creating-basic-ui-elements)
- [uGUI Canvas](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/UICanvas.html)
- [uGUI Optimization](https://unity3d.com/how-to/unity-ui-optimization-tips)
- [uGUI Element Sizing](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/HOWTO-UIFitContentSize.html)
- [uGUI Events](https://learn.unity.com/tutorial/working-with-the-event-system)
- [uGUI Button Clicks](https://www.youtube.com/watch?v=kdkrjCF0KCo)

### Animations

- [Animation Parameters](https://www.youtube.com/watch?v=q195HRyB_Aw)

### Input Management - Touches

- [Touch on device - Legacy](https://docs.unity3d.com/ScriptReference/Input.GetTouch.html)
- [Multi-touch on device - Legacy](https://docs.unity3d.com/ScriptReference/Input-touches.html)
- [Mobile input - Legacy](https://docs.unity3d.com/Manual/MobileInput.html)
- [New Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/QuickStartGuide.html)

### iOS Build Settings

- [Building for iOS](https://docs.unity3d.com/Manual/BuildSettingsiOS.html)
- [Texture Support by Platform](https://docs.unity3d.com/Manual/class-TextureImporterOverride.html)

### Universal Render Pipeline

- [URP Intro](https://blogs.unity3d.com/2019/09/20/how-the-lightweight-render-pipeline-is-evolving/)
- [URP Sample Project](https://github.com/Verasl/BoatAttack)
- [URP Conference Talk](https://www.youtube.com/watch?v=Bvl9rCVbMas)
- [URP Manual - Shadows](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/universalrp-asset.html#shadows)
- [URP Control - Forum Topic](https://forum.unity.com/threads/urp-volume-cs-how-to-access-the-override-settings-at-runtime-via-script.813093/)
- [URP SSAO](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@10.0/manual/post-processing-ssao.html)
- [URP Skyboxes](https://docs.unity3d.com/Manual/skyboxes-using.html)

### Assets

- [Free Skyboxes](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)
- [Viking Village - URP Sample](https://assetstore.unity.com/packages/essentials/tutorial-projects/viking-village-urp-29140)
- [Speed Tree](https://assetstore.unity.com/publishers/9474)

### VSCode Environment Setup

- [VSCode Setup Steps](https://code.visualstudio.com/docs/other/unity)


## Assets

- Baked-in URP sample project assets
- [Clean Vector Icons](https://assetstore.unity.com/packages/2d/gui/icons/clean-vector-icons-132084)
- [Skybox Series Free](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)

Author: Terry Latanville
