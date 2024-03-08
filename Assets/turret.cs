using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public Transform turretTip;
    public Transform turretBase;
    public float timeBetweenShots;
    public float bulletPower;
    public GameObject targetedPlayer;
    public GameObject bullet;
    public Material blackColour;
    public Material disabledColour;
    public GameObject[] turretParts;

    private bool turretDisabled = false;
    private bool canShoot = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.gameObject.layer == 10)
            targetedPlayer = other.transform.root.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.gameObject == targetedPlayer)
            targetedPlayer = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (turretDisabled || collision.gameObject.layer == 14)
            return;

        StartCoroutine(disableTurret(6f));
    }

    private IEnumerator disableTurret(float s)
    {
        turretDisabled = true;
        foreach (GameObject part in turretParts)
            part.GetComponent<MeshRenderer>().material = disabledColour;

        yield return new WaitForSeconds(s);

        turretDisabled = false;
        foreach (GameObject part in turretParts)
            part.GetComponent<MeshRenderer>().material = blackColour;
    }

    private IEnumerator shoot()
    {
        canShoot = false;
        GameObject b = Instantiate(bullet, turretTip.position, turretBase.rotation);
        b.GetComponent<Rigidbody>().AddForce(b.transform.forward * bulletPower);
        yield return new WaitForSeconds(timeBetweenShots);
        Destroy(b);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedPlayer != null && !turretDisabled)
            turretBase.LookAt(targetedPlayer.transform.position);

        if (canShoot && targetedPlayer != null && !turretDisabled)
            StartCoroutine(shoot());
    }
}
