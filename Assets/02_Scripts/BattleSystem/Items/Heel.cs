using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heel : MonoBehaviour
{
    public int count;
    public float timer;

    List<Unit> units = new List<Unit>();
    void Start()
    {
        
    }

    void Update()
    {
        if(timer <= 0)
        {
            if (count > 0)
            {
                HeelUnits(100);

            }
            else if (count > -3)
            {
                HeelUnits(30);
            }
            else Destroy(gameObject);
            count--;
            timer = 1;
            
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
    void HeelUnits(float value)
    {
        foreach (Unit unit in units)
        {
            unit.HeelHP(value);
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit"))
        {
            units.Add(coll.GetComponent<Unit>());
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit"))
        {
            units.Remove(coll.GetComponent<Unit>());
        }
    }
}
