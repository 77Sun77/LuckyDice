using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : Unit
{
    List<Unit> units = new List<Unit>();
    private void Start()
    {
        first_Setting();
    }
    void Update()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Unit");

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, detectRange, 0, Vector2.zero, 0, layerMask);

        if (hits.Length != 0)
        {
            List<Unit> units = new List<Unit>();
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != gameObject) units.Add((Unit)hit.collider.GetComponent(typeof(Unit)));
            }

            if(this.units.Count != 0)
            {
                foreach (Unit unit in this.units)
                {
                    if (!units.Contains(unit)) unit.isBuff = false;
                }
            }
            this.units.Clear();
            foreach (Unit unit in units)
            {
                unit.isBuff = true;
                this.units.Add(unit);
            }

        }
    }
}
