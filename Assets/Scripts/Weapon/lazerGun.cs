using UnityEngine;

public class lazerGun : iWeapon
{
    public GameObject lazerPrefab;
    [HideInInspector] public Transform firePoint;

    private void Awake()
    {
        weaponTypeName = "lazer_gun";
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
        
    }

    public void Fire(float damage)
    {
        GameObject lazer = Instantiate(lazerPrefab, firePoint.position, firePoint.rotation);
        lazer.GetComponent<Lazer>().lazerSpeed = damageSpeed;
        lazer.GetComponent<Lazer>().fireTag = transform.parent.tag;

        lazer.GetComponent<Lazer>().damageDelt = damage * dmgMultiplier;
        lazer.GetComponent<Lazer>().maxLength = raySize;
        lazer.GetComponent<Lazer>().increaseRate = increaseRate;

    }
}
