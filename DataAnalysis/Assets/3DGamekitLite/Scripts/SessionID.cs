using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SessionID : MonoBehaviour
{
    private string serverUrl = "https://citmalumnes.upc.es/~polfo/session.php";
    public string lastSessionId;
    void Start()
    {
        StartCoroutine(FetchLastSessionId());
    }


    IEnumerator FetchLastSessionId()
    {
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            lastSessionId = request.downloadHandler.text;

            // Check if the session ID is "0" (indicating no session IDs found)
            if (lastSessionId == "0")
            {
                Debug.LogWarning("No session IDs found");
            }
            else
            {
                Debug.Log("Last Session ID: " + lastSessionId);
            }
        }
        else
        {
            Debug.LogError("Error fetching last session ID: " + request.error);
        }
    }
}
