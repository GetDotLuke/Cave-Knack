using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    [XmlAttribute("name")]
    public string name;

    [XmlElement("quantity")]
    public float quantity;

    [XmlElement("value")]
    public float value;
    
    
}
