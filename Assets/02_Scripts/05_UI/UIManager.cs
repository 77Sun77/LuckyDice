using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public GameObject Blind, MapMask;
    public Material UI_Mat;

    [Header("Store")]
    public GameObject StorePanel;
    public Sprite[] StoreImg;
    public Image[] StoreSlots;
    int StoreNum;
    public bool IsStorePanelOn;

    private void Awake()
    {
        instance = this;

        startText = startTxt.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Initialize_Store();
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
        Invoke(nameof(ResetUI), 4);
    }

    public void Initialize_Store()
    {
        SetStoreImg();
        GameManager.instance.OnWaveEnd += Active_StorePanel;
    }

    public void Active_StorePanel()
    {
        IsStorePanelOn = true;
        StorePanel.SetActive(true);
        SetStoreImg();
    }
    
    public void UnActive_StorePanel()
    {
        IsStorePanelOn = false;
        StorePanel.SetActive(false);
    }
    /// <summary>
    /// 상점의 이미지 재설정
    /// </summary>
    public void SetStoreImg()
    {
        for (int i = 0; i < StoreSlots.Length; i++)
        {
            int imgNum = (int)AllyGenerator.instance.Store[i];

            StoreSlots[i].sprite = StoreImg[imgNum];
        }
    }

}
