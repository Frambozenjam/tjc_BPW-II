using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Player : MonoBehaviour
{
    //Settings
    public float f_InteractionDistance = 5f;
    public float f_CarryDistance = 5f;
    public float f_ThrowForce = 500f;
    public float f_MovementSpeed = 3f;
    public float f_SprintSpeedMultiplyer = 1.5f;
    public float f_LookSensitivity_X = 1f;
    public float f_LookSensitivity_Y = 1f;
    public float f_CameraClampAngle = 80f;
    public float f_JumpStrength = 100f;
    public float f_Gravity = -9.81f;

    //Controls
    string Control_Interact = "e";
    string Control_Forward = "w";
    string Control_Left = "a";
    string Control_Backward = "s";
    string Control_Right = "d";
    string Control_SelectSlot_01 = "1";
    string Control_SelectSlot_02 = "2";
    string Control_SelectSlot_03 = "3";
    string Control_LeftClick = "mouse 0";
    string Control_RightClick = "mouse 1";
    string Control_MiddleClick = "mouse 2";

    //References
    public GameObject obj_CarryingParent;
    public GameObject obj_CameraParent;
    Rigidbody comp_Rigidbody;

    //Changing variables
    Vector3 v3_Velocity;
    int i_SelectedSlot = 1;
    bool b_HoldingObject = false;
    GameObject obj_Interactable;
    bool b_HoldingObjectisColliding = false;
    float f_CameraRotation_X;
    float f_CameraRotation_Y;
    bool b_MenuOpen = false;
    bool b_Sprinting = false;

    // Start is called before the first frame update
    void Start()
    {
        //Set character controller reference
        comp_Rigidbody = GetComponent<Rigidbody>();
        //Hide cursor
        Function_CursorMode("Locked");
        //set camera rotation values
        Vector3 v3_CameraRotation = transform.localRotation.eulerAngles;
        //f_CameraRotation_X = v3_CameraRotation.x;
        //v3_CameraRotation = obj_CameraParent.transform.localRotation.eulerAngles;
        //f_CameraRotation_Y = v3_CameraRotation.y;
    }

    // Update is called once per frame
    void Update()
    {
        Function_CheckInputs();
        Function_InputMovement();
        Function_Camera();

        if(b_HoldingObject == true)
        {
            Function_UpdateCarriedObject();
        }
    }

    void FixedUpdate()
    {
        Function_ApplyMovement();
    }

    //Check for inputs
    void Function_CheckInputs()
    {
        //Input for mouse
        if (Input.GetKeyDown(Control_LeftClick)) { Function_LeftClick(); }
        if (Input.GetKeyDown(Control_RightClick)) { Function_RightClick(); }
        if (Input.GetKeyDown(Control_MiddleClick)) { Function_MiddleClick(); }

        //old
        {
            //Input for movement
            //if (Input.GetKey(Control_Forward)) { Function_InputMovement("Forward"); }
            //if (Input.GetKey(Control_Left)) { Function_InputMovement("Left"); }
            //if (Input.GetKey(Control_Backward)) { Function_InputMovement("Backward"); }
            //if (Input.GetKey(Control_Right)) { Function_InputMovement("Right"); }
            //if (Input.GetKeyDown(KeyCode.Space)) { Function_InputMovement("Jump");  }
        }

        if (Input.GetKey(KeyCode.LeftShift)) { b_Sprinting = true; } else { b_Sprinting = false; }
        //UI
        if (Input.GetKeyDown(KeyCode.Escape)) { Function_ToggleMenu(); }
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
        if(b_MenuOpen == true)
        {
            Function_ToggleMenu();
        }
        else
        {
            if (b_HoldingObject == true)
            {
                obj_Interactable.GetComponent<Rigidbody>().useGravity = true;
                obj_Interactable.transform.SetParent(null);
                obj_Interactable.GetComponent<Rigidbody>().AddForce(obj_CarryingParent.transform.forward * f_ThrowForce);
                Debug.Log("Object thrown.");
                b_HoldingObject = false;
            }
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

    //apply movement speed to velocity from inputs
    void Function_InputMovement()
    {
        float f_Speed = f_MovementSpeed;
        if (b_Sprinting == true)
        {
            f_Speed = f_MovementSpeed * f_SprintSpeedMultiplyer;
        }
        else
        {
            f_Speed = f_MovementSpeed;
        }

        Vector3 v3_Forward = Input.GetAxisRaw("Vertical") * transform.forward;
        Vector3 v3_Right = Input.GetAxisRaw("Horizontal") * transform.right;
        v3_Velocity = (v3_Right + v3_Forward).normalized * f_Speed;

        //old movement
        {
            //if(s_Direction == "Forward")
            //{
            //    comp_Rigidbody.velocity = (transform.forward * f_MovementSpeed * Time.deltaTime);
            //}
            //if (s_Direction == "Backward")
            //{
            //    comp_Rigidbody.velocity = (-transform.forward * f_MovementSpeed * Time.deltaTime);
            //}
            //if (s_Direction == "Right")
            //{
            //    comp_Rigidbody.velocity = (transform.right * f_MovementSpeed * Time.deltaTime);
            //}
            //if (s_Direction == "Left")
            //{
            //    comp_Rigidbody.velocity = (-transform.right * f_MovementSpeed * Time.deltaTime);
            //}
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            comp_Rigidbody.AddForce(Vector3.up * f_JumpStrength);
        }
    }

    //Gravity
    void Function_ApplyMovement()
    {
        if(v3_Velocity != Vector3.zero)
        {
            comp_Rigidbody.MovePosition(comp_Rigidbody.position + v3_Velocity * Time.fixedDeltaTime);
        }
        //reset velocity to disable sliding
        comp_Rigidbody.velocity = new Vector3(0, comp_Rigidbody.velocity.y, 0);
    }

    //Camera
    void Function_Camera()
    {
        float f_Mouse_X = Input.GetAxis("Mouse X");
        float f_Mouse_Y = -Input.GetAxis("Mouse Y");

        f_CameraRotation_X += f_Mouse_X * f_LookSensitivity_X * Time.deltaTime;
        f_CameraRotation_Y += f_Mouse_Y * f_LookSensitivity_Y * Time.deltaTime;

        f_CameraRotation_Y = Mathf.Clamp(f_CameraRotation_Y, -f_CameraClampAngle, f_CameraClampAngle);

        obj_CameraParent.transform.localRotation = Quaternion.Euler(f_CameraRotation_Y, 0.0f, 0.0f);
        transform.rotation = Quaternion.Euler(0.0f, f_CameraRotation_X, 0.0f);
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

        //Keep position at handle
        obj_Interactable.transform.position = obj_CarryingParent.transform.position;
        obj_Interactable.transform.rotation = Quaternion.Euler(-90, obj_CarryingParent.transform.localRotation.y, obj_CarryingParent.transform.localRotation.z);

        //old
        {
            //if(obj_Interactable.transform.localPosition.x != 0)
            //{
            //    if(obj_Interactable.transform.localPosition.x > 0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3( -0.1f, 0, 0);
            //    }
            //    else if(obj_Interactable.transform.localPosition.x < -0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3(0.1f, 0, 0);
            //    }
            //    if (obj_Interactable.transform.localPosition.y > 0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3(0, -0.1f, 0);
            //    }
            //    else if (obj_Interactable.transform.localPosition.y < -0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3(0, 0.1f, 0);
            //    }
            //    if (obj_Interactable.transform.localPosition.z > 0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3(0, 0, -0.1f);
            //    }
            //    else if (obj_Interactable.transform.localPosition.z < -0.6f)
            //    {
            //        obj_Interactable.transform.localPosition += new Vector3(0, 0, 0.1f);
            //    }
            //}
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

    //Change cursor visibility
    void Function_CursorMode(string s_CursorMode)
    {
        if(s_CursorMode == "Unlocked")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if(s_CursorMode == "Locked")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Menu
    void Function_ToggleMenu()
    {
        if(b_MenuOpen == true)
        {
            //close menu and hide cursor
            Function_CursorMode("Locked");
            b_MenuOpen = false;
        }
        else if(b_MenuOpen == false)
        {
            //open menu and show cursor
            Function_CursorMode("Unlocked");
            b_MenuOpen = true;
        }
    }
}
