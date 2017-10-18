using System.Collections;
using System.Collections.Generic;
using DemoServices.DataObjects;
using UnityEngine;

public class TextPlacer1 : MonoBehaviour
{
    public GameObject Container;

    public GameObject RemoteObjectTemplate;

    // Use this for initialization
    IEnumerator Start()
    {
        var headers = new Dictionary<string, string>
        {
            { "ZUMO-API-VERSION", "2.0.0" }
        };
        var downloader = new WWW("http://demoservicestechdays2017.azurewebsites.net/api/spatialtext", null, headers);
        yield return downloader;
        var textList = JsonUtility.FromJson<SpatialTextList>(downloader.text);
        for (int i = 0; i < textList.texts.Count; i++)
        {
            var textData = textList.texts[i];
            var text = Instantiate(RemoteObjectTemplate, Container.transform);
            var controller = text.GetComponent<TextController>();
            controller.SetText(textData.text, textData.x, textData.y, textData.z);
        }
    }
}
