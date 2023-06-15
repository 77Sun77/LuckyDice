using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotation : MonoBehaviour
{
    Rigidbody rigid;

    public int count;

    float ranXTorque, ranYTorque, ranZTorque;

    public bool isSimulated, notSimulated;

    GameObject target;

    public Transform dice;
    public int Graduation;
    float min;

    bool roll;

    public enum DIceKind { Ally, Item };
    public DIceKind _DiceKind;

    public void SetDice(int count, float x, float y, float z, GameObject target, DIceKind _DiceKind)
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
            if (_DiceKind == DIceKind.Item) gameObject.layer = 14;
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

        if (Vector3.Magnitude(rigid.velocity) < 0.1f)
        {
            if(_DiceKind == DIceKind.Ally)
            {
                if (isSimulated)
                {
                    min = dice.transform.GetChild(0).transform.position.z;
                    Graduation = 1;
                    for (int i = 0; i < 6; i++)
                    {
                        float z = dice.transform.GetChild(i).transform.position.z;
                        if (min > z)
                        {
                            min = z;
                            Graduation = i + 1;
                        }
                    }
                    Physics.autoSimulation = true;
                    isSimulated = false;
                }
                if (!roll && notSimulated)
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
            else
            {
                if (isSimulated)
                {
                    min = dice.transform.GetChild(0).transform.position.z;
                    Graduation = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        float z = dice.transform.GetChild(i).transform.position.z;
                        if (min > z)
                        {
                            min = z;
                            Graduation = i + 1;
                        }
                    }
                    Physics.autoSimulation = true;
                    isSimulated = false;
                }
                if (!roll && notSimulated)
                {
                    int num = DiceManager.instance.number;
                    if (num == 0) PawnGenerator.instance.Roll(GoogleSheetManager.instance.HealPotion);
                    else if (num == 1) PawnGenerator.instance.Roll(GoogleSheetManager.instance.BombExplosion);
                    else if (num == 2) PawnGenerator.instance.Roll(GoogleSheetManager.instance.Barrier);
                    else PawnGenerator.instance.Roll(GoogleSheetManager.instance.CharMove);

                    roll = true;
                }
            }
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
        Destroy(target, 4f);
        Destroy(gameObject);
        target.GetComponent<DiceRotation>().notSimulated = true;
    }
}
