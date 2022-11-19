using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static Vector3 LimitPosition(Vector3 position) {
        Vector3 pos = Camera.main.WorldToViewportPoint(position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        return Camera.main.ViewportToWorldPoint(pos);
    }
}
