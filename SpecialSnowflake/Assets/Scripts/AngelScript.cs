using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelScript : MonoBehaviour {

    private GameObject snowflake;

	public void DoUpdate () {
        if (snowflake == null)
        {
            snowflake = FindObjectOfType<Player>().gameObject;
            if (snowflake == null) return;
        }

        transform.position = new Vector2(transform.position.x, snowflake.transform.position.y);
	}
}
