using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public float maxHP;
    public float curHP;

    public GameObject HPBar_Cur;
    public Object Target;

    public Vector3 HPBarOffset;

    Unit unit;
    Enemy enemy;

    public void InitializeHPBar(Object obj, Vector3 _hPBarOffset)
    {
        Target = obj;
        HPBarOffset = _hPBarOffset;

        if (Target is Unit)
        {
            unit = Target as Unit;
            maxHP = unit.maxHP;
            //Debug.Log("À¯´Ö");
        }
        else if (Target is Enemy)
        {
            enemy = Target as Enemy;
            maxHP = enemy.maxHP;
            //Debug.Log("Àû");
        }
    }


    private void Update()
    {
        if (unit)
        {
            curHP = unit.hp;
            maxHP = unit.maxHP;
            transform.position = Camera.main.WorldToScreenPoint(unit.transform.position + HPBarOffset);

        }
        else if (enemy)
        {
            curHP = enemy.hp;

            maxHP = enemy.maxHP;
            transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position + HPBarOffset);
        }
        else
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        if(curHP != 0 && maxHP != 0)
        {
            Vector3 curHP_X_Scale = new Vector3(curHP / maxHP, 1, 1);
            HPBar_Cur.transform.localScale = curHP_X_Scale;
        }
        
    }
}
