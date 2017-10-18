using System;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public void SetText(string text, float x, float y, float z, bool? fromCache = null)
    {
        var t = GetComponent<TextMesh>();
        t.text = text;
        transform.position = new Vector3(x,y,z);
        if (fromCache != null)
        {
            t.color = fromCache.Value ? Color.green : Color.red;
        }
    }
}
