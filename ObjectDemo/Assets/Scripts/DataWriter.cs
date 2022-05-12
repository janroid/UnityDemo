using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataWriter
{
    string savePath;

    BinaryWriter binaryWriter;
    public DataWriter() {
        savePath = Path.Combine(Application.persistentDataPath, "gameCache");
        Debug.Log("savePath = " + savePath);
    }

    private BinaryWriter GetWriter(){
        if(binaryWriter == null){
            binaryWriter = new BinaryWriter(File.Open(savePath, FileMode.Create));
        }
        return binaryWriter;
    }

    public void CloseWriter(){
        if(binaryWriter != null){
            binaryWriter.Flush();
            binaryWriter.Close();
            binaryWriter = null;
        }
    }

    public void Write(Vector2 v){
        BinaryWriter bw = GetWriter();
        bw.Write(v.x);
        bw.Write(v.y);
    }

    public void Write(Vector3 v){
        BinaryWriter bw = GetWriter();
        bw.Write(v.x);
        bw.Write(v.y);
        bw.Write(v.z);
    }

    public void Write(int v){
        BinaryWriter bw = GetWriter();
        bw.Write(v);
    }

    public void Write(Quaternion q){
        BinaryWriter bw = GetWriter();
        bw.Write(q.x);
        bw.Write(q.y);
        bw.Write(q.z);
        bw.Write(q.w);
    }

    public void Write(Color c){
        BinaryWriter bw = GetWriter();
        bw.Write(c.r);
        bw.Write(c.g);
        bw.Write(c.b);
        bw.Write(c.a);
    }

}
