using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

internal sealed class AspectRatioLocker : MonoBehaviour
{
    public static AspectRatioLocker Instance {get; private set;}
    private void Awake()
    {
        Instance = this;
    }
    public static Rect GetMaxRenderRect(int widthRatio, int heightRatio, Rect bounds)
    {
        float unit = Mathf.Min(bounds.width / widthRatio, bounds.height / heightRatio);
        return new Rect(0, 0, Mathf.Floor(unit * widthRatio), Mathf.Floor(unit * heightRatio));
    }
    public static Rect AlignRectToMiddle(Rect target, Rect bounds)
    {
        var (tx, ty) = GetMid(target);
        var (bx, by) = GetMid(bounds);
        return new Rect(target.x + bx - tx, target.y + by - ty, target.width, target.height);
    }
    public static void LockAspectRatio(int widthRatio, int heightRatio)
    {
        var bounds = new Rect(0, 0, Screen.width, Screen.height);
        //print(AlignRectToMiddle(GetMaxRenderRect(widthRatio, heightRatio, bounds), bounds));
        Camera.main.pixelRect = AlignRectToMiddle(GetMaxRenderRect(widthRatio, heightRatio, bounds), bounds);
    }
    private static (float, float) GetMid(Rect rect)
    {
        return (rect.x + rect.width / 2, rect.y + rect.height / 2);
    }
}
