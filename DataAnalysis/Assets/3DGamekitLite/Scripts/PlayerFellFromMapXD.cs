using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFellFromMapXD : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "DeathVolume")
        {
            FindObjectOfType<SendDeath>().RetrievePlayerData();
        }
    }
}
