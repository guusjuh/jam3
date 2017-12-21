using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public float wavy;
    public Color onLightColor;
    public Color offLightColor;

    bool goToOn;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        wavy = (Mathf.Sin(Time.time) / 2.0f + 0.5f);

        if (wavy >= 0.99f)
        {
            goToOn = false;
        }
        if (wavy <= 0.01f)
        {
            goToOn = true;
        }

        GetComponent<SpriteRenderer>().color = Color.Lerp(offLightColor, onLightColor, wavy);
	}
}
