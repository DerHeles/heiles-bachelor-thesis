using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NeuronVR_MenuManager : MonoBehaviour
{
    [Header("Gestures")]
    public Color handBasicColor;
    public Color handInterfaceColor;
    public GameObject[] menuBones;
    public GameObject[] menuBonesRight;
    public float minMenuGestureTime = 2.5f;
    public GameObject grabBoneLeft;
    public GameObject grabBoneRight;

    [Header("Equipment")]
    public NeuronVR_Equipment sword;
    public NeuronVR_Equipment shield;

    [Header("Items")]
    public NeuronVR_Item itemCube;
    public NeuronVR_Item itemStar;
    public NeuronVR_Item itemBall;
    public Transform spawnLocation;

    [Header("Teleporting")]
    public NeuronVR_LaserPointerHand laserPointer;
    public NeuronVR_ViveController m_controller1;
    public NeuronVR_ViveController m_controller2;

    [Header("Menu")]
    public NeuronVR_HoverButton m_menuButtonVive;
    public NeuronVR_HoverButton m_menuButtonTeleport;
    public NeuronVR_HoverButton m_menuButtonExit;

    public NeuronVR_InventoryButton m_inventoryButtonCube;
    public NeuronVR_InventoryButton m_inventoryButtonStar;
    public NeuronVR_InventoryButton m_inventoryButtonBall;

    public NeuronVR_HoverInfo m_infoVive;
    public NeuronVR_HoverInfo m_infoTeleport;

    [Header("Left Hand")]
    public Transform leftHandTransform;
    public NeuronVR_Hand leftHand;

    public Transform leftHand_Thumb_1;
    public Transform leftHand_Thumb_2;
    public Transform leftHand_Thumb_3;

    public Transform leftHand_Index;
    public Transform leftHand_Index_1;
    public Transform leftHand_Index_2;
    public Transform leftHand_Index_3;

    public Transform leftHand_Middle;
    public Transform leftHand_Middle_1;
    public Transform leftHand_Middle_2;
    public Transform leftHand_Middle_3;

    public Transform leftHand_Ring;
    public Transform leftHand_Ring_1;
    public Transform leftHand_Ring_2;
    public Transform leftHand_Ring_3;

    public Transform leftHand_Pinky;
    public Transform leftHand_Pinky_1;
    public Transform leftHand_Pinky_2;
    public Transform leftHand_Pinky_3;

    [Header("Right Hand")]
    public Transform rightHandTransform;
    public NeuronVR_Hand rightHand;

    public Transform rightHand_Thumb_1;
    public Transform rightHand_Thumb_2;
    public Transform rightHand_Thumb_3;

    public Transform rightHand_Index;
    public Transform rightHand_Index_1;
    public Transform rightHand_Index_2;
    public Transform rightHand_Index_3;

    public Transform rightHand_Middle;
    public Transform rightHand_Middle_1;
    public Transform rightHand_Middle_2;
    public Transform rightHand_Middle_3;

    public Transform rightHand_Ring;
    public Transform rightHand_Ring_1;
    public Transform rightHand_Ring_2;
    public Transform rightHand_Ring_3;

    public Transform rightHand_Pinky;
    public Transform rightHand_Pinky_1;
    public Transform rightHand_Pinky_2;
    public Transform rightHand_Pinky_3;

    // Left hand
    private bool isGrabbingLeft = false;
    private bool holdingMenuGestureLeft = false;

    // Right hand
    private bool isGrabbingRight = false;
    private bool holdingMenuGestureRight = false;
    
    private float teleportCooldown = 0f;

    private NeuronVR_Hand.HandID handUsingMenuGesture = NeuronVR_Hand.HandID.None;
    private float currentMenuGestureTime = 0f;
    private float tolerance = 22.5f;

    private enum MenuState
    {
        Closed, Main, Inventory, Teleport, Vive
    }

    private MenuState currentState = MenuState.Closed;

    // Use this for initialization
    void Start () {
        // Just in case some of the objects are active in hierarchy
        Debug_DisableEverything();
    }

    // Update is called once per frame
    void Update () {
        isGrabbingLeft = CheckGrabLeft();
        isGrabbingRight = CheckGrabRight();

        leftHand.SetGrabbing(isGrabbingLeft);
        rightHand.SetGrabbing(isGrabbingRight);

        holdingMenuGestureLeft = CheckMenuGestureLeft();
        holdingMenuGestureRight = CheckMenuGestureRight();

        switch (currentState)
        {
            case MenuState.Closed:
                // Left hand - check gesture to open Main Menu
                if (handUsingMenuGesture != NeuronVR_Hand.HandID.Right)
                {
                    // Count gesture time if not grabbing with left hand and the menu gesture is shown
                    if (!leftHand.IsGrabbing() && holdingMenuGestureLeft)
                    {
                        handUsingMenuGesture = NeuronVR_Hand.HandID.Left;
                        currentMenuGestureTime += Time.deltaTime;

                        // Color hands to indicate that the MenuGesture is valid
                        foreach (var item in menuBones)
                        {
                            item.GetComponent<Renderer>().material.color = handInterfaceColor;
                        }

                        if (currentMenuGestureTime >= minMenuGestureTime)
                        {
                            // Open Main Menu
                            m_menuButtonVive.gameObject.SetActive(true);
                            m_menuButtonExit.gameObject.SetActive(true);
                            m_menuButtonTeleport.gameObject.SetActive(true);

                            m_menuButtonVive.ResetAction();
                            m_menuButtonExit.ResetAction();
                            m_menuButtonTeleport.ResetAction();

                            currentState = MenuState.Main;
                        }
                    }
                    else // No menu gesture is shown (any longer)
                    {
                        foreach (var bone in menuBones)
                        {
                            bone.GetComponent<Renderer>().material.color = handBasicColor;
                        }

                        currentMenuGestureTime = 0f;
                        handUsingMenuGesture = NeuronVR_Hand.HandID.None;
                    }
                }

                // Right hand - check gesture to open inventory
                if(handUsingMenuGesture != NeuronVR_Hand.HandID.Left)
                {
                    // Count gesture time if not grabbing with right hand and the menu gesture is shown
                    if (!rightHand.IsGrabbing() && holdingMenuGestureRight)
                    {
                        handUsingMenuGesture = NeuronVR_Hand.HandID.Right;
                        currentMenuGestureTime += Time.deltaTime;

                        // Color hands to indicate that the MenuGesture is valid
                        foreach (var item in menuBonesRight)
                        {
                            item.GetComponent<Renderer>().material.color = handInterfaceColor;
                        }

                        if (currentMenuGestureTime >= minMenuGestureTime)
                        {
                            // Open Inventory Menu
                            m_inventoryButtonCube.gameObject.SetActive(true);
                            m_inventoryButtonBall.gameObject.SetActive(true);
                            m_inventoryButtonStar.gameObject.SetActive(true);

                            currentState = MenuState.Inventory;
                        }
                    }
                    else // No menu gesture is shown (any longer)
                    {
                        foreach (var bone in menuBonesRight)
                        {
                            bone.GetComponent<Renderer>().material.color = handBasicColor;
                        }

                        currentMenuGestureTime = 0f;
                        handUsingMenuGesture = NeuronVR_Hand.HandID.None;
                    }
                }
                break;
            case MenuState.Main:
                // Check if not grabbing with left hand and the menu gesture is shown
                if (!leftHand.IsGrabbing() && holdingMenuGestureLeft)
                {
                    // Do nothing if still holding menu gesture
                }
                else // No menu gesture is shown (any longer)
                {
                    // Close Main Menu
                    m_menuButtonVive.gameObject.SetActive(false);
                    m_menuButtonExit.gameObject.SetActive(false);
                    m_menuButtonTeleport.gameObject.SetActive(false);

                    currentState = MenuState.Closed;

                    foreach (var bone in menuBones)
                    {
                        bone.GetComponent<Renderer>().material.color = handBasicColor;
                    }

                    currentMenuGestureTime = 0f;
                    handUsingMenuGesture = NeuronVR_Hand.HandID.None;
                }
                break;
            case MenuState.Inventory:
                // Check if not grabbing with right hand and the menu gesture is shown
                if (!rightHand.IsGrabbing() && holdingMenuGestureRight)
                {
                    // Do nothing if still holding the menu gesture
                }
                else // No menu gesture is shown (any longer)
                {
                    // Close Inventory Menu
                    m_inventoryButtonCube.ResetCollidingHands();
                    m_inventoryButtonBall.ResetCollidingHands();
                    m_inventoryButtonStar.ResetCollidingHands();

                    m_inventoryButtonCube.gameObject.SetActive(false);
                    m_inventoryButtonBall.gameObject.SetActive(false);
                    m_inventoryButtonStar.gameObject.SetActive(false);

                    currentState = MenuState.Closed;

                    foreach (var bone in menuBonesRight)
                    {
                        bone.GetComponent<Renderer>().material.color = handBasicColor;
                    }

                    currentMenuGestureTime = 0f;
                    handUsingMenuGesture = NeuronVR_Hand.HandID.None;
                }
                break;
            case MenuState.Teleport:
                // Check if not grabbing with left hand and the menu gesture is shown
                if (!leftHand.IsGrabbing() && holdingMenuGestureLeft)
                {
                    // Check teleport
                    teleportCooldown -= Time.deltaTime;
                    if (teleportCooldown < 0f)
                        teleportCooldown = 0f;
                    if(teleportCooldown == 0f && isGrabbingRight)
                    {
                        // Only set cooldown if teleport performed
                        if(laserPointer.Teleport())
                            teleportCooldown = 3.0f;
                    }
                }
                else // No menu gesture is shown (any longer)
                {
                    // Close Teleport Menu
                    laserPointer.DeactivateLaser();
                    laserPointer.gameObject.SetActive(false);
                    m_infoTeleport.gameObject.SetActive(false);

                    currentState = MenuState.Closed;

                    foreach (var bone in menuBones)
                    {
                        bone.GetComponent<Renderer>().material.color = handBasicColor;
                    }

                    currentMenuGestureTime = 0f;
                    handUsingMenuGesture = NeuronVR_Hand.HandID.None;

                    rightHand.AllowItemGrab();
                }
                break;
            case MenuState.Vive:
                // Check if not grabbing with left hand and the menu gesture is shown
                if (!leftHand.IsGrabbing() && holdingMenuGestureLeft)
                {
                    // Do nothing if still holding menu gesture
                }
                else // No menu gesture is shown (any longer)
                {
                    // Close Vive Menu
                    m_infoVive.gameObject.SetActive(false);

                    currentState = MenuState.Closed;

                    foreach (var bone in menuBones)
                    {
                        bone.GetComponent<Renderer>().material.color = handBasicColor;
                    }

                    currentMenuGestureTime = 0f;
                    handUsingMenuGesture = NeuronVR_Hand.HandID.None;
                }
                break;
        }
    }

    private float SignedAngle(Vector3 v1, Vector3 v2, Vector3 normal)
    {
        float angle = Vector3.Angle(v1, v2);
        float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(v1, v2)));
        return angle * sign;
    }

    public void FindControllers()
    {
        int count = 0;
        if (m_controller1.gameObject.activeInHierarchy)
            count++;
        if (m_controller2.gameObject.activeInHierarchy)
            count++;

        m_infoVive.ViveControllersFound(count);

        StartCoroutine("DelayedControllerVibration");
    }

    private IEnumerator DelayedControllerVibration()
    {
        
        if(m_controller1.gameObject.activeInHierarchy)
        {
            if(m_controller2.gameObject.activeInHierarchy)
            {
                m_controller1.FindController();                
                yield return new WaitForSeconds(2f); // Trigger second controller delayed so the user can distinguish the controller locations 
                m_controller2.FindController();
            }
            else
            {
                m_controller1.FindController();
            }
        }
        else
        {
            if(m_controller2.gameObject.activeInHierarchy)
            {
                m_controller2.FindController();
            }
            else
            {
                // No controller was found
            }
        }
    }


    public void TriggerTeleportButton()
    {
        // Close Main Menu
        m_menuButtonVive.gameObject.SetActive(false);
        m_menuButtonExit.gameObject.SetActive(false);
        m_menuButtonTeleport.gameObject.SetActive(false);

        // Open Teleport Menu
        m_infoTeleport.gameObject.SetActive(true);
        laserPointer.gameObject.SetActive(true);
        currentState = MenuState.Teleport;

        rightHand.ProhibitItemGrab();
        laserPointer.ActivateLaser();

        // To avoid too many teleports while holding the teleport gesture
        teleportCooldown = 1f;
    }

    public void TriggerViveButton()
    {
        // Close Main Menu
        m_menuButtonVive.gameObject.SetActive(false);
        m_menuButtonExit.gameObject.SetActive(false);
        m_menuButtonTeleport.gameObject.SetActive(false);

        // Open Vive Menu
        m_infoVive.gameObject.SetActive(true);
        currentState = MenuState.Vive;

        // Find controllers
        FindControllers();
    }

    public void TriggerExitButton()
    {
         if(Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }

    public void SpawnCube()
    {
        // Queue respawn until the vive sync has position the player
        itemCube.QueueRespawn();
    }

    public void SpawnStar()
    {
        // Queue respawn until the vive sync has position the player
        itemStar.QueueRespawn();
    }

    public void SpawnBall()
    {
        // Queue respawn until the vive sync has position the player
        itemBall.QueueRespawn();
    }

    private void Debug_DisableEverything()
    {
        m_menuButtonVive.gameObject.SetActive(false);
        m_menuButtonExit.gameObject.SetActive(false);
        m_menuButtonTeleport.gameObject.SetActive(false);
        m_inventoryButtonCube.gameObject.SetActive(false);
        m_inventoryButtonBall.gameObject.SetActive(false);
        m_inventoryButtonStar.gameObject.SetActive(false);
        laserPointer.gameObject.SetActive(false);
        m_infoTeleport.gameObject.SetActive(false);
        m_infoVive.gameObject.SetActive(false);
    }

    public void PickupItem(NeuronVR_Item.ItemID id)
    {
        switch (id)
        {
            case NeuronVR_Item.ItemID.Undefined:
                break;
            case NeuronVR_Item.ItemID.Ball:
                m_inventoryButtonBall.PutItem();
                break;
            case NeuronVR_Item.ItemID.Star:
                m_inventoryButtonStar.PutItem();
                break;
            case NeuronVR_Item.ItemID.Cube:
                m_inventoryButtonCube.PutItem();
                break;
        }
    }

    private bool CheckGrabLeft()
    {
        bool grabbing = true;

        // Thumb is not checked for grabbing since the sensor might not be accurate enough

        //if (leftHand_Index.localEulerAngles.z < 15f || leftHand_Index.localEulerAngles.z > 135f)
        //    grabbing = false;
        if (leftHand_Index_1.localEulerAngles.z < 15f || leftHand_Index_1.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Index_2.localEulerAngles.z < 15f || leftHand_Index_2.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Index_3.localEulerAngles.z < 15f || leftHand_Index_3.localEulerAngles.z > 135f)
            grabbing = false;

        //if (leftHand_Middle.localEulerAngles.z < 15f || leftHand_Middle.localEulerAngles.z > 135f)
        //    grabbing = false;
        if (leftHand_Middle_1.localEulerAngles.z < 15f || leftHand_Middle_1.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Middle_2.localEulerAngles.z < 15f || leftHand_Middle_2.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Middle_3.localEulerAngles.z < 15f || leftHand_Middle_3.localEulerAngles.z > 135f)
            grabbing = false;

        //if (leftHand_Ring.localEulerAngles.z < 15f || leftHand_Ring.localEulerAngles.z > 135f)
        //    grabbing = false;
        if (leftHand_Ring_1.localEulerAngles.z < 15f || leftHand_Ring_1.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Ring_2.localEulerAngles.z < 15f || leftHand_Ring_2.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Ring_3.localEulerAngles.z < 15f || leftHand_Ring_3.localEulerAngles.z > 135f)
            grabbing = false;

        //if (leftHand_Pinky.localEulerAngles.z < 15f || leftHand_Pinky.localEulerAngles.z > 135f)
        //    grabbing = false;
        if (leftHand_Pinky_1.localEulerAngles.z < 15f || leftHand_Pinky_1.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Pinky_2.localEulerAngles.z < 15f || leftHand_Pinky_2.localEulerAngles.z > 135f)
            grabbing = false;
        if (leftHand_Pinky_3.localEulerAngles.z < 15f || leftHand_Pinky_3.localEulerAngles.z > 135f)
            grabbing = false;

        Renderer rend = grabBoneLeft.GetComponent<Renderer>();
        if(rend != null)
        {
            // Color forearm  to indicate active grabbing
            if(grabbing)
                rend.material.color = Color.green;
            else
                rend.material.color = Color.red;
        }

        return grabbing;
    }

    private bool CheckGrabRight()
    {
        bool grabbing = true;

        // Thumb is not checked for grabbing since the sensor might not be accurate enough

        //if (rightHand_Index.localEulerAngles.z > 345f || rightHand_Index.localEulerAngles.z < 225f)
        //    grabbing = false;
        if (rightHand_Index_1.localEulerAngles.z > 345f || rightHand_Index_1.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Index_2.localEulerAngles.z > 345f || rightHand_Index_2.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Index_3.localEulerAngles.z > 345f || rightHand_Index_3.localEulerAngles.z < 235f)
            grabbing = false;

        //if (rightHand_Middle.localEulerAngles.z < 345f || rightHand_Middle.localEulerAngles.z > 225f)
        //    grabbing = false;
        if (rightHand_Middle_1.localEulerAngles.z > 345f || rightHand_Middle_1.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Middle_2.localEulerAngles.z > 345f || rightHand_Middle_2.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Middle_3.localEulerAngles.z > 345f || rightHand_Middle_3.localEulerAngles.z < 235f)
            grabbing = false;

        //if (rightHand_Ring.localEulerAngles.z < 345f || rightHand_Ring.localEulerAngles.z > 225f)
        //    grabbing = false;
        if (rightHand_Ring_1.localEulerAngles.z > 345f || rightHand_Ring_1.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Ring_2.localEulerAngles.z > 345f || rightHand_Ring_2.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Ring_3.localEulerAngles.z > 345f || rightHand_Ring_3.localEulerAngles.z < 235f)
            grabbing = false;

        //if (rightHand_Pinky.localEulerAngles.z < 345f || rightHand_Pinky.localEulerAngles.z > 225f)
        //    grabbing = false;
        if (rightHand_Pinky_1.localEulerAngles.z > 345f || rightHand_Pinky_1.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Pinky_2.localEulerAngles.z > 345f || rightHand_Pinky_2.localEulerAngles.z < 235f)
            grabbing = false;
        if (rightHand_Pinky_3.localEulerAngles.z > 345f || rightHand_Pinky_3.localEulerAngles.z < 235f)
            grabbing = false;

        Renderer rend = grabBoneRight.GetComponent<Renderer>();
        if (rend != null)
        {
            // Color forearm  to indicate active grabbing
            if (grabbing)
                rend.material.color = Color.green;
            else
                rend.material.color = Color.red;
        }

        return grabbing;
    }

    private bool CheckMenuGestureLeft()
    {
        float angleZLeft = SignedAngle(Vector3.right, -leftHandTransform.up, Vector3.forward);
        float angleXLeft = SignedAngle(Vector3.forward, -leftHandTransform.up, Vector3.right);

        return (90 - tolerance < angleZLeft && angleZLeft < 90 + tolerance && -90 - tolerance < angleXLeft && angleXLeft < -90 + tolerance);
    }

    private bool CheckMenuGestureRight()
    {
        float angleZRight = SignedAngle(Vector3.right, -rightHandTransform.up, Vector3.forward);
        float angleXRight = SignedAngle(Vector3.forward, -rightHandTransform.up, Vector3.right);

        return (90 - tolerance < angleZRight && angleZRight < 90 + tolerance && -90 - tolerance < angleXRight && angleXRight < -90 + tolerance);
    }

    public void GivePlayerWeapons()
    {
        sword.gameObject.SetActive(true);
        shield.gameObject.SetActive(true);
    }
}
