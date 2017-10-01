using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Projectile : MonoBehaviour {
    
    public float veloctiy = 0.5f;
    public Vector3 shootDirection;

    private bool repelled = false;

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 10.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(shootDirection * veloctiy * Time.deltaTime, Space.World);
    }

    public void Repel()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = Color.gray;
        }
        veloctiy *= -1.0f;
        repelled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var cannon = other.gameObject.GetComponentInParent<NeuronVR_Cannon>();
        if (cannon != null && repelled)
        {
            cannon.Destroy();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.5f);
        }
    }
}
