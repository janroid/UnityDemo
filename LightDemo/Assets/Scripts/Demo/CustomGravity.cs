using UnityEngine;
using System.Collections.Generic;

public static class CustomGravity{

    static List<GravitySource> sources = new List<GravitySource>();
    public static Vector3 GetGravity(Vector3 pos){
        Vector3 g = Vector3.zero;
        for(int i = 0; i < sources.Count; i++){
            g += sources[i].getGravity(pos);
        }

        return g;
    }

    public static Vector3 GetGravity(Vector3 pos, out Vector3 upAxis){
        Vector3 g = GetGravity(pos);

        upAxis = -g.normalized;
        return g;
    }

    public static Vector3 GetUpAxis(Vector3 pos){
        Vector3 g = GetGravity(pos);

        return -g.normalized;

    }

    public static void Register(GravitySource source){
        Debug.Assert(!sources.Contains(source), "已经存在此定义重力了！");

        sources.Add(source);
    }

    public static void Unregister(GravitySource source){
        Debug.Assert(sources.Contains(source), "不存在此定义重力！");

        sources.Remove(source);
    }

}