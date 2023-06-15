using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemyKind enemyKind;

    [Header("EnemyOnly")]
    public int money;

    public float speed;
    public float ModifiedSpeed;

    public int explosionStack;
    public bool isOnCC;

    Coroutine runnigCor_Slow;
    Coroutine runnigCor_DotDamage;
    float curTick;

    private void Awake()
    {
        first_Setting();
    }

    protected override void first_Setting()
    {
        base.first_Setting();
        isEnemy = true;
        ModifiedSpeed = speed;
    }

    protected override void Update()
    {
        base.Update();

        Move();
    }

    protected override void Search_Targets()
    {
        targets.Clear();

        foreach (var Tile in detectRange_List.GetTileInRange(pawn.X, pawn.Y))
        {
            if (Tile.Ally != null) targets.Add(Tile.Ally);
        }
        isTargetDetected = targets.Count != 0;
    }

    protected override bool TryAttack()//적을 공격
    {
        bool _canAttack = false;

        if (pawn.IsOverCenter)
        {
            Active_Attack(damage);//적이 중앙을 넘어왔을때 근접 발동
            _canAttack = true;
        }

        return _canAttack;
    }

    protected override void Die()
    {
        GameManager.instance.Set_Money(money);
        EnemyGenerator.instance.SpawnedEnemies.Remove(this);
        base.Die();
    }

    void Move()
    {
        if (!isAttacking && !isOnCC)
        {
            transform.Translate(Vector2.left * (ModifiedSpeed * TileManager.Instance.XScale_Tile) * Time.deltaTime);
        }
    }
    public void GetSlow(float _slowValue, float _slowDuration)
    {
        if (runnigCor_Slow != null)
            StopCoroutine(runnigCor_Slow);

        runnigCor_Slow = StartCoroutine(GetSlow_Cor(_slowValue, _slowDuration));
    }
    IEnumerator GetSlow_Cor(float _slowValue, float _slowDuration)
    {
        float curTime = _slowDuration;

        while (curTime > 0f)
        {
            ModifiedSpeed = speed - _slowValue;
            curTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        ModifiedSpeed = speed;
    }
    public void GetKnockBack(float forceValue)
    {
        StartCoroutine(GetKnockBack_Cor(forceValue));
    }
    IEnumerator GetKnockBack_Cor(float forceValue)
    {
        isOnCC = true;

        while (forceValue > 0)
        {
            transform.Translate(Vector2.right * forceValue * Time.deltaTime);
            forceValue -= Time.deltaTime*5;
            yield return new WaitForFixedUpdate();
        }

        isOnCC = false;
    }


    public void TakeDotDamage(float dotDamage, float duration, float tickRate)
    {
        if(runnigCor_DotDamage!=null)
        StopCoroutine(runnigCor_DotDamage);

        runnigCor_DotDamage = StartCoroutine(TakeDotDamage_Cor(dotDamage, duration, tickRate));
    }
    IEnumerator TakeDotDamage_Cor(float dotDamage,float duration,float tickRate)
    {
        float curTime = duration;
        
        while (curTime > 0f)
        {
            curTick -= Time.deltaTime;
            curTime -= Time.deltaTime;

            if (curTick < 0f)
            {
                TakeDamage(dotDamage,this.gameObject);
                curTick = tickRate;
            }
            yield return new WaitForFixedUpdate();
        }        
    }


    public void TakeDamageByBomb()
    {
        hp -= 50;
        Die();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Base"))
        {
            coll.gameObject.GetComponent<Base>().HP -= 1;
            pawn.RemoveTilePawn();
            Destroy(gameObject);
        }
    }
}