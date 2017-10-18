using HoloToolkit.Unity;
using Microsoft.Toolkit.Uwp.UI;
using System;
using DemoServices.DataObjects;

public class TextCacheManager : Singleton<TextCacheManager>
{
    private InMemoryStorage<SpatialTextList> _cache;
    // Use this for initialization
    void Start()
    {
        _cache = new InMemoryStorage<SpatialTextList> {MaxItemCount = 20000};
    }

    public SpatialTextList GetItem(string requestUri)
    {
        if(_cache == null)
        {
            return null;
        }
        var result = _cache.GetItem(requestUri, TimeSpan.FromDays(5));
        return result != null ? result.Item : null;
    }

    public void SetItem(string requestUri, SpatialTextList result)
    {
        if (_cache == null)
        {
            return;
        }
        _cache.SetItem(new InMemoryStorageItem<SpatialTextList>(requestUri, DateTime.Now, result));
    }
}
