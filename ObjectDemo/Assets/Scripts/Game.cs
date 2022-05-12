using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class Game : MonoBehaviour
{

    public KeyCode KEY_CREATE, KEY_SAVE, KEY_LOAD,KEY_RESET,KEY_DELETE;
    public ShapeFactory shapeFactory;

    public float CSpeed{get;set;}
    public float DSpeed{get;set;}

    public float lastSpeed = 0f;

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float createProcress, deleteProcess;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KEY_CREATE)){
            CreateShape();
        }else if(Input.GetKeyDown(KEY_LOAD)){
            Load();
        }else if(Input.GetKeyDown(KEY_SAVE)){
            Save();
        }else if(Input.GetKeyDown(KEY_RESET)){
            ResetShape();
        }else if(Input.GetKeyDown(KEY_DELETE)){
            DeleteShape();
        }

        createProcress += Time.deltaTime * CSpeed;
        if(createProcress > 1){
            CreateShape();
            createProcress -= 1;
        }

        deleteProcess += Time.deltaTime * DSpeed;
        if(deleteProcess > 1){
            DeleteShape();
            deleteProcess -= 1;
        }
    }

    void CreateShape(){
        Shape o = shapeFactory.GetRandom();
        Transform transform = o.transform;
        transform.localPosition = Random.insideUnitSphere * 5;
        transform.localRotation = Random.rotation;
        transform.localScale = Vector3.one * Random.Range(0.1f, 1.0f);
        Material m = o.meshRenderer.material;
        int id = Random.Range(0,2);
        o.setMat(shapeFactory.GetMaterial(id), id);
        Color color = Random.ColorHSV(0f,1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f);
        o.setColor(color);  
        shapeFactory.addShape(o);
    }

    void DeleteShape(){
        if(shapeFactory.gameShapes.Count <= 0){
            return;
        }
        int index = Random.Range(0, shapeFactory.gameShapes.Count);
        Shape o = shapeFactory.gameShapes[index];
        shapeFactory.gameShapes[index] = shapeFactory.gameShapes[shapeFactory.gameShapes.Count - 1];
        shapeFactory.gameShapes[shapeFactory.gameShapes.Count - 1] = o;
        shapeFactory.DeleteShape(o);
    }

    void Save(){
        DataWriter writer = new DataWriter();
        shapeFactory.saveAllShape(writer);
        writer.CloseWriter();
    }

    void Load(){
        DataReader reader = new DataReader();
        shapeFactory.loadShapes(reader);
    }

    void ResetShape(){
        shapeFactory.DeleteAllShape();
    }
}
