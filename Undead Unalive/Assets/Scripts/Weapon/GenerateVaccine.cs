using System.Collections;
using UnityEngine;

public class GenerateVaccine : MonoBehaviour
{
    public GameObject theVaccine;
    public int xPos;
    public int zPos;
    public int vaccineCount;

    private void Start()
    {
        StartCoroutine(vaccineDrop());

    }

    IEnumerator vaccineDrop()
    {
        while(vaccineCount<10)
        {
            xPos = Random.Range(1, 4500);
            zPos = Random.Range(1, 3000);
            Instantiate(theVaccine, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            vaccineCount += 1;
        }
    }
}