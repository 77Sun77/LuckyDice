using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public abstract class Unit : MonoBehaviour
{
    [HideInInspector]
    public Pawn pawn;
    public bool isEnemy;

    public float maxHP, hp, defense;
    public float minDamage;

    public float damage;
    public float delayTime, time;
    public int Rating, UpgradeCount;

    public List<Vector2> detectRange_List;
    public List<Vector2> AOERange_List;//���� ���� ����
    public Vector2 AOEPos;//���� ���� ���� ��ġ(�߽���)

    public List<Unit> targets = new List<Unit>();

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

        Rating = 1;
        UpgradeCount = 1;


    }

    protected void SpawnHPBar()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject go = Instantiate(HPBarPrefab, canvas.transform);
        go.name = $"{transform.name} HPBar";
        hPBar = go.GetComponent<HPBar>();

        HPBarOffset = new Vector3(0, -0.6f);
        hPBar.InitializeHPBar(this);
    }

    void Start()
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

        SyncHPBar();
    }

    //���߿� Ȯ������ ����
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
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    public void AOE_Attack()
    {
        List<Unit> targets = new();

        AOEPos = new Vector2(this.GetClosestTarget(this.targets).pawn.X, this.GetClosestTarget(this.targets).pawn.Y);
        foreach (var _tile in GetTileInRange((int)AOEPos.x, (int)AOEPos.y, AOERange_List))
        {
            if (_tile.EnemyList.Count != 0)
            {
                targets.AddRange(_tile.EnemyList);
            }
            //������ �ӽ� �ڵ�
            
            StartCoroutine(Do_AOE_Effect(_tile,Color.red));
        }

        foreach (var _enemy in targets)
        {
            _enemy.TakeDamage(damage);
        }
        time = delayTime;
    }
    /// <summary>
    /// ������ �ӽ� �ڵ�
    /// </summary>
    /// <param name="tileSR"></param>
    /// <param name="originColor"></param>
    /// <returns></returns>
    protected IEnumerator Do_AOE_Effect(Tile _tile, Color effectColor)
    {
        SpriteRenderer tileSR = _tile.GetComponent<SpriteRenderer>();
        tileSR.color = effectColor;
        yield return new WaitForSeconds(0.3f);
        tileSR.color = _tile.originColor;
    }

    public Unit GetClosestTarget(List<Unit> targets)
    {
        Unit target = null;
        float minDistance = float.MaxValue;

        foreach (var _target in targets)
        {
            float distance = (transform.position - _target.transform.position).sqrMagnitude;
            if (minDistance > distance)
            {
                minDistance = distance;
                target = _target;
            }
        }

        return target;
    }

    public void TakeDamage(float damage)
    {
        float defense = this.defense;
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

    //public void EnableObj(GameObject original)
    //{
    //    enabled = false;
    //    GetComponent<SynthesisUnit>().unitKind = unitKind;
    //    GetComponent<SynthesisUnit>().Original = original;

    //    SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
    //    Color color = mySprite.color;
    //    color.a = 0.5f;
    //    mySprite.color = color;
    //}

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
        Destroy(gameObject);
    }

}
public enum AllyKind { Warrior, Sorcerer, Debuffer, Tanker, Buffer, Archer, ITEM };
public enum EnemyKind { Blind, Eat, Head, Oppressed, Prayer };
public enum AttackType { None , Active, Projectile, AreaOfEffect, AOE_Melee };//����,����ü,����,���� ����

