using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotation : MonoBehaviour
{
    Rigidbody rigid;

    public int count;

    float ranXTorque, ranYTorque, ranZTorque;

    public bool isSimulated;

    GameObject target;

    public Transform dice;
    public int Graduation;
    float max;

    public void SetDice(int count, float x, float y, float z, GameObject target)
    {
        if (count == 0)
        {
            isSimulated = false;
            gameObject.SetActive(false);
        }
        else
        {
            isSimulated = true;
            transform.GetChild(0).gameObject.layer = 14;
            this.target = target;
        }
        ranXTorque = x;
        ranYTorque = y;
        ranZTorque = z;


    }
    private void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        AddRandomRotation();
        if (isSimulated)
        {

            Physics.autoSimulation = false;

            for (int i = 0; i < 600; i++)
            {
                if (!Physics.autoSimulation)
                    Physics.Simulate(Time.fixedDeltaTime);


            }
        }
    }
    void AddRandomRotation()
    {

        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
    }

    void Update()
    {

        if (Vector3.Magnitude(rigid.velocity) < 0.1f && isSimulated)
        {
            max = dice.transform.GetChild(0).transform.position.y;
            Graduation = 1;
            for (int i = 0; i < 6; i++)
            {
                float y = dice.transform.GetChild(i).transform.position.y;
                if (max < y)
                {
                    max = y;
                    Graduation = i + 1;
                }
            }
            Physics.autoSimulation = true;
            isSimulated = false;
        }



    }
    public void DestroyDice()
    {
        Destroy(target);
        Destroy(gameObject);
    }
    public void OnDice()
    {
        target.SetActive(true);
        Destroy(target, 5f);
        Destroy(gameObject);
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
