# Symphonie_EasyWayer
# Easy Water - High-Quality Customizable Water Shader for Unity URP

A user-friendly, high-quality water shader system designed specifically for the Unity Universal Render Pipeline (URP).

## Features

* **Fully Customizable Water Effects via Shader Graph:**
    * Create unique and dynamic water surfaces with the power of Shader Graph.
* **Multi-Layered Wave Animation System:**
    * Natural-looking waves generated with Voronoi noise.
    * Fractal wave overlay for enhanced detail.
    * Adjustable wave direction, speed, and intensity.
* **Advanced Visual Effects:**
    * Depth-based water transparency.
    * Customizable Fresnel reflections.
    * Dynamic normal map blending.
    * Adjustable refraction effects.
* **Performance Optimization:**
    * Modular shader subgraph system.
    * Optimized calculation methods.
    * Scalable performance.

## System Requirements

* Unity 2021.3 or later
* Universal Render Pipeline (URP)

## Quick Start

1.  Import the "Easy Water" folder into your Unity project.
2.  Create a plane or water mesh in your scene.
3.  Drag and drop the water material from the "Material" folder onto your water object.
4.  Adjust the material parameters as needed.
5.  **Disable "Cast Shadows" within the Mesh Renderer's "Lighting" settings on your water GameObject.**
6.  **Ensure "Opaque Texture" is enabled within the URP Pipeline Asset settings.**

## Key Parameter Descriptions

### Basic Settings

* **Water Color:** Adjust the base color of the water.
* **Transparency:** Control the overall transparency of the water.
* **Wave Intensity:** Adjust the overall strength of the water waves.

### Wave Settings

* **Wave Direction:** Control the direction of wave movement.
* **Wave Speed:** Adjust the speed of wave motion.
* **Wave Size:** Control the size of the waves.

### Visual Effects

* **Reflection Intensity:** Adjust the intensity of reflections on the water surface.
* **Refraction Intensity:** Control the degree of refraction distortion of underwater objects.
* **Depth Attenuation:** Adjust the color transition of the water as it changes with depth.

## Example Scene

A complete example scene is included in the "Example" folder, showcasing the various effects and uses of the water shader.

## Performance Optimization Tips

* Adjust the wave detail level according to project requirements.
* Reduce wave complexity for mobile platforms.
* Balance visual effects and performance by adjusting parameters.

## Important Notes

* **Disable "Cast Shadows":** To prevent unwanted shadow artifacts on the water surface, please disable the "Cast Shadows" option in the Mesh Renderer's "Lighting" settings of your water GameObject.
* **Enable "Opaque Texture":** For proper rendering in the URP, make sure the "Opaque Texture" option is enabled in your URP Pipeline Asset settings.