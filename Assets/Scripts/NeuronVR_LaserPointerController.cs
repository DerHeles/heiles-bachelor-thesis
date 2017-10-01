using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_LaserPointerController : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform headTransform;
    public Transform cameraRigTransform;
    public Vector3 teleportReticleOffset; // To avoid z-fighting
    public LayerMask teleportMask;
    public GameObject teleportReticlePrefab;

    private GameObject laser;
    private GameObject reticle;
    private Transform laserTransform;
    private Vector3 hitPoint;
    private Transform teleportReticleTransform;
    private bool canTeleport;
    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    private void Update()
    {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;

            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                if (hit.collider.gameObject.CompareTag("CanTeleport"))
                {
                    reticle.SetActive(true);
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                    canTeleport = true;
                }
                else
                    reticle.SetActive(false);
                hitPoint = hit.point;
                ShowLaser(hit);
            }
        }
        else
        {
            laser.SetActive(false);
            reticle.SetActive(false);
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && canTeleport)
        {
            Teleport();
        }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }

    private void Teleport()
    {
        canTeleport = false;
        reticle.SetActive(false);
        // Difference is required to preserve the player's position relative to the cameraRig
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        difference.y = 0;
        cameraRigTransform.position = hitPoint + difference;
    }
}
