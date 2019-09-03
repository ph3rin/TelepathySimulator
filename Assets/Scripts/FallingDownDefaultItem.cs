using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class FallingDownDefaultItem : FallingDownItem
{
    [SerializeField] private string m_modiferName;
    [SerializeField] private float m_modiferExpireTime, m_playerGrowth, m_difficultyGrowth;
    protected override void HandlePlayerCollison(FallingDownPlayer player)
    {
        player.AddModifier(m_modiferName, m_modiferExpireTime);
        player.Grow(m_playerGrowth);
        FallingDownSession.Instance.AddDifficulty(m_difficultyGrowth);
        //foreach (var o in FindObjectsOfType<GameObject>())
        //{
        //    if (o.CompareTag("Block") && Vector2.Distance(o.transform.position, player.transform.position) < 2.5f)
        //    {
        //        //Instantiate(o, o.transform.position, Quaternion.identity).AddComponent<ShatteredBlockBehaviour>()
        //        //    .transform.SetParent(null);
        //        Destroy(o);
        //    }
        //}
    }

}
