using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;

    public int number;

    public GameObject[] AllyDices, ItemDices;
    void Start()
    {
        instance = this;
    }
    void Update()
    {

    }
    

    public void DiceControl(int number, DiceRotation.DIceKind _DiceKind)
    {
        this.number = number-1;
        if (_DiceKind == DiceRotation.DIceKind.Ally)
        {
            Instantiate(AllyDices[number-1]);
        }
        else
        {
            Instantiate(ItemDices[number-1]);
        }
    }
}
