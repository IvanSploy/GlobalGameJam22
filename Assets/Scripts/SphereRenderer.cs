using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRenderer : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float gizmosDrawRange = 1f;
    public Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, gizmosDrawRange);
    }
}
