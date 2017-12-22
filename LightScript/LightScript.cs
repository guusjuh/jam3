using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public float wavy;
    public Color onLightColor;
    public Color offLightColor;

    float timeFactor;

	// Use this for initialization
	void Start () {
        timeFactor = Random.Range(1.0f, 5.0f);

        onLightColor.r = Random.Range(0.2f, 1.0f);
        onLightColor.g = Random.Range(0.2f, 1.0f);
        onLightColor.b = Random.Range(0.2f, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {

        wavy = (Mathf.Sin(Time.time * timeFactor) / 2.0f + 0.5f);

        GetComponent<SpriteRenderer>().color = Color.Lerp(offLightColor, onLightColor, wavy);
	}
}
