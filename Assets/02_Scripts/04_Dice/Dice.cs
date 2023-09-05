using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rigid;
    public float ranXTorque, ranYTorque, ranZTorque;
    public float dirX, dirY, dirZ;

    bool roll;

    public DiceRotation.DIceKind _DiceKind;
    void Start()
    {
        transform.eulerAngles = new Vector3(dirX, dirY, dirZ);
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
        Destroy(gameObject, 4);
        //UIManager.instance.Invoke_ResetUI(); // 이 자리에 나중에 얻은 애니메이션 호출하는 함수 실행
    }

    private void Update()
    {
        if (Vector3.Magnitude(rigid.velocity) < 0.1f)
        {
            if (!roll)
            {
                if(_DiceKind == DiceRotation.DIceKind.Ally)
                {
                    int num = GameManager.instance.AllyIndex_Return(DiceManager.instance.number);
                    //if (num == 0) GameManager.instance.inventory.Add_Inventory("Warrior", 1);
                    //else if (num == 1) GameManager.instance.inventory.Add_Inventory("Sorcerer", 1);
                    //else if (num == 2) GameManager.instance.inventory.Add_Inventory("Lancer", 1);
                    //else if (num == 3) GameManager.instance.inventory.Add_Inventory("Tanker", 1);
                    //else if (num == 4) GameManager.instance.inventory.Add_Inventory("Buffer", 1);
                    //else GameManager.instance.inventory.Add_Inventory("Archer", 1);
                    AllyGenerator.instance.Roll(DiceManager.instance.number);
                    roll = true;
                }
                else
                {
                    int num = DiceManager.instance.number;
                    if (num == 0) GameManager.instance.inventory.Add_Inventory("HealPotion");
                    else if (num == 1) GameManager.instance.inventory.Add_Inventory("Bomb");
                    else if (num == 2) GameManager.instance.inventory.Add_Inventory("Barrier");
                    else GameManager.instance.inventory.Add_Inventory("CharMove");

                    roll = true;
                }
            }
        }
    }

}
