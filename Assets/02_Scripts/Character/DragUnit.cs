using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUnit : MonoBehaviour
{
    Unit target;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target != null) return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, 1);
            if (hit)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    GameObject hitObj = Instantiate(hit.collider.gameObject);
                    hitObj.AddComponent<SynthesisUnit>();
                    target = (Unit)hitObj.GetComponent(typeof(Unit));
                    target.EnableObj(hit.collider.gameObject);

                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(target != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.transform.position = mousePos;
            }
            

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                SynthesisUnit unit = target.GetComponent<SynthesisUnit>();
                if (unit.target != null)
                {
                    unit.target.Rating++;
                    Destroy(unit.Original);
                }
                Destroy(target.gameObject);

                target = null;
            }
            

        }

    }
}
