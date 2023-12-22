using Gamekit3D;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using Gamekit3D.Message;
using static Gamekit3D.Damageable;

public class PhpSend : MonoBehaviour, IMessageReceiver
{
    [SerializeField]
    private Damageable damageable;

    private string lastId;
    private string lastSessionId;

    private void OnEnable()
    {
        damageable.onDamageMessageReceivers.Add(this);
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

    IEnumerator PostToServerSession(string url, string jsonData, Action callback)
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
                // Check if downloadHandler is not null before accessing text property
                if (www.downloadHandler != null)
                {
                    Debug.Log("Form upload complete! Response: " + www.downloadHandler.text);
                    // Handle the response as needed
                }
                else
                {
                    Debug.LogError("Download handler is null!");
                }
            }
        }
    }

    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        if (type == MessageType.DAMAGED)
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
                    PosZ = senderDamageable.transform.position.z
                };

                string jsonData = JsonUtility.ToJson(damageData);

                // Post JSON data to the server
                StartCoroutine(PostToServer("https://citmalumnes.upc.es/~juanam15/Damage.php", jsonData));
            }
        }
    }

    // Function to check if a value is a valid numeric value
    private bool IsValidNumericValue(float value)
    {
        // Add additional validation logic if needed
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}

public class PositionData
{
    public float PosX;
    public float PosY;  
    public float PosZ;  
}
