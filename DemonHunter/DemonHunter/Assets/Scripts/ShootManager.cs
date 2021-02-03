using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour {

    public float shootRate = 1;
    public float attackPower = 30;
    private float timer;
    private Transform transform;
    private Light light;
    private ParticleSystem gunParticle;
    private LineRenderer lineRenderer;
    private int rayground;
    private AudioSource audio;
    private ZoomManager zoomManager;

    // Use this for initialization
    void Start () {
        transform = GetComponent<Transform>();
        light = GetComponent<Light>();
        gunParticle = GetComponentInChildren<ParticleSystem>();
        lineRenderer = GetComponent<LineRenderer>();
        rayground = LayerMask.GetMask("Ground");
        audio = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer >= 1/shootRate){
            timer -= 1 / shootRate;
            Shoot();

        }
            
	}

    void Shoot(){
        light.enabled = true;
        gunParticle.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0,transform.position);
        Ray gunRay = new Ray(transform.position,transform.forward);
        RaycastHit raycastHit;
        if(Physics.Raycast(gunRay,out raycastHit,200,rayground)){
            lineRenderer.SetPosition(1,raycastHit.point);
            if(raycastHit.collider.tag == Tags.Zoms){
                raycastHit.collider.GetComponent<ZoomManager>().TakeDamage(attackPower, raycastHit.point);;

            }


        }else{
            lineRenderer.SetPosition(1,transform.position + transform.forward*100);
        }
        audio.Play();
        Invoke("ShootEnd",0.05f);
    }

    void ShootEnd(){
        light.enabled = false;
        lineRenderer.enabled = false;

    }
}
