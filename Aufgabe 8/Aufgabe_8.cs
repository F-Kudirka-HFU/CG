﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    
    public class FirstSteps : RenderCanvas
    {
    
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        // Init is called on startup. 
        private float _camAngle = 0;
        private TransformComponent cubeTransform1;
        private TransformComponent cubeTransform2;
        private TransformComponent cubeTransform3;
        public override void Init()
        {
            // Set the clear color for the backbuffer to light green (intensities in R, G, B, A).
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
      RC.ClearColor = new float4(0.7f, 1, 0.5f, 1);

      // Create a scene with a cube
      // The three components: one XForm, one Material and the Mesh

    cubeTransform1 = new TransformComponent {Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 20)};
    cubeTransform2 = new TransformComponent {Scale = new float3(2, 2, 2), Translation = new float3(0, 0, 20)};
    cubeTransform3 = new TransformComponent {Scale = new float3(.5f, .5f, .5f), Translation = new float3(10, 15, 20)};
      var cubeMaterial1 = new MaterialComponent
      {
          Diffuse = new MatChannelContainer {Color = new float3(0, 1, 0)},
          Specular = new SpecularChannelContainer {Color = float3.One, Shininess = 4}
      };

      var cubeMaterial2 = new MaterialComponent {
          Diffuse = new MatChannelContainer {Color = new float3(1f,.5f,0)},
      };
      var cubeMaterial3 = new MaterialComponent {
          Diffuse = new MatChannelContainer {Color = new float3(1f,.2f,.5f)},
      };
      var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

      // Assemble the cube node containing the three components
      var cubeNode = new SceneNodeContainer();
      cubeNode.Components = new List<SceneComponentContainer>();
      cubeNode.Components.Add(cubeTransform1);
      cubeNode.Components.Add(cubeMaterial1);
      cubeNode.Components.Add(cubeMesh);

      var cubeNode2 = new SceneNodeContainer();
      cubeNode2.Components = new List<SceneComponentContainer>();
      cubeNode2.Components.Add(cubeTransform2);
      cubeNode2.Components.Add(cubeMaterial2);
      cubeNode2.Components.Add(cubeMesh);
      
      var cubeNode3 = new SceneNodeContainer();
      cubeNode3.Components = new List<SceneComponentContainer>();
      cubeNode3.Components.Add(cubeTransform3);
      cubeNode3.Components.Add(cubeMaterial3);
      cubeNode3.Components.Add(cubeMesh);

      // Create the scene containing the cube as the only object
      _scene = new SceneContainer();
      _scene.Children = new List<SceneNodeContainer>();
      _scene.Children.Add(cubeNode);
      _scene.Children.Add(cubeNode2);
      _scene.Children.Add(cubeNode3);

      // Create a scene renderer holding the scene above
      _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);
            RC.View = float4x4.CreateTranslation(0, 0, 60) * float4x4.CreateRotationY(_camAngle);
            cubeTransform1.Rotation += new float3(.1f, 0, 0);
            _camAngle += 90.0f * M.Pi/180.0f * DeltaTime;
             cubeTransform2.Translation = new float3(0, 5 * M.Sin(3 * DeltaTime), 0);
            _sceneRenderer.Render(RC);
            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}