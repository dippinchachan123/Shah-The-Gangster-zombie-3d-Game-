using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadEventHandler : MonoBehaviour
{
    public GameObject[] Mags;//[MagOriginal,MagHandAttched,MagInstantiate(physics propertied)]
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void MagAttachtoHand()
    {
        Mags[0].active = false;
        Mags[1].active = true;

    }
    public void MagDeattach()
    {

        Instantiate(Mags[2], Mags[1].transform.position, Mags[1].transform.rotation);
        Mags[1].active = false;
    }
    public void NewMagAttach()
    {
        Mags[1].active = true;
    }
    public void MagAttachtoGun()
    {
        Mags[0].active = true;
        Mags[1].active = false;
    }
}
