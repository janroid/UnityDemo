using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)]
    float Distance = 5f;

    [SerializeField, Min(0f)]
    float focusRadius = 1.0f;

    [SerializeField, Range(0f, 1f)]
    float focusCentering = 0.5f;

    public Transform focus = default;
    private Vector3 focusPoint;
    private void Awake() {
        focusPoint = focus.position;
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
        UpdateFocusPoint();
        Vector3 direction = transform.forward;
        transform.localPosition = focusPoint - direction * Distance;
    }

    void UpdateFocusPoint(){
        Vector3 curFocusPoint = focus.position;
          
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
}
