using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rigid;
    public float ranXTorque, ranYTorque, ranZTorque;
    public float dirX, dirY, dirZ;
    int count;

    void Start()
    {
        transform.eulerAngles = new Vector3(dirX, dirY, dirZ);
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
        Destroy(gameObject, 4);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (count == 0)
        {
            count++;
            rigid.AddForce(Vector3.up * 100f);
        }
    }
}
