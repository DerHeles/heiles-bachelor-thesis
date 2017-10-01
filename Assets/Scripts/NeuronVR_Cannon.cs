using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Cannon : MonoBehaviour {

    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    public float startCooldown = 4.0f;
    public float cooldown = 5.0f;

    private float reloadTime;
    private bool destroid = false;
    private bool shooting = false;

    // Use this for initialization
    void Start () {
        reloadTime = startCooldown;
	}
	
	// Update is called once per frame
	void Update () {
        if (destroid || !shooting)
            return;

        reloadTime -= Time.deltaTime;

		if(reloadTime <= 0.0f)
        {
            // Shoot
            Quaternion rot = Quaternion.Euler(0, transform.localEulerAngles.y + 90, 90);
            GameObject proj = Instantiate(projectilePrefab, projectileSpawn.position, rot);
            proj.GetComponent<NeuronVR_Projectile>().shootDirection = transform.forward;
            reloadTime = cooldown;
        }
	}

    public void Destroy()
    {
        destroid = true;
    }

    public void StartShooting()
    {
        shooting = true;
    }
}
