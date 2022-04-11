using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace verb
{
    /// <summary>
    /// Data object that contains the kind of prefab
    /// </summary>
    [XmlRoot("XMLSceneData")]
    public class XMLSceneData
    {
        [XmlArray("SceneObjects")]
        public List<XMLObjectData> sceneObjects;
    }

    [XmlRoot("XMLObjectData")]
    public class XMLObjectData
    {
        [XmlElement("PrefabName")]
        public string prefabName;

        [XmlArray("ObjectPositions")]
        public List<Vector3> objectPositions;

        [XmlElement("ComponentName")]
        public string componentName;

        [XmlElement("ComponentAssemblyName")]
        public string componentAssemblyName;
    }
}
