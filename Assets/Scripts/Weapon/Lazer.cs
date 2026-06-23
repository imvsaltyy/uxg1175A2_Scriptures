using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float lazerSpeed = 1f;
    public string fireTag = "";
    public float damageDelt;

    public float maxLength;
    public float increaseRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.up * lazerSpeed * Time.deltaTime);

        Vector3 scale = transform.localScale;
        
        if (scale.x < maxLength)
        {
            scale.x += increaseRate * Time.deltaTime;
            scale.x = Mathf.Min(scale.x, maxLength);
            transform.localScale = scale;
        }
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
