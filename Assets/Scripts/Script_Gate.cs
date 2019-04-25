using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Gate : MonoBehaviour
{
    //Permanent references
    public Material mat_BlueOff;
    public Material mat_OrangeOff;
    public Material mat_RedOff;
    public Material mat_BlueOn;
    public Material mat_OrangeOn;
    public Material mat_RedOn;
    public GameObject obj_PedestalBlue;
    public GameObject obj_PedestalOrange;
    public GameObject obj_PedestalRed;
    public GameObject obj_Stone1;
    public GameObject obj_Door;

    Material[] mat_Array;

    //Variables of pedestals
    bool b_BlueActive = false;
    bool b_OrangeActive = false;
    bool b_RedActive = false;
    bool b_OpenedDoor = false;


    // Update is called once per frame
    void Update()
    {
        Function_CheckPedestals();
    }

    void Function_CheckPedestals()
    {
        if(obj_PedestalBlue.GetComponent<Script_Pedestal>().b_isActivated == true)
        {
            b_BlueActive = true;
            mat_Array = obj_Stone1.GetComponent<Renderer>().materials;
            mat_Array[2] = mat_BlueOn;
            obj_Stone1.GetComponent<Renderer>().materials = mat_Array;
        }
        else
        {
            b_BlueActive = false;
            mat_Array = obj_Stone1.GetComponent<Renderer>().materials;
            mat_Array[2] = mat_BlueOff;
            obj_Stone1.GetComponent<Renderer>().materials = mat_Array;
        }
        if(obj_PedestalOrange.GetComponent<Script_Pedestal>().b_isActivated == true)
        {
            b_OrangeActive = true;
            mat_Array = obj_Door.GetComponent<Renderer>().materials;
            mat_Array[2] = mat_OrangeOn;
            obj_Door.GetComponent<Renderer>().materials = mat_Array;
        }
        else
        {
            b_OrangeActive = false;
            mat_Array = obj_Door.GetComponent<Renderer>().materials;
            mat_Array[2] = mat_OrangeOff;
            obj_Door.GetComponent<Renderer>().materials = mat_Array;
        }
        if(obj_PedestalRed.GetComponent<Script_Pedestal>().b_isActivated == true)
        {
            b_RedActive = true;
            mat_Array = obj_Door.GetComponent<Renderer>().materials;
            mat_Array[3] = mat_RedOn;
            obj_Door.GetComponent<Renderer>().materials = mat_Array;
        }
        else
        {
            b_RedActive = false;
            mat_Array = obj_Door.GetComponent<Renderer>().materials;
            mat_Array[3] = mat_RedOff;
            obj_Door.GetComponent<Renderer>().materials = mat_Array;
        }

        if(b_BlueActive == true && b_OrangeActive == true && b_RedActive == true && b_OpenedDoor == false)
        {
            GetComponent<Animator>().Play ("Rotating");
            b_OpenedDoor = true;
            GameObject ref_ManagerAudio = GameObject.Find("ManagerAudio");
            if (ref_ManagerAudio != null) ref_ManagerAudio.GetComponent<Script_ManagerAudio>().Function_PlayAudio("s_Magic");
        }
    }
}
