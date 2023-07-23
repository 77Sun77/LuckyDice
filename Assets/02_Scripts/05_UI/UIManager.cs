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

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        HP_Txt.text = $"x{GameManager.instance._base.HP}";
        Gold_Txt.text = $"x{GameManager.instance.money}";   
    }




}
