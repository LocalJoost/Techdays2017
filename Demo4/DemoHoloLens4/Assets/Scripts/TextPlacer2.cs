using System.Collections;
using System.Collections.Generic;
using DemoServices.DataObjects;
using UnityEngine;

public class TextPlacer2 : MonoBehaviour
{
    public GameObject Container;

    public GameObject RemoteObjectTemplate;

    private bool _hasAquiredData;

    private List<GameObject> _createdTexts;

    private SpatialTextList _downloadedTexts;

    private string _postfix = "horizontal";

    // Use this for initialization
    IEnumerator Start()
    {
        _createdTexts = new List<GameObject>();
        yield return StartGetText();
    }

    void Update()
    {
        CheckDrawText();
    }

    private IEnumerator StartGetText()
    {
        var url = string.Format("http://demoservicestechdays20174.azurewebsites.net/api/spatialtexts/{0}", _postfix);
        _downloadedTexts = GetFromCache(url);
        if (_downloadedTexts != null)
        {
            yield return null;
            _hasAquiredData = true;
        }
        else
        {
            yield return GetFromWeb(url);
        }
    }

    private IEnumerator GetFromWeb(string url)
    {
        var headers = new Dictionary<string, string>
        {
            { "ZUMO-API-VERSION", "2.0.0" }
        };
        var downloader = new WWW(url, null, headers);
        yield return downloader;
        _downloadedTexts = JsonUtility.FromJson<SpatialTextList>(downloader.text);
        TextCacheManager.Instance.SetItem(downloader.url, _downloadedTexts);
        _downloadedTexts.FromCache = false;
        _hasAquiredData = true;
        Debug.Log("Retrieved from web");
    }

    private SpatialTextList GetFromCache(string url)
    {
        var downloadedTexts = TextCacheManager.Instance.GetItem(url);
        if (downloadedTexts != null)
        {
            Debug.Log("Retrieved from cache");
            downloadedTexts.FromCache = true;
            return downloadedTexts;
        }
        return null;
    }


    private void CheckDrawText()
    {
        if (_hasAquiredData)
        {
            foreach (var text in _createdTexts)
            {
                Destroy(text);
            }
            _createdTexts.Clear();

            for (int i = 0; i < _downloadedTexts.texts.Count; i++)
            {
                var textData = _downloadedTexts.texts[i];
                var text = Instantiate(RemoteObjectTemplate, Container.transform);
                var controller = text.GetComponent<TextController>();
                controller.SetText(textData.text, textData.x, textData.y, textData.z, _downloadedTexts.FromCache);
                _createdTexts.Add(text);
            }
            _downloadedTexts = null;

            _hasAquiredData = false;

            StartCoroutine(WaitAndAndReaload());

        }
    }

    IEnumerator WaitAndAndReaload()
    {
        yield return new WaitForSeconds(2.0f);
        _postfix = _postfix == "horizontal" ? "vertical" : "horizontal";
        yield return StartGetText();
    }
}
