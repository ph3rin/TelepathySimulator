using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class __TestLockAspectRatio : MonoBehaviour
{
    private void Update()
    {
        AspectRatioLocker.LockAspectRatio(9, 16);
    }
}
