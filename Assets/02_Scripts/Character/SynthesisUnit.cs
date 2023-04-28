using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisUnit : MonoBehaviour
{
    public Unit.Kind unitKind;
    public GameObject Original;
    public Unit target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit"))
        {
            if (coll.gameObject == Original) return;
            
            if(target == null || Vector2.Distance(transform.position, target.transform.position) > Vector2.Distance(transform.position, coll.transform.position))
            {
                target = (Unit)coll.GetComponent(typeof(Unit));
                
            }

        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Unit"))
        {
            if (coll.gameObject == target.gameObject) target = null;
        }
    }
}
