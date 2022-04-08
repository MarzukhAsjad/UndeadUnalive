using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class GeneratePickups : MonoBehaviour
{
    public GameObject thePickup;
    private List<Vector3>spawnArray = new List<Vector3>();
    private float xPos;
    private float zPos;
    public Transform ParentCube;

    private void Start()
    {
        StartCoroutine(pickupDrop());

    }

    IEnumerator pickupDrop()
    {
        CountChildren(ParentCube);

        foreach (Vector3 vector in spawnArray)
        {
           
            xPos = vector.x;
            zPos = vector.z;
            Instantiate(thePickup, new Vector3(xPos, 2, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
    void CountChildren(Transform a)
    {
        foreach (Transform b in a)
        {
            GameObject temp = b.gameObject;
            Vector3 bounds = temp.GetComponent<Renderer>().bounds.center;
            spawnArray.Add(bounds);
        }
    }
}

