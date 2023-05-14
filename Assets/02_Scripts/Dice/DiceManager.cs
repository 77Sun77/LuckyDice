using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;

    public GameObject prefab;

    public float MaxTorqueScale;

    public int number, count;

    float ranXTorque, ranYTorque, ranZTorque, dirX, dirY, dirZ;

    GameObject temp;
    DiceRotation diceTemp;
    bool isShot;

    public GameObject[] Dices;
    void Start()
    {
        instance = this;
    }
    void Update()
    {

    }
    IEnumerator LoopDice(int number)
    {
        isShot = true;
        this.number = number;
        print(this.number);
        while (isShot)
        {
            if(count > 5)
            {
                Instantiate(Dices[number - 1]);
                isShot = false;
                break;
            }
            dirX = Random.Range(0, 360);
            dirY = Random.Range(0, 360);
            dirZ = Random.Range(0, 360);
            ranXTorque = Random.Range(0f, MaxTorqueScale);
            ranYTorque = Random.Range(0f, MaxTorqueScale);
            ranZTorque = Random.Range(0f, MaxTorqueScale);
            temp = null;
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(prefab, new Vector3(4, 0f, -2f), Quaternion.Euler(dirX, dirY, dirZ));
                go.GetComponent<DiceRotation>().SetDice(i, ranXTorque, ranYTorque, ranZTorque, temp);
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
                print(dirX + " " + dirY +" "+ dirZ + " " + ranXTorque + " " + ranYTorque + " " + ranZTorque + " ");
            }
            else
            {
                diceTemp.DestroyDice();
            }
            count++;

        }
        count = 0;
    }

    public void DiceControl(int number)
    {
        StartCoroutine(LoopDice(number));
    }
}
