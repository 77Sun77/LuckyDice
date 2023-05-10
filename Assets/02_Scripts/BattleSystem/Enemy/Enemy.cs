using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public new string name;
    public float damage,maxHP,hp, defense, speed;
    public float delayTime, time;
    public int money;

    public bool isAttack;
    public List<Vector2> detectRange_List;
    public List<Unit> UnitList = new();
    
    private Unit unit;
    public Pawn pawn;

    public enum Debuff { None, Damage, Defense, Speed };
    public Debuff debuff;

    public float minDamage;

    [Header("HPBar 관련")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset;

    void Start()
    {
        SpawnHPBar();
        pawn = GetComponent<Pawn>();
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

    protected void Update()
    {
        Search_New();
        Move();

        if(isAttack)
        {
            if(time <= 0)
            {
                Attack();
                time = delayTime;
            }
            time -= Time.deltaTime;
        }
    }

    void Move()
    {
        if(!isAttack)
        {
            transform.Translate(Vector2.left * (speed*TileManager.Instance.XScale_Tile) * Time.deltaTime);
        }
    }

    void Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(0.5f, 0, 0), Vector2.left, 1.75f, layerMask); // distance == 칸 x
        if (hit)
        {
            if (hit.collider.CompareTag("Unit") && unit == null) // && unit == null 추가
            {
                this.unit = hit.collider.GetComponent<Unit>(); // 타겟을 지정
                // 애니메이션 발동
                isAttack = true;
            }
        }
        else
        {
            isAttack = false;
            // 애니메이션 초기화
            time = delayTime;
        }
    }

    protected void Search_New()
    {
        UnitList.Clear();
        //detectRange안의 Tile의 Unit 가져오기
        foreach (var _detectRange in detectRange_List)
        {
            int x = pawn.X + (int)_detectRange.x;
            int y = pawn.Y + (int)_detectRange.y;
            if (!TileManager.Instance.IsRightRange(x, y)) continue;
            var TileUnit = TileManager.Instance.TileArray[x, y].TileUnit;
            if(TileUnit != null)
            UnitList.Add(TileUnit);
            //Debug.Log($"{x},{y}");
        }
        
        isAttack = UnitList.Count != 0 && pawn.IsOverCenter;
    }

    public void Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        foreach (var _unit in UnitList)
        {
            _unit.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        damage -= defense;
        if (damage < minDamage) damage = minDamage;
        hp -= damage;
        DIe();
    }

    public void TakeDamageByBomb()
    {
        hp -= 50;
        DIe();
    }

    void DIe()
    {
        if (hp <= 0)
        {
            GameManager.instance.Set_Money(money);
            pawn.RemoveTilePawn();
            Destroy(gameObject);
        }
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
