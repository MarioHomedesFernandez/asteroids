using UnityEngine;

public class Asteroide : MonoBehaviour
{
    public bool esMitad = false;
    private Vector3 direccionLineaRecta;
    private float velocidadMovimiento = 2f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            velocidadMovimiento = rb.linearVelocity.magnitude;
            if (velocidadMovimiento == 0f) velocidadMovimiento = 2f;
            
            direccionLineaRecta = rb.linearVelocity.normalized;
            if (direccionLineaRecta == Vector3.zero)
            {
                direccionLineaRecta = transform.up;
            }
        }
    }

    void Update()
    {
        transform.position += direccionLineaRecta * velocidadMovimiento * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            Destroy(other.gameObject);

            if (esMitad)
            {
                Destroy(gameObject);
            }
            else
            {
                if (Random.value <= 0.25f)
                {
                    Vector3 pos1 = transform.position + new Vector3(0.5f, 0.5f, 0f);
                    Vector3 pos2 = transform.position + new Vector3(-0.5f, -0.5f, 0f);

                    GameObject ast1 = Instantiate(gameObject, pos1, Quaternion.identity);
                    GameObject ast2 = Instantiate(gameObject, pos2, Quaternion.identity);

                    ast1.transform.localScale = transform.localScale * 0.5f;
                    ast2.transform.localScale = transform.localScale * 0.5f;

                    Asteroide script1 = ast1.GetComponent<Asteroide>();
                    Asteroide script2 = ast2.GetComponent<Asteroide>();

                    if (script1 != null) script1.esMitad = true;
                    if (script2 != null) script2.esMitad = true;

                    Rigidbody rb1 = ast1.GetComponent<Rigidbody>();
                    Rigidbody rb2 = ast2.GetComponent<Rigidbody>();

                    Vector2 dir1 = Random.insideUnitCircle.normalized;
                    Vector2 dir2 = Random.insideUnitCircle.normalized;

                    Vector3 nuevaDir1 = new Vector3(dir1.x, dir1.y, 0f);
                    Vector3 nuevaDir2 = new Vector3(dir2.x, dir2.y, 0f);

                    if (rb1 != null) rb1.linearVelocity = nuevaDir1 * velocidadMovimiento;
                    if (rb2 != null) rb2.linearVelocity = nuevaDir2 * velocidadMovimiento;

                    // Corregir rotación del PNG para que miren hacia su nueva dirección
                    if (ast1 != null)
                    {
                        Quaternion rot1 = Quaternion.LookRotation(Vector3.forward, nuevaDir1);
                        ast1.transform.rotation = rot1 * Quaternion.Euler(0, 0, 180f);
                    }
                    if (ast2 != null)
                    {
                        Quaternion rot2 = Quaternion.LookRotation(Vector3.forward, nuevaDir2);
                        ast2.transform.rotation = rot2 * Quaternion.Euler(0, 0, 180f);
                    }
                }

                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}