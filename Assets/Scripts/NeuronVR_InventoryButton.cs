using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NeuronVR_InventoryButton : MonoBehaviour
{
    public Image centerImage;
    public Image image;

    public Sprite collectedSprite;
    public Sprite emptySprite;

    [SerializeField]
    public UnityEvent actionEvent;

    public float fillPerSecond = 0.5f;

    private float currentFill = 0f;
    private int collidingHands = 0;
    private Transform camTransform;
    private bool empty = true;

    // Use this for initialization
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty)
            return;

        if (collidingHands > 0)
        {
            currentFill += fillPerSecond * Time.deltaTime;
            if (currentFill >= 1.0f)
            {
                // Trigger action
                currentFill = 1.0f;
                TakeItem();
            }
        }
        else
        {
            currentFill -= fillPerSecond * Time.deltaTime;
            if (currentFill < 0.0f)
                currentFill = 0.0f;
        }
        image.fillAmount = currentFill;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hand = other.gameObject.GetComponent<NeuronVR_Hand>();
        if (hand != null)
        {
            if (hand.id == NeuronVR_Hand.HandID.Left)
                collidingHands++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hand = other.gameObject.GetComponent<NeuronVR_Hand>();
        if (hand != null)
        {
            if (hand.id == NeuronVR_Hand.HandID.Left)
                collidingHands--;
        }
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

    public void PutItem()
    {
        centerImage.sprite = collectedSprite;
        empty = false;
        currentFill = 0f;
    }

    private void TakeItem()
    {
        centerImage.sprite = emptySprite;
        empty = true;
        actionEvent.Invoke();
        currentFill = 0f;
    }

    public void ResetCollidingHands()
    {
        collidingHands = 0;
    }
}
