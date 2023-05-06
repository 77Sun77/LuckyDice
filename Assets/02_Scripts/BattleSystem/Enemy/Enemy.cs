using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string name;
    public float damage,maxhp,hp, defense, speed;
    public float delayTime, time;
    public float drop_Gold;

    public bool isAttack;

    Unit unit;

    public enum Debuff { None, Damage, Defense, Speed };
    public Debuff debuff;

    [Header("HPBar ����")]
    public GameObject HPBarPrefab;
    protected HPBar hPBar;
    public Vector3 HPBarOffset = new Vector3(0, -37);

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

        //hPBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + HPBarOffset;
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
        if (hp <= 0) Destroy(gameObject);
    }
    public void TakeDamage()
    {
        hp -= 50;
        if (hp <= 0) Destroy(gameObject);
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
