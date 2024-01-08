using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendAttack : MonoBehaviour
{

    private SessionID sessionID;
    public GameObject playerr;
    public static Action OnPlayerAttacked;
    private void Start()
    {
        sessionID = FindObjectOfType<SessionID>();
    }
    private void OnEnable()
    {
        OnPlayerAttacked += RetrievePlayerData;
    }

    // Start is called before the first frame update
    IEnumerator PostToServer(string url, string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", jsonData);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete! Response: " + www.downloadHandler.text);
                // Handle the response as needed
            }
        }
    }

    private void RetrievePlayerData()
    {
        // Create a JSON object with position information
        PositionData damageData = new PositionData()
        {
            PosX = playerr.transform.position.x,
            PosY = playerr.transform.position.y,
            PosZ = playerr.transform.position.z,
            SessionId = int.Parse(sessionID.lastSessionId)
        };

        string jsonData = JsonUtility.ToJson(damageData);

        Debug.Log(jsonData);
        // Post JSON data to the server
        StartCoroutine(PostToServer("https://citmalumnes.upc.es/~polfo/Attack.php", jsonData));
    }


}
