using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NeuronVR_HoverInfo : MonoBehaviour
{    
    public Text infoText;

    private Transform camTransform;

    // Use this for initialization
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing
    }

    private void LateUpdate()
    {
        Vector3 myPos = transform.position;
        Vector3 camPos = camTransform.position;

        // Only rotate around y axis
        camPos.y = myPos.y;

        Vector3 camToMyPos = myPos - camPos;
        transform.rotation = Quaternion.LookRotation(camToMyPos);
    }

    public void ViveControllersFound(int count)
    {
        if(count == 1)
        {
            infoText.text = "1 Vive Controller found";
        }
        else if(count == 2)
        {
            infoText.text = "2 Vive Controllers found";
        }
        else
        {
            infoText.text = "No Vive Controllers found";
        }
    }
}
