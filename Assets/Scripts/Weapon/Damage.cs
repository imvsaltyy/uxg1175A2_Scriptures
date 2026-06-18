using System.Collections;
using UnityEngine;

public class Damage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HP Deducted");
        //StartCoroutine(damageFlash(collision));
    }

    IEnumerator damageFlash(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(2f);

        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

    }
}
