using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    GameObject ballPrefab;
    [SerializeField] List<Vector2> ballPositions = new List<Vector2>();
    List<ChristmasBall> ballGOs = new List<ChristmasBall>();

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
    bool tutorialTextOn = false;
    float passedTime = 0.0f;

    public void Start()
    {
        background = Instantiate(Resources.Load<GameObject>("Prefabs/Background"));
        canvas = Instantiate(Resources.Load<GameObject>("Prefabs/Canvas"));
        canvas.transform.Find("EnterNamePanel").GetComponentInChildren<Button>().onClick.AddListener(NameEntered);
        canvas.transform.Find("RestartPanel").GetComponentInChildren<Button>().onClick.AddListener(Restart);

        camScript = Camera.main.GetComponent<CameraScript>();
        camScript.Initialize(background);

        ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
        lightPrefab = Resources.Load<GameObject>("Prefabs/Light");

        if (ballPositions.Count > 0 && ballPrefab != null)
        {
            for (int i = 0; i < ballPositions.Count; i++)
            {
                ballGOs.Add(Instantiate(ballPrefab, ballPositions[i], Quaternion.identity).GetComponent<ChristmasBall>());
                ballGOs[ballGOs.Count - 1].Initialize();
            }
        }

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
        angels[0] = Instantiate(angelPrefab, new Vector2(7.1f, 0.0f), Quaternion.identity).GetComponent<AngelScript>();
        angels[0].transform.localScale = new Vector3(-0.6f, 0.6f, 1);
        angels[1] = Instantiate(angelPrefab, new Vector2(-7.1f, 0.0f), Quaternion.identity).GetComponent<AngelScript>();
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
        for (int i = 0; i < ballGOs.Count; i++)
        {
            ballGOs[i].DoUpdate();
        }

        passedTime += Time.deltaTime;

        if (hintTextOn)
        {
            if(passedTime > 4.0f)
            {
                canvas.transform.Find("InGamePanel").Find("Text").gameObject.SetActive(false);
                hintTextOn = false;
            }
        }

        if (tutorialTextOn)
        {
            if (passedTime > 4.0f)
            {
                canvas.transform.Find("InGamePanel").Find("Image").gameObject.SetActive(false);
                tutorialTextOn = false;
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
        playerGO.transform.Find("Particle").GetComponent<ParticleSystemRenderer>().material.mainTexture = snowflakeControl.SnowFlake.texture;

        canvas.transform.Find("EnterNamePanel").gameObject.SetActive(false);
        camScript.Initialize2();

        hintTextOn = true;
        tutorialTextOn = true;
        canvas.transform.Find("InGamePanel").gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("main");
    }

    public void Win()
    {
        // zoom
        camScript.ShowCard();

        // fancy pancy aan
        player.transform.position = new Vector3(0.0f, 10.5f, 0.0f);

        canvas.transform.Find("WinPanel").gameObject.SetActive(true);
        canvas.transform.Find("WinPanel").Find("Text").GetComponent<Text>().text = enteredName;
        canvas.transform.Find("RestartPanel").gameObject.SetActive(true);
    }
}
