using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public float maxHP;
    public float curHP;

    public GameObject HPBar_Cur;
    public Object Target;

    Unit unit;
    Enemy enemy;

    public void InitializeHPBar(Object obj)
    {
        Target = obj;

        if (Target is Unit)
        {
            unit = Target as Unit;
            maxHP = unit.maxhp;
            Debug.Log("À¯´Ö");
        }
        else if (Target is Enemy)
        {
            enemy = Target as Enemy;
            maxHP = enemy.maxhp;
            Debug.Log("Àû");
        }
    }


    private void Update()
    {
        if (unit)
        {
            curHP = unit.hp;
            transform.position = Camera.main.WorldToScreenPoint(unit.transform.position) + unit.HPBarOffset;
        }
        else if (enemy)
        {
            curHP = enemy.hp;
            transform.position = Camera.main.WorldToScreenPoint(enemy.transform.position) + enemy.HPBarOffset;
        }
        else
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        Vector3 curHP_X_Scale = new Vector3(curHP / maxHP, 1, 1);
        HPBar_Cur.transform.localScale = curHP_X_Scale;

        
    }



}
