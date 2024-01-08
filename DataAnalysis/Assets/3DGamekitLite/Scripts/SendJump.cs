using Gamekit3D;
using Gamekit3D.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendJump : MonoBehaviour
{
    
    private SessionID sessionID;
    public GameObject playerr;
    private PlayerController playerController;
    private void Start()
    {
        sessionID = FindObjectOfType<SessionID>();
        playerController = playerr.GetComponent<PlayerController>();
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

    
}
