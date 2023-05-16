using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomb : MonoBehaviour
{
    public int count;
    public float timer;

    List<Enemy> enemies = new List<Enemy>();
    void Start()
    {
        
    }

    void Update()
    {
        if (timer <= 0)
        {
            if (count > 0)
            {
                for (int index = enemies.Count - 1; index >= 0; index--)
                {
                    enemies[index].TakeDamageByBomb();
                }
                

            }
            else Destroy(gameObject);
            count--;
            timer = 1.5f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            enemies.Add(coll.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            enemies.Remove(coll.GetComponent<Enemy>());
        }
    }
}
