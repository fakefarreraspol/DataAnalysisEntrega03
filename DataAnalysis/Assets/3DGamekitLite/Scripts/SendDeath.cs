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
        if (type == MessageType.DEAD)
        {
            if (msg is Damageable.DamageMessage)
            {
                Damageable.DamageMessage damageMessage = (Damageable.DamageMessage)msg;

                Damageable senderDamageable = (Damageable)sender;

                // Check for valid numeric values before sending
                if (!IsValidNumericValue((int)senderDamageable.transform.position.x) ||
                    !IsValidNumericValue((int)senderDamageable.transform.position.y) ||
                    !IsValidNumericValue((int)senderDamageable.transform.position.z))
                {
                    Debug.LogError("Invalid position values!");
                    return;
                }

                // Create a JSON object with position information
                PositionData damageData = new PositionData()
                {
                    PosX = senderDamageable.transform.position.x,
                    PosY = senderDamageable.transform.position.y,
                    PosZ = senderDamageable.transform.position.z,
                    SessionId = int.Parse(sessionID.lastSessionId)
                };

                string jsonData = JsonUtility.ToJson(damageData);

                // Post JSON data to the server
                StartCoroutine(PostToServer("https://citmalumnes.upc.es/~polfo/Death.php", jsonData));
            }
        }
    }
    private bool IsValidNumericValue(float value)
    {
        // Add additional validation logic if needed
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}
