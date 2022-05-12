using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeFactory", menuName = "ObjectDemo/Assets/ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject {

    public Material[] materials;
    public Shape[] shapes;

    public List<Shape> gameShapes;
    public List<Shape>[] shapePools;

    public ShapeFactory(){
        gameShapes = new List<Shape>();
    }

    public Shape GetShape(int id){
        if(shapes[id] == null){
            id = 0;
        }

        if(shapePools == null){
            shapePools = new List<Shape>[shapes.Length];
            for(int i = 0; i < shapes.Length; i++){
                shapePools[i] = new List<Shape>();
            }
        }
        
        var pools = shapePools[id];
        Shape instance;
        if(pools != null && pools.Count > 0)
        {
            instance = pools[pools.Count - 1];
            pools.RemoveAt(pools.Count - 1);
            instance.gameObject.SetActive(true);
            return instance; 
        }else{
            instance = Instantiate(shapes[id]);
            instance.ShapeID = id;
        }

        return instance;
    }

    public Shape GetRandom(){
        int i = Random.Range(0, shapes.Length);

        return GetShape(i);
    }

    public Material GetMaterial(int id){
        if(materials[id] == null){
            id = 0;
        }

        return materials[id];
    }

    public Material RandomMaterial(){
        int i = Random.Range(0, materials.Length);
        return materials[i];
    }

    public void addShape(Shape o){
        gameShapes.Add(o);
    }

    public int getShapeCount(){
        return gameShapes.Count;
    }

    public void saveAllShape(DataWriter writer){
        writer.Write(gameShapes.Count);
        foreach(Shape o in gameShapes)
        {
            o.Save(writer);
        }
    }

    public void loadShapes(DataReader reader){
        int count = reader.ReadInt();
        for(int i = 0; i < count; i++){
            int id = reader.ReadInt();
            Shape o = GetShape(id);
            o.Load(reader);
            gameShapes.Add(o);
        }
    }

    public void DeleteAllShape()
    {
        for (int i = gameShapes.Count; i > 0; i--){
            DeleteShape(gameShapes[i-1]);
        }
        gameShapes.Clear();
    }

    public void DeleteShape(Shape o)
    {
        if(o == null){
            Debug.Log("DeleteShape error : shape is null");
            return;
        }
        int id = o.ShapeID;
        if(shapePools[id] != null){
            o.gameObject.SetActive(false);
            shapePools[id].Add(o);
            gameShapes.Remove(o);
        }
    }
}
