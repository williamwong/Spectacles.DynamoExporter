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


    private readonly Geometry _dynamoGeometry;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dynamoGeometry">Dynamo geometry from node input to convert</param>
    internal DynamoExporter(Geometry dynamoGeometry)
    {
      _dynamoGeometry = dynamoGeometry;
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
    /// <param name="geometries">List of SpectaclesGeometry objects</param>
    public override void OnAddGeometries(List<SpectaclesGeometry> geometries)
    {
      _spectaclesGeometries = geometries;

      // TODO convert Dynamo geometry to Spectacles geometry
      
      Console.WriteLine(_dynamoGeometry);
      Console.WriteLine(_spectaclesGeometries);
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
