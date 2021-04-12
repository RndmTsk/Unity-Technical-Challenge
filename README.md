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


# Solution

This solution is a self-contained project which provides touch controls for interacting with the displayed models.

## Controls

A number of different elements can be controled by various gestures, and UI elements in the App

### Gestures

- A single finger swipe will rotate the model in the desired direction
- Dragging with two fingers around the screen will translate the model
- Pinching with two fingers will scale the model down, but no smaller than <INSERT MINIMUM SIZE>
- Stretching with two fingers will scale the model up, but no bigger than <INSERT MAXIMUM SIZE>

### Models

- Different models can be selected by the buttons down the left-hand side of the view
- Several are primitive shapes
- A couple are more detailed, and complicated shapes, borrowed from existing Unity projects


### Universal Render Pipeline

- A few elements of the render pipeline can be controlled
- ...


## Challenges

- ...


Author: Terry Latanville