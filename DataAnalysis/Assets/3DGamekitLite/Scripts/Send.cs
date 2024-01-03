using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Send : MonoBehaviour
{
    private GameObject player;
    private SessionID IDsession; 
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SendPositionData", 3, 3);
    }

    private void SendPositionData()
    {
        // Create a JSON object with position information
        PositionData damageData = new PositionData()
        {
            PosX = player.transform.position.x,
            PosY = player.transform.position.y,
            PosZ = player.transform.position.z,
            SessionId = int.Parse(IDsession.lastSessionId)
        };

        string jsonData = JsonUtility.ToJson(damageData);

        // Post JSON data to the server
        StartCoroutine(PostToServer("", jsonData)); ///////////////////////falta url
    }


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
}
