using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;

    private void Start()
    {
        Destroy(gameObject, 5);

    }

    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check to ensure it is not friendly fire
        if (fireTag != collision.tag)
        {
            Debug.Log("Dealing Damage");
            Destroy(gameObject);

            if (collision.tag == "Player")
            {
                //
                collision.gameObject.GetComponent<PlayerStats>().baseHP -= damageDelt;
            }

            else if (collision.tag == "Enemy")
            {
                collision.gameObject.GetComponent<spawnEnemy>().HP -= damageDelt;   
            }

        }

    }
}
