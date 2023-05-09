using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float damage,maxHP,hp, defense;
    public float minDamage;
    public Vector2 detectRange, attackRange;
    public List<Vector2> detectRange_List;

    public float delayTime, time;
    public int Rating, UpgradeCount;

    /// <summary>
    /// 타일에 있는 EnemyList들의 List
    /// </summary>
    List<List<Enemy>> EnemyList_List = new();
    /// <summary>
    /// TileEnemyList들의 합
    /// </summary>
    protected List<Enemy> enemies = new List<Enemy>();

    public enum Kind { Warrior, Sorcerer, Debuffer, Tanker, Buffer, Archer, ITEM };
    public Kind unitKind;

    public bool isAttack, isBuff;

    public enum AttackType { Active, Projectile, AreaOfEffect };//근접,투사체,광역
    public AttackType attackType;

    public SpriteRenderer mySprite;
    public Animator anim;

    public GameObject ProjectilePrefab;
    private Pawn pawn;
    
    [Header("HPBar 관련")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset;

    protected void first_Setting()
    {
        mySprite = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        pawn = GetComponent<Pawn>();

        SpawnHPBar();
        if (detectRange.y > 1)
        {
            detectRange.y -= 0.1f;
        }
        if (attackRange.y > 1)
        {
            attackRange.y -= 0.1f;
        }
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
            if (attackType == AttackType.Active && GetClosestEnemy(enemies).pawn.IsOverCenter) Active_Attack();//적이 타일 정중앙에 있을때 때리게 하기 위함
            else if (attackType == AttackType.Projectile) Projectile_Attack();
            else if (attackType == AttackType.AreaOfEffect) Debug.Log("광역기");
            time = delayTime;

            time -= Time.deltaTime;
        }
        SyncHPBar();
    }

    protected void Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Vector2 pos = transform.position;

        pos.x -= 0.875f; // 칸 x/2
        Vector2 range = detectRange; // 새로운 변수
        range.x += (detectRange.x * 0.75f)+1.25f; // 칸 x*칸 추가 범위(플레이어 : 1, 칸 : 1.75) + (칸x-0.5)
        RaycastHit2D hit = Physics2D.BoxCast(pos, range, 0, Vector2.right, range.x / 2, layerMask); // detectRange -> range

        if (hit) isAttack = true;
        else
        {
            isAttack = false;
            time = delayTime;
        }

        range = attackRange; // 새로운 변수
        range.x += (attackRange.x * 0.75f)+1.25f; // 칸 x*칸 추가 범위(플레이어 : 1, 칸 : 1.75) + (칸x-0.5)
        RaycastHit2D[] hits = Physics2D.BoxCastAll(pos, range, 0, Vector2.right, range.x / 2, layerMask); // attackRange -> range

        if (hits.Length != 0)
        {
            enemies.Clear();

            foreach (RaycastHit2D hitEnemy in hits)
            {
                enemies.Add(hitEnemy.collider.GetComponent<Enemy>());
            }
        }
    }

    protected void Search_New()
    {
        enemies.Clear();
        EnemyList_List.Clear();
        //detectRange안의 Tile의 EnemyList 가져오기
        foreach (var _detectRange in detectRange_List)
        {
            int x = pawn.X + (int)_detectRange.x;
            int y = pawn.Y + (int)_detectRange.y;
            if (!TileManager.Instance.IsRightRange(x, y)) continue;
            var EnemyList = TileManager.Instance.TileArray[x, y].EnemyList;
            EnemyList_List.Add(EnemyList);
            //Debug.Log($"{x},{y}");
        }
        //enemies에 전부 추가
        foreach (var EnemyList in EnemyList_List)
        {
            if (EnemyList.Count != 0)
            {
                enemies.AddRange(EnemyList);
            }
        }

        isAttack = enemies.Count != 0;
    }

    /// <summary>
    /// 단일 근접
    /// </summary>
    public void Active_Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        float damage = this.damage;
        if (isBuff)
        {
            //damage += 20;
        }
        
        GetClosestEnemy(enemies).TakeDamage(damage);
    }
    /// <summary>
    /// 유도 투사체
    /// </summary> 
    public void Projectile_Attack()
    {
        // 투사체 프리팹 소환
        Vector3 vec = GetClosestEnemy(enemies).transform.position - transform.position;
        //vec.x -= 1f;
        GameObject bullet = Instantiate(ProjectilePrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        //bullet.GetComponent<Projectile>().Set_Projectile(this, 3, damage);//투사체 자체에서 설정할 수 있도록 바꾸기
        bullet.transform.right = vec;
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

    void SyncHPBar()//Buffer랑 Debuffer에게도 적용되게 수정해주세용
    {
        hPBar.curHP = hp;
    }

    public void Upgrade(float price)
    {
        // money -= /*지정된 가격*/ price;
        UpgradeCount++;
    }

    protected void Upgrade(float maxHP, float damage, float defense) // 각각 캐릭터의 Update에서 UpgradeCount에 맞게 능력치 맞춰서 호출
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
