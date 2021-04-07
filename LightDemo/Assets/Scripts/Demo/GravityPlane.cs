using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlane : GravitySource
{
    public float gravity = 9.81f;

    [SerializeField, Min(0f)]
    float range = 10f;

    public override Vector3 getGravity(Vector3 position)
    {
        Vector3 up = transform.up;
        float distance = Vector3.Dot(up, position - transform.position);
        if(distance > range){
            return Vector3.zero;
        }

        float g = - gravity;
        if(distance > 0){
            g *= 1f - distance / range;
            Debug.Log("distance = " + distance + ", g = " + g);
        }

        return g * up;
    }

    private void OnDrawGizmos() {
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Vector3 size = new Vector3(11f, 0f, 11f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, size);

        Vector3 scale = transform.localScale;
        scale.y = range;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.up, size);

       
    }
    
}
