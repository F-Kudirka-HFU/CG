using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;

namespace Fusee.Tutorial.Core
{
    public static class SimpleMeshes 
    {

        public static MeshComponent CreateCylinder(float radius, float height, int segments)
        {
            //Initalisierung
            float3[] verts = new float3[4 * segments + 2];    // one vertex per segment and one extra for the center point
            float3[] norms = new float3[4 * segments + 2];    // one normal at each vertex
            ushort[] tris  = new ushort[4 * 3 * segments];  // a triangle per segment. Each triangle is made of three indices
            //Winkel Delta
            float delta = 2 * M.Pi / segments;
            // Startpunkte
            verts[0] = new float3(radius, 0.5f * height, 0);
            verts[1] = verts[0];
            verts[2] = new float3(radius, -0.5f * height, 0);
            verts[3] = verts[2];
            
            norms[0] = new float3(1, 0.5f * height, 0);
            norms[1] = new float3(1,0,0);
            norms[2] = new float3(1,0,0);
            norms[3] =  new float3(1, -0.5f * height, 0);
        
            //Mittelpunkte
            verts[4*segments] = new float3 (0, 0.5f*height, 0);
            norms[4*segments] = new float3 (0, 0.5f*height, 0);
            verts[4*segments+1] = new float3 (0, -0.5f*height, 0);
            norms[4*segments+1] = new float3 (0, -0.5f*height, 0);

            for (int i = 1; i < segments; i++)
            {
                verts[4 * i] = new float3(radius*M.Cos(i*delta), 0.5f*height,radius*M.Sin(i*delta));
                norms[i * 4] = new float3(M.Cos(i * delta), 0.5f * height, M.Sin(i * delta));
                
                verts[4 * i + 1] = new float3(radius*M.Cos(i*delta), 0.5f*height,radius*M.Sin(i*delta));
                norms[4 * i + 1] = new float3(M.Cos(i * delta), 0, M.Sin(i * delta));

                verts[4 * i + 2] = new float3(radius * M.Cos(i * delta), -0.5f * height, radius * M.Sin(i * delta));
                norms[4 * i + 2] = new float3(M.Cos(i * delta), 0, M.Sin(i * delta));

                verts[4 * i + 3] = new float3(radius * M.Cos(i * delta), -0.5f * height, radius * M.Sin(i * delta));
                norms[4 * i + 3] = new float3(M.Cos(i * delta), -0.5f * height, M.Sin(i * delta));

                // top triangle
   tris[12*(i-1) + 1] = (ushort) (4*segments);       // top center point
   tris[12*(i-1) + 0] = (ushort) (4*i + 0);      // current top segment point
   tris[12*(i-1) + 2] = (ushort) (4*(i-1) + 0);      // previous top segment point

   // side triangle 1
   tris[12*(i-1) + 3] = (ushort) (4*(i-1) + 2);      // previous lower shell point
   tris[12*(i-1) + 4] = (ushort) (4*i+ 2);      // current lower shell point
   tris[12*(i-1) + 5] = (ushort) (4*i+ 1);      // current top shell point

   // side triangle 2
   tris[12*(i-1) + 6] = (ushort) (4*(i-1) + 2);      // previous lower shell point
   tris[12*(i-1) + 7] = (ushort) (4*i     + 1);      // current top shell point
   tris[12*(i-1) + 8] = (ushort) (4*(i-1) + 1);      // previous top shell point

   // bottom triangle
   tris[12*(i-1) + 9]  = (ushort) (4*segments+1);    // bottom center point
   tris[12*(i-1) + 11] = (ushort) (4*(i-1) + 3);     // current bottom segment point
   tris[12*(i-1) + 10] = (ushort) (4*i+ 3);     // previous bottom segment point
            }



                tris[12*segments -12] = (ushort) (4*segments);       // top center point
                tris[12*segments -11] = (ushort) (4*(segments-1));      // current top segment point
                tris[12*segments -10] = 0;      // previous top segment point

                // side triangle 1
                tris[12*segments -9] = (ushort) (4*(segments-1) + 2);      // previous lower shell point
                tris[12*segments -8] = 2;      // current lower shell point
                tris[12*segments -7] = 1;      // current top shell point

                // side triangle 2
                tris[12*segments-1 -4] = (ushort) (4*(segments-1) + 2);      // previous lower shell point
                tris[12*segments-1 -5] = 1;      // current top shell point
                tris[12*segments-1 -6] = (ushort) (4*(segments-1) + 1);      // previous top shell point

                // bottom triangle
                tris[12*segments -3]  = (ushort) (4*segments+1);    // bottom center point
                tris[12*segments -1] = (ushort) (4*(segments-1) + 3);     // current bottom segment point
                tris[12*segments -2] = (ushort) 3;     // previous bottom segment point


            return new MeshComponent
            {
                Vertices = verts,
                Normals = norms,
                Triangles = tris,
            };
        }
    }
}
