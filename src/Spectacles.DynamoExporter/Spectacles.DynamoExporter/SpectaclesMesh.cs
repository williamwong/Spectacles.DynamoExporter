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
    private readonly Color[] _color;

    private SpectaclesMesh(mt.Mesh mesh, Color[] color)
    {
      _mesh = mesh;
      _color = color;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <param name="parameters"></param>
    [IsVisibleInDynamoLibrary(false)]
    public void Tessellate(IRenderPackage package, TessellationParameters parameters)
    {
        package.RequiresPerVertexColoration=true;
        

        //THIS IS BUGGY, not fully working
        for (int i = 0; i < _mesh.VertexIndicesByTri().Count; i += 3)
            {
                int A = _mesh.VertexIndicesByTri()[i];
                int B= _mesh.VertexIndicesByTri()[i+1];
                int C= _mesh.VertexIndicesByTri()[i+2];
                package.AddTriangleVertex(_mesh.Vertices()[A].X, _mesh.Vertices()[A].Y, _mesh.Vertices()[A].Z);
                package.AddTriangleVertex(_mesh.Vertices()[B].X, _mesh.Vertices()[B].Y, _mesh.Vertices()[B].Z);
                package.AddTriangleVertex(_mesh.Vertices()[C].X, _mesh.Vertices()[C].Y, _mesh.Vertices()[C].Z);

                //TO-DO: color by face and by vertex (probably do this in the constructor depending on the number of colors connected in the input)
                package.AddTriangleVertexColor(_color[0].Red, _color[0].Green, _color[0].Blue, _color[0].Alpha);
                package.AddTriangleVertexColor(_color[0].Red, _color[0].Green, _color[0].Blue, _color[0].Alpha);
                package.AddTriangleVertexColor(_color[0].Red, _color[0].Green, _color[0].Blue, _color[0].Alpha);

                package.AddTriangleVertexNormal(_mesh.TriangleNormals()[A].X, _mesh.TriangleNormals()[A].Y, _mesh.TriangleNormals()[A].Z);
                package.AddTriangleVertexNormal(_mesh.TriangleNormals()[B].X, _mesh.TriangleNormals()[B].Y, _mesh.TriangleNormals()[B].Z);
                package.AddTriangleVertexNormal(_mesh.TriangleNormals()[C].X, _mesh.TriangleNormals()[C].Y, _mesh.TriangleNormals()[C].Z);

                package.AddTriangleVertexUV(-1.0, -1.0);
                package.AddTriangleVertexUV(-1.0, -1.0);
                package.AddTriangleVertexUV(-1.0, -1.0);
            } 
    }

    /// <summary>
    ///   Creates a SpectaclesGeometry from a Dynamo Mesh
    /// </summary>
    /// <param name="mesh">mesh to create</param>
    /// <param name="color">color to apply to the mesh</param>
    /// <returns>A Spectacles Geometry that can be exported by the Scene compiler</returns>
    [MultiReturn("SpectaclesGeometry", "originalMesh")]
    public static Dictionary<string, object> ByToolkitMeshAndColor(mt.Mesh mesh, Color[] color)
    {
      var m = new SpectaclesMesh(mesh, color);

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

      //TO DO: user data
      //populate userData objects

      g.data = data;

      return new Dictionary<string, object>
      {
        {"SpectaclesGeometry", g},
        {"originalMesh", m}
      };
    }
  }
}