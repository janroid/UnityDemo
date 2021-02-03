using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionUtil : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f;

	[SerializeField, Range(0f, 100f)]
	float maxAcceleration = 10f, maxAirAcceleration = 1f;

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;

	[SerializeField, Range(0, 90)]
	float maxGroundAngle = 25f, maxStairsAngle = 50f;

	[SerializeField, Range(1f, 100f)]
	public float maxSnapSpeed = 100f;

	[SerializeField, Range(1f, 100f)]
	float probeDistance = 1f;
	[SerializeField]
	LayerMask probeMask = -1, stairsMask = -1;

	Rigidbody body;

	Vector3 velocity, desiredVelocity;

	Vector3 contactNormal; // 当前接触地面的法线

	bool desiredJump;

	int groundContactCount; //地面接触数量

	bool OnGround => groundContactCount > 0;

	int jumpPhase;

	float minGroundDotProduct, minStairsDotProduct; // 地面法线最小角度
	int stepsSinceLastGrounded = 0;
	int stepsSinceLastJump = 0;

    // Start is called before the first frame update
    void OnValidate () {
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
		minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
	}

	void Awake () {
		body = GetComponent<Rigidbody>();
		OnValidate();
	}

	void Update () {
		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

		desiredJump |= Input.GetButtonDown("Jump");

		GetComponent<Renderer>().material.SetColor("_Color", OnGround? Color.red : Color.white); 
	}

	void FixedUpdate () {
		UpdateState();
		AdjustVelocity();

		if (desiredJump) {
			desiredJump = false;
			Jump();
		}

		body.velocity = velocity;
		ClearState();
	}

	void ClearState () {
		groundContactCount = 0;
		contactNormal = Vector3.zero;
	}

	void UpdateState () {
		velocity = body.velocity;
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		if (OnGround || SnapToGround()) {
			jumpPhase = 0;
			stepsSinceLastGrounded = 0;
			if (groundContactCount > 1) {
				contactNormal.Normalize();
			}
		}
		else {
			contactNormal = Vector3.up;
		}
	}

	void AdjustVelocity () {
		Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

		float currentX = Vector3.Dot(velocity, xAxis);
		float currentZ = Vector3.Dot(velocity, zAxis);

		float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

		float newX =
			Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
		float newZ =
			Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	}

	void Jump () {
		if (OnGround || jumpPhase < maxAirJumps) {
			jumpPhase += 1;
			stepsSinceLastJump = 0;
			float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
			float alignedSpeed = Vector3.Dot(velocity, contactNormal);
			if (alignedSpeed > 0f) {
				jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
			}
			velocity += contactNormal * jumpSpeed;
		}
	}

	void OnCollisionEnter (Collision collision) {
		EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
		EvaluateCollision(collision);
	}

	void EvaluateCollision (Collision collision) {
		for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
			if (normal.y >= getMinDot(collision.gameObject.layer)) {
				groundContactCount += 1;
				contactNormal += normal;
			}
		}
	}

	Vector3 ProjectOnContactPlane (Vector3 vector) {
		return vector - contactNormal * Vector3.Dot(vector, contactNormal);
	}
    
	bool SnapToGround(){
		if(stepsSinceLastGrounded > 1){
			return false;
		}

		float speed = velocity.magnitude;
		if(speed > maxSnapSpeed){
			
			return false;
		}

		if(!Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, probeDistance, probeMask)){
			return false;
		}

		if(hit.normal.y < getMinDot(hit.collider.gameObject.layer) || stepsSinceLastJump <= 2){
			return false;
		}

		groundContactCount = 1;
		contactNormal = hit.normal;
		
		float dot = Vector3.Dot(velocity, hit.normal);
		if(dot > 0){
			velocity = (velocity - hit.normal * dot).normalized * speed; // 减去坡度造成的力改变的速度
		}

		return true;
	}

	// 判断当前碰撞点属于哪个层
	float getMinDot(int layer){
		if((stairsMask & ( 1 << layer)) != 0){
			return minStairsDotProduct;
		}
		return minGroundDotProduct;
	}
}
