using Gamekit3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFellFromMapXD : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<DeathVolume>())
        {
            FindObjectOfType<SendDeath>().RetrievePlayerData();
        }
    }
}
