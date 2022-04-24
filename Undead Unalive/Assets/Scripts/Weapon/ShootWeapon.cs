using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public GameObject vaccine;

    public GameObject grenade;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, timeBetweenShots;

    public int bulletsPerTap = 1;

    public bool allowButtonHold;

    int vaccinesLeft, grenadesLeft;

    bool shootingVaccine, shootingGrenade, readyToShoot;

    public Camera fpsCam;
    public Transform attackPoint;

    public GameObject muzzleFlash;

    public bool allowInvoke = true;
    public AudioSource gunShot;

    private void Awake()
    {
        readyToShoot = true;
        gunShot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // disable shoot when game paused
        if (GameManager.Instance.isPaused) return;

        vaccinesLeft = ScoringSystem.vaccineCount;
        grenadesLeft = ScoringSystem.grenadeCount;
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) 
        { 
            shootingVaccine = Input.GetKey(KeyCode.Mouse0); 
            shootingGrenade = Input.GetKey(KeyCode.G);
        }
        else 
        {
            shootingVaccine = Input.GetKeyDown(KeyCode.Mouse0);
            shootingGrenade = Input.GetKeyDown(KeyCode.G);
        }

        if (readyToShoot && shootingVaccine && vaccinesLeft > 0)
        {
            Shoot("Vaccine");
            gunShot.Play();
        }

        if (readyToShoot && shootingGrenade && grenadesLeft > 0)
        {
            Shoot("Grenade");
        }
    }

    private void Shoot(string weapon)
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        if (weapon == "Vaccine")
        {
            GameObject currentBullet = Instantiate(vaccine, attackPoint.position, Quaternion.identity);

            currentBullet.transform.forward = directionWithoutSpread.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

            ScoringSystem.vaccineCount -= 1;
        }

        if(weapon=="Grenade")
        {
            GameObject currentBullet = Instantiate(grenade, attackPoint.position, Quaternion.identity);

            currentBullet.transform.forward = directionWithoutSpread.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

            ScoringSystem.grenadeCount -= 1;
        }

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

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