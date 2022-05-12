using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public Transform transform;
    public MeshRenderer meshRenderer;
    static MaterialPropertyBlock propertyBlock;
    static int propertyID = Shader.PropertyToID("_Color");
    private int shapeID;
    private int materialID;
    private Color color;
    public int MaterialID
    {
        set{
            materialID = value;
        }
        get{
            return materialID;
        }
    }

    public int ShapeID
    {
        set{
            shapeID = value;
        }

        get{
            return shapeID;
        }
    }
    private void Awake() {
        transform = GetComponent<Transform>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void setMat(Material material, int ID){
        materialID = ID;
        meshRenderer.material = material;
        meshRenderer.material.color = color;
    }

    public void setColor(Color color){
        Debug.Log("Shape-Color = "+color.ToString());
        this.color = color;
        if(propertyBlock == null){
            propertyBlock = new MaterialPropertyBlock();
        }
        propertyBlock.SetColor("_Color",color);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    public void Save(DataWriter writer){
        writer.Write(shapeID);
        writer.Write(transform.localPosition);
        writer.Write(transform.localRotation);
        writer.Write(transform.localScale);
        writer.Write(meshRenderer.material.color);
        writer.Write(materialID);

        Debug.Log("Shape-save-ID = " + shapeID);
    }

    public int Load(DataReader reader){
        Vector3 pos = reader.ReadVec3();
        Quaternion qua = reader.ReadQuat();
        Vector3 scale = reader.ReadVec3();
        Color color = reader.ReadColor();
        int maID = reader.ReadInt();

        transform.localPosition = pos;
        transform.localRotation = qua;
        transform.localScale = scale;
        setColor(color);
        
        

        return maID;
    }

}
