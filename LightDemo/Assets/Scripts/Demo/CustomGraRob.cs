using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGraRob : MonoBehaviour
{

    Rigidbody body;

    private void Awake() {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float delayTime = 0f;
    private void FixedUpdate() {
        if(body.IsSleeping()){
            return;
        }

        if(body.velocity.sqrMagnitude < 0.0001f){
            delayTime += Time.deltaTime;
            if(delayTime >= 1f){
                return;
            }
        }else{
            delayTime = 0f;
        }

        body.AddForce(CustomGravity.GetGravity(body.position), ForceMode.Acceleration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
