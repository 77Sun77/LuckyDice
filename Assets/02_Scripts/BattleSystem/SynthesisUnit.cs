using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisUnit : MonoBehaviour
{
    public Kind unitKind;
    public GameObject Original;
    public Unit target;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit"))
        {
            if (coll.gameObject == Original) return;
            Unit unit = (Unit)coll.GetComponent(typeof(Unit));
            Unit original = (Unit)Original.GetComponent(typeof(Unit));
            bool isRating = (target == null || Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, coll.transform.position)) && unit.Rating == original.Rating && original.unitKind == unit.unitKind;
            if (isRating)
            {
                target = (Unit)coll.GetComponent(typeof(Unit));   
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit") && target != null)
        {
            if (coll.gameObject == target.gameObject) target = null;
        }
    }
}
