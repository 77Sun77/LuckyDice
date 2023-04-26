using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Vector3 sideGravity;
    public float GravityScale;
    public float TorqueScale;

    public List<DiceFace> diceFaces;

    public const int SumBothSide = 6;
    public int DiceValue;

    public bool IsMovingDone;

    Rigidbody rigid;

    private void Awake()
    {
        sideGravity = new Vector3(0, 0, GravityScale);
        rigid = transform.GetComponent<Rigidbody>();
        AddRandomRotation();
    }

    private void Update()
    {
        rigid.AddForce(sideGravity*Time.deltaTime);

        IsMovingDone = rigid.velocity.magnitude < 1f;

        if (IsMovingDone)
            IndicateNum();


    }

    void AddRandomRotation()
    {
        float ranXTorque = Random.Range(0f, TorqueScale);
        float ranYTorque = Random.Range(0f, TorqueScale);
        float ranZTorque = Random.Range(0f, TorqueScale);

        rigid.AddTorque(new Vector3(ranXTorque,ranYTorque,ranZTorque));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    void IndicateNum()
    {
        for (int i = 0; i < diceFaces.Count; i++)
        {
            if(diceFaces[i].IsContacting)
            {
                DiceValue = SumBothSide - i;
            }
        }
    }

}
