using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Enemy : MonoBehaviour
//{
//    public new string name;
//    public float damage,maxHP,hp, defense, speed;
//    public float delayTime, time;
//    public int money;

//    public bool isAttack;
//    public List<Vector2> detectRange_List;
//    public List<Unit> UnitList = new();

//    private Unit unit;
//    public Pawn pawn;

//    public enum Debuff { None, Damage, Defense, Speed };
//    public Debuff debuff;

//    public float minDamage;

//    [Header("HPBar 관련")]
//    public GameObject HPBarPrefab;
//    protected HPBar hPBar;
//    public Vector3 HPBarOffset;

//    void Start()
//    {
//        SpawnHPBar();
//        pawn = GetComponent<Pawn>();
//    }

//    protected void SpawnHPBar()
//    {
//        GameObject canvas = GameObject.Find("Canvas");
//        GameObject go = Instantiate(HPBarPrefab, canvas.transform);
//        go.name = $"{transform.name} HPBar";
//        hPBar = go.GetComponent<HPBar>();

//        HPBarOffset = new Vector3(0, -0.6f);
//        hPBar.InitializeHPBar(this);
//    }

//    protected void Update()
//    {
//        Search_New();
//        Move();

//        if(isAttack)
//        {
//            if(time <= 0)
//            {
//                Attack();
//                time = delayTime;
//            }
//            time -= Time.deltaTime;
//        }
//    }

//    void Move()
//    {
//        if(!isAttack)
//        {
//            transform.Translate(Vector2.left * (speed*TileManager.Instance.XScale_Tile) * Time.deltaTime);
//        }
//    }

//    void Search()
//    {
//        int layerMask = 1 << LayerMask.NameToLayer("Unit");
//        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(0.5f, 0, 0), Vector2.left, 1.75f, layerMask); // distance == 칸 x
//        if (hit)
//        {
//            if (hit.collider.CompareTag("Unit") && unit == null) // && unit == null 추가
//            {
//                this.unit = hit.collider.GetComponent<Unit>(); // 타겟을 지정
//                // 애니메이션 발동
//                isAttack = true;
//            }
//        }
//        else
//        {
//            isAttack = false;
//            // 애니메이션 초기화
//            time = delayTime;
//        }
//    }

//    protected void Search_New()
//    {
//        UnitList.Clear();
//        //detectRange안의 Tile의 Unit 가져오기
//        foreach (var _detectRange in detectRange_List)
//        {
//            int x = pawn.X + (int)_detectRange.x;
//            int y = pawn.Y + (int)_detectRange.y;
//            if (!TileManager.Instance.IsRightRange(x, y)) continue;
//            var TileUnit = TileManager.Instance.TileArray[x, y].TileUnit;
//            if(TileUnit != null)
//            UnitList.Add(TileUnit);
//            //Debug.Log($"{x},{y}");
//        }

//        isAttack = UnitList.Count != 0 && pawn.IsOverCenter;
//    }

//    public void Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
//    {
//        foreach (var _unit in UnitList)
//        {
//            _unit.TakeDamage(damage);
//        }
//    }

//    public void TakeDamage(float damage)
//    {
//        damage -= defense;
//        if (damage < minDamage) damage = minDamage;
//        hp -= damage;
//        DIe();
//    }

//    public void TakeDamageByBomb()
//    {
//        hp -= 50;
//        DIe();
//    }

//    void DIe()
//    {
//        if (hp <= 0)
//        {
//            GameManager.instance.Set_Money(money);
//            pawn.RemoveTilePawn();
//            Destroy(gameObject);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D coll)
//    {
//        if (coll.CompareTag("Base"))
//        {
//            coll.gameObject.GetComponent<Base>().HP -= 1;
//            pawn.RemoveTilePawn();
//            Destroy(gameObject);
//        }
//    }
//}
public class Enemy : Unit
{
    public EnemyKind enemyKind;

    [Header("EnmeyOnly")]
    public int money;
    public float speed;
   
    protected override void first_Setting()
    {
        base.first_Setting();
        isEnemy = true;
    }

    protected override void Update()
    {
        base.Update();
        Move();
    }
    
    protected override void Search_Targets()
    {
        targets.Clear();

        foreach (var Tile in GetTileInRange(pawn.X, pawn.Y, detectRange_List))
        {
            if (Tile.TileUnit != null) targets.Add(Tile.TileUnit);
        }
        isTargetDetected = targets.Count != 0;
    }

    protected override bool TryAttack()//적을 공격
    {
        bool _canAttack = false;

        switch (attackType)
        {
            case AttackType.Active:
                if (pawn.IsOverCenter)
                {
                    Active_Attack();//적이 중앙을 넘어왔을때 근접 발동
                    _canAttack = true;
                }
                break;

            case AttackType.Projectile:
                Projectile_Attack();
                _canAttack = true;
                break;

            case AttackType.AreaOfEffect:
                AOE_Attack();
                _canAttack = true;
                break;

            case AttackType.AOE_Melee:
                if (pawn.IsOverCenter)
                {
                    AOE_Attack();
                    _canAttack = true;
                }
                break;
        }
        return _canAttack;
    }

    protected override void Die()
    {
        GameManager.instance.Set_Money(money);
        base.Die();
    }

    void Move()
    {
        if (!isAttacking)
        {
            transform.Translate(Vector2.left * (speed * TileManager.Instance.XScale_Tile) * Time.deltaTime);
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