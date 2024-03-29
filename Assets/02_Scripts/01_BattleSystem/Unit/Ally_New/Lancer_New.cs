using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer_New : Ally
{
    [Header("Lancer_R2")]
    public List<Vector2> AOERange;
    public int curAtkCount;
    public int RangeAtkDelay;
    public float KnockBackValue;

    [Header("Lancer_R3")]
    public List<Vector2> AOERange_R3;
    public float KnockBackValue_R3;
    public float RangeAtkTickDelay;
    public float RangeAtkDuration;
    

    new void Update()
    {
        switch (GameManager.instance.unitUpgrade[2]) // 업글 능력치 적용
        {
            case 1:
                Upgrade(0, 0, 0);
                break;
            case 2:
                Upgrade(0, 0, 0);
                break;
            case 3:
                Upgrade(0, 0, 0);
                break;

        }
        base.Update();
    }

    protected override bool TryAttack()
    {
        switch (Rating)
        {
            case 1:
                Active_Attack(damage);
                return true;

            case 2:
                if (curAtkCount == RangeAtkDelay)
                {
                    curAtkCount = 0;
                    Vector2 targetPos = new Vector2(pawn.X + 1,pawn.Y);
                    List<Unit> units = AOE_Attack(damage,targetPos, AOERange);

                    foreach (Unit unit in units)
                    {
                        unit.GetComponent<Enemy>().GetKnockBack(KnockBackValue);
                    }
                }
                else
                {
                    Active_Attack(damage);
                    curAtkCount++;
                }
                return true;

            case 3:
                StartCoroutine(Do_R3_Attack(RangeAtkTickDelay, RangeAtkDuration));
                return true;
        }

        return false;
    }

    IEnumerator Do_R3_Attack(float rangeAtkTickDelay,float rangeAtkDuration)
    {
        Debug.Log("Do_R3_Attack");
        float curTickTimer = 0f;
        while (rangeAtkDuration > 0)
        {
            if (curTickTimer <= 0)
            {
                curTickTimer = rangeAtkTickDelay;
                Vector2 targetPos = new Vector2(pawn.X + 2, pawn.Y);
                List<Unit> units = AOE_Attack(damage,targetPos, AOERange_R3);

                foreach (Unit unit in units)
                {
                    unit.GetComponent<Enemy>().GetKnockBack(KnockBackValue_R3);
                }
            }

            curTickTimer -= Time.deltaTime;
            rangeAtkDuration -= Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
    }


}
