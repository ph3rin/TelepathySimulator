using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class FallingDownExplosionItem : FallingDownItem
{
    [SerializeField] private float m_explosionRadius;

    protected override void HandlePlayerCollison(FallingDownPlayer player)
    {
        foreach (var o in FindObjectsOfType<GameObject>())
        {
            if (o.CompareTag("Block") && Vector2.Distance(o.transform.position, player.transform.position) < m_explosionRadius)
            {
                Destroy(o);
            }
        }
    }
}
