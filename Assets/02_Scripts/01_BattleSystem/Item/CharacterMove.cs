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
            int layerMask = (1 << LayerMask.NameToLayer("Tile"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0, layerMask);
            if (hit && hit.collider.GetComponent<Tile>().Ally)
            {
                hit.collider.GetComponent<Tile>().Ally.GetComponent<Ally>().isMove = true;
                Destroy(gameObject);
            }
            else Destroy(gameObject);
        }
        
    }

}
