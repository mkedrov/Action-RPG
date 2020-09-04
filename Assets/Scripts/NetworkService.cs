using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkService
{
    // this script was edited to receive weather data in JSON format instead on XML in original version
    
    //private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml&APPID=b85d41ca7c53dadf9f014bd922a63e1c"; // URL for the request sending
    
    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Moscow,us&APPID=b85d41ca7c53dadf9f014bd922a63e1c"; // A request for weather in Moscow, Russia
    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";
    
    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.Send(); // pause for download
            
            if (request.isNetworkError)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                callback(request.downloadHandler.text); // delegate call
            }
        }
    }
    
    public IEnumerator GetWeatherJSON(Action<string> callback) // the function RETURNS IEnumerator!
    {
        return CallAPI(jsonApi, callback);
    }
    
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.Send();
        callback(DownloadHandlerTexture.GetContent(request));
    }
}
