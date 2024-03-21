using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using TMPro;

[DefaultExecutionOrder(-1)]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    protected override void Awake()
    {
        base.Awake();

        //LoadData();
        //Settings.firstTime = false;
    }
}
