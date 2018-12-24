using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManage : GameInstance<DataManage> {

    public int Level;

    public void Init()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            Level = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level",1);
            Level = PlayerPrefs.GetInt("level");
        }
    }
}
