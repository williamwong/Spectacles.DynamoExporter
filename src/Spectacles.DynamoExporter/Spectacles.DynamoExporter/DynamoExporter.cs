using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Spectacles.Net;
using Spectacles.Net.Data;

namespace Spectacles.DynamoExporter
{
  /// <summary>
  /// A custom implementation of the BaseSpectaclesExporter
  /// to convert Dynamo geometry to the Spectacles format
  /// </summary>
  internal class DynamoExporter : BaseSpectaclesExporter
  {
    private Metadata _metadata;
    private List<SpectaclesGeometry> _spectaclesGeometries;
    private List<SpectaclesMaterial> _spectaclesMaterials;
    private SpectaclesObject _spectaclesObject;


    private readonly List<SpectaclesGeometry> _inputGeometries;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="inputGeometries">Dynamo geometry from node input to convert</param>
    internal DynamoExporter(List<SpectaclesGeometry> inputGeometries)
    {
      _inputGeometries = inputGeometries;
    }
    
    /// <summary>
    /// Provides Metadata object to add additional information
    /// </summary>
    /// <param name="metadata">A Metadata instance</param>
    public override void OnAddMetadata(Metadata metadata)
    {
      _metadata = metadata;

      _metadata.generator = "DynamoExporter";
      _metadata.version   = 1.0;
      _metadata.type      = "Object";
      
      Console.WriteLine(_metadata);
    }

    /// <summary>
    /// Provides SpectaclesGeometry list to add geometry to
    /// </summary>
    /// <param name="spectaclesGeometries">List of SpectaclesGeometry objects</param>
    public override void OnAddGeometries(List<SpectaclesGeometry> spectaclesGeometries)
    {
      _spectaclesGeometries = spectaclesGeometries;
      _spectaclesGeometries.AddRange(_inputGeometries);
    }

    /// <summary>
    /// Provides SpectaclesMaterial list to add materials to
    /// </summary>
    /// <param name="materials">List of SpectaclesMaterial objects</param>
    public override void OnAddMaterials(List<SpectaclesMaterial> materials)
    {
      _spectaclesMaterials = materials;

      // TODO convert Dynamo materials to Spectacles materials
      
      Console.WriteLine(_spectaclesMaterials);
    }

    /// <summary>
    /// Provides SpectaclesObject to add scene objects to
    /// </summary>
    /// <param name="spectaclesObject">A SpectaclesObject instance</param>
    public override void OnAddSpectacleObject(SpectaclesObject spectaclesObject)
    {
      _spectaclesObject = spectaclesObject;

      // TODO convert Dynamo scenes to Spectacles scenes
      
      Console.WriteLine(_spectaclesObject);
    }

    /// <summary>
    /// Optional override of base ExportToJson method
    /// </summary>
    /// <param name="path">Path to export json file</param>
    /// <returns>The serialized JSON Spectacles representation</returns>
    public override string ExportToJson(string path)
    {
      Console.WriteLine("Exporting to JSON");

      return base.ExportToJson(path);
    }
  }
}
