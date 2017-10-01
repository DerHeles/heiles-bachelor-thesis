using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NeuronVR_HoverButton : MonoBehaviour
{
    public Image image;
    public Image centerImage;
    public Color finishedColor = Color.yellow;
    private Color normalColor;

    public bool isInterfaceButton = false;
    public float fillPerSecond = 0.5f;

    [SerializeField]
    public UnityEvent actionEvent;

    private float currentFill = 0;
    private int collidingHands = 0;
    private bool actionFired = false;
    private bool locked = false;

    private AudioSource actionSound;
    private Transform camTransform;
    private List<NeuronVR_Hand> handsHoldingItems;
    

    public void TriggerAction()
    {

    }

    private void Awake()
    {
        normalColor = image.color;
    }

    // Use this for initialization
    void Start()
    {
        if(!isInterfaceButton) // Since interface buttons are disabled after invoking the action, they don't play a sound
            actionSound = GetComponent<AudioSource>();
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (actionFired || locked)
            return;

        if (collidingHands > 0)
        {
            currentFill += fillPerSecond * Time.deltaTime;
            if (currentFill >= 1.0f)
            {
                // Trigger action
                currentFill = 1.0f;
                actionEvent.Invoke();
                actionFired = true;

                image.color = finishedColor;
                centerImage.color = new Color(0.75f, 0.75f, 0.75f);

                if(!isInterfaceButton)
                    actionSound.Play();
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
            if (isInterfaceButton)
            {
                // Interface buttons only get triggered by right hand
                if (hand.id == NeuronVR_Hand.HandID.Right)
                {
                    // Hands holding an item do not trigger the button
                    if (hand.HoldingItem() && !handsHoldingItems.Contains(hand))
                    {
                        handsHoldingItems.Add(hand);
                    }
                    else
                        collidingHands++;
                }
            }
            else
            {
                // Hands holding an item do not trigger the button
                if (hand.HoldingItem() && !handsHoldingItems.Contains(hand))
                {
                    handsHoldingItems.Add(hand);
                }
                else
                    collidingHands++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var hand = other.gameObject.GetComponent<NeuronVR_Hand>();
        if (hand != null)
        {
            if (isInterfaceButton)
            {
                // Interface buttons only get triggered by right hand
                if (hand.id == NeuronVR_Hand.HandID.Right)
                {
                    // Hands holding an item do not trigger the button
                    if (hand.HoldingItem() && handsHoldingItems.Contains(hand))
                    {
                        handsHoldingItems.Remove(hand);
                    }
                    else
                        collidingHands--;
                }
            }
            else
            {
               // Hands holding an item do not trigger the button
                if (hand.HoldingItem() && handsHoldingItems.Contains(hand))
                {
                    handsHoldingItems.Remove(hand);
                }
                else
                    collidingHands--;
            }
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

    public void Unlock()
    {
        locked = false;
        centerImage.color = Color.white;
    }

    public void Lock()
    {
        locked = true;
        centerImage.color = Color.red;
    }

    public void ResetAction()
    {
        actionFired = false;
        currentFill = 0f;
        image.fillAmount = currentFill;
        image.color = normalColor;
        centerImage.color = Color.white;
        collidingHands = 0;
    }
}
