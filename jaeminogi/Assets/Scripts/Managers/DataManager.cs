
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using System.Linq;

public interface ILoader<Key, value>
{
    Dictionary<Key, value> MakeDic(); //데이터 Dicitonary로 만듬
    bool Validate();  //유효성 검사
}

public class DataManager
{
  

    public void Init(Action onComplete)
    {
       
        onComplete?.Invoke();
    }
  




    void LoadSingleJson<Value>(string key, Action<Value> callback)
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            Value item = JsonUtility.FromJson<Value>(textAsset.text);
            callback.Invoke(item);
        });
    }
    void LoadJson<Loader, Key, Value>(string key, Action<Loader> callback) where Loader : ILoader<Key, Value>, new()
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            if (textAsset == null)
            {
                Debug.LogError($"Failed to load JSON: {key}");
                return;
            }


            try
            {
                // JsonUtility로 직접 로드
                Loader wrapper = JsonUtility.FromJson<Loader>(textAsset.text);
                if (wrapper == null)
                {
                    Debug.LogError($"JsonUtility failed to parse: {key}");
                    return;
                }

                callback?.Invoke(wrapper);
                Debug.Log($"Successfully loaded JSON: {key}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while parsing JSON from {key}: {ex.Message}");
            }
        });
    }

    void LoadSingleXml<Value>(string key, Action<Value> callback)
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            XmlSerializer xs = new XmlSerializer(typeof(Value));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            {
                callback?.Invoke((Value)xs.Deserialize(stream));
            }
        });
    }

    void LoadXml<Loader, Key, Value>(string key, Action<Loader> callback) where Loader : ILoader<Key, Value>, new()
    {
        Managers.Resource.LoadAsync<TextAsset>(key, (textAsset) =>
        {
            XmlSerializer xs = new XmlSerializer(typeof(Loader));
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
            {
                callback?.Invoke((Loader)xs.Deserialize(stream));
            }
        });
    }
}
