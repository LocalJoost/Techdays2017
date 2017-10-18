using System;
using System.Collections.Generic;
using DemoServices.DataObjects;
#if UNITY_UWP
using System.Net.Http;
using Microsoft.WindowsAzure.MobileServices;
#endif

using UnityEngine;

public class TextPlacer1 : MonoBehaviour
{
    public GameObject Container;

    public GameObject RemoteObjectTemplate;


    // Use this for initialization
    void Start()
    {

#if UNITY_UWP
        var client  = new MobileServiceClient(new Uri("http://demoservicestechdays20172.azurewebsites.net"));
        client.InvokeApiAsync<List<SpatialText>>(
                "SpatialText", HttpMethod.Get, null).ContinueWith(t => ProcessTexts(t.Result));
#endif
    }

    private void ProcessTexts(List<SpatialText> texts)
    {
        MainThreadExecuter.Instance.Add(() =>
        {
            for (int i = 0; i < texts.Count; i++)
            {
                var textData = texts[i];
                var text = Instantiate(RemoteObjectTemplate, Container.transform);
                var controller = text.GetComponent<TextController>();
                controller.SetText(textData.Text, textData.X, textData.Y, textData.Z);
            }
        });
    }
}
