using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
public class Firing : MonoBehaviour
{
    public Transform MainPlayer;
    public Transform[] GunsTransform = new Transform[2];
    public GameObject Gun;
    bool ButtonDownMouseLeft = false;
    bool ButtonDownMouseRight = false;
    bool transformUpdated = false;
    public Transform Cam;
    public Transform CamCine;
    public TrailRenderer trialRend;
    public Transform Muzzlept;
    ///
    float Reference1;
    float Reference2;
    Vector3 Reference3;
    float Reference4;
    float Reference5;
    float Reference6;
    float Reference7;
    float Reference8;
    float Reference9;
    Vector3 Reference10;
    /// 
    ///
    float Timeref1 = 0;
    float Timeref2 = 0;
    /// 
    [HideInInspector]
    public bool Aim;
    [HideInInspector]
    public bool fire;
    public int Bullets_total;
    [HideInInspector]
    public int Bullets_left;//out of 30 bullets
    bool Reloading_bool = false;
    [HideInInspector]
    public bool OnlyFireOver = false;
    Cinemachine.CinemachineFreeLook CamRigs;
    float CamRadius_init;
    Vector3 CamOffset_init;
    public Vector3 CamOffset_final;
    float[] CamSpeed_init = new float[2];
    public GameObject AimingRig;
    public GameObject AimingRig2;
    public Rig[] ReloadingRig = new Rig[2];
    public GameObject LeftHandIk;
    Animator anim;
    Animator anim2;
    public GameObject Riglayers;
    public float Rotation_Offset;

   
    //
    public ParticleSystem[] MuzzleFire = new ParticleSystem[2];
    public ParticleSystem Metaleffect;
    float EmitNo = 1;

