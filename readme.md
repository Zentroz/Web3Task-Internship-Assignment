# Web3Task Internship Assignment

## Overview
This project demonstrates a physics-based ball interaction system with curved surface traversal mechanics.  
The primary focus of the assignment is implementing intuitive touch controls and smooth surface-following behavior based on velocity projection and surface normals.

---

# Controls

1. Tap and hold the ball.
2. Drag your finger in the opposite direction of the intended launch.
3. Release to launch the ball.

The farther the drag distance, the stronger the launch force applied to the ball.

---

# Implementation

## Launch System
The launch mechanic is implemented using a drag-based input system:

- Player input is captured through touch interaction.
- The drag direction is inverted to calculate the launch direction.
- Launch force is determined by the drag magnitude.
- Velocity is applied directly to the ball’s rigidbody for responsive movement.

## Surface Traversal System
The ball is designed to move smoothly across curved surfaces while maintaining momentum consistency.

The system evaluates:
- Surface normals
- Ball velocity
- Tangent direction of the contacted surface

When the required conditions are satisfied, the velocity is adjusted to better align with the surface curvature.

---

# Surface Interaction Logic

## Surface Follow Requirements

For the ball to properly follow a surface curve:

- Ball velocity must align with the surface tangent direction.
- Ball speed must remain above a configurable threshold.

## Velocity Assistance

If the follow requirements are met:

- The current velocity is projected relative to the surface normal.
- The result is more smooth and stable movement across curves.

## Failed Surface Hit

A collision is considered a failed hit when:

- The impact vector (inverse velocity direction) strongly aligns with the surface normal.

---

# Challenges Faced

- Maintaining smooth movement across highly curved surfaces.
- Preventing abrupt velocity changes during transitions.
- Balancing responsiveness while preserving realistic physics behavior.
- Detecting valid surface-follow states.

---

# Possible Improvements

- Introduce dynamic camera feedback during movement such as camera shake.
- Add adjustable sensitivity and launch-force tuning.
- Implement more particles and sound effects for better game feel.
- Optimize surface detection logic for more complex environments.

---

# Technical Notes

- Surface-follow behavior is assisted rather than fully scripted.
- Threshold values are exposed in the Inspector for easier tuning.
