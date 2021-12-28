using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform Cam;
    public float weight;
    float Rot_init;
    public Vector2 Range;
    public bool DoClamp = false;
    // Start is called before the first frame update
    void Start()
    {
        if ((weight) > 1)
        {
            weight = 1;
        }
        else if ((weight) < 0)
        {
            weight = 0;
        }
        Rot_init = transform.rotation.eulerAngles.x;

    }

    // Update is called once per frame
    void Update()
    {
        float rot_cam = Cam.transform.rotation.eulerAngles.x;
        Vector3 rot_trans = transform.rotation.eulerAngles;
        float Value;
        if (DoClamp)
        {
            Value = AngleClamp((Rot_init + (rot_cam * weight)) % 360,Range);
        }
        else
        {
            Value = (Rot_init + (rot_cam * weight)) % 360;
        }
        
        Quaternion r = Quaternion.Euler(Value, rot_trans.y, rot_trans.z);
  
        transform.rotation = r;

        
    }
    float AngleClamp(float Value, Vector2 Range)
    {
        if ((Value >= 0) && (Value < 220))
        {
            return Mathf.Clamp(Value, 0, Range.x);
        }
        else if ((Value >= 220))
        {
            return Mathf.Clamp(Value, Range.y, 360);
        }
        else
        {
            return 1;
        }
    }
}
