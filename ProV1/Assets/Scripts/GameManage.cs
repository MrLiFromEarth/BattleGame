using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour {

    public Player player;
    public Enemy enemy;
    public GameObject playerGobj;
    public GameObject enemyGobj;
    public static GameManage Instance;
    public bool isGameStart = false;
    public float gameTime = 0;
    private void Awake()
    {
        Instance = this;
        DataManage.Instance.Init();
        AudioSourceManage.Instance.Init(transform.Find("AudioSourceManage").GetComponent<AudioSource>());
    }
    // Use this for initialization
    void Start () {

        AddHero();
    }
	
	// Update is called once per frame
	void Update () {

        if (isGameStart)
        {
            gameTime += Time.deltaTime;
            if (player.transform.gameObject.activeSelf && enemy.transform.gameObject.activeSelf && gameTime >= 1f)
            {
                player.Update();
                enemy.Update();
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("level", player.Level);
    }
    public void AddHero()
    {
        AddPlayer();
        AddEnemy();
    }
    private void AddPlayer()
    {
        if (player == null)
        {
            player = new Player();
            player.Init(playerGobj.transform);
            player.transform.position = new Vector3(0, 0, -3);
            player.transform.localEulerAngles = new Vector3(0,0,0);
        }
        else
        {
            player.transform.gameObject.SetActive(true);
            player.transform.position = new Vector3(0, 0, -3);
            player.transform.localEulerAngles = new Vector3(0, 0, 0);
            player.Init();
        }

    }
    private void AddEnemy()
    {
        if (enemy == null)
        {
            enemy = new Enemy();
            enemy.Init(enemyGobj.transform);
            enemy.transform.position = new Vector3(0, 0, 3);
            enemy.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            enemy.transform.gameObject.SetActive(true);
            enemy.transform.position = new Vector3(0, 0, 3);
            enemy.transform.localEulerAngles = new Vector3(0, 180, 0);
            enemy.Init();
        }
    }
}
