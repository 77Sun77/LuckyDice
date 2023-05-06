using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragUnit : MonoBehaviour
{
    bool isDrag;
    float timer;
    Vector2 mousePos;
    Unit target;
    int layerMask;
    void Start()
    {
        timer = 0.5f;
        layerMask = 1 << LayerMask.NameToLayer("Unit");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target != null) return;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.forward, 1, layerMask);
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

                if (isDrag)
                {
                    target.transform.position = mousePos;
                    return;
                }
                else if (target != null)
                {
                    if (Vector2.Distance(this.mousePos, mousePos) > 0.5f)
                    {
                        target.transform.position = mousePos;
                        isDrag = true;

                    }
                }


                if (!isDrag && Vector2.Distance(mousePos, target.transform.position) > 0.5f) timer = 0.5f;
                if (timer <= 0)
                {
                    //target.transform.position = mousePos;
                    isDrag = true;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
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

                timer = 0.5f;

                isDrag = false;
                target = null;
            }

        }

    }
}
