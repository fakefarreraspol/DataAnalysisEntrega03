using Gamekit3D;
using Gamekit3D.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendDeath : MonoBehaviour, IMessageReceiver
{
    [SerializeField] private Damageable damageable;
    private SessionID sessionID;
    public GameObject playerr;
    private void Start()
    {
        sessionID = FindObjectOfType<SessionID>();
    }
    private void OnEnable()
    {
        damageable.onDamageMessageReceivers.Add(this);
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

    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        Debug.Log("message");
        if (type == MessageType.DEAD)
        {
            Debug.Log("HOLA");
            

            RetrievePlayerData();
            
        }
    }

    public void RetrievePlayerData()
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
        StartCoroutine(PostToServer("https://citmalumnes.upc.es/~polfo/Death.php", jsonData));
    }

}
