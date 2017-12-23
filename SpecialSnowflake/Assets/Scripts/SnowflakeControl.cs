using UnityEngine;
using System;
using System.Collections;

public class SnowflakeControl : MonoBehaviour
{
    private int seed = 0;

    private Color flakeColor = Color.white;
    private SnowFlake snowFlake;
    public SnowFlake SnowFlake { get { return snowFlake; } }

    public void Initialize(string name)
    {
        InitFlakes(name);
    }

    private int CalculateSeed(string name)
    {
        char[] charArray = name.ToCharArray();
        int seed = 0;

        for (int i = 0; i < charArray.Length; i++)
            seed += ((int)charArray[i] * (i + 1));

        return seed;
    }

    private void InitFlakes(string name)
    {
        seed = CalculateSeed(name.ToLower());
        snowFlake = new SnowFlake(seed);
    }
}