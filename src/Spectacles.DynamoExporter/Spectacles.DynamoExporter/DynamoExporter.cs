using System;
using System.Collections.Generic;
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


    private readonly List<SpectaclesGeometry> _inputGeometries;
    private readonly List<SpectaclesMaterial> _inputMaterials;
    private readonly SpectaclesObject _inputObject;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="inputGeometries">Dynamo geometry from node input to convert</param>
    /// <param name="inputMaterials"></param>
    /// <param name="inputObject"></param>
    internal DynamoExporter(List<SpectaclesGeometry> inputGeometries, List<SpectaclesMaterial> inputMaterials, SpectaclesObject inputObject)
    {
      _inputGeometries = inputGeometries;
      _inputMaterials = inputMaterials;
      _inputObject = inputObject;
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
    }

    /// <summary>
    /// Provides SpectaclesGeometry list to add geometry to
    /// </summary>
    /// <param name="spectaclesGeometries">List of SpectaclesGeometry objects</param>
    public override void OnAddGeometries(List<SpectaclesGeometry> spectaclesGeometries)
    {
      spectaclesGeometries.AddRange(_inputGeometries);
    }

    /// <summary>
    /// Provides SpectaclesMaterial list to add materials to
    /// </summary>
    /// <param name="spectaclesMaterials">List of SpectaclesMaterial objects</param>
    public override void OnAddMaterials(List<SpectaclesMaterial> spectaclesMaterials)
    {
      spectaclesMaterials.AddRange(_inputMaterials);
    }

    /// <summary>
    /// Provides SpectaclesObject to add scene objects to
    /// </summary>
    /// <param name="spectaclesObject">A SpectaclesObject instance</param>
    public override void OnAddSpectacleObject(SpectaclesObject spectaclesObject)
    {
      spectaclesObject.uuid = _inputObject.uuid;
      spectaclesObject.children = _inputObject.children;
      spectaclesObject.type = _inputObject.type;
      spectaclesObject.matrix = _inputObject.matrix;
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
