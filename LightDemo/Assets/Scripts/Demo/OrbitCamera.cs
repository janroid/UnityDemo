using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)]
    float distance = 5f;

    [SerializeField]
    LayerMask obstructionMask = -1;

    [SerializeField, Min(0f)]
    float focusRadius = 1.0f;  //聚焦半径

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;

    [SerializeField, Range(-89f, 89f)]
    float minVerticalAngle = 30, maxVerticalAngle = 60;

    [SerializeField]
    float alignDelay = 5f;

    [SerializeField, Range(0f, 90f)]
	float alignSmoothRange = 45f;

    [SerializeField, Range(0f, 360f)]
    float upAlignmentSpeed = 180f; // 更换重力场景时，摄像机转换角度的速度

    public Transform focus = default;
    
    private Vector3 focusPoint, lastFocusPoint; //上一次位置

    private Vector2 orbitAngles = new Vector2(45f, 0f); // 摄像机默认的观察方向

    private const float MIN_ANGLE = 0.001f;

    private float lastManualRotationTime = 0f;

    Quaternion gravityAlignment = Quaternion.identity;
    Quaternion orbitRotation;
    Camera regularCamera;
    SphereCollider carCollider;

    Vector3 CameraHalfExtends {
        get {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);

            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            
            return halfExtends;
        }
    }

    private void OnValidate() {
        if(minVerticalAngle > maxVerticalAngle){
            minVerticalAngle = maxVerticalAngle;
        }
    }
    private void Awake() {
        regularCamera = GetComponent<Camera>();
        focusPoint = focus.position;
        transform.localRotation = orbitRotation = Quaternion.Euler(orbitAngles);
        carCollider = focus.GetComponent<SphereCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {
        UpdateGravityAlignment();

        UpdateFocusPoint();
        if(ManualRotation() || AutomaticRotation()){
            ConstrainAngles();
            orbitRotation = Quaternion.Euler(orbitAngles);
        }

        // Quaternion lookRotation = Quaternion.Euler(orbitAngles);  //把欧拉角的摄像机方向转换为四元数
        Quaternion lookRotation = gravityAlignment * orbitRotation;
        Vector3 lookDirection = lookRotation * Vector3.forward; //得到旋转向量
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        
        Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPostion = lookPosition + rectOffset;
        Vector3 castFrom = focus.position;
        Vector3 castLine = rectPostion - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;  // 归一化

        if(Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, lookRotation, castDistance, obstructionMask)){
            if(carCollider.radius > hit.distance){
                hit.distance = carCollider.radius + 0.1f;
            }

            rectPostion = castFrom + castDirection * hit.distance;
            Debug.Log("POS = "+rectPostion);
            lookPosition = rectPostion  - rectOffset;
        }

        transform.SetPositionAndRotation(lookPosition,lookRotation);

    }
    
    // 计算重力场切换时，相机对齐速度，对齐太快的话，使用差值，好增加一个过渡动画。
    void UpdateGravityAlignment(){
        Vector3 fromUp = gravityAlignment * Vector3.up;
        Vector3 toUp = CustomGravity.GetUpAxis(focusPoint);
        float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp),-1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        float maxAngle = upAlignmentSpeed * Time.deltaTime;

        Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;

        if(angle <= maxAngle){
            gravityAlignment = newAlignment;
        }else{
            gravityAlignment = Quaternion.SlerpUnclamped(gravityAlignment, newAlignment, maxAngle / angle);
        }

    }

    //相机跟随球移动
    void UpdateFocusPoint(){

        Vector3 curFocusPoint = focus.position;
        lastFocusPoint = focusPoint;
        if(focusRadius > 0f){
            float len = Vector3.Distance(focusPoint, curFocusPoint);
            float t = 0f;
            if(len > 0.01f && focusCentering > 0){
                t = Mathf.Pow(focusCentering, Time.unscaledDeltaTime);
            }

            if(len > focusRadius){
                t = Mathf.Min(t, focusRadius / len ); //当球速度快，相机与目标瞬间距离很大时，使用根据距离计算的时间进行插值运算。
            }

            focusPoint = Vector3.Lerp(curFocusPoint, focusPoint, t);
        }else{
            focusPoint = curFocusPoint;
        }
        
    }

    // 限制角度在一定范围内可旋转
    void ConstrainAngles(){
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);
            if(orbitAngles.y > 360f){
                orbitAngles.y -= 360f;
            }

            if(orbitAngles.y < 0){
                orbitAngles.y += 360f;
            }
    }

    // 接收按键，计算相机旋转
    bool ManualRotation(){
        Vector2 input = new Vector2(
            Input.GetAxis("Vertical Camera"),
            Input.GetAxis("Horizontal Camera")
        );

        if(input.x < -MIN_ANGLE || input.x > MIN_ANGLE || input.y < -MIN_ANGLE || input.y > MIN_ANGLE){
            orbitAngles += rotationSpeed * input * Time.unscaledDeltaTime;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    // 判断及计算自动对齐
    bool AutomaticRotation(){
        if(Time.unscaledTime - lastManualRotationTime < alignDelay){
            return false;
        }

        Vector3 alignedDelta = Quaternion.Inverse(gravityAlignment) * (focusPoint - lastFocusPoint);

        Vector2 moveDir = new Vector2(alignedDelta.x, alignedDelta.z);
        float movementDeltaSqr = moveDir.sqrMagnitude;

        if(movementDeltaSqr < 0.0001f){
            return false;
        }

        //利用三角函数取旋转角度。
        float angle = getAngle(moveDir / Mathf.Sqrt(movementDeltaSqr));
        float subAngle = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, angle));
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);;
        if(subAngle < alignSmoothRange){
            rotationChange *= subAngle / alignSmoothRange;
        }else if(180f - subAngle < alignSmoothRange){
            rotationChange *= (180 - subAngle) / alignSmoothRange;
        }

        orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, angle, rotationChange);

        return true;
    }

    // 转换为角度
    // angleDir: 标准化后的向量
    float getAngle(Vector2 angleDir){
        float angle = Mathf.Acos(angleDir.y) * Mathf.Rad2Deg;

        return (angleDir.x < 0) ? -angle : angle; // 当x<0,表示往逆时针旋转角度，使用负角度或者用360 - angle也可以
    }
}
