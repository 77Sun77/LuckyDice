using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject prefab;

    public float MaxTorqueScale;
    Rigidbody rigid;

    public int count;

    float ranXTorque, ranYTorque, ranZTorque, dirX, dirY, dirZ;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dirX = Random.Range(0, 360);
            dirY = Random.Range(0, 360);
            dirZ = Random.Range(0, 360);
            ranXTorque = Random.Range(0f, MaxTorqueScale);
            ranYTorque = Random.Range(0f, MaxTorqueScale);
            ranZTorque = Random.Range(0f, MaxTorqueScale);
            for (int i = 0; i < 2; i++)
            {
                Instantiate(prefab, new Vector3(4, 2.25f, 0), Quaternion.Euler(dirX, dirY, dirZ)).GetComponent<DIDIce>().SetDice(i, ranXTorque, ranYTorque, ranZTorque);
            }
        }


    }

}
