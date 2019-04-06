using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Interactable : MonoBehaviour
{

    [SerializeField] float f_ThrowForce = 500f;
    [SerializeField] float f_CarryDistance;

    Vector3 v_ObjectPosition;

    public GameObject obj_Interactable;
    public GameObject obj_Carrier;
    public bool b_isHolding;


    // Update is called once per frame
    void Update()
    {
        
    }
}
