using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    Player player;
    public Transform target;
    private Camera cam;
    bool finished;
    float timer = 0.0f;

    public Vector2 margin;
    public Vector2 smooting;

    public BoxCollider2D boundary;

    private Vector3
        _min,
        _max;

    private float cameraHalfWidth;
    private float cameraSize;

    public bool isFollowing { get; set; }

    public void Initialize(GameObject boundary)
    {
        this.boundary = boundary.GetComponent<BoxCollider2D>();

        _min = this.boundary.bounds.min;
        _max = this.boundary.bounds.max;
        isFollowing = true;
        cam = GetComponent<Camera>();
        cameraHalfWidth = cam.orthographicSize * ((float)Screen.width / Screen.height);
        cameraSize = cam.orthographicSize;
    }

    public void Initialize2()
    {
        player = FindObjectOfType<Player>();
        target = player.gameObject.transform;
    }

    public void DoUpdate()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;

        if (isFollowing)
        {
            if (Mathf.Abs(x - target.position.x) > margin.x)
                x = Mathf.Lerp(x, target.position.x, smooting.x * Time.deltaTime);
            if (Mathf.Abs(y - target.position.y) > margin.y)
                y = Mathf.Lerp(y, target.position.y, smooting.y * Time.deltaTime);
        }

        if (timer < 1.0f)
        {
            timer += 0.1f;
        }
        else
        {
            timer = 0.0f;
        }

        if (finished)
        {
            if (cam.orthographicSize < 10.5f)
                cam.orthographicSize += 0.05f;
        }

        cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);
        cameraSize = GetComponent<Camera>().orthographicSize;

        x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
        y = Mathf.Clamp(y, _min.y + cameraSize, _max.y - cameraSize);

        transform.position = new Vector3(x, y, -10);
    }

    public void ShowCard()
    {
        finished = true;
    }
}