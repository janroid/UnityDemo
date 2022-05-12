using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataReader
{
    string savePath;
    BinaryReader binaryReader;
    // Start is called before the first frame update
    public DataReader() {
        savePath = Path.Combine(Application.persistentDataPath, "gameCache");
    }

    private BinaryReader GetReader(){
        if(binaryReader == null){
            binaryReader = new BinaryReader(File.Open(savePath, FileMode.Open));
        }
        return binaryReader;
    }

    public Vector2 ReadVec2(){
        BinaryReader reader = GetReader();
        Vector2 vec2 = new Vector2();
        vec2.x = reader.ReadSingle();
        vec2.y = reader.ReadSingle();

        return vec2;
    }

    public Vector3 ReadVec3(){
        BinaryReader reader = GetReader();
        Vector3 vec3 = new Vector3();
        vec3.x = reader.ReadSingle();
        vec3.y = reader.ReadSingle();
        vec3.z = reader.ReadSingle();

        return vec3;
    }

    public int ReadInt(){
        BinaryReader reader = GetReader();
        return reader.ReadInt32();
    }

    public Color ReadColor(){
        BinaryReader reader = GetReader();
        Color color = new Color();
        color.r = reader.ReadSingle();
        color.g = reader.ReadSingle();
        color.b = reader.ReadSingle();
        color.a = reader.ReadSingle();

        return color;
    }

    public Quaternion ReadQuat(){
        BinaryReader reader = GetReader();
        Quaternion q = new Quaternion();
        q.x = reader.ReadSingle();
        q.y = reader.ReadSingle();
        q.z = reader.ReadSingle();
        q.w = reader.ReadSingle();

        return q;
    }


}
