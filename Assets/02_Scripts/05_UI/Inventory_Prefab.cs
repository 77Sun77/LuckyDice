using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory_Prefab : MonoBehaviour
{
    public enum Obj_Kind { Dice, Unit, Item };
    public Obj_Kind Kind;

    public GameObject prefab;
    public string objectType;

    public int Rating;

    TextMeshProUGUI text;

    public enum Dice_Kind { Ally, Item}
    public Dice_Kind d_Kind;

    GameObject img;

    // Ally ����
    public bool isSynthesis;
    public List<GameObject> SynthesisUnits;

    SynthesisIcon synthesis;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick_Btn);
        img = transform.Find("BG").gameObject;
        if (Kind == Obj_Kind.Unit)
        {
            text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

            
        }
        //else if(Kind == Obj_Kind.Dice)
        //{
        //    if(d_Kind == Dice_Kind.Ally)
        //    {
        //        prefab = UIManager.instance.allyDiceControl;
        //    }
        //    else
        //    {
        //        prefab = UIManager.instance.itemDiceControl;
        //    }
        //}
        
    }
    void Update()
    {
        if (text)
        {
            string s = "";
            for(int i=1;i<=Rating;i++)
            {
                s += "I";
            }
            text.text = s;
        }

        if (PawnPlacementManager.instance.ObjTemp == gameObject) img.SetActive(true);
        else img.SetActive(false);

        if (isSynthesis)
        {
            isSynthesis = SynthesisUnits.Count == 3 ? true : false;
            foreach (GameObject go in SynthesisUnits)
            {
                if (go == null)
                {
                    isSynthesis = false;
                    break;
                }
            }

            SpawnSynthesis();
        }
        else
        {
            SynthesisUnits.Clear();
            DestroySynthesis();
        }
    }

    public void OnClick_Btn()
    {
        //UIManager.instance.Blind.SetActive(true);

        // if (Kind != Obj_Kind.Dice) UIManager.instance.MapMask.SetActive(true);
        // else UIManager.instance.MapMask.SetActive(false);
        if (Kind == Obj_Kind.Dice && (DiceManager.instance.allyDiceControl.IsRollingDice || DiceManager.instance.itemDiceControl.IsRollingDice)) return;

        if (PawnPlacementManager.instance.ObjTemp)
        {
            PawnPlacementManager.instance.ObjTemp = null;
            PawnPlacementManager.instance.isActive = false;
        }
        else
        {
            PawnPlacementManager.instance.ObjTemp = gameObject;
            PawnPlacementManager.instance.isActive = true;
        }
        

    }

    public void OnClick_Dice()
    {
        if (GameManager.instance.inventory.contents.childCount == 15) return;
        if (d_Kind == Dice_Kind.Item) 
        {
            UIManager.instance.Trigger_StorePanel(false);
        }
        else if (d_Kind == Dice_Kind.Ally)
        {
            
            UIManager.instance.Trigger_StorePanel(true);
        }
        
        //prefab.GetComponent<DiceControl>().temp = gameObject;

    }

    public void SpawnSynthesis()
    {
        if (!synthesis)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject go = Instantiate(UIManager.instance.SysthesisIcon_Prefab, canvas.transform);
            go.name = $"{transform.name} Synthesis Icon";

            synthesis = go.GetComponent<SynthesisIcon>();
            synthesis.Initialize_SynthesisIcon(_obj:this);
        }

    }
    public void DestroySynthesis()
    {
        if (synthesis)
        {
            Destroy(synthesis.gameObject);
            synthesis = null;
        }

    }
}
