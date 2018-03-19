using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;


[XmlRoot("ItemCollection")]
public class ItemContainer {


    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<ItemDatabase> items = new List<ItemDatabase>();

    public static ItemContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(ItemContainer));

        StringReader reader = new StringReader(_xml.text);

        ItemContainer items = serializer.Deserialize(reader) as ItemContainer;

        reader.Close();

        return items;
    }


}
