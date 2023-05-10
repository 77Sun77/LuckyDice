using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float maxHP,hp, defense;
    public float minDamage;
    //public Vector2 detectRange, attackRange;

    public List<Vector2> detectRange_List;
    public List<Vector2> AOERange_List;//���� ���� ����
    public Vector2 AOEPos;//���� ���� ���� ��ġ(�߽���)

    public float damage;
    public float delayTime, time;
    public int Rating, UpgradeCount;

    /// <summary>
    /// Ÿ�Ͽ� �ִ� EnemyList���� List
    /// </summary>
    List<List<Enemy>> EnemyList_List = new();
    /// <summary>
    /// TileEnemyList���� ��
    /// </summary>
    protected List<Enemy> enemies = new List<Enemy>();

    public enum Kind { Warrior, Sorcerer, Debuffer, Tanker, Buffer, Archer, ITEM };
    public Kind unitKind;

    public bool isAttack, isBuff;

    public enum AttackType { Active, Projectile, AreaOfEffect, AOE_Melee};//����,����ü,����,���� ����
    public AttackType attackType;

    public SpriteRenderer mySprite;
    public Animator anim;

    public GameObject ProjectilePrefab;
    public Pawn pawn;
    
    [Header("HPBar ����")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset;

    protected void first_Setting()
    {
        mySprite = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        pawn = GetComponent<Pawn>();

        SpawnHPBar();
        //if (detectRange.y > 1)
        //{
        //    detectRange.y -= 0.1f;
        //}
        //if (attackRange.y > 1)
        //{
        //    attackRange.y -= 0.1f;
        //}
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
    protected void Update()
    {
        Search_New();

        if (isAttack && time <= 0)
        {
            switch (attackType)
            {
                case AttackType.Active:
                    if (GetClosestEnemy(enemies).pawn.IsOverCenter) Active_Attack();//���� �߾��� �Ѿ������ ���� �ߵ�
                    break;
                case AttackType.Projectile:
                    Projectile_Attack();
                    break;
                case AttackType.AreaOfEffect:
                    AOE_Attack();
                    break;
                case AttackType.AOE_Melee:
                    if (GetClosestEnemy(enemies).pawn.IsOverCenter) AOE_Attack();
                    break;
            }
        }
        time -= Time.deltaTime;

        SyncHPBar();
    }

    //protected void Search()
    //{
    //    int layerMask = 1 << LayerMask.NameToLayer("Enemy");
    //    Vector2 pos = transform.position;

    //    pos.x -= 0.875f; // ĭ x/2
    //    Vector2 range = detectRange; // ���ο� ����
    //    range.x += (detectRange.x * 0.75f)+1.25f; // ĭ x*ĭ �߰� ����(�÷��̾� : 1, ĭ : 1.75) + (ĭx-0.5)
    //    RaycastHit2D hit = Physics2D.BoxCast(pos, range, 0, Vector2.right, range.x / 2, layerMask); // detectRange -> range

    //    if (hit) isAttack = true;
    //    else
    //    {
    //        isAttack = false;
    //        time = delayTime;
    //    }

    //    range = attackRange; // ���ο� ����
    //    range.x += (attackRange.x * 0.75f)+1.25f; // ĭ x*ĭ �߰� ����(�÷��̾� : 1, ĭ : 1.75) + (ĭx-0.5)
    //    RaycastHit2D[] hits = Physics2D.BoxCastAll(pos, range, 0, Vector2.right, range.x / 2, layerMask); // attackRange -> range

    //    if (hits.Length != 0)
    //    {
    //        enemies.Clear();

    //        foreach (RaycastHit2D hitEnemy in hits)
    //        {
    //            enemies.Add(hitEnemy.collider.GetComponent<Enemy>());
    //        }
    //    }
    //}

    protected void Search_New()
    {
        enemies.Clear();
        EnemyList_List.Clear();
        //detectRange���� Tile�� EnemyList ��������
        foreach (var Tile in GetTileInRange(pawn.X,pawn.Y,detectRange_List))
        {
            EnemyList_List.Add(Tile.EnemyList);
        }
        
        //enemies�� ���� �߰�
        foreach (var EnemyList in EnemyList_List)
        {
            if (EnemyList.Count != 0)
            {
                enemies.AddRange(EnemyList);
            }
        }

        isAttack = enemies.Count != 0;
    }
    //���߿� Ȯ������ ����
    List<Tile> GetTileInRange(int targetX,int targetY,List<Vector2> targetRange)
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
        GetClosestEnemy(enemies).TakeDamage(damage);
        time = delayTime;
    }
    /// <summary>
    /// ���� ����ü
    /// </summary> 
    public void Projectile_Attack()
    {
        // ����ü ������ ��ȯ
        GameObject bullet = Instantiate(ProjectilePrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        bullet.GetComponent<Projectile>().SetTarget(GetClosestEnemy(enemies).gameObject);//����ü ��ü���� ������ �� �ֵ��� �ٲٱ�
        time = delayTime;
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    public void AOE_Attack()
    {
        List<Enemy> targets = new();

        AOEPos = new Vector2(GetClosestEnemy(enemies).pawn.X, GetClosestEnemy(enemies).pawn.Y);
        foreach (var _tile in GetTileInRange((int)AOEPos.x, (int)AOEPos.y, AOERange_List))
        {
            if (_tile.EnemyList.Count != 0)
            {
                targets.AddRange(_tile.EnemyList);
            }
            //������ �ӽ� �ڵ�
            var tileSR = _tile.GetComponent<SpriteRenderer>();
            Color OriginColor = tileSR.color;
            StartCoroutine(Do_AOE_Effect(tileSR, OriginColor));
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
    IEnumerator Do_AOE_Effect(SpriteRenderer tileSR, Color originColor)
    {
        tileSR.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        tileSR.color = originColor;
    }

    public Enemy GetClosestEnemy(List<Enemy> enemies)
    {
        Enemy targetEnemy = null;
        float minDistance = float.MaxValue;

        foreach (var _enemy in enemies)
        {
            float distance = (transform.position - _enemy.transform.position).sqrMagnitude;
            if (minDistance > distance)
            {
                minDistance = distance;
                targetEnemy = _enemy;
            }
        }

        return targetEnemy;
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
        if (unitKind != Kind.ITEM)
        {
            hp += value;
            if (hp > maxHP) hp = maxHP;
        } 
        
    }

    public void EnableObj(GameObject original)
    {
        enabled = false;
        GetComponent<SynthesisUnit>().unitKind = unitKind;
        GetComponent<SynthesisUnit>().Original = original;

        SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
        Color color = mySprite.color;
        color.a = 0.5f;
        mySprite.color = color;
    }

    void SyncHPBar()//Buffer�� Debuffer���Ե� ����ǰ� �������ּ���
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

    void Die()
    {
        pawn.RemoveTilePawn();
        Destroy(gameObject);
    }

}
