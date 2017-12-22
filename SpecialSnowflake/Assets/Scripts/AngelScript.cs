using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelScript : MonoBehaviour {

    public GameObject snowflake;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector2(transform.position.x, snowflake.transform.position.y);
		
	}
}
