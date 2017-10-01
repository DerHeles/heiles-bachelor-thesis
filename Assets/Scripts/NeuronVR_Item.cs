using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Item : MonoBehaviour
{
    public enum ItemID
    {
        Undefined, Ball, Star, Cube
    }

    public ItemID id = ItemID.Undefined;
    public NeuronVR_MenuManager menuManager;
    public NeuronVR_InventoryDropZone inventoryDropZone;

    // Used to avoid destroying the items
    public Transform despawnTransform;
    private Vector3 despawnPosition;

    private bool respawnQueued = false;
    private Vector3 respawnPosition;

    private Transform initialParent;
    private bool isInInventoryZone = false;

    private NeuronVR_DoorDropZone doorDropZone;
    private bool isInDoorZone = false;


    // Use this for initialization
    void Start()
    {
        initialParent = transform.parent;
        despawnPosition = despawnTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        NeuronVR_InventoryDropZone dropZone = other.gameObject.GetComponent<NeuronVR_InventoryDropZone>();
        if (dropZone != null && IsGrabbed())
        {
            isInInventoryZone = true;
        }
        NeuronVR_DoorDropZone doorDropZone = other.gameObject.GetComponent<NeuronVR_DoorDropZone>();
        if (doorDropZone != null && this.doorDropZone == null && doorDropZone.GetID() == id)
        {
            isInDoorZone = true;
            this.doorDropZone = doorDropZone;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NeuronVR_InventoryDropZone dropZone = other.gameObject.GetComponent<NeuronVR_InventoryDropZone>();
        if (dropZone != null && IsGrabbed())
        {
            isInInventoryZone = false;
        }
        NeuronVR_DoorDropZone doorDropZone = other.gameObject.GetComponent<NeuronVR_DoorDropZone>();
        if (doorDropZone != null && this.doorDropZone == doorDropZone)
        {
            isInDoorZone = false;
            this.doorDropZone = null;
        }
    }

    public void Grab(Transform parent)
    {
        transform.parent = parent;
        inventoryDropZone.ItemGrabbed();
    }

    public bool Release()
    {
        inventoryDropZone.ItemReleased();
        if(isInDoorZone)
        {
            doorDropZone.ItemDropped();
            transform.parent = initialParent;
            isInDoorZone = false;
            transform.position = despawnPosition;
            return true;
        }
        if (isInInventoryZone)
        {
            menuManager.PickupItem(id);
            transform.parent = initialParent;
            transform.position = despawnPosition;
            isInInventoryZone = false;

            return true;
        }

        transform.parent = initialParent;
        return false;
    }

    public bool IsGrabbed()
    {
        return transform.parent != initialParent;
    }
    public void Spawn(Vector3 spawnLocation)
    {
        transform.position = spawnLocation;
    }

    public void QueueRespawn()
    {
        respawnQueued = true;
    }

    public void Respawn(Vector3 respawnLocation)
    {
        transform.position = respawnLocation;
        respawnQueued = false;
    }
    public bool HasRespawned()
    {
        return respawnQueued;
    }
}
