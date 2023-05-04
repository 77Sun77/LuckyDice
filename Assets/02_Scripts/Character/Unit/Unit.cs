using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float damage, hp, defense;
    public Vector2 detectRange, attackRange;
    public float delayTime, time;
    public int Rating;

    protected List<Enemy> enemys = new List<Enemy>();

    public enum Kind { Warrior, Sorcerer, Debuffer, Tanker, Buffer, Archer };
    public Kind unitKind;

    public bool isAttack, isBuff;

    public enum AttackType { Active, Projectile };
    public AttackType attackType;

    public SpriteRenderer mySprite;
    public Animator anim;

    public GameObject ProjectilePrefab;
    protected void first_Setting()
    {
        mySprite = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
        if (detectRange.y > 1)
        {
            detectRange.y -= 0.1f;
        }
        if (attackRange.y > 1)
        {
            attackRange.y -= 0.1f;
        }
        Rating = 1;
    }
    void Start()
    {

    }
    protected void Update()
    {
        Search();

        if (isAttack)
        {
            if (time <= 0)
            {
                if (attackType == AttackType.Active) Active_Attack();
                else Projectile_Attack();
                time = delayTime;
            }
            time -= Time.deltaTime;
        }
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
            enemys.Clear();

            foreach (RaycastHit2D hitEnemy in hits)
            {
                enemys.Add((Enemy)hitEnemy.collider.GetComponent(typeof(Enemy)));
            }
        }
    }

    public void Active_Attack() // 타겟 타입 구분, 애니메이션 이벤트 키프레임
    {
        if (enemys.Count != 0)
        {
            foreach (Enemy enemy in enemys)
            {
                float damage = this.damage;
                if (isBuff)
                {
                    //damage += 20;
                }
                enemy.TakeDamage(damage);
            }
        }

    }
    public void Projectile_Attack()
    {
        // 투사체 프리팹 소환
        Instantiate(ProjectilePrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity).GetComponent<Projectile>().Set_Projectile(this, 3, damage);
    }

    public void TakeDamage(float damage)
    {
        float defense = this.defense;
        if (isBuff)
        {
            //defense += 20;
        }
        damage -= defense;
        hp -= damage;
        if (hp <= 0) Destroy(gameObject);
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

}
