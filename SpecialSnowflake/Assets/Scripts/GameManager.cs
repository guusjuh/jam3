using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    GameObject ballPrefab;
    [SerializeField] List<Vector2> ballPositions = new List<Vector2>();
    List<GameObject> ballGOs = new List<GameObject>();

    GameObject lightPrefab;
    [SerializeField] List<Vector2> lightPositions = new List<Vector2>();
    List<GameObject> lightGOs = new List<GameObject>();
    [SerializeField] List<Vector2> starPositions = new List<Vector2>();
    List<GameObject> starGOs = new List<GameObject>();

    AngelScript[] angels = new AngelScript[2];

    GameObject background;
    GameObject canvas;

    GameObject playerGO;
    Player player;

    SnowflakeControl snowflakeControl;
    Sprite snowflakeSprite;

    CameraScript camScript;

    string enteredName;

    bool hintTextOn = false;
    float passedTime = 0.0f;
    Color currentColor;

    public void Start()
    {
        background = Instantiate(Resources.Load<GameObject>("Prefabs/Background"));
        canvas = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas"));
        canvas.GetComponentInChildren<Button>().onClick.AddListener(NameEntered);

        camScript = Camera.main.GetComponent<CameraScript>();
        camScript.Initialize(background);

        ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        lightPrefab = Resources.Load<GameObject>("Prefabs/Light");

        if (ballPositions.Count > 0 && ballPrefab != null)
            ballGOs = InstantiateMultipleOf(ballPrefab, ballPositions);

        if (lightPositions.Count > 0 && lightPrefab != null)
            lightGOs = InstantiateMultipleOf(lightPrefab, lightPositions);

        if (starPositions.Count > 0 && lightPrefab != null)
            starGOs = InstantiateMultipleOf(lightPrefab, starPositions);

        for (int i = 0; i < lightGOs.Count; i++)
        {
            lightGOs[i].GetComponent<LightScript>().Initialize();
        }

        for (int i = 0; i < starGOs.Count; i++)
        {
            starGOs[i].GetComponent<LightScript>().isStar = true;
            starGOs[i].GetComponent<LightScript>().Initialize();
        }

        GameObject angelPrefab = Resources.Load<GameObject>("Prefabs/Angel");
        angels[0] = Instantiate(angelPrefab, new Vector2(7.3f, 0.0f), Quaternion.identity).GetComponent<AngelScript>();
        angels[0].transform.localScale = new Vector3(-0.6f, 0.6f, 1);
        angels[1] = Instantiate(angelPrefab, new Vector2(-7.3f, 0.0f), Quaternion.identity).GetComponent<AngelScript>();
    }

    public void Update()
    {
        if (playerGO == null || player == null) return;

        camScript.DoUpdate();
        player.DoUpdate();
        for(int i = 0; i < angels.Length; i++)
        {
            angels[i].DoUpdate();
        }

        if (hintTextOn)
        {
            passedTime += Time.deltaTime;
            if(passedTime > 1.5f)
            {
                currentColor.a -= 0.01f * Time.deltaTime;
                canvas.transform.Find("InGamePanel").Find("Text").gameObject.GetComponent<Text>().color = currentColor;
            }
        }
    }

    private List<GameObject> InstantiateMultipleOf(GameObject prefab, List<Vector2> positions)
    {
        List<GameObject> gameobjects = new List<GameObject>();

        for (int i = 0; i < positions.Count; i++)
            gameobjects.Add(GameObject.Instantiate(prefab, positions[i], Quaternion.identity));

        return gameobjects;
    }

    public void NameEntered()
    {
        enteredName = canvas.GetComponentInChildren<InputField>().text;
        if (enteredName == "Type your name ..." || enteredName == "") return;

        snowflakeControl = new SnowflakeControl();
        snowflakeControl.Initialize(enteredName);

        playerGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player = playerGO.GetComponent<Player>();
        player.Initialize();

        snowflakeSprite = Sprite.Create(snowflakeControl.SnowFlake.texture, 
                          new Rect(0.0f, 0.0f, snowflakeControl.SnowFlake.texture.width, snowflakeControl.SnowFlake.texture.height), 
                          new Vector2(0.5f, 0.5f)); 
        playerGO.GetComponent<SpriteRenderer>().sprite = snowflakeSprite;

        canvas.transform.Find("EnterNamePanel").gameObject.SetActive(false);
        camScript.Initialize2();

        hintTextOn = true;
        canvas.transform.Find("InGamePanel").gameObject.SetActive(true);
        currentColor = canvas.transform.Find("InGamePanel").gameObject.GetComponent<Image>().color;
    }

    public void Win()
    {
        // zoom
        camScript.ShowCard();

        // fancy pancy aan
        player.transform.position = new Vector3(0.0f, 10.5f, 0.0f);

        canvas.transform.Find("WinPanel").gameObject.SetActive(true);
        canvas.transform.Find("WinPanel").Find("Text").GetComponent<Text>().text = enteredName;
    }
}
