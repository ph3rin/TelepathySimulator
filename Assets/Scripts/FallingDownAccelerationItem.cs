using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class FallingDownAccelerationItem : FallingDownItem
{
    protected override void HandlePlayerCollison(FallingDownPlayer player)
    {
        player.AddModifier("Acceleration", 10f);
    }
}
