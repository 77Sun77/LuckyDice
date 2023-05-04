using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFace : MonoBehaviour
{
    public bool IsContacting;
    public bool IsLookAtGround;

    public float RayDistance;

    private void Update()
    {
        Ray ray = new Ray(transform.position,transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit rayHit;
        Physics.Raycast(ray, out rayHit);

        if (rayHit.collider)
        {
            IsLookAtGround = rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground");
            RayDistance = rayHit.distance;
        }
        else RayDistance = 1000;

    }

    public void OnTriggerStay(Collider other)
    {
        IsContacting = other.gameObject.layer == LayerMask.NameToLayer("Ground");

    }
}
