using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_ViveController : MonoBehaviour
{
    public NeuronVR_ViveSync viveSync;

    private SteamVR_TrackedObject trackedObject;

    // Use this for initialization
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug code for NeuronParent mode
        //SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    viveSync.togglePositionTracking();
        //}
    }

    private IEnumerator Vibrate()
    {
        float time = 0.6f;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObject.index);
            device.TriggerHapticPulse(800);
            yield return null;
        }
    }

    public void FindController()
    {
        StartCoroutine("Vibrate");
    }
}
