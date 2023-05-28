using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Item
{
    public override void Attack()
    {
        
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            int layerMask = (1 << LayerMask.NameToLayer("Unit"));
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, Vector2.zero, 0, layerMask);
            if (hit)
            {
                hit.collider.GetComponent<Ally>().isMove = true;
                Destroy(gameObject);
            }
        }
        
    }

}
