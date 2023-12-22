using Gamekit3D;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PhpSend : MonoBehaviour
{
    //private Damageable damageable;
    private void OnEnable()
    {
        //damageable = GetComponent<Damageable>();

        //// Subscribe to the damageable's event without modifying the Damageable script
        //damageable.OnReceiveDamage.AddListener(OnReceiveDamageHandler);
        // Subscribe to the OnReceiveDamage event using SendMessage
        gameObject.SendMessage("OnReceiveDamageSubscribe", this, SendMessageOptions.RequireReceiver);
    }


    private void OnDisable()
    {
        // Unsubscribe from the OnReceiveDamage event using SendMessage
        gameObject.SendMessage("OnReceiveDamageUnsubscribe", this, SendMessageOptions.RequireReceiver);
    }
    
    private void OnPlayerReceiveDamage(string posX, string posY, string posZ)
    {
        StartCoroutine(PostToServer("https://citmalumnes.upc.es/~juanam15/Damage.php", posX, posY, posZ));
    }

    IEnumerator PostToServer(string url, string posX, string posY, string posZ)
    {
        // Create the WWWForm and add fields
        WWWForm form = new WWWForm();
        form.AddField("posX", posX);
        form.AddField("posY", posY);
        form.AddField("posZ", posZ);

        // Send the UnityWebRequest
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
