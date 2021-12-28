using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobTarget : MonoBehaviour
{
    public Camera Cam;
    public Transform muzzlepoint;
    Ray r;
    public RaycastHit Hitinfo;
    public float OffsetFactor;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update_fire()
    {
        r.origin = Cam.transform.position;
        r.direction = Vector3.RotateTowards(Cam.transform.forward,Cam.transform.up,1.57f*OffsetFactor,0);
        if(Physics.Raycast(r, out Hitinfo))
        {
        
        
        }


    }
}
