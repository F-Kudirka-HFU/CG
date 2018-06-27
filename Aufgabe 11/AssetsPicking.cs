using System;
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
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private TransformComponent _rightRearTransform;
        private TransformComponent _leftRearTransform;
        private TransformComponent _rightFrontTransform;
        private TransformComponent _leftFrontTransform;
        private TransformComponent _grabbleTransform;
        private TransformComponent _upperGrabbleTransform;
        private ScenePicker _scenePicker;
        private PickResult _currentPick;
        private float3 _oldColor;
        private float _cam;

        // Init is called on startup. 
        public override void Init()
   {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = AssetStorage.Get<SceneContainer>("CubeCar.fus");

            _rightRearTransform = _scene.Children.FindNodes(node => node.Name == "RightRearWheel")?.FirstOrDefault()?.GetTransform();
            _leftRearTransform = _scene.Children.FindNodes(node => node.Name == "LeftRearWheel")?.FirstOrDefault()?.GetTransform();
            _rightFrontTransform = _scene.Children.FindNodes(node => node.Name == "RightFrontWheel")?.FirstOrDefault()?.GetTransform();
            _leftFrontTransform = _scene.Children.FindNodes(node => node.Name == "LeftFrontWheel")?.FirstOrDefault()?.GetTransform();
            _grabbleTransform = _scene.Children.FindNodes(node => node.Name == "Grabble")?.FirstOrDefault()?.GetTransform();
            _upperGrabbleTransform = _scene.Children.FindNodes(node => node.Name == "UpperGrabble")?.FirstOrDefault()?.GetTransform();
            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
            _scenePicker = new ScenePicker(_scene);
            _cam = M.Pi;
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            //_baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 40) * float4x4.CreateRotationX(-(float)Atan(15.0 / 40.0)) * float4x4.CreateRotationY(_cam);
            if (Mouse.LeftButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                PickResult newPick = null;
                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                }
                if (newPick?.Node != _currentPick?.Node)
                {
                    if (_currentPick != null)
                    {
                        _currentPick.Node.GetMaterial().Diffuse.Color = _oldColor;
                    }
                    if (newPick != null)
                    {
                        var mat = newPick.Node.GetMaterial();
                        _oldColor = mat.Diffuse.Color;
                        mat.Diffuse.Color = new float3(1, 0.4f, 0.4f);
                    }
                    _currentPick = newPick;
                }
            }
            if(_currentPick != null) {
            switch(_currentPick.Node.Name) {
                case "Grabble":
                _grabbleTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                break;
                case "UpperGrabble":
                _upperGrabbleTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                break;
                case "LeftRearWheel":
                _leftRearTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                break;
                case "RightRearWheel":
                _rightRearTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                break;
                case "LeftFrontWheel":
                 _leftFrontTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                 break;
                 case "RightFrontWheel":
                  _rightFrontTransform.Rotation.x += 0.1f * Keyboard.WSAxis * DeltaTime * 20;
                  break;
            }
            }
           
            if (Mouse.RightButton) {
                _cam += .01f * DeltaTime * Mouse.Velocity.x;
            }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();

        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45� Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
