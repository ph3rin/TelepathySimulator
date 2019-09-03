using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class FallingDownCamera : MonoBehaviour
{
    private void Update()
    {
        if (FallingDownSession.Instance.State == FallingDownGameState.InGame)
        {
            var maxDepth = float.MaxValue;
            foreach (var player in FallingDownSession.Instance.EnumeratePlayers())
            {
                maxDepth = Mathf.Min(maxDepth, player.transform.position.y);
            }
            transform.position = new Vector3(transform.position.x, maxDepth, transform.position.z);
        }

    }
}
