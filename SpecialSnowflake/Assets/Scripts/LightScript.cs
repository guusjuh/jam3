using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour {

    public float wavy;
    private Color onLightColor;
    public Color offLightColor;

    public bool isStar = false;

    float timeFactor;

	// Use this for initialization
	public void Initialize () {
        timeFactor = Random.Range(1.0f, 5.0f);

        if (!isStar)
        {
            onLightColor.r = Random.Range(0.2f, 1.0f);
            onLightColor.g = Random.Range(0.2f, 1.0f);
            onLightColor.b = Random.Range(0.2f, 1.0f);
            onLightColor.a = 1.0f;
        }
        else
        {
            onLightColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            transform.localScale /= 2.0f;
        }
    }
	
	// Update is called once per frame
	void Update () {

        wavy = (Mathf.Sin(Time.time * timeFactor) / 2.0f + 0.5f);

        GetComponent<SpriteRenderer>().color = Color.Lerp(offLightColor, onLightColor, wavy);
	}
}
