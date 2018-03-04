using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCamera : MonoBehaviour {
    Camera camera;

    [SerializeField]
    Transform planeNowScreen;

    [SerializeField]
    Transform plane1024x600;

    public bool doScreenAdaptive = false;

    const float planeHalfHeight = 5;
    public float oldWidth = 1024;
    public float oldHeight =600;

    // Use this for initialization
    void Start () {
        camera = GetComponent<Camera>();

        var oldRatio = oldWidth / oldHeight;
        var newRatio = (float)Screen.width / Screen.height;
        var z = findZtoFitCameraHeight();

        ResetPlane(newRatio, z, planeNowScreen);
        ResetPlane(oldRatio, z, plane1024x600);

        if (!doScreenAdaptive)
            return;

        var scale=oldRatio/newRatio;
        if (scale > 1)
        {
            //PerspectiveCamera 不能設定orthographicSize 
            //所以要自己重算fieldOfView
            //tan(halfTheda)* z = scale*planeHalfHeight
            var halfTheda = Mathf.Atan(scale*planeHalfHeight / z);
            var newFov = 2 * halfTheda * Mathf.Rad2Deg;
            camera.fieldOfView = newFov;
        } 
    }

    
    void ResetPlane(float ratio,float z,Transform target)
    {
        target.localPosition = new Vector3(0, 0, z);
        target.localScale = new Vector3(ratio, 1, 1);
    }

    //用來算出佔滿整個Camera height的Plane
    float findZtoFitCameraHeight()
    {
        var halfAngle = 0.5f * camera.fieldOfView;
        var radian = Mathf.Deg2Rad * halfAngle;
        //fieldOfView是以height方向來測量!!
        //Mathf.Tan(radian)=planeHalfHeight/z
        var z = planeHalfHeight / Mathf.Tan(radian);
        return z;
    }
}
