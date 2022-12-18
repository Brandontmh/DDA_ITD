using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletOrigin;
    public GameObject bulletPrefab;

    public void Fire()
    {
        //Instantiate a bullet using the bulletPrefab
        // Use the position of the bulletOrigin as the start position of the new bullet
        // Use the rotation of the gun as the start rotation of the new bullet
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.position, transform.rotation);

        // Add a force to the bullet, in the same direction as the bulletOrigin
        newBullet.GetComponent<Rigidbody>().AddForce(bulletOrigin.forward * 50f);
    }
}
