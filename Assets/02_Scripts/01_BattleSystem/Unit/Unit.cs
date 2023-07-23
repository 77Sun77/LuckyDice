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
    
    public List<Unit> targets = new List<Unit>();
    public Unit attackingTarget;

    public bool isTargetDetected,isAttacking,isBuff;//isAttacking의 쓰임이 모호함 분석해볼것

    public SpriteRenderer mySprite;
    public Animator anim, shadowAnim;
    public GameObject shadow;

    public HPBar hPBar;
    


    //public AttackType attackType;

    //public GameObject ProjectilePrefab;

    //public List<Vector2> AOERange_List;//광역 공격 범위
    //public Vector2 AOEPos;//광역 공격 시전 위치(중심점)
    protected virtual void OnEnable()
    {
        first_Setting();
    }
    protected virtual void first_Setting()
    {
        mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        if(shadow) shadowAnim = shadow.GetComponent<Animator>();
        // anim = GetComponent<Animator>();
        pawn = GetComponent<Pawn>();

        SpawnHPBar();

        //Rating = 1;
        //UpgradeCount = 1;
        mySprite.sortingLayerName = "Grab";

    }

    public virtual void SpawnHPBar()
    {
        StartCoroutine(nameof(SpawnHPBar_Cor));
    }
    IEnumerator SpawnHPBar_Cor()
    {
        yield return UIManager.instance;
        if (!hPBar)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject go = Instantiate(UIManager.instance.HPBar_Prefab, canvas.transform);
            go.name = $"{transform.name} HPBar";
            hPBar = go.GetComponent<HPBar>();

            Vector3 HPBarOffset = new Vector3(0, -0.6f);
            hPBar.InitializeHPBar(this, HPBarOffset);
        }
            
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

        if (pawn.pastTile) mySprite.sortingLayerName = (pawn.pastTile.Y + 1) + "_Hierarchy";
        
    }

    protected abstract void Search_Targets();
    protected abstract bool TryAttack();
    

    /// <summary>
    /// 단일 근접
    /// </summary>
    public GameObject Active_Attack(float _damage) // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        GetClosestTarget(targets).TakeDamage(_damage, this.gameObject);
        time = delayTime;
        Debug.Log(gameObject.name);
        return GetClosestTarget(targets).gameObject;
    }
    /// <summary>
    /// 유도 투사체
    /// </summary> 
    public GameObject Projectile_Attack(float _damage, GameObject projectile)
    {
        // 투사체 프리팹 소환
        GameObject bullet = Instantiate(projectile, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
        bullet.GetComponent<Projectile>().SetProjectile(_damage, GetClosestTarget(targets).gameObject);//투사체 자체에서 설정할 수 있도록 바꾸기
        time = delayTime;
        Debug.Log("Shot");
        
        return bullet;
    }
    /// <summary>
    /// 광역 공격
    /// </summary>
    public virtual List<Unit> AOE_Attack(float _damage, Vector2 AOEPos, List<Vector2> aoeRange)
    {
        List<Unit> attackTargets = new();

        //Vector2 AOEPos = new Vector2(this.GetClosestTarget(this.targets).pawn.X, this.GetClosestTarget(this.targets).pawn.Y);
        foreach (var _tile in aoeRange.GetTileInRange((int)AOEPos.x, (int)AOEPos.y))
        {
            if (_tile.EnemyList.Count != 0)
            {
                attackTargets.AddRange(_tile.EnemyList);
            }
            //디버깅용 임시 코드
            _tile.Do_AOE_Effect(Color.red);
        }

        foreach (var _enemy in attackTargets)
        {
            _enemy.TakeDamage(_damage, this.gameObject);
        }
        time = delayTime;

        return attackTargets;
    }

    //0.05내외
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

    public virtual void TakeDamage(float _damage,GameObject atkTarget)
    {
        float defense = this.modifiedDefense;
      
        _damage -= defense;
        if (_damage < minDamage) _damage = minDamage;
        hp -= _damage;
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

    protected void SyncHPBar()
    {
        if (hPBar == null)
            return;

        hPBar.curHP = hp;
    }

    public void Upgrade(float price)
    {
        // money -= /*지정된 가격*/ price;
        UpgradeCount++;
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
public enum AttackType { None , Active, Projectile, AreaOfEffect, AOE_Melee };//근접,투사체,광역,광역 근접

