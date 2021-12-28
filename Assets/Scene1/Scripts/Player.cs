using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController CharController;
    public Transform Cam;
    public GameObject Playr;
    public float Speed_F;
    public float Speed_S;
    float CurrentVelocity;
    public float Rotation_Sensitivity = 0.2f;
    public Vector3 g = new Vector3(0,-9.8f,0);
    public Vector3 vel = Vector3.zero;
    public float JumpSpeed;
    public Transform GroundCheckPoint;
    public LayerMask GroundLayer;
    public float GroundCheckRadius = 0.1f;
    public float JumpGroundCheckRadius;
    public Transform GroundedDetector_Leg;
    public float Speed_player_Forwad;
    float Speed_F_init;
    float Speed_S_init;
    bool pressedkey = false;
    //SmoothDamp_reference Variable
    float currentVelocity = 0.01f;
    float currentVelocity1 = 0.01f;
    float currentVelocity2 = 0.01f;
    float currentVelocity3 = 0.01f;
    float currentVelocity4 = 0.01f;
    float currentVelocity5 = 0.01f;
    float currentVelocity6 = 0.01f;
    //
    Quaternion RotInit;
    float velleft_f = 0;
    float velleft_h = 0;

    //Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CharController = transform.GetComponent<CharacterController>();
        Speed_F_init = Speed_F;
        Speed_S_init = Speed_S;
    }

    // Update is called once per frame
    void Update()
    {
        //Accesing the Firing Class
        Firing Firing_class = transform.GetChild(0).gameObject.GetComponent<Firing>();
        //Gravity

        vel += g * Time.deltaTime;
        CharController.Move(vel*(Time.deltaTime)*2);

        bool ifGrounded = Physics.CheckSphere(GroundCheckPoint.position, GroundCheckRadius, GroundLayer);
        if (ifGrounded)
        {
            if (vel.y < 0)
            {
                vel.y = -1f;
                Playr.GetComponent<Animator>().SetBool("JumpComplete", false);
            }
                       
        }
        //Jump and In Air Detection
        bool ifJumpGrounded = Physics.CheckSphere(GroundedDetector_Leg.position, JumpGroundCheckRadius, GroundLayer);
        if(ifJumpGrounded == false)
        {
            Playr.GetComponent<Animator>().SetBool("JumpAir", true);
        }
        else
        {
            Playr.GetComponent<Animator>().SetBool("JumpAir", false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vel.y = JumpSpeed*velleft_f*0.1f;
                Playr.GetComponent<Animator>().SetBool("JumpComplete", true);
            }
        }


        //Moving and rotation according to Camera
        //Playr.GetComponent<Animator>().SetFloat("speed", 0);

        velleft_f = SmoothDamp(velleft_f, 0f);
        Playr.GetComponent<Animator>().SetFloat("vel_f",velleft_f);
        velleft_h = Mathf.SmoothDamp(velleft_h, 0.005f, ref currentVelocity6, 0.1f);
        Playr.GetComponent<Animator>().SetFloat("vel_h", velleft_h);
        Vector2 Axis_Array = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //Forward Movements
        if (Mathf.Abs(Axis_Array[1]) > 0)
        {
            Vector3 direction;
            if ((Firing_class.Aim) || (Firing_class.Aim && Firing_class.fire))
            {
                direction = Vector3.ProjectOnPlane((transform.position - Cam.position).normalized , Vector3.up).normalized;
                Vector3 offset = (Vector3.Cross(Vector3.up, direction).normalized *Firing_class.Rotation_Offset);
                direction = direction + offset;
            }
            else
            {
                direction = Vector3.ProjectOnPlane((transform.position - Cam.position).normalized, Vector3.up).normalized;
            }
            
            CharController.Move(direction * Axis_Array[1] * Time.deltaTime * Speed_F);
            Quaternion Torotation = Quaternion.LookRotation(Vector3.ProjectOnPlane (direction,Vector3.up), Vector3.up);
            transform.GetChild(0).transform.localRotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Torotation,Rotation_Sensitivity * Time.deltaTime);
            
            
            Playr.GetComponent<Animator>().SetFloat("vel_f", (Speed_F / Speed_F_init) * Mathf.Sign(Axis_Array[1]));
            velleft_f = Speed_F / Speed_F_init * Mathf.Sign(Axis_Array[1]);
            
           
            Playr.GetComponent<Animator>().SetFloat("speed",Axis_Array.magnitude);

           
            if (PressedKey(KeyCode.LeftControl) && !(Firing_class.Aim))
            {
                Speed_F = Mathf.SmoothDamp(Speed_F, Speed_F_init , ref currentVelocity, 0.5f);
            }
            else
            {
                Speed_F = Mathf.SmoothDamp(Speed_F, Speed_F_init * 0.1f + Speed_player_Forwad, ref currentVelocity1, 0.1f);
            }
        }
        else
        {
            if (Speed_F > 0.001)
            {
                Speed_F = Mathf.SmoothDamp(Speed_F, 0, ref currentVelocity2, 0.1f);
            } 
        }

        //Sideways Movement
        if (Mathf.Abs(Axis_Array[0]) > 0)
        {
            Vector3 direction;
            if ((Firing_class.Aim) || (Firing_class.Aim && Firing_class.fire))
            {
                direction = Vector3.ProjectOnPlane((transform.position - Cam.position).normalized, Vector3.up).normalized;
                Vector3 offset = (Vector3.Cross(Vector3.up, direction).normalized * Firing_class.Rotation_Offset);
                direction = direction + offset;
            }
            else
            {
                direction = Vector3.ProjectOnPlane((transform.position - Cam.position).normalized, Vector3.up).normalized;
            }
            
            CharController.Move(Vector3.Cross(Vector3.up, direction).normalized * Mathf.Pow(Axis_Array[0], 1) * Time.deltaTime * Mathf.Clamp(Speed_S,0,7));


            Quaternion Torotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, Vector3.up), Vector3.up);
            transform.GetChild(0).transform.localRotation = Quaternion.RotateTowards(transform.GetChild(0).transform.rotation, Torotation, Rotation_Sensitivity * Time.deltaTime);


            
            Playr.GetComponent<Animator>().SetFloat("vel_h", (Speed_S / Speed_S_init) * Mathf.Sign(Axis_Array[0]));
            velleft_h = Speed_S / Speed_S_init * Mathf.Sign(Axis_Array[0]);
            

            Playr.GetComponent<Animator>().SetFloat("speed", Axis_Array.magnitude);


            if (PressedKey(KeyCode.LeftControl)&& !(transform.GetChild(0).gameObject.GetComponent<Firing>().Aim))
            {
                Speed_S = Mathf.SmoothDamp(Speed_S, Speed_S_init, ref currentVelocity3, 0.5f);
            }
            else
            {
                Speed_S = Mathf.SmoothDamp(Speed_S, Speed_S_init * 0.1f, ref currentVelocity4, 0.1f);
            }
        }
        else
        {
             if (Speed_S > 0.001)
             {
                Speed_S = Mathf.SmoothDamp(Speed_S, 0, ref currentVelocity5, 0.1f);
             }
             
            
        }
        //setting the right position of Player - (Amit Shah)
        Vector3 pos = transform.GetChild(0).transform.GetChild(0).localPosition;
        if (pos.x != 0)
        {
            pos.x = 0;
            pos.z = 0;

            transform.GetChild(0).transform.GetChild(0).localPosition = pos;
        }
        

        

    }
    bool PressedKey(KeyCode Key)
    {
        
        if (Input.GetKeyDown(Key))
        {
            pressedkey = true;
        }
        if (Input.GetKeyUp(Key))
        {
            pressedkey = false;
        }
        return pressedkey;
    }

    float SmoothDamp(float a,float b,float velleft = 0.1f)
    {
        if (a!= 0)
        {
            a = a - Mathf.Sign(a - b)*0.0072f*Mathf.Pow(Speed_F/Speed_F_init,- 0.0005f);
        }
        if (Mathf.Abs(a - b) < 0.01)
        {
            a = b;
        }
        return a;

    }


}



