using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;

public class nerfGun : iWeapon
{
    public GameObject bulletPrefab;
    [HideInInspector] public Transform firePoint;

    private float nextTimeToFire = 0f;

    private void Awake()
    {
        weaponTypeName = "nerf_gun";
        firePoint = transform.GetChild(0).gameObject.transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time >= nextTimeToFire)
        //{
        //    nextTimeToFire = Time.time + fireRate;
        //    Fire();
        //}

    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().bulletSpeed = damageSpeed;
        bullet.GetComponent<Bullet>().fireTag = transform.parent.tag;

    }

}
