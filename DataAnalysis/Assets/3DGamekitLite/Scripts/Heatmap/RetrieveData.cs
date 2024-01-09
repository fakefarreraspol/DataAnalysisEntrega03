using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveData : MonoBehaviour
{
    List<Vector3> damagedPositionsList = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetDamagedData());
    }

    IEnumerator GetDamagedData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~polfo/GetDamage.php");

        yield return www;

        // Debug: Print the received text
        Debug.Log("Received data: " + www.text);

        string[] damagedData = www.text.Split("<br>");
        Vector3Int[] damagedDataInt = new Vector3Int[damagedData.Length - 3];

        for (int i = 2; i < damagedData.Length - 1; i++)
        {
            // Debug: Print each part before parsing
            Debug.Log("Parsing part: " + damagedData[i]);

            string[] parts = damagedData[i].Split(" ");
            
            string xPart = parts[0];
            string yPart = parts[1];
            string zPart = parts[2];

            // Extract integer part by splitting at the decimal point
            int xInt = int.Parse(xPart.Split('.')[0]);
            int yInt = int.Parse(yPart.Split('.')[0]);
            int zInt = int.Parse(zPart.Split('.')[0]);

            Debug.Log("hola");
            Vector3Int vector = new Vector3Int(xInt, yInt, zInt);
            damagedDataInt[i - 2] = vector;
        }

        // Add the parsed vectors to your damagedPositionsList
        for (int i = 0; i < damagedDataInt.Length; i++)
        {
            damagedPositionsList.Add(damagedDataInt[i]);
        }
        for(int i = 0;i < damagedPositionsList.Count;i++)
        {
            Debug.Log(i.ToString()+ " : "+ damagedPositionsList[i]);
        }
    }
}
