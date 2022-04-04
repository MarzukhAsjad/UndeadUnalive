using System.Collections;
using UnityEngine;

public class GeneratePickups : MonoBehaviour
{
    public GameObject thePickup;
    public int xPos;
    public int zPos;
    public int pickupCount;

    private void Start()
    {
        StartCoroutine(pickupDrop());

    }

    IEnumerator pickupDrop()
    {
        while(pickupCount<10)
        {
            xPos = Random.Range(1, 4500);
            zPos = Random.Range(1, 3000);
            Instantiate(thePickup, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            pickupCount += 1;
        }
    }
}