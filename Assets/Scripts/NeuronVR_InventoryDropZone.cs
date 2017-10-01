using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_InventoryDropZone : MonoBehaviour {

    private int howManyItemsGrabbed = 0;

    // Use this for initialization
    void Start () {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
            renderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        // Do nothing
	}

    public void ItemGrabbed()
    {
        if(howManyItemsGrabbed == 0)
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = true;
        }
        howManyItemsGrabbed++;
    }

    public void ItemReleased()
    {
        if (howManyItemsGrabbed == 1)
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
                renderer.enabled = false;
        }
        howManyItemsGrabbed--;
    }
}
