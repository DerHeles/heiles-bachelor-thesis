using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronVR_Door : MonoBehaviour
{
    public float moveTime = 2.0f;           //Time it will take object to move, in seconds.
    private Rigidbody rb;               //The Rigidbody2D component attached to this object.
    private float inverseMoveTime;          //Used to make movement more efficient.

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothRotation(float rotation)
    {
        float startRotation = transform.eulerAngles.y;
        float finalRotation = startRotation + rotation;

        float remainingRotation = finalRotation - startRotation;
        float currentRotation = startRotation;
        float rotationPerSecond = inverseMoveTime * remainingRotation;

        if (remainingRotation > 0f)
        {
            while (remainingRotation > float.Epsilon)
            {
                if ((remainingRotation - rotationPerSecond * Time.deltaTime) < 0)
                {
                    currentRotation = finalRotation;
                    remainingRotation = 0f;
                }
                else
                {
                    currentRotation += rotationPerSecond * Time.deltaTime;
                    remainingRotation -= rotationPerSecond * Time.deltaTime;
                }
                rb.MoveRotation(Quaternion.AngleAxis(currentRotation, transform.up));

                yield return null;
            }
        }
        else
        {
            while (remainingRotation < -float.Epsilon)
            {
                if ((remainingRotation - rotationPerSecond * Time.deltaTime) > 0)
                {
                    currentRotation = finalRotation;
                    remainingRotation = 0f;
                }
                else
                {
                    currentRotation += rotationPerSecond * Time.deltaTime;
                    remainingRotation -= rotationPerSecond * Time.deltaTime;
                }
                rb.MoveRotation(Quaternion.AngleAxis(currentRotation, transform.up));

                yield return null;
            }
        }

    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter.
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while (sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb.MovePosition(newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(SmoothMovement(rb.position + 3.5f * transform.right));
    }

    public void CloseDoor()
    {
        StartCoroutine(SmoothMovement(rb.position + -3.5f * transform.right));
    }

    public void RotateDoor()
    {
        StartCoroutine(SmoothRotation(90.0f));
    }

    public void SlideDoor()
    {
        StartCoroutine(SmoothMovement(rb.position + 3.5f * transform.right * 1.0f));
    }
}