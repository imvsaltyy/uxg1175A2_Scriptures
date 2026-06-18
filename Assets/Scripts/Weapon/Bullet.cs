using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1f;
    public string fireTag = "";

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
        if (fireTag != collision.tag)
        {
            Debug.Log("Dealing Damage");
            Destroy(gameObject);

        }

    }
}
