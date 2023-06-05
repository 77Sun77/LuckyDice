using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rigid;
    public float ranXTorque, ranYTorque, ranZTorque;
    public float dirX, dirY, dirZ;
    int count;

    bool roll;
    void Start()
    {
        transform.eulerAngles = new Vector3(dirX, dirY, dirZ);
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
        Destroy(gameObject, 4);
    }

    private void Update()
    {
        if (Vector3.Magnitude(rigid.velocity) < 0.1f)
        {
            if (!roll)
            {
                int num = GameManager.instance.AllyIndex_Return(DiceManager.instance.number);
                if (num == 0) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Warrior[0]);
                else if (num == 1) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Sorcerer[0]);
                else if (num == 2) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Lancer[0]);
                else if (num == 3) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Tanker[0]);
                else if (num == 4) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Buffer[0]);
                else PawnGenerator.instance.Roll(GoogleSheetManager.instance.Archer[0]);
                roll = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (count == 0)
        {
            count++;
           // rigid.AddForce(Vector3.back * 100f);
        }
    }
}
