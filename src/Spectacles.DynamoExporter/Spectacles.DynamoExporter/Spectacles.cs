using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Autodesk.DesignScript.Runtime;
using Spectacles.Net.Data;

namespace Spectacles.DynamoExporter
{
  /// <summary>
  /// Nodes for converting Dynamo geometry to Spectacles-compatible JSON output
  /// </summary>
  public class Spectacles
  {
    /// <summary>
    /// Private constructor
    /// </summary>
    private Spectacles()
    {
    }

    /// <summary>
    /// Launches the Spectacles Viewer in your web browser
    /// </summary>
    /// <param name="launch">If true, launch Spectacles</param>
    public static void Launch(bool launch)
    {
      if (launch)
      {
        Process.Start("http://tt-acm.github.io/Spectacles.WebViewer/");
      }
    }

    /// <summary>
    /// Compiles Spectacles objects into a JSON representation of a THREE.js scene, 
    /// which can be opened using the Spectacles viewer.
    /// </summary>
    /// <param name="write">Write the Spectacles JSON file to disk?</param>
    /// <param name="path">Directory to export output</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="geometries"></param>
    /// <param name="materials"></param>
    /// <returns>The serialized JSON and a message designating success or failure</returns>
    [MultiReturn("json", "message")]
    public static Dictionary<string, string> SceneCompiler(bool write, string path, string fileName, List<SpectaclesGeometry> geometries, List<SpectaclesMaterial> materials)
    {
      if (!write)
      {
        return null;
      }
      
      if (!Directory.Exists(path))
      {
        throw new Exception("Path does not exist");
      }

      var outputPath = Path.Combine(path, $"{fileName}.json");

      // TODO remove hard coded values

//      var testGeometries = new List<SpectaclesGeometry>
//      {
//        new SpectaclesGeometry
//        {
//          uuid = "99025f60-76d2-4749-8321-a1438550ec68",
//          type = "Geometry",
//          data = new SpectaclesGeometryData
//          {
//            vertices = new List<double>
//            {
//              0.0,0.0,0.0,-10.0,0.0,0.0,-10.0,10.0,0.0,0.0,10.0,0.0,-10.0,0.0,0.0,-10.0,0.0,10.0,-10.0,10.0,10.0,-10.0,10.0,0.0,-10.0,0.0,10.0,0.0,0.0,10.0,0.0,10.0,10.0,-10.0,10.0,10.0,0.0,0.0,10.0,0.0,0.0,0.0,0.0,10.0,0.0,0.0,10.0,10.0,0.0,0.0,0.0,0.0,0.0,10.0,-10.0,0.0,10.0,-10.0,0.0,0.0,0.0,10.0,0.0,-10.0,10.0,0.0,-10.0,10.0,10.0,0.0,10.0,10.0
//            },
//            faces = new List<int>()
//            {
//              0,0,1,2,0,2,3,0,0,4,5,6,0,6,7,4,0,8,9,10,0,10,11,8,0,12,13,14,0,14,15,12,0,16,17,18,0,18,19,16,0,20,21,22,0,22,23,20
//            },
//            normals = new List<double>(),
//            uvs = new List<double>(),
//            scale = 1
//          }
//        }
//      };

//      var testMaterials = new List<SpectaclesMaterial>
//      {
//        new SpectaclesMaterial
//        {
//          uuid = "18114196-238c-440d-a7ce-14d516a8a3da",
//          type = "MeshBasicMaterial",
//          color = "0xFF0000",
//          side = 2,
//          opacity = 1.0,
//          transparent = true
//        }
//      };

      var testObject = new SpectaclesObject
      {
        uuid = "7b12588b-0a09-455a-b3d8-60160e9b0a5",
        type = "Scene",
        matrix = new double[] {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
        children = new List<SpectaclesObject>
        {
          new SpectaclesObject
          {
            uuid = "28fbff18-819b-41e2-9bd2-38a9a139f169",
            name = "mesh0",
            type = "Mesh",
            geometry = geometries[0].uuid,
            material = materials[0].uuid,
            matrix = new double[] {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1},
            userData = new Dictionary<string, string>()
          }
        },
        userData = new Dictionary<string, string>()
      };

      testObject.userData.Add("layers", "Default");
      testObject.children[0].userData.Add("layer", "Default");

      var exporter = new DynamoExporter(geometries, materials, testObject);

      try
      {
        var json = exporter.ExportToJson(outputPath);

        return new Dictionary<string, string>
        {
          { "json", json },
          { "message", $"Spectacles file exported to {outputPath}" }
        };
      }
      catch (Exception)
      {
        return new Dictionary<string, string>
        {
          { "json", null },
          { "message", "There was a problem writing the JSON to the file." }
        };
      }
      
    }
  }
}
