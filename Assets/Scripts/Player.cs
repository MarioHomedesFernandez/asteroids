using UnityEngine;

public class Player : MonoBehaviour
{
    public static int SCORE = 0;

    public float speedForce = 200f;
    public float rotationSpeed = 15f;

    public GameObject gun, bulletPrefab;

    private Rigidbody rb;
    private Camera camara;
    private float margenPantalla = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main;
    }

    void Update()
    {
        float force = Input.GetAxis("Vertical") * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;

        Vector3 direccion = transform.right;

        rb.AddForce(direccion * force * speedForce);
        transform.Rotate(Vector3.forward, -rotation * rotationSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.targetVector = transform.right;
        }

        AplicarPantallaEnvolvente();
    }

    void AplicarPantallaEnvolvente()
    {
        if (camara == null) return;

        float altoCamara = camara.orthographicSize;
        float anchoCamara = altoCamara * camara.aspect;
        Vector3 centroCamara = camara.transform.position;

        Vector3 posActual = transform.position;

        // Comprobación eje X (Izquierda / Derecha)
        if (posActual.x > centroCamara.x + anchoCamara + margenPantalla)
        {
            posActual.x = centroCamara.x - anchoCamara - margenPantalla;
        }
        else if (posActual.x < centroCamara.x - anchoCamara - margenPantalla)
        {
            posActual.x = centroCamara.x + anchoCamara + margenPantalla;
        }

        // Comprobación eje Y (Arriba / Abajo)
        if (posActual.y > centroCamara.y + altoCamara + margenPantalla)
        {
            posActual.y = centroCamara.y - altoCamara - margenPantalla;
        }
        else if (posActual.y < centroCamara.y - altoCamara - margenPantalla)
        {
            posActual.y = centroCamara.y + altoCamara + margenPantalla;
        }

        transform.position = posActual;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Asteroide>() != null)
        {
            Destroy(gameObject);
        }
    }
}