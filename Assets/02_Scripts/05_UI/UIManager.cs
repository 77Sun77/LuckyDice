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

    public DiceControl allyDiceControl, itemDiceControl;

    public GameObject Blind, MapMask;
    public Material UI_Mat;

    [Header("Ally Store")]
    public GameObject allyStorePanel;
    public Sprite[] allyStoreImg;
    public Image[] allyStoreSlots;
    int StoreNum;
    public bool IsAllyStorePanelOn;
    [Header("Ally Store")]
    public GameObject itemStorePanel;
    public Sprite[] itemStoreImg;
    public Image[] itemStoreSlots;
    public bool IsitemStorePanelOn;

    public enum ItemKind { Barrier, HealPotion, BombExplosion, CharacterMove , SELL};
    public ItemKind[] itemStore;
    private void Awake()
    {
        instance = this;

        startText = startTxt.GetComponent<TextMeshProUGUI>();

        itemStore = new ItemKind[4];
        ResetStore();
    }
    private void Start()
    {
        Initialize_Store();
        GameManager.instance.OnWaveStart += StartGame;
        GameManager.instance.OnWaveStart += UnActive_StorePanel;

        GameManager.instance.OnWaveEnd += EndGame;
        GameManager.instance.OnWaveEnd += ResetStore;
    }

    private void Update()
    {
        HP_Txt.text = GameManager.instance._base.HP.ToString();
        Gold_Txt.text = GameManager.instance.money.ToString();
        startText.text = "Wave " + EnemyGenerator.instance.CurWaveIndex;
        if (allyDiceControl.IsRollingDice || itemDiceControl.IsRollingDice) return;//주사위가 굴러가는 도중 상점 끄기 비활성화

        if (PawnPlacementManager.instance.isInventoryHold) UnActive_StorePanel();

        

        if (IsAllyStorePanelOn && Input.GetKeyDown(KeyCode.Escape)) UnActive_StorePanel();
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
        allyDiceControl.gameObject.SetActive(false);
        itemDiceControl.gameObject.SetActive(false);
    }

    public void Invoke_ResetUI() // 나중에 얻은 아이템 띄우는 함수로 교체
    {
        Invoke(nameof(ResetUI), 4);
    }

    public void Initialize_Store()
    {
        SetStoreImg();
        //GameManager.instance.OnWaveEnd += Active_StorePanel;
    }

    public void Active_StorePanel()
    {

        IsAllyStorePanelOn = true;
        allyStorePanel.SetActive(true);
        allyDiceControl.gameObject.SetActive(true);
        SetStoreImg();
    }
    
    public void UnActive_StorePanel()
    {
        IsAllyStorePanelOn = false;
        allyStorePanel.SetActive(false);
        allyDiceControl.gameObject.SetActive(false);
        
        IsitemStorePanelOn = false;
        itemStorePanel.SetActive(false);
        itemDiceControl.gameObject.SetActive(false);
    }

    public void Trigger_StorePanel(bool isAlly)
    {

        if (isAlly)
        {
            IsAllyStorePanelOn = !IsAllyStorePanelOn;
            allyStorePanel.SetActive(IsAllyStorePanelOn);
            allyDiceControl.gameObject.SetActive(IsAllyStorePanelOn);
        }
        else
        {
            IsitemStorePanelOn = !IsitemStorePanelOn;
            itemStorePanel.SetActive(IsitemStorePanelOn);
            itemDiceControl.gameObject.SetActive(IsitemStorePanelOn);
        }
        
        
        SetStoreImg();
    }



    /// <summary>
    /// 상점의 이미지 재설정
    /// </summary>
    /// 
    public enum ImageType { ally, item, All, SELL }
    
    public void SetStoreImg(ImageType kind = ImageType.All)
    {
        if(kind == ImageType.ally || kind == ImageType.All)
        {
            for (int i = 0; i < allyStoreSlots.Length; i++)
            {

                int imgNum = (int)AllyGenerator.instance.Store[i];

                allyStoreSlots[i].sprite = allyStoreImg[imgNum];
            }
        }
        if(kind == ImageType.item || kind == ImageType.All)
        {
            for (int i = 0; i < itemStoreSlots.Length; i++)
            {
                int imgNum = (int)itemStore[i];

                itemStoreSlots[i].sprite = itemStoreImg[imgNum];
            }
        }
        
    }

    void ResetStore()
    {
        for (int i = 0; i < itemStore.Length; i++)
        {
            int randomInt = Random.Range(0, 4);
            itemStore.SetValue(randomInt, i);
        }
        SetStoreImg(ImageType.item);
    }
    public void OnClick_Toggle(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }
    public void Roll(int num)
    {

        if (itemStore[num] != ItemKind.SELL)
        {
            switch (itemStore[num])
            {
                case ItemKind.Barrier:
                    GameManager.instance.inventory.Add_Inventory("Barrier");
                    break;
                case ItemKind.BombExplosion:
                    GameManager.instance.inventory.Add_Inventory("Bomb");
                    break;
                case ItemKind.HealPotion:
                    GameManager.instance.inventory.Add_Inventory("HealPotion");
                    break;
                case ItemKind.CharacterMove:
                    GameManager.instance.inventory.Add_Inventory("CharMove");
                    break;
    
            }

            itemStore.SetValue(ItemKind.SELL, num);
            SetStoreImg();
        }
    }
}
