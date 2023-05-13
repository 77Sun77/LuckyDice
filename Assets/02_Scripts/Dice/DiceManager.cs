using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject prefab;

    public float MaxTorqueScale;

    public int number;

    float ranXTorque, ranYTorque, ranZTorque, dirX, dirY, dirZ;

    GameObject temp;
    DiceRotation diceTemp;
    bool isShot;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && temp == null && !isShot)
        {
            StartCoroutine(LoopDice());
        }

    }
    IEnumerator LoopDice()
    {
        isShot = true;
        number = Random.Range(1, 7);
        print(number);
        while (isShot)
        {
            dirX = Random.Range(0, 360);
            dirY = Random.Range(0, 360);
            dirZ = Random.Range(0, 360);
            ranXTorque = Random.Range(0f, MaxTorqueScale);
            ranYTorque = Random.Range(0f, MaxTorqueScale);
            ranZTorque = Random.Range(0f, MaxTorqueScale);
            temp = null;
            for (int i = 0; i < 2; i++)
            {
                GameObject go = Instantiate(prefab, new Vector3(4, 2.25f, 0), Quaternion.Euler(dirX, dirY, dirZ));
                go.GetComponent<DiceRotation>().SetDice(i, ranXTorque, ranYTorque, ranZTorque, temp);
                if (i == 0) temp = go;
                else diceTemp = go.GetComponent<DiceRotation>();
            }
            yield return new WaitForFixedUpdate();
            while (diceTemp.Graduation == 0)
            {
                yield return null;
            }

            if (diceTemp.Graduation == number)
            {
                diceTemp.OnDice();
                isShot = false;
            }
            else
            {
                diceTemp.DestroyDice();
            }
        }
        
    }
}
