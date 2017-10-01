using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Hand : MonoBehaviour
{
    public enum HandID
    {
        None, Left, Right
    }

    public HandID id = HandID.None;
    public Transform weaponHoldingTransform;

    // Objects the hand is colliding with - those are not necessarily grabbed
    private NeuronVR_Item collidingItem = null;
    private NeuronVR_Equipment collidingEquipment = null;

    private bool holdingItem = false;
    private bool holdingEquipment = false;

    private bool isGrabbing = false;
    private bool allowItemGrab = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool HoldingItem()
    {
        return holdingItem;
    }

    public void SetGrabbing(bool grabbing)
    {
        // If the state of grabbing didn't change, do nothing
        if (grabbing == isGrabbing)
            return;

        // Performed grab gesture
        if (grabbing)
        {
            // If item grabbing is allowed (e.g. it's forbidden for the right hand in teleport mode since 
            // the grab gesture is used then to activate the teleport)
            if (allowItemGrab)
            {
                // If colliding with an item & that item is not already grabbed by the other hand
                if (collidingItem != null && !collidingItem.IsGrabbed())
                {
                    holdingItem = true;
                    collidingItem.Grab(transform);
                }

                // Equipment
                if(collidingEquipment != null)
                {
                    holdingEquipment = true;
                    collidingEquipment.Grab(weaponHoldingTransform);
                }
            }
        }
        // Not grabbing
        else // Released grab
        {
            // Release item if holding one
            if (holdingItem && collidingItem != null)
            {
                bool itemDroppedInInventoryOrDoorZone = collidingItem.Release();
                if (itemDroppedInInventoryOrDoorZone)
                    collidingItem = null;
                holdingItem = false;
            }

            // Equipment
            if(holdingEquipment && collidingEquipment != null)
            {
                collidingEquipment.Release();
                collidingEquipment = null;
            }
        }

        isGrabbing = grabbing;
    }

    public bool IsGrabbing()
    {
        return isGrabbing;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Item
        NeuronVR_Item item = other.gameObject.GetComponent<NeuronVR_Item>();
        if (item != null && this.collidingItem == null)
        {
            collidingItem = item;
        }

        // Equipment
        NeuronVR_Equipment equip = other.gameObject.GetComponentInParent<NeuronVR_Equipment>();
        if (equip != null && collidingEquipment == null)
        {
            if (equip.GetEquipmentType() == NeuronVR_Equipment.Type.Shield && id == HandID.Left)
            {
                collidingEquipment = equip;
            }
            else if (equip.GetEquipmentType() == NeuronVR_Equipment.Type.Sword && id == HandID.Right)
            {
                collidingEquipment = equip;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Item
        NeuronVR_Item item = other.gameObject.GetComponent<NeuronVR_Item>();
        if (item != null && this.collidingItem == item)
        {
            collidingItem = null;
        }

        // Equipment
        NeuronVR_Equipment equip = other.gameObject.GetComponent<NeuronVR_Equipment>();
        if (equip != null && equip == collidingEquipment)
        {
            collidingEquipment = null;
        }
    }

    public void AllowItemGrab()
    {
        allowItemGrab = true;
    }

    public void ProhibitItemGrab()
    {
        allowItemGrab = false;
    }

    public bool IsEmpty()
    {
        return collidingItem == null;
    }

    public void DropItem()
    {
        collidingItem = null;
        allowItemGrab = true;
    }
}
