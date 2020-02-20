using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobileHorizMovement
{
    Accelerometer,
    ScreenTouch
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehavior : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private Rigidbody rb;

    [Tooltip("How fast the ball moves left/right")]
    public float dodgeSpeed = 5.0f;

    [Tooltip("How fast the ball advances")]
    [Range(0,10)]
    public float rollSpeed = 5.0f;

    //New input controls for mobile
    public MobileHorizMovement horizonMovement = MobileHorizMovement.Accelerometer;
    [Header("Swipe Properties")]
    [Tooltip("How far must a player swipe before triggering the action")]
    public float minSwipeDistance = 2.0f;
    [Tooltip("How far will the player move")]
    public float swipeMove = 3.0f;

    [HideInInspector]
    public Vector2 touchStart;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    float CalculateMovement(Vector3 pixelPos)
    {
        var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);

        float xMove = 0;

        if (worldPos.x < 0.5f)
        {
            xMove = -1;
        }
        else
        {
            xMove = 1;
        }
        return xMove * dodgeSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Deprecated move code
        //var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        //rb.AddForce(horizontalSpeed, 0, rollSpeed);

        //Adding in movement code

        float horizontalSpeed = 0.0f;

#if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
        horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        
       /* if (Input.GetMouseButton(0))
        {
            horizontalSpeed = CalculateMovement(Input.mousePosition);
        }*/

#endif
#if UNITY_IOS || UNITY_ANDROID
        if (horizonMovement == MobileHorizMovement.Accelerometer)
        {
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            if (horizonMovement == MobileHorizMovement.ScreenTouch)
            {
                horizontalSpeed = CalculateMovement(myTouch.position);
            }
            SwipeTeleport(myTouch);
            TouchObjects(myTouch);
        }
#endif
        //Keyboard
        //var horizontalSpeed = Input.GetAxis("Horizontal")*dodgeSpeed;

        ////Mouse Support
        //if (Input.GetMouseButton(0))
        //{
        //    var worldPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //    float xMove = 0;

        //    if (worldPos.x<0.5f)
        //    {
        //        xMove = -1;
        //    }
        //    else
        //    {
        //        xMove = 1;
        //    }
        //    horizontalSpeed = xMove * dodgeSpeed;
        //}

        //TouchSupport
        //if (Input.touchCount>0)
        //{
        //    Touch myTouch = Input.touches[0];
        //    var worldPos = Camera.main.ScreenToViewportPoint(myTouch.position);
        //    float xMove = 0;

        //    if (worldPos.x < 0.5f)
        //    {
        //        xMove = -1;
        //    }
        //    else
        //    {
        //        xMove = 1;
        //    }
        //    horizontalSpeed = xMove * dodgeSpeed;
        //}

        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }


    //Added in movement functions
    private void SwipeTeleport(Touch myTouch)
    {
        if (myTouch.phase == TouchPhase.Began)
        {
            touchStart = myTouch.position;
        }
        if (myTouch.phase == TouchPhase.Ended)
        {
            Vector2 touchEnded = myTouch.position;
            float x = touchEnded.x - touchStart.x;

            if (Mathf.Abs(x) < minSwipeDistance)
            {
                return;
            }
            Vector3 moveDirection;
            if (x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                moveDirection = Vector3.right;
            }

            RaycastHit hit;
            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (moveDirection * swipeMove));
            }
        }
    }

    private static void TouchObjects(Touch touch)
    {

        Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }

    }



}
