using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManage : MonoBehaviour {

    [SerializeField]
    private Button startGame;
    [SerializeField]
    private Button closeBtn;
    [SerializeField]
    private Text levelTx;
    [SerializeField]
    private GameObject gamePanel;
    public static UIManage Instance;

    private float startButtonTime;
    private void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    void Start () {

        levelTx.text = "LEVEL: " + DataManage.Instance.Level;
    }
	
	// Update is called once per frame
	void Update () {

        StartGameButtonActive();

    }

    private void AddListenerFun()
    {

    }

    public void StartGameFun()
    {
        startButtonTime = 0;
        levelTx.gameObject.SetActive(false);
        startGame.gameObject.SetActive(false);
        GameManage.Instance.AddHero();
        LevelChangeFun();
        gamePanel.SetActive(false);
        GameManage.Instance.isGameStart = true;
    }

    //startButton 几秒后出现
    private void StartGameButtonActive()
    {
        if (!GameManage.Instance.isGameStart)
        {
            startButtonTime += Time.deltaTime;
            if (startButtonTime >= 2.0f)
            {
                startGame.gameObject.SetActive(true);
            }
        }
    }

    public void CloseFun()
    {
        Application.Quit();
    }

    private void LevelChangeFun()
    {
        levelTx.text = "LEVEL: " + GameManage.Instance.player.Level;
    }

    public void KillFun()
    {
        GameManage.Instance.isGameStart = false;    //可以开始游戏
        GameManage.Instance.gameTime = 0;           //游戏开始延迟重置
        LevelChangeFun();
        levelTx.gameObject.SetActive(true);         //显示等级
        gamePanel.SetActive(true);
    }
}
