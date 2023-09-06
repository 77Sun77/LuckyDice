using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rigid;
    public float ranXTorque, ranYTorque, ranZTorque;
    public float dirX, dirY, dirZ;

    bool IsStart;

    public DiceRotation.DIceKind _DiceKind;
    void Start()
    {
        IsStart = false;
        transform.eulerAngles = new Vector3(dirX, dirY, dirZ);
        rigid = transform.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.left * 200);
        rigid.AddTorque(new Vector3(ranXTorque, ranYTorque, ranZTorque));
        Invoke(nameof(Check_Starting),1f);
        //UIManager.instance.Invoke_ResetUI(); // 이 자리에 나중에 얻은 애니메이션 호출하는 함수 실행
    }

    private void Update()
    {
        if (rigid.angularVelocity.sqrMagnitude == 0f)
        {
            if (DiceManager.instance.allyDiceControl.IsRollingDice && IsStart)
            {
                if(_DiceKind == DiceRotation.DIceKind.Ally)
                {
                    int num = GameManager.instance.AllyIndex_Return(DiceManager.instance.number);
                    AllyGenerator.instance.Roll(DiceManager.instance.number);
                    DiceManager.instance.allyDiceControl.enable = true;//멈췄던 주사위 게이지 다시 재생
                    DiceManager.instance.allyDiceControl.IsRollingDice = false;
                    Destroy(gameObject);
                }
                else
                {
                    int num = DiceManager.instance.number;
                    if (num == 0) GameManager.instance.inventory.Add_Inventory("HealPotion");
                    else if (num == 1) GameManager.instance.inventory.Add_Inventory("Bomb");
                    else if (num == 2) GameManager.instance.inventory.Add_Inventory("Barrier");
                    else GameManager.instance.inventory.Add_Inventory("CharMove");

                    DiceManager.instance.allyDiceControl.IsRollingDice = false;
                }
            }

        }
    }

    void Check_Starting()//주사위가 스폰되자마자 발동되는 것 방지
    {
        IsStart = true;
    }

}
