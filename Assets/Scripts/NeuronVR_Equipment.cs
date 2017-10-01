using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Equipment : MonoBehaviour {

    public enum Type
    {
        None, Sword, Shield
    }

    public Type type = Type.None;
    public NeuronVR_RoomManager roomManager;
    public Transform lootLocationTransform;

    private Transform initialParent;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private bool lootedByPlayer = false;
    private bool inHand = false;

	// Use this for initialization
	void Start () {
        initialParent = transform.parent;
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;

        transform.parent = lootLocationTransform;
        transform.rotation = lootLocationTransform.rotation;
        transform.position = lootLocationTransform.position;
        transform.localScale = lootLocationTransform.localScale;
    }

    // Update is called once per frame
    void Update () {
        if (lootedByPlayer)
            return;
        transform.Rotate(Vector3.up, 30f * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(type == Type.Sword && inHand)
        {
            NeuronVR_Enemy enemy = other.gameObject.GetComponent<NeuronVR_Enemy>();
            if(enemy != null)
            {
                enemy.Kill();
                roomManager.EnemyKilled();
            }
        }
        else if(type == Type.Shield && inHand)
        {
            NeuronVR_Projectile proj = other.gameObject.GetComponent<NeuronVR_Projectile>();
            if(proj != null)
            {
                proj.Repel();
                roomManager.CannonDestroid();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {

    }

    public Type GetEquipmentType()
    {
        return type;
    }

    public void Release()
    {
        inHand = false;

        // Snap back to body
        transform.parent = initialParent;
        transform.localRotation = initialRotation;
        transform.localPosition = initialPosition;
        transform.localScale = initialScale;
    }

    public void Grab(Transform parent)
    {
        inHand = true;

        // Snap to new parent (player's hand)
        transform.parent = parent;
        transform.localScale = new Vector3(1, 1, 1);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;

        if(!lootedByPlayer)
        {
            if(type == Type.Shield)
            {
                roomManager.StartCannonEvent();
            }
            else if(type == Type.Sword)
            {
                roomManager.StartEnemyEvent();
            }
            lootedByPlayer = true;
        }
    }
}
