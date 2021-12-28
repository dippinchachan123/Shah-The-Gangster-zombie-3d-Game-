using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public GameObject Target;
    Vector3 Target_initialPos;
    Vector3 Camera_initialPos;
    // Start is called before the first frame update
    void Start()
    {
        Target_initialPos = Target.transform.position;
        Camera_initialPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            transform.position = Target.transform.position + (Camera_initialPos - Target_initialPos);

        }
    }
}
