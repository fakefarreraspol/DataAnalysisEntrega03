using Gamekit3D;
using Gamekit3D.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyDead : MonoBehaviour
{

    private SessionID sessionID;
    public GameObject playerr;
    public static Action<string> OnEnemyDied;

    private List<ChomperBehavior> chompers;
    private List<SpitterBehaviour> spitter;
    private void Start()
    {
        sessionID = FindObjectOfType<SessionID>();
    }
    public void OnEnable()
    {
        OnEnemyDied += GetEnemyInfo;
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
    private void Update()
    {
        
    }
    private void GetEnemyInfo(string name)
    {
        // Create a JSON object with position information
        EnemigoMaloEpicoSigmaMaleDataLooksmaxingMewingLladosFitness damageData = new EnemigoMaloEpicoSigmaMaleDataLooksmaxingMewingLladosFitness()
        {
            PosX = playerr.transform.position.x,
            PosY = playerr.transform.position.y,
            PosZ = playerr.transform.position.z,
            SessionId = int.Parse(sessionID.lastSessionId),
            tableName = "EnemyDeath",
            eName = name
        };

        string jsonData = JsonUtility.ToJson(damageData);

        Debug.Log(jsonData);
        // Post JSON data to the server
        StartCoroutine(PostToServer("https://citmalumnes.upc.es/~polfo/EnemyDeadTable.php", jsonData));
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
