using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoLi : MonoBehaviour
{
    
    Quaternion dQuaternion = Quaternion.identity;
    Transform trans;
    
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        dQuaternion = Quaternion.FromToRotation(Vector3.up, Vector3.right) * Quaternion.FromToRotation(Vector3.right,Vector3.up);
        trans.rotation = dQuaternion;
    }
}
