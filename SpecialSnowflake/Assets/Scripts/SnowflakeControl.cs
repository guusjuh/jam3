using UnityEngine;
using System;
using System.Collections;

public class SnowflakeControl : MonoBehaviour
{
    [SerializeField] private int seed = 0;
    public string name = "";

    Color flakeColor = Color.white;
    [SerializeField] SnowFlake snowFlake;

    public void Start()
    {
        InitFlakes();

        //TODO: this is the 'player' object!
        GameObject cube = GameObject.FindGameObjectWithTag("Player");  
        Sprite sprite = Sprite.Create(snowFlake.texture, new Rect(0.0f,0.0f,snowFlake.texture.width,snowFlake.texture.height), new Vector2(0.5f, 0.5f)); 
        cube.GetComponent<SpriteRenderer>().sprite = sprite;//snowFlake.texture;
    }

    private int CalculateSeed(string name)
    {
        char[] charArray = name.ToCharArray();
        int seed = 0;

        for (int i = 0; i < charArray.Length; i++)
            seed += ((int)charArray[i] * (i + 1));

        return seed;
    }

    private void InitFlakes()
    {
        seed = CalculateSeed(name);
        snowFlake = new SnowFlake(seed);
    }
}