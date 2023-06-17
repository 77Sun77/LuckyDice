using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;

    public GameObject AllyDice, ItemDice;

    public float MaxTorqueScale;

    public int number, count;

    float ranXTorque, ranYTorque, ranZTorque, dirX, dirY, dirZ;

    GameObject temp;
    DiceRotation diceTemp;
    bool isShot;

    public GameObject[] AllyDices, ItemDices;
    void Start()
    {
        instance = this;
    }
    void Update()
    {

    }
    IEnumerator LoopDice(int number, DiceRotation.DIceKind _DiceKind)
    {
        isShot = true;
        this.number = number;
        while (isShot)
        {
            if(count > 5)
            {
                if(_DiceKind == DiceRotation.DIceKind.Ally) Instantiate(AllyDices[number - 1]);
                else Instantiate(ItemDices[number - 1]);
                isShot = false;
                break;
            }

            dirX = Random.Range(0, 360);
            dirY = Random.Range(0, 360);
            dirZ = Random.Range(0, 360);
            if(_DiceKind == DiceRotation.DIceKind.Ally)
            {
                ranXTorque = Random.Range(0f, MaxTorqueScale);
                ranYTorque = Random.Range(0f, MaxTorqueScale);
                ranZTorque = Random.Range(0f, MaxTorqueScale);
            }
            else
            {
                ranXTorque = Random.Range(MaxTorqueScale, MaxTorqueScale*2);
                ranYTorque = Random.Range(MaxTorqueScale, MaxTorqueScale*2);
                ranZTorque = Random.Range(MaxTorqueScale, MaxTorqueScale*2);
            }
            temp = null;
            
            for (int i = 0; i < 2; i++)
            {
                GameObject go = null;
                if (_DiceKind == DiceRotation.DIceKind.Ally)
                {
                    go = Instantiate(AllyDice, new Vector3(4, 0f, -2f), Quaternion.Euler(dirX, dirY, dirZ));
                    
                }
                else
                {
                    go = Instantiate(ItemDice, new Vector3(4, 0f, -2f), Quaternion.Euler(dirX, dirY, dirZ));
                }
                go.GetComponent<DiceRotation>().SetDice(i, ranXTorque, ranYTorque, ranZTorque, temp, _DiceKind);
                if (i == 0) temp = go;
                else diceTemp = go.GetComponent<DiceRotation>();
            }
            
            yield return new WaitForFixedUpdate();
            while (diceTemp.Graduation == 0)
            {
                yield return null;
            }

            if (diceTemp.Graduation == this.number)
            {
                diceTemp.OnDice();
                isShot = false;
                print(dirX + " " + dirY + " " + dirZ);
                print(ranXTorque + " " + ranYTorque + " " + ranZTorque);
            }
            else
            {
                diceTemp.DestroyDice();
            }
            count++;

        }
        count = 0;
    }

    public void DiceControl(int number, DiceRotation.DIceKind _DiceKind)
    {
        StartCoroutine(LoopDice(number, _DiceKind));
    }
}
