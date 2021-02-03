 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Transform player;
    private Transform mycamera;

    public float smooth = 3f;

	// Use this for initialization
	void Start () {
                player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
                mycamera = GetComponent<Transform>();
                mycamera.LookAt(player);
                
            
                
	}

	// Update is called once per frame
	void FixedUpdate () {
                Vector3 newPos = player.position + new Vector3(0,6,-6);

                mycamera.position = Vector3.Lerp(mycamera.position,newPos,Time.deltaTime * smooth);
	}
}
