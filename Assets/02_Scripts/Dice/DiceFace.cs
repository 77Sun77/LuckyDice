using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFace : MonoBehaviour
{
    public bool IsContacting;

    public void OnTriggerStay(Collider other)
    {
        IsContacting = other.gameObject.layer == LayerMask.NameToLayer("Ground");

    }
}
