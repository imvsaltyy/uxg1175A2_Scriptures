using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;

public class nerfGun : iWeapon
{
    public GameObject bulletPrefab;
    [HideInInspector] public Transform firePoint;
    AudioManager audioManager;

    private void Awake()
    {
        weaponTypeName = "nerf_gun";
        firePoint = transform.GetChild(0).gameObject.transform;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
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

    //Takes in the attack/damage value based on who's firing the gun.
    public override void Fire(float damage)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().bulletSpeed = damageSpeed;
        bullet.GetComponent<Bullet>().fireTag = ownerTag;

        //Final Damage output is based on character's damage * weapon multiplier
        bullet.GetComponent<Bullet>().damageDelt = damage * dmgMultiplier;

        Debug.Log("Bullet fired by: " + ownerTag);

        audioManager.PlaySFX(audioManager.charactershoot);
    }

}
