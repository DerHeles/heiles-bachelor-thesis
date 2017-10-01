using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Enemy : MonoBehaviour {

    public bool agressive = false;
    public bool jumping = false;

    private Transform camTransform;
    private Rigidbody rb;
    private float jumpingCooldown = 0.0f;

    // Use this for initialization
    void Start ()    {
        camTransform = Camera.main.transform;
        rb = GetComponentInChildren<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAgressive()
    {
        agressive = true;
    }

    private void FixedUpdate()
    {
        if(jumping)
        {
            jumpingCooldown -= Time.deltaTime;
            if(jumpingCooldown <= 0f)
            {
                jumpingCooldown = 2.0f;
                rb.AddForce(new Vector3(0, 4.0f), ForceMode.Impulse);
            }
        }
    }

    public void Kill()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.red;
        }
        Destroy(transform.parent.gameObject, 0.5f);
    }

    private void LateUpdate()
    {
        if(agressive)
        {
            Vector3 myPos = transform.position;
            Vector3 camPos = camTransform.position;

            // Only rotate around y axis
            camPos.y = myPos.y;

            Vector3 camToMyPos = myPos - camPos;
            transform.rotation = Quaternion.LookRotation(camToMyPos);
        }
    }
}
