using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float HP = 100;
    private Animator manimator;
    private PlayerMove playerMove;
    private SkinnedMeshRenderer render;
    private ShootManager shootManager;

    public float smotting = 2;

	// Use this for initialization
	void Awake () {
        manimator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        render = GameObject.FindWithTag(Tags.PlayerRender).GetComponent<SkinnedMeshRenderer>();
        shootManager = this.GetComponentInChildren<ShootManager>();
	}
	
	// Update is called once per frame
	void Update () {
        render.material.color = Color.Lerp(render.material.color,Color.white,smotting*Time.deltaTime);
	}

    public void PlayHurt(float hurt){
        if (HP <= 0) return;

        HP -= hurt;

        Debug.Log(render.material.color);

        render.material.color = Color.red;

        if(HP <= 0){
            manimator.SetBool("Death",true);
            Dead();
        }
    }

    void Dead(){
        playerMove.enabled = false;
        shootManager.enabled = false;
    }
}
