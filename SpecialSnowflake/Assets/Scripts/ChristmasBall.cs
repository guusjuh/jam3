using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasBall : MonoBehaviour
{
    float time;
    float totalTime = 3;
    bool direction;
    float schommelForce = 7.5f;

    public void Initialize()
    {
        totalTime = Random.Range(2.0f, 3.5f);
        schommelForce = Random.Range(6, 8);

        time = totalTime / 2.0f;
    } 

    public void DoUpdate()
    {
        time += Time.deltaTime;

        //schommel
        if (time < totalTime)
        {
            if (direction)
            {
                transform.RotateAround(transform.position + new Vector3(0, 1), Vector3.forward, schommelForce * Time.deltaTime);// (new Vector2(schommelForce * Time.deltaTime * 100, 0));
            }
            else
            {
                transform.RotateAround(transform.position + new Vector3(0, 1), Vector3.forward, -schommelForce * Time.deltaTime);// (new Vector2(schommelForce * Time.deltaTime * 100, 0));
            }
        }
        else
        {
            time = 0;
            direction = !direction;
        }
    }
}
