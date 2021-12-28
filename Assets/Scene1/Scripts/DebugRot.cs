using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRot : MonoBehaviour
{
    float Rot_init;
    public Camera Cam;
    // Start is called before the first frame update
    void Start()
    {
        Rot_init = transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(AngelToStdScale(Rot_init + Cam.transform.rotation.eulerAngles.x));
        }
        
    }
    float AngelToStdScale(float Value)
    {
        return (Value % 360);
    }
}
