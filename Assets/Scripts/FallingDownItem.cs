using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


internal enum FallingDownItemState
{
    Idle, Obtained
}
[RequireComponent(typeof(CircleCollider2D))]
internal abstract class FallingDownItem : MonoBehaviour
{
    private FallingDownItemState m_state = FallingDownItemState.Idle;
    private float m_currentStateStart;
    protected abstract void HandlePlayerCollison(FallingDownPlayer player);
    private void OnCollisionEnter2D(Collision2D collison)
    {
        if (m_state == FallingDownItemState.Idle)
        {
            m_state = FallingDownItemState.Obtained;
            GetComponent<Collider2D>().enabled = false;
            var player = collison.gameObject.GetComponent<FallingDownPlayer>();
            var animatior = GetComponent<Animator>();
            animatior.SetBool("isObtained", true);

            Debug.Assert(player != null);
            HandlePlayerCollison(player);
        }
    }
}