using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimUtil : MonoBehaviour
{
    
    public float animTime = 0f;

    public float MaxDistance = 3.0f;

    // Rigidbody body;

    private void Awake() {
        // body = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onAnimMiddle(float a){
        Debug.Log("onAnimMiddle ： " + a);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Vector3 pos = GetComponent<Transform>().position;
        // float y = 0.2f + (animTime * MaxDistance);
        // Debug.Log("y = " + y);
        // body.MovePosition(new Vector3(pos.x, y, pos.z));
        
    }
}
