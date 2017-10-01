using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;

public class NeuronVR_ViveSync : MonoBehaviour {

    public enum SyncMode
    {
        ViveParent,
        NeuronParent
    }

    public SyncMode mode = SyncMode.ViveParent;

    // Vive hmd position
    public Transform viveCameraEye;
    // Head/hmd holder position on the player's robot
    public Transform neuronHead;
    // Hip position of the player's robot
    public Transform neuronHips;

    // Laser pointer for teleport mode (teleporting with controllers is handled through other script)
    public NeuronVR_LaserPointerHand laserPointer;

    // Position to respawn items
    public Transform respawnPosition;

    // Items to respawn in front of the player
    public NeuronVR_Item[] items;

    private void LateUpdate()
    {
        // Position and rotation tracking is done by the vive
        if (mode == SyncMode.ViveParent)
        {
            // Since the hips are the root of the player's robot you have to position the hips
            Vector3 headHipOffset = neuronHead.position - neuronHips.position;
            neuronHips.position = viveCameraEye.position - headHipOffset;

            // Using the laserpointer needs to be checked after position syncing 
            // since the laser's position is relative to the player's position
            if (laserPointer.gameObject.activeInHierarchy)
                laserPointer.CheckLaserWithSyncOffset();

            // Items need to be respawn after the position syncing
            // since the respawn position is relative to the player's position
            foreach (var item in items)
            {
                if(item.HasRespawned())
                {
                    item.Respawn(respawnPosition.position);
                }
            }
        }
        else
        {
            // Not implemented due to higher accuracy of the vive position tracking
        }
    }

    // Debug code for NeuronParent mode
    //public void togglePositionTracking()
    //{
    //    InputTracking.disablePositionalTracking = !InputTracking.disablePositionalTracking;
    //}
}
