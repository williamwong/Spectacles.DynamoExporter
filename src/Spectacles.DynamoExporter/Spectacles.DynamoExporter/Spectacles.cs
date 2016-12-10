using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

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
    /// <param name="geometry">Geometry to input</param>
    /// <returns>The serialized JSON and a message designating success or failure</returns>
    [MultiReturn("json", "message")]
    public static Dictionary<string, string> SceneCompiler(bool write, string path, string fileName, Geometry geometry)
    {
      if (!write)
      {
        return null;
      }
      
      if (!Directory.Exists(path))
      {
        throw new Exception("Path does not exist");
      }

      var outputPath = Path.Combine(path, fileName);

      var exporter = new DynamoExporter(geometry);

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
