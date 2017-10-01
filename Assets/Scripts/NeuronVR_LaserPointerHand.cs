using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_LaserPointerHand : MonoBehaviour
{
    public GameObject laser;
    public GameObject reticle;
    public Transform headTransform;
    public Transform cameraRigTransform;
    public Vector3 teleportReticleOffset;  // To avoid z-fighting
    public LayerMask teleportMask;

    public Transform viveCameraEye;
    public Transform neuronHead;
    public Transform neuronHips;
    
    public GameObject laserStart;

    private Transform laserTransform;
    private Vector3 hitPoint;
    private Transform teleportReticleTransform;
    private Vector3 teleportPosition;

    private bool canTeleport;
    private bool isInTeleportMode;

    private void Awake()
    {
        laserTransform = laser.transform;
        teleportReticleTransform = reticle.transform;
    }

    private void Update()
    {
       // Do nothing (check actual teleport via vivesync & menu manager)
    }

    public bool Teleport()
    {
        if(canTeleport)
        {
            canTeleport = false;
            // Difference is required to preserve the player's position relative to the cameraRig
            Vector3 difference = cameraRigTransform.position - headTransform.position;
            difference.y = 0;
            teleportPosition = hitPoint + difference;
            
            cameraRigTransform.position = teleportPosition;
            return true;
        }
        return false;
    }

    public void CheckLaserWithSyncOffset()
    {
        if(isInTeleportMode)
        {
            RaycastHit hit;

            if (Physics.Raycast(laserStart.transform.position + transform.right * 0.1f, transform.right, out hit, 100, teleportMask))
            {
                if (hit.collider.gameObject.CompareTag("CanTeleport"))
                {
                    reticle.SetActive(true);
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                    canTeleport = true;
                }
                else
                {
                    canTeleport = false;
                    reticle.SetActive(false);
                }

                hitPoint = hit.point;
                laser.SetActive(true);
                laserTransform.position = Vector3.Lerp(laserStart.transform.position + transform.right * 0.1f, hitPoint, .5f);
                laserTransform.LookAt(hitPoint);
                laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
                    hit.distance);
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }
    }

    public void ActivateLaser()
    {
        isInTeleportMode = true;
        laser.SetActive(true);
        reticle.SetActive(true);
    }

    public void DeactivateLaser()
    {
        isInTeleportMode = false;
        laser.SetActive(false);
        reticle.SetActive(false);
    }
}
