using System;
using UnityEngine;

[Serializable]
public class SnowFlake
{
    private static System.Random rndGen;
    private const int DEPTH = 6;

    public int branchAngle = 30;
    public float branching = 0.33f;
    public float shrinking = 0.66f;
    public int size = 150;

    public float minx, miny, maxx, maxy;
    public Texture2D texture;

    public SnowFlake(int seed)
    {
        rndGen = new System.Random(seed);
        branchAngle = rndGen.Next(15, 165);                         
        branching = 0.2f + (float)rndGen.NextDouble() * 0.6f;        
        shrinking = 0.3f + (float)rndGen.NextDouble() * 0.4f;
        
        if (shrinking * branching > 0.36f) shrinking /= 2.0f;
        if (shrinking * branching < 0.1f) branching *= 2.0f;

        texture = new Texture2D(1024, 1024, TextureFormat.RGB24, false);
        Draw(texture);
        texture.Apply();
    }

    private void MakeTextureTransparant()
    {
        if (texture == null) return;

        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                texture.SetPixel(i, j, new Color(0, 0, 0, 0));
            }
        }
    }

    private Vector2 Rotate(Vector2 point, Vector2 center, float ang)
    {
        ang = ang / 180.0f * (float)Math.PI;
        return new Vector2(
          ((point.x - center.x) * (float)Math.Cos(ang) -
          (point.y - center.y) * (float)Math.Sin(ang)) + center.x,
          ((point.x - center.x) * (float)Math.Sin(ang) +
          (point.y - center.y) * (float)Math.Cos(ang)) + center.y);
    }

    private void DrawLine(Vector2 start, Vector2 end, bool textureExists = true)
    {
        if (!textureExists)
        {
            if (start.x < minx) minx = start.x;
            if (start.y < miny) miny = start.y;
            if (start.x > maxx) maxx = start.x;
            if (start.y > maxy) maxy = start.y;

            if (end.x < minx) minx = end.x;
            if (end.y < miny) miny = end.y;
            if (end.x > maxx) maxx = end.x;
            if (end.y > maxy) maxy = end.y;
        }
        else
        {
            texture.DrawLine(start, end, Color.white);
        }
    }

    private void DrawBranch(Vector2 start, Vector2 end, float width, int depth, bool textureExists = true)
    {
        Vector2 cnt = new Vector2(start.x + (end.x - start.x) * branching, start.y + (end.y - start.y) * branching);
        Vector2 nend = new Vector2(cnt.x + (end.x - start.x) * shrinking, cnt.y + (end.y - start.y) * shrinking);

        if (depth == DEPTH)
        {
            DrawLine(cnt, Rotate(nend, cnt, branchAngle), textureExists);
            DrawLine(cnt, Rotate(nend, cnt, -branchAngle), textureExists);
        }
        else
        {
            DrawBranch(cnt, Rotate(nend, cnt, branchAngle), width * 0.6f, depth + 1, textureExists);
            DrawBranch(cnt, Rotate(nend, cnt, -branchAngle), width * 0.6f, depth + 1, textureExists);
            DrawBranch(start, cnt, width * 0.6f, depth + 1, textureExists);
            DrawBranch(cnt, end, width * 0.6f, depth + 1, textureExists);
        }
    }

    public void Draw(Texture2D output)
    {        
        minx = maxx = Vector2.zero.x;
        maxy = miny = Vector2.zero.y;

        // calculate size
        Vector2 cnt = Vector2.zero;
        Vector2 start = new Vector2(0, 0 - size);
        for (int i = 0; i < 6; i++)
            DrawBranch(cnt, Rotate(start, cnt, i * 60), 2.0f, 0, false);

        int iminx = (int)minx - 2, 
            imaxx = (int)maxx + 2,
            iminy = (int)miny - 2, 
            imaxy = (int)maxy + 2;
        texture = new Texture2D(imaxx - iminx, imaxy - iminy);

        MakeTextureTransparant();

        cnt = new Vector2(-iminx, -iminy);
        start = new Vector2(-iminx, -size - iminy);
        for (int i = 0; i < 6; i++)
            DrawBranch(cnt, Rotate(start, cnt, i * 60), 2.0f, 0);
    }
}