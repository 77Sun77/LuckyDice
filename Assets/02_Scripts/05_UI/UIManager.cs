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
        GameManager.instance.OnWaveStart += UnActive_StorePanel;

        GameManager.instance.OnWaveEnd += EndGame;
    }

    private void Update()
    {
        HP_Txt.text = GameManager.instance._base.HP.ToString();
        Gold_Txt.text = GameManager.instance.money.ToString();
        startText.text = "Wave " + EnemyGenerator.instance.CurWaveIndex;

        if (Input.GetKeyDown(KeyCode.Tab)&&!PawnPlacementManager.instance.isInventoryHold) Trigger_StorePanel();//���� �����״�

        if (IsStorePanelOn && Input.GetKeyDown(KeyCode.Escape)) UnActive_StorePanel();
            
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


    public void ResetUI() // ���߿� ���� ������â ���� Ȯ�ι�ư�� ����
    {
        UI.SetActive(true);
        allyDiceControl.SetActive(false);
        itemDiceControl.SetActive(false);
    }

    public void Invoke_ResetUI() // ���߿� ���� ������ ���� �Լ��� ��ü
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
        allyDiceControl.SetActive(true);
        SetStoreImg();
    }
    
    public void UnActive_StorePanel()
    {
        IsStorePanelOn = false;
        StorePanel.SetActive(false);
        allyDiceControl.SetActive(false);
    }

    public void Trigger_StorePanel()
    {
        IsStorePanelOn = !IsStorePanelOn;
        StorePanel.SetActive(IsStorePanelOn);
        allyDiceControl.SetActive(IsStorePanelOn);
        SetStoreImg();
    }



    /// <summary>
    /// ������ �̹��� �缳��
    /// </summary>
    public void SetStoreImg()
    {
        for (int i = 0; i < StoreSlots.Length; i++)
        {
            int imgNum = (int)AllyGenerator.instance.Store[i];

            StoreSlots[i].sprite = StoreImg[imgNum];
        }
    }


    public void OnClick_Toggle(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }

}
