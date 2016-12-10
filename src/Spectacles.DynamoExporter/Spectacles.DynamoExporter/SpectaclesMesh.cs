using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using DSCore;
using Spectacles.Net.Data;
using mt = Autodesk.Dynamo.MeshToolkit;
using Math = System.Math;

namespace Spectacles.DynamoExporter
{
  /// <summary>
  /// 
  /// </summary>
  public class SpectaclesMesh : IGraphicItem
  {
    private readonly mt.Mesh _mesh;

    private SpectaclesMesh(mt.Mesh mesh)
    {
      _mesh = mesh;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <param name="parameters"></param>
    [IsVisibleInDynamoLibrary(false)]
    public void Tessellate(IRenderPackage package, TessellationParameters parameters)
    {
      var counter = 0;

      foreach (var v in _mesh.Vertices())
      {
        package.AddTriangleVertex(v.X, v.Y, v.Z);
        var p = _mesh.VertexNormals()[counter];
        package.AddTriangleVertexNormal(p.X, p.Y, p.Z);
        counter++;
      }
    }

    /// <summary>
    ///   Creates a SpectaclesGeometry from a Dynamo Mesh
    /// </summary>
    /// <param name="mesh">mesh to create</param>
    /// <param name="color">color to apply to the mesh</param>
    /// <returns>A Spectacles Geometry that can be exported by the Scene compiler</returns>
    [MultiReturn("SpectaclesGeometry", "SpectaclesMaterial", "originalMesh")]
    public static Dictionary<string, object> ByToolkitMeshAndColor(mt.Mesh mesh, Color[] color)
    {
      var m = new SpectaclesMesh(mesh);

      var g = new SpectaclesGeometry
      {
        uuid = Guid.NewGuid().ToString(),
        type = "Geometry"
      };

      var data = new SpectaclesGeometryData
      {
        vertices = new List<double>(),
        faces = new List<int>(),
        normals = new List<double>(),
        uvs = new List<double>(),
        scale = 1,
        visible = true,
        castShadow = true,
        receiveShadow = false,
        doubleSided = true
      };

      //populate data object properties


      //populate vertices
      foreach (var v in mesh.Vertices())
      {
        data.vertices.Add(Math.Round(v.X*-1.0, 5));
        data.vertices.Add(Math.Round(v.Z, 5));
        data.vertices.Add(Math.Round(v.Y, 5));
      }

      //populate faces
      for (var i = 0; i < mesh.VertexIndicesByTri().Count; i += 3)
      {
        data.faces.Add(0);

        data.faces.Add(mesh.VertexIndicesByTri()[i]);
        data.faces.Add(mesh.VertexIndicesByTri()[i + 1]);
        data.faces.Add(mesh.VertexIndicesByTri()[i + 2]);
      }

      //this will display the mesh
      //var displayMesh = mt.Display.MeshDisplay.ByMeshColor(mesh, color);


      //TO DO:
      //populate vertex colors

      var alpha = color[0].Alpha;
      var red = color[0].Red.ToString("X2");
      var green = color[0].Green.ToString("X2");
      var blue = color[0].Blue.ToString("X2");

      var mat = new SpectaclesMaterial
      {
          uuid = Guid.NewGuid().ToString(),
          type = "MeshBasicMaterial",
          color = $"0x{red}{green}{blue}",
          side = 2,
          opacity = 1.0,
          transparent = true
      };

      //TO DO: user data
      //populate userData objects

      g.data = data;

      return new Dictionary<string, object>
      {
        {"SpectaclesGeometry", g},
        {"SpectaclesMaterial", mat },
        {"originalMesh", m}
      };
    }
  }
}