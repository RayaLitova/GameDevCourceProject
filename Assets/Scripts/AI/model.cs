using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;

public class GoogleAIManager : MonoBehaviour
{
    private static string apiKey = null;
    private static string model = "gemini-1.5-flash";
    private static string baseUrl = "https://generativelanguage.googleapis.com/v1beta/models/";
    void Awake()
    {
        GetApiKeyFromFile();
    }

    private static void GetApiKeyFromFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "google_api_key.txt");
        if (File.Exists(filePath))
            apiKey = File.ReadAllText(filePath).Trim();
        else
            Debug.LogError("API Key file not found at: " + filePath);
    }

    public void SendRequest(string message)
    {
        if (string.IsNullOrEmpty(message)) 
            return;

        StartCoroutine(SendToGemini(ProcessInput(message)));
    }
    
    private IEnumerator SendToGemini(string message)
    {
        var request = new GeminiRequest
        {
            contents = new Content[]
            {
                new Content
                {
                    parts = new Part[]
                    {
                        new Part { text = message }
                    }
                }
            }
        };
        
        string jsonRequest = JsonUtility.ToJson(request);
        string url = $"{baseUrl}{model}:generateContent?key={apiKey}";
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string responseJson = webRequest.downloadHandler.text;
                ProcessResponse(responseJson);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}");
            }
        }
    }
    
    private void ProcessResponse(string jsonResponse)
    {
        try
        {
            GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(jsonResponse);
            
            if (response.candidates != null && response.candidates.Length > 0)
            {
                string aiResponse = response.candidates[0].content.parts[0].text;
                OnAIResponse(aiResponse);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse response: " + e.Message);
            Debug.LogError("Raw response: " + jsonResponse);
        }
    }
    
    protected virtual string ProcessInput(string input)
    {
        return input;
    }

    protected virtual void OnAIResponse(string response)
    {
        Debug.Log("Response: " + response);
    }
}

[System.Serializable]
public class GeminiRequest
{
    public Content[] contents;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}

[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}