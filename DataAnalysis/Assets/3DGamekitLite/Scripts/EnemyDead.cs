using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyDead : MonoBehaviour, IMessageReceiver
{

    private SessionID sessionID;



    public List<Damageable> enemies = new List<Damageable>();

    private void Start()
    {
        sessionID = FindObjectOfType<SessionID>();

        Damageable[] enemiesInScene = FindObjectsOfType<Damageable>();

        for (int i = 0; i < enemiesInScene.Length; i++)
        {
            enemies.Add(enemiesInScene[i]);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].onDamageMessageReceivers.Add(this);
        }
    }
    public void OnEnable()
    {
        
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
        EnemigoMaloEpicoSigmaMaleDataLooksmaxingMewingLladosFitness damageData = new EnemigoMaloEpicoSigmaMaleDataLooksmaxingMewingLladosFitness()
        {
            PosX = senderDamageable.transform.position.x,
            PosY = senderDamageable.transform.position.y,
            PosZ = senderDamageable.transform.position.z,
            SessionId = int.Parse(sessionID.lastSessionId),
            tableName = "EnemyDeath",

        };

        string jsonData = JsonUtility.ToJson(damageData);

        // Post JSON data to the server
        StartCoroutine(PostToServer("https://citmalumnes.upc.es/~polfo/EnemyDeadTable.php", jsonData));
    }

    private bool IsValidNumericValue(float value)
    {
        // Add additional validation logic if needed
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }

}

class EnemigoMaloEpicoSigmaMaleDataLooksmaxingMewingLladosFitness
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public int SessionId;
    public string tableName;
    public string eName;
}