    //HitInfo
    public KnobTarget Knobtarget;
    // Start is called before the first frame update
    void Start()
    {
        anim = MainPlayer.GetComponent<Animator>();
        anim2 = Riglayers.GetComponent<Animator>();
        CamRigs = CamCine.GetComponent<CinemachineFreeLook>();
        CamRadius_init = CamRigs.m_Orbits[1].m_Radius;
        CamOffset_init = CamCine.GetComponent<CinemachineCameraOffset>().m_Offset;
        CamSpeed_init[0] = CamRigs.m_XAxis.m_MaxSpeed;
        CamSpeed_init[1] = CamRigs.m_YAxis.m_MaxSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        Aim = PressedButton(1, ref ButtonDownMouseRight);
        
        fire = PressedButton(0, ref ButtonDownMouseLeft);
        Reloading();
        ReloadingAnimation();
        if (fire)
        {
            Knobtarget.GetComponent<KnobTarget>().Update_fire();
            if (Timer(0.15f, ref Timeref1))
            {
                RaycastHit hit = Knobtarget.GetComponent<KnobTarget>().Hitinfo;
                trialRend = Instantiate(trialRend,Muzzlept.position,Quaternion.identity);
                trialRend.AddPosition(Muzzlept.position);
                Metaleffect.transform.position = hit.point;
                Metaleffect.transform.forward = hit.normal;
                Metaleffect.Emit(1);
                foreach (var particle in MuzzleFire)
                {
                    particle.Emit(1);
                    trialRend.transform.position = hit.point;
                    try
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal*5, hit.point, ForceMode.Impulse);
                    }
                    catch { }
                    
                }
                
            }
        }
        else
        {
            Timeref1 = 0.151f;
        }
        anim.SetBool("Aim", Aim);
        anim.SetBool("Fire",fire);

        //CamOffset
        CinemachineCameraOffset Camcine_offset = CamCine.GetComponent<CinemachineCameraOffset>();
        if (Aim)
        {
            
            //Aiming and firing in Right Direction 
            Vector3 direction = (transform.parent.position - Cam.position).normalized;
            
            Quaternion Torotation = Quaternion.LookRotation(direction + (Vector3.Cross(Vector3.up, direction).normalized*Rotation_Offset), Vector3.up);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, Torotation, transform.parent.GetComponent<Player>().Rotation_Sensitivity * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y , 0);
            AimingRig.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig.GetComponent<Rig>().weight, 1, ref Reference1, 0.05f);
            AimingRig2.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig2.GetComponent<Rig>().weight, 1, ref Reference4, 0.05f);
            Camcine_offset.m_Offset = Vector3.SmoothDamp(Camcine_offset.m_Offset,CamOffset_final,ref Reference3, 0.09f);
            CamRigs.m_XAxis.m_MaxSpeed = CamSpeed_init[0] / 2;
            CamRigs.m_YAxis.m_MaxSpeed = CamSpeed_init[1] / 2;

        }
        else if(!Aim && !OnlyFireOver)
        {

            AimingRig.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig.GetComponent<Rig>().weight, 0.0001f, ref Reference2, 0.2f);
            AimingRig2.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig2.GetComponent<Rig>().weight, 0.0001f, ref Reference5, 0.05f);
            
            Quaternion rot = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
            transform.localRotation = rot;
            transformUpdated = false;
            Camcine_offset.m_Offset = Vector3.SmoothDamp(Camcine_offset.m_Offset, CamOffset_init, ref Reference10, 0.13f);

            CamRigs.m_XAxis.m_MaxSpeed = CamSpeed_init[0];
            CamRigs.m_YAxis.m_MaxSpeed = CamSpeed_init[1];
            
        }
        if ((!Aim) && fire)
        {
            Vector3 direction = (transform.parent.position - Cam.position).normalized;

            Quaternion Torotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, Torotation, transform.parent.GetComponent<Player>().Rotation_Sensitivity *3f* Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            AimingRig.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig.GetComponent<Rig>().weight, 1, ref Reference1, 0.05f);
            AimingRig2.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig2.GetComponent<Rig>().weight, 1, ref Reference4, 0.05f);
            OnlyFireOver = true;
        }
        else
        {
            if (OnlyFireOver)
            {
                if (Timer(0.5f, ref Timeref2))
                {
                    AimingRig.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig.GetComponent<Rig>().weight, 0.0001f, ref Reference2, 0.2f);
                    AimingRig2.GetComponent<Rig>().weight = Mathf.SmoothDamp(AimingRig2.GetComponent<Rig>().weight, 0.0001f, ref Reference5, 0.05f);

                    Quaternion rot = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
                    transform.localRotation = rot;
                    transformUpdated = false;
                    OnlyFireOver = false;
                }
            }
            
            
        }







    }
    bool PressedButton(int key,ref  bool ButtonDown)
    {
        
        if (Input.GetMouseButtonDown(key))
        {
            ButtonDown = true;
            

        }
        if (Input.GetMouseButtonUp(key))
        {
            ButtonDown = false;
        }
        return ButtonDown;
    }
    bool Timer(float time,ref float Timeref)
    {
        Timeref += Time.deltaTime;
        if (Timeref > time)
        {
            Timeref = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
    void Reloading()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
          
            Reloading_bool = true;

        }
        if (isPlaying(anim2, "Reload"))
        {
            Reloading_bool = false;
        }

    }
    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            return true;
        else
            return false;
    }
    void ReloadingAnimation()
    {
        if (Reloading_bool)
        {
            anim2.SetBool("Reloading", true);
            ReloadingRig[0].weight = Mathf.SmoothDamp(ReloadingRig[0].weight, 1, ref Reference6, 0.3f);
            ReloadingRig[1].weight = Mathf.SmoothDamp(ReloadingRig[1].weight, 1, ref Reference7, 0.3f);
        }
        else
        {
            anim2.SetBool("Reloading", false);
            ReloadingRig[0].weight = Mathf.SmoothDamp(ReloadingRig[0].weight, 0, ref Reference8, 0.05f);
            ReloadingRig[1].weight = Mathf.SmoothDamp(ReloadingRig[1].weight, 0, ref Reference9, 0.05f);
        }
    }
   
}
