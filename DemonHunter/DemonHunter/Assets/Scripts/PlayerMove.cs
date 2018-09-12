using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public int speed = 5;

    private Rigidbody myrigidbody;
    private Transform player;
    private Animator anim;
    private int groundIndex;
    private Camera mainCamera;

    public const float PRECISION = 0.000001f;

	// Use this for initialization
	void Start () {
        myrigidbody = GetComponent<Rigidbody>();
        player = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        groundIndex = LayerMask.GetMask("Ground");
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // 载具类型的移动
        //Vector3 globalDirectionForward = player.TransformDirection(Vector3.forward);
        //Vector3 ForwardDirection = z * globalDirectionForward;//物体前后移动的方向
        //Vector3 GlobalDirectionRight = player.TransformDirection(Vector3.right);
        //Vector3 RightDirection = x * GlobalDirectionRight;//物体左右移动的方向
        //Vector3 MainDirection = ForwardDirection + RightDirection;
        //方案一
        //myrigidbody.AddForce(MainDirection * speed, ForceMode.Force);
        //方案二
        //myrigidbody.AddRelativeForce((x * Vector3.right + z * Vector3.forward) * speed, ForceMode.Force);
        //方案三
        //myrigidbody.velocity = new Vector3(
        //    speed * x,
        //    myrigidbody.velocity.y,
        //    speed * z
        //);
        //方案四
        //角色的移动
        myrigidbody.MovePosition(player.position + new Vector3(x, 0, z) * speed * Time.deltaTime);

        // 转向
        Ray mouceRay = mainCamera.ScreenPointToRay(Input.mousePosition); // 鼠标发送的射线
        RaycastHit raycastHit;
        if (Physics.Raycast(mouceRay, out raycastHit, 200, groundIndex))
        {
            Vector3 target = raycastHit.point - player.position;
            target.y = 0;
            //player.LookAt(target);
            Quaternion newRotation = Quaternion.LookRotation(target);
            myrigidbody.MoveRotation(newRotation);
        }

        if (Mathf.Abs(x) > PRECISION || Mathf.Abs(z) > PRECISION)
        {
            anim.SetBool("Move", true);
            anim.Play("Move");
        }
        else
        {
            anim.SetBool("Move", false);
            anim.Play("Idle");
        }

	}

}
