using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowVaccine : MonoBehaviour
{
    public GameObject vaccine;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, timeBetweenShots;

    public int bulletsPerTap = 1;

    public bool allowButtonHold;

    int bulletsLeft;

    bool shooting, readyToShoot;

    public Camera fpsCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;

    public bool allowInvoke = true;

    private void Awake()
    {
        
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        bulletsLeft = ScoringSystem.vaccineCount;
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(readyToShoot && shooting && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        GameObject currentBullet = Instantiate(vaccine, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithoutSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        ScoringSystem.vaccineCount-=1;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
