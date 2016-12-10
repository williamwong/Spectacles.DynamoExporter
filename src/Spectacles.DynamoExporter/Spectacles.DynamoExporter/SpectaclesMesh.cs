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
    public class SpectaclesMesh:IGraphicItem
    {

        private SpectaclesMesh(mt.Mesh mesh) { this._mesh = mesh; }
        private mt.Mesh _mesh;
        /// <summary>
        /// Creates a SpectaclesGeometry from a Dynamo Mesh
        /// </summary>
        /// <param name="mesh">mesh to create</param>
        /// <param name="color">color to apply to the mesh</param>
        /// <returns>A Spectacles Geometry that can be exported by the Scene compiler</returns>
        public static Dictionary<string, object> ByToolkitMeshAndColor(mt.Mesh mesh, Color[] color)
        {
            SpectaclesMesh m = new SpectaclesMesh(mesh);

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

            

            //TO DO:
            //populate vertex colors

            //TO DO: user data
            //populate userData objects

            g.data = data;

            return new Dictionary<string, object> {
                { "SpectaclesGeometry", g},
                { "originalMesh", m }
            };
        }

        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            //foreach (var e in _mesh.Edges())
            //{
            //    package.AddLineStripVertex(e.StartPoint.X, e.StartPoint.Y, e.StartPoint.Z);
            //    package.AddLineStripVertex(e.EndPoint.X, e.EndPoint.Y, e.EndPoint.Z);
            //}

            int counter = 0;

            foreach (var v in _mesh.Vertices())
            {
                
                package.AddTriangleVertex(v.X, v.Y, v.Z);
                Vector p = _mesh.VertexNormals()[counter];
                package.AddTriangleVertexNormal(p.X,p.Y,p.Z);
                counter++;
                
            }
            
   

        }
    }
}
