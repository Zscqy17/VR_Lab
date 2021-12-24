using System.IO;
using System.Xml.Serialization;
using System;
using UnityEngine;
using System.Text;

public class XMLOp
{
    public static void Serialize(object item, string path)
    {
        XmlSerializer serializer = new XmlSerializer(item.GetType());
        StreamWriter writer = new StreamWriter(path,true, Encoding.UTF8);
        serializer.Serialize(writer.BaseStream, item);
        writer.Close();
    }

    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        if (path == null)
        {
            Debug.LogError("hai");
        }
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
    //public static string Deserialize(string path)
    //{
    //    XmlSerializer serializer = new XmlSerializer(typeof(string), new XmlRootAttribute("StateAsset"));
    //    if (path == null)
    //    {
    //        Debug.LogError("hai");
    //    }
    //    StreamReader reader = new StreamReader(path);
    //    Debug.Log("1");
    //    string deserialized = (string)serializer.Deserialize(reader.BaseStream);
    //    Debug.Log("2");
    //    reader.Close();
    //    return deserialized;
    //}
}