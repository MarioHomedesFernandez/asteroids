using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public float duracion = 3f;
    public Vector3 targetVector;

    void Start()
    {
        Destroy(gameObject, duracion);
    }

    void Update()
    {
        transform.Translate(targetVector * speed * Time.deltaTime);
    }

    // Usamos OnTriggerEnter para que la bala detecte al asteroide de forma ultra fiable
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Asteroide>() != null)
        {
            Destroy(other.gameObject); // Destruye el asteroide
            Destroy(gameObject);       // Destruye la bala
        }
    }
}