using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeuronVR_DoorDropZone : MonoBehaviour {

    public NeuronVR_Item.ItemID id;
    public UnityEvent actionEvent;
    public Material collectedMaterial;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, 20f * Time.deltaTime, Space.World);
    }

    public void ItemDropped()
    {
        actionEvent.Invoke();

        if(id == NeuronVR_Item.ItemID.Star)
        {
            Renderer[] childrenRends = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in childrenRends)
            {
                rend.material = collectedMaterial;
            }
        }
        else
        {
            var rend = GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = collectedMaterial;
            }
        }
    }

    public NeuronVR_Item.ItemID GetID()
    {
        return id;
    }
}
