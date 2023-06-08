using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public abstract class Unit : MonoBehaviour
{
    [HideInInspector]
    public Pawn pawn;
    public bool isEnemy;

    public float maxHP, hp,defense,modifiedDefense;
    public float minDamage;

    public float damage;
    public float delayTime, time;
    public int Rating, UpgradeCount;

    public List<Vector2> detectRange_List;
    public List<Vector2> AOERange_List;//���� ���� ����
    public Vector2 AOEPos;//���� ���� ���� ��ġ(�߽���)

    public List<Unit> targets = new List<Unit>();
    public Unit attackingTarget;

    public bool isTargetDetected,isAttacking,isBuff;

    public AttackType attackType;

    public GameObject ProjectilePrefab;

    public SpriteRenderer mySprite;
    public Animator anim;

    [Header("HPBar ����")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset;

    

    //public bool flag;
    //public int i;

    protected virtual void first_Setting()
    {
        mySprite = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        pawn = GetComponent<Pawn>();

        SpawnHPBar();

       // Rating = 1;
        UpgradeCount = 1;


    }

    protected virtual void SpawnHPBar()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(HPBarPrefab, canvas.transform);
        go.name = $"{transform.name} HPBar";
        hPBar = go.GetComponent<HPBar>();

        HPBarOffset = new Vector3(0, -0.6f);
        hPBar.InitializeHPBar(this);
    }

    void Awake()
    {
        first_Setting();
    }
    protected virtual void Update()
    {
        Search_Targets();
       
        if (isTargetDetected)
        {
            if (time <= 0)
            {
                isAttacking = TryAttack();
            }
        }
        else isAttacking = false;
        time -= Time.deltaTime;

        CheckDefenseBuff();

        SyncHPBar();
    }

    //������� �ʴ� �ڵ�(Ȯ��޼����� GetTileInRange ����� ��)
    protected virtual List<Tile> GetTileInRange(int targetX, int targetY, List<Vector2> targetRange)
    {
        List<Tile> TileList = new();

        foreach (var targetTile in targetRange)
        {
            int x = targetX + (int)targetTile.x;
            int y = targetY + (int)targetTile.y;
            if (!TileManager.Instance.IsRightRange(x, y)) continue;

            var Tile = TileManager.Instance.TileArray[x, y];
            TileList.Add(Tile);

            //Debug.Log($"{x},{y}");
        }
        return TileList;
    }

    protected abstract void Search_Targets();
    protected abstract bool TryAttack();
    

    /// <summary>
    /// ���� ����
    /// </summary>
    public void Active_Attack() // Ÿ�� Ÿ�� ����, �ִϸ��̼� �̺�Ʈ Ű������
    {
        float damage = this.damage;
        if (isBuff)
        {
            //damage += 20;
        }
        GetClosestTarget(targets).TakeDamage(damage);
        time = delayTime;
        Debug.Log(gameObject.name);
    }
    /// <summary>
    /// ���� ����ü
    /// </summary> 
    public void Projectile_Attack()
    {
        // ����ü ������ ��ȯ
        GameObject bullet = Instantiate(ProjectilePrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        bullet.GetComponent<Projectile>().SetProjectile(damage, GetClosestTarget(targets).gameObject);//����ü ��ü���� ������ �� �ֵ��� �ٲٱ�
        time = delayTime;
        Debug.Log("Shot");
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    public void AOE_Attack()
    {
        List<Unit> attackTargets = new();

        AOEPos = new Vector2(this.GetClosestTarget(this.targets).pawn.X, this.GetClosestTarget(this.targets).pawn.Y);
        foreach (var _tile in AOERange_List.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.EnemyList.Count != 0)
            {
                attackTargets.AddRange(_tile.EnemyList);
            }
            //������ �ӽ� �ڵ�
            _tile.Do_AOE_Effect(Color.red);
        }

        foreach (var _enemy in attackTargets)
        {
            _enemy.TakeDamage(damage);
        }
        time = delayTime;
    }
    //0.05����
    public Unit GetClosestTarget(List<Unit> targets)
    {
        float minDistance = float.MaxValue;

        foreach (var _target in targets)
        {
            float distance = (transform.position - _target.transform.position).sqrMagnitude;
            
            if (attackingTarget != null)
            {
                if (minDistance > distance + 0.05f)
                {
                    minDistance = distance;
                    attackingTarget = _target;
                }
            }
            else
            {
                if (minDistance > distance)
                {
                    minDistance = distance;
                    attackingTarget = _target;
                }
            }
        }

        return attackingTarget;
    }

    public void TakeDamage(float damage)
    {
        float defense = this.modifiedDefense;
        if (isBuff)
        {
            //defense += 20;
        }
        damage -= defense;
        if (damage < minDamage) damage = minDamage;
        hp -= damage;
        if (hp <= 0) Die();
    }
    public void HealHP(float value)
    {
        hp += value;
        if (hp > maxHP) hp = maxHP;
    }
    public void TakeDefenseBuff(float value)
    {
       if (isBuff) return;
       
       isBuff = true;
       modifiedDefense = defense + value;
    }
    public void CheckDefenseBuff()
    {
        if (!isBuff) modifiedDefense = defense;
    }

    protected void SyncHPBar()//Buffer�� Debuffer���Ե� ����ǰ� �������ּ���
    {
        hPBar.curHP = hp;
    }

    public void Upgrade(float price)
    {
        // money -= /*������ ����*/ price;
        UpgradeCount++;
    }

    protected void Upgrade(float maxHP, float damage, float defense) // ���� ĳ������ Update���� UpgradeCount�� �°� �ɷ�ġ ���缭 ȣ��
    {
        this.maxHP = maxHP;
        this.damage = damage;
        this.defense = defense;
    }

    protected virtual void Die()
    {
        pawn.RemoveTilePawn();
        StartCoroutine(Destroy_this());
    }

    IEnumerator Destroy_this()
    {
        yield return new WaitForSeconds(0.03f);
        Destroy(gameObject);
    }


}
public enum AllyKind { Warrior, Sorcerer, Lancer, Tanker, Buffer, Archer, ITEM };
public enum EnemyKind { Blind, Eat, Head, Oppressed, Prayer };
public enum AttackType { None , Active, Projectile, AreaOfEffect, AOE_Melee };//����,����ü,����,���� ����

