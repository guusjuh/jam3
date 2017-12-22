using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Vector2 touchOrigin;
    float time;
    float totalTime = 4;
    bool direction;
    bool dwarrel = true;

    float dwarrelForce = 0.4f;
    float flakeGravity = -0.6f;

	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, flakeGravity);

        time = totalTime / 2;
        direction = true;
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        reset();
    }

    void reset()
    {
        this.transform.position = new Vector3(0, 21, 0);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, flakeGravity);
        time = totalTime / 2;
        dwarrel = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(dwarrel){
            time += Time.deltaTime;
            //schommel
            if(time < totalTime){
                if (direction)
                {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(dwarrelForce * Time.deltaTime * 100, 0));
                }
                else
                {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-dwarrelForce * Time.deltaTime * 100, 0));
                }
            }
            else
            {
                time = 0;
                direction = !direction;
            }
        }
        else if(Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.x) < .2f){
            dwarrel = true;
            direction = !direction;
        }

        //reached the end
        if (this.transform.position.y < 0)
        {
            reset();
        }

        float horizontal = 0;     //Used to store the horizontal move direction.
            
        //Check if we are running either in the Unity editor or in a standalone build.
        #if UNITY_STANDALONE || UNITY_WEBPLAYER
            
        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
            
        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int) (Input.GetAxisRaw ("Vertical"));
            
        //Check if moving horizontally, if so set vertical to zero.
        if(horizontal != 0)
        {
            vertical = 0;
        }
        //Check if we are running on iOS, Android, Windows Phone 8 or Unity iPhone
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            
        //Check if Input has registered more than zero touches
        if (Input.touchCount > 0)
        {
            //Store the first touch detected.
            Touch myTouch = Input.touches[0];

            //Check if the phase of that touch equals Began
            if (myTouch.phase == TouchPhase.Began)
            {
                //If so, set touchOrigin to the position of that touch
                touchOrigin = myTouch.position;
            }
                
            //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                //Set touchEnd to equal the position of this touch
                Vector2 touchEnd = myTouch.position;
                    
                //Calculate the difference between the beginning and end of the touch on the x axis.
                float x = touchEnd.x - touchOrigin.x;

                dwarrel = false;

                horizontal = (x / Screen.width * 80);
            }
        }
            
        #endif //End of mobile platform dependendent compilation section started above with #elif
        //Check if we have a non-zero value for horizontal or vertical
        if(horizontal != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            this.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontal, 0));

        }
        
	}
}
