using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        CustomGravity.Register(this);
    }

    private void OnDisable() {
        CustomGravity.Unregister(this);
    }

    public virtual Vector3 getGravity(Vector3 position){
        return Physics.gravity;
    }
}
