using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string name;
    public float damage,maxHP,hp, defense, speed;
    public float delayTime, time;
    public float drop_Gold;

    public bool isAttack;
    public int money;
    Unit unit;

    public enum Debuff { None, Damage, Defense, Speed };
    public Debuff debuff;

    [Header("HPBar ����")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset;

    void Start()
    {
        SpawnHPBar();
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
        Search();
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
            transform.Translate(Vector2.left * (speed/15) * Time.deltaTime);
        }
    }

    void Search()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");
        RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(0.5f, 0, 0), Vector2.left, 1.75f, layerMask); // distance == ĭ x
        if (hit)
        {
            if (hit.collider.CompareTag("Unit") && unit == null) // && unit == null �߰�
            {
                this.unit = hit.collider.GetComponent<Unit>(); // Ÿ���� ����
                // �ִϸ��̼� �ߵ�
                isAttack = true;
            }
        }
        else
        {
            isAttack = false;
            // �ִϸ��̼� �ʱ�ȭ
            time = delayTime;
        }
    }

    public void Attack() // Ÿ�� Ÿ�� ����, �ִϸ��̼� �̺�Ʈ Ű������
    {
        unit.TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        damage -= defense;
        if (damage < 5) damage = 5;
        hp -= damage;
        DIe();
    }
    public void TakeDamage()
    {
        hp -= 50;
        DIe();
    }
    void DIe()
    {
        if (hp <= 0)
        {
            GameManager.instance.Set_Money(money);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Base"))
        {
            coll.gameObject.GetComponent<Base>().HP -= 1;
            Destroy(gameObject);
        }
    }
}
