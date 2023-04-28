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
                    target.EnableObj();

                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(target != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, 1);

                target.transform.position = mousePos;
            }
            

        }
        if (Input.GetMouseButtonUp(0))
        {
            if (target != null)
            {
                Destroy(target.gameObject);
                target = null;
            }
            

        }

    }
}
