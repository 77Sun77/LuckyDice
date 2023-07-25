using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TextMeshProUGUI HP_Txt;
    public TextMeshProUGUI Gold_Txt;

    [Header("Prefab Container")]
    public GameObject HPBar_Prefab;
    public GameObject SysthesisIcon_Prefab;

    public GameObject UI;

    public FadeInOut fade;
    public GameObject startBtn, startTxt;
    TextMeshProUGUI startText;

    public GameObject allyDiceControl, itemDiceControl;
    private void Awake()
    {
        instance = this;

        startText = startTxt.GetComponent<TextMeshProUGUI>();

        
    }
    private void Start()
    {
        GameManager.instance.OnWaveStart += StartGame;
        GameManager.instance.OnWaveEnd += EndGame;
    }

    private void Update()
    {
        HP_Txt.text = GameManager.instance._base.HP.ToString();
        Gold_Txt.text = GameManager.instance.money.ToString();
        startText.text = "Wave " + EnemyGenerator.instance.CurWaveIndex;

    }

    public void StartGame()
    {
        startBtn.SetActive(false);
        fade.StartFade(startTxt);
    }

    public void EndGame()
    {
        startBtn.SetActive(true);
    }


    public void ResetUI() // 나중에 얻은 아이템창 이후 확인버튼에 연결
    {
        UI.SetActive(true);
        allyDiceControl.SetActive(false);
        itemDiceControl.SetActive(false);
    }

    public void Invoke_ResetUI() // 나중에 얻은 아이템 띄우는 함수로 교체
    {
        Invoke("ResetUI", 4);
    }
}
