using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    //Settings
    public float f_InteractionDistance = 5f;
    public float f_CarryDistance = 5f;
    public float f_ThrowForce = 500f;

    //Controls
    string Control_Interact = "e";
    string Control_SelectSlot_01 = "1";
    string Control_SelectSlot_02 = "2";
    string Control_SelectSlot_03 = "3";
    string Control_LeftClick = "mouse 0";
    string Control_RightClick = "mouse 1";
    string Control_MiddleClick = "mouse 2";

    //References
    public GameObject obj_CarryingParent;

    //Changing variables
    int i_SelectedSlot = 1;
    bool b_HoldingObject = false;
    GameObject obj_Interactable;
    bool b_HoldingObjectisColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Function_CheckInputs();

        if(b_HoldingObject == true)
        {
            Function_UpdateCarriedObject();
        }
    }

    //Check for inputs
    void Function_CheckInputs()
    {
        //Input for mouse
        if (Input.GetKeyDown(Control_LeftClick)) { Function_LeftClick(); }
        if (Input.GetKeyDown(Control_RightClick)) { Function_RightClick(); }
        if (Input.GetKeyDown(Control_MiddleClick)) { Function_MiddleClick(); }
        //Input for interact
        if (Input.GetKeyDown(Control_Interact)) { Function_Interact(); }
        //Input for selecting slot
        if (Input.GetKeyDown(Control_SelectSlot_01)) { Function_ChangeSelectedSlot(1); };
        if (Input.GetKeyDown(Control_SelectSlot_02)) { Function_ChangeSelectedSlot(2); };
        if (Input.GetKeyDown(Control_SelectSlot_03)) { Function_ChangeSelectedSlot(3); };
    }

    //Left Click
    void Function_LeftClick()
    {
        Debug.Log("Left clicked.");

        if (b_HoldingObject == true)
        {
            obj_Interactable.GetComponent<Rigidbody>().useGravity = true;
            obj_Interactable.transform.SetParent(null);
            obj_Interactable.GetComponent<Rigidbody>().AddForce(obj_CarryingParent.transform.forward * f_ThrowForce);
            Debug.Log("Object thrown.");
            b_HoldingObject = false;
        }
    }

    //Right Click
    void Function_RightClick()
    {
        Debug.Log("Right clicked.");
    }

    //Middle Click
    void Function_MiddleClick()
    {
        Debug.Log("Middle clicked.");
    }

    //Try to interact
    void Function_Interact()
    {
        Debug.Log("Interact pressed.");

        if(b_HoldingObject == true)
        {
            obj_Interactable.GetComponent<Rigidbody>().useGravity = true;
            obj_Interactable.transform.SetParent(null);
            Debug.Log("Dropped Object due to interact pressed.");
            b_HoldingObject = false;
        }
        else
        {
            obj_Interactable = Function_Raycast();
            if (obj_Interactable != null)
            {
                if (obj_Interactable.gameObject.tag == "tag_Interactable")
                {
                    obj_Interactable.GetComponent<Rigidbody>().useGravity = false;
                    obj_Interactable.GetComponent<Rigidbody>().detectCollisions = true;
                    obj_Interactable.transform.SetParent(obj_CarryingParent.transform);
                    Debug.Log("This object is interactable. Holding Object.");
                    b_HoldingObject = true;
                }
                else
                {
                    Debug.Log("This object is not interactable.");
                }
            }
        }
    }

    //Update carried object
    void Function_UpdateCarriedObject()
    {
        obj_Interactable.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj_Interactable.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        if (obj_Interactable)
        {

        }
        //Keep position at handle
        if(obj_Interactable.transform.localPosition.x != 0)
        {
            if(obj_Interactable.transform.localPosition.x > 0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3( -0.1f, 0, 0);
            }
            else if(obj_Interactable.transform.localPosition.x < -0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3(0.1f, 0, 0);
            }
            if (obj_Interactable.transform.localPosition.y > 0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3(0, -0.1f, 0);
            }
            else if (obj_Interactable.transform.localPosition.y < -0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3(0, 0.1f, 0);
            }
            if (obj_Interactable.transform.localPosition.z > 0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3(0, 0, -0.1f);
            }
            else if (obj_Interactable.transform.localPosition.z < -0.6f)
            {
                obj_Interactable.transform.localPosition += new Vector3(0, 0, 0.1f);
            }
        }
    }

    //Interact Raycast
    GameObject Function_Raycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Debug show raycast
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward * f_InteractionDistance);
        Debug.DrawRay(Camera.main.transform.position, forward, Color.red, 1, false);

        //Continue if something is hit.
        if (Physics.Raycast (ray, out hit, f_InteractionDistance))
        {
            Debug.Log("Object raycasthit: " + hit.collider.name);
            return hit.collider.gameObject;
        }
        return null;
    }

    //Change selected slot
    void Function_ChangeSelectedSlot(int i_SlotToSelect)
    {
        i_SelectedSlot = i_SlotToSelect;
        Debug.Log("Slot " + i_SelectedSlot + " selected.");
    }
}
