using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Spectacles.Net.Data;
using mt = Autodesk.Dynamo.MeshToolkit;
using Autodesk.DesignScript.Interfaces;
using DSCore;
using Dynamo.Visualization;

namespace Spectacles.DynamoExporter
{
    public class SpectaclesMesh
    {

        private SpectaclesMesh() { }
        /// <summary>
        /// Creates a SpectaclesGeometry from a Dynamo Mesh
        /// </summary>
        /// <param name="mesh">mesh to create</param>
        /// <param name="color">color to apply to the mesh</param>
        /// <returns>A Spectacles Geometry that can be exported by the Scene compiler</returns>
        [MultiReturn("SpectaclesGeometry", "message")]
        public static Dictionary<string, object> ByToolkitMeshAndColor(mt.Mesh mesh, Color[] color)
        {

            SpectaclesGeometry g = new SpectaclesGeometry();
            g.uuid = Guid.NewGuid().ToString();
            g.type = "Geometry";

            SpectaclesGeometryData data = new SpectaclesGeometryData();

            //populate data object properties

            data.vertices = new List<double>();
            data.faces = new List<int>();
            data.normals = new List<double>();
            data.uvs = new List<double>();
            data.scale = 1;
            data.visible = true;
            data.castShadow = true;
            data.receiveShadow = false;
            data.doubleSided = true;

            //populate vertices
            foreach (var v in mesh.Vertices())
            {
                data.vertices.Add(System.Math.Round(v.X * -1.0, 5));
                data.vertices.Add(System.Math.Round(v.Z, 5));
                data.vertices.Add(System.Math.Round(v.Y, 5));
            }

            int faceNumber = mesh.VertexIndicesByTri().Count / 3;

            //populate faces
            for (int i = 0; i < faceNumber; i++)
            {
                data.faces.Add(i);
            }

            //this will display the mesh
            mt.Display.MeshDisplay displayMesh = mt.Display.MeshDisplay.ByMeshColor(mesh, color);

            var factory = new DefaultRenderPackageFactory();
            var package = factory.CreateRenderPackage();
            displayMesh.Tessellate(package, new TessellationParameters());
            

            //TO DO:
            //populate vertex colors

            //TO DO: user data
            //populate userData objects

            g.data = data;

            return new Dictionary<string, object> {
                { "SpectaclesGeometry", g},
                { "message", "success" }
            };
        }
    }
}
