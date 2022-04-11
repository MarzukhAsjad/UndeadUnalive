using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class GeneratePickups : MonoBehaviour
{
    public GameObject theVaccine;
    public GameObject theMask;
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
        int index = 0;
        foreach (Vector3 vector in spawnArray)
        {
           
            xPos = vector.x;
            zPos = vector.z;
            if (index % 2 == 0)
            {
                Instantiate(theVaccine, new Vector3(xPos, 1, zPos), Quaternion.identity);
            } else
            {
                Instantiate(theMask, new Vector3(xPos, 1, zPos), Quaternion.identity);

            }
            index += 1;
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

