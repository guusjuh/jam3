using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakeCollision : MonoBehaviour {

    public int score;
    public GameObject dieParticles; //Optioneel.

    private Vector3 originalPos;

	// Use this for initialization
	void Start () {
        score = 0;

        originalPos = transform.position;		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        score = 0;
        transform.position = originalPos;

        if (dieParticles)
        {
            dieParticles.SetActive(true);
        }
    }
}
