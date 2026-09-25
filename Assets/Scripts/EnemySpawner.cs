using UnityEngine; 

public class EnemySpawner : MonoBehaviour 
{ 
    public GameObject enemyPrefab; 
    public float spawnPorMinuto = 30f; 
    public float spawnRateIncrement = 1f; 
    public float velocidadEnemigo = 2f; 
    private float nextSpawn = 0; 
    private float fixedZ = -6.08f; 
    private Camera camara; 
    private float margenFuera = 1.5f; 

    void Start() 
    { 
        camara = Camera.main; 
    } 

    void Update() 
    { 
        if (Time.time >= nextSpawn) 
        { 
            nextSpawn = Time.time + 60f / spawnPorMinuto; 
            SpawnearEnemigo(); 
            spawnPorMinuto += spawnRateIncrement; 
        } 
    } 

    void SpawnearEnemigo() 
    { 
        float altoCamara = camara.orthographicSize; 
        float anchoCamara = altoCamara * camara.aspect; 
        Vector3 centroCamara = camara.transform.position; 
        Vector3 posicionSpawn = Vector3.zero; 
        int ladoAleatorio = Random.Range(0, 4); 

        switch (ladoAleatorio) 
        { 
            case 0: 
                posicionSpawn = new Vector3(Random.Range(-anchoCamara - margenFuera, anchoCamara + margenFuera) + centroCamara.x, centroCamara.y + altoCamara + margenFuera, fixedZ); 
                break; 
            case 1: 
                posicionSpawn = new Vector3(Random.Range(-anchoCamara - margenFuera, anchoCamara + margenFuera) + centroCamara.x, centroCamara.y - altoCamara - margenFuera, fixedZ); 
                break; 
            case 2: 
                posicionSpawn = new Vector3(centroCamara.x - anchoCamara - margenFuera, Random.Range(-altoCamara - margenFuera, altoCamara + margenFuera) + centroCamara.y, fixedZ); 
                break; 
            case 3: 
                posicionSpawn = new Vector3(centroCamara.x + anchoCamara + margenFuera, Random.Range(-altoCamara - margenFuera, altoCamara + margenFuera) + centroCamara.y, fixedZ); 
                break; 
        } 

        GameObject enemigo = Instantiate(enemyPrefab, posicionSpawn, Quaternion.identity); 

        if (enemigo.GetComponent<Asteroide>() == null)
        {
            enemigo.AddComponent<Asteroide>();
        }

        Vector3 puntoObjetivo = new Vector3( 
            Random.Range(-anchoCamara * 0.8f, anchoCamara * 0.8f) + centroCamara.x, 
            Random.Range(-altoCamara * 0.8f, altoCamara * 0.8f) + centroCamara.y, 
            fixedZ 
        ); 

        Vector3 direccion = (puntoObjetivo - posicionSpawn).normalized; 
        Quaternion rotacionMirada = Quaternion.LookRotation(Vector3.forward, direccion); 
        enemigo.transform.rotation = rotacionMirada * Quaternion.Euler(0, 0, 180f); 

        Rigidbody rb = enemigo.GetComponent<Rigidbody>(); 
        if (rb != null) 
        { 
            rb.linearVelocity = direccion * velocidadEnemigo; 
        } 
        else 
        { 
            MovimientoRecto mov = enemigo.AddComponent<MovimientoRecto>(); 
            mov.direccion = direccion; 
            mov.velocidad = velocidadEnemigo; 
        } 

        DestruirFueraDePantalla dest = enemigo.AddComponent<DestruirFueraDePantalla>(); 
        dest.camaraRef = camara; 
    } 
} 

public class MovimientoRecto : MonoBehaviour 
{ 
    [HideInInspector] public Vector3 direccion; 
    [HideInInspector] public float velocidad; 

    void Update() 
    { 
        transform.position += direccion * velocidad * Time.deltaTime; 
    } 
} 

public class DestruirFueraDePantalla : MonoBehaviour 
{ 
    [HideInInspector] public Camera camaraRef; 
    private bool haEntradoEnPantalla = false; 
    private float margenPermitido = 5f; 

    void Update() 
    { 
        if (camaraRef == null) return; 

        float altoCamara = camaraRef.orthographicSize; 
        float anchoCamara = altoCamara * camaraRef.aspect; 
        Vector3 centroCamara = camaraRef.transform.position; 

        bool estaEnPantalla = transform.position.x >= centroCamara.x - anchoCamara - 1f && 
                              transform.position.x <= centroCamara.x + anchoCamara + 1f && 
                              transform.position.y >= centroCamara.y - altoCamara - 1f && 
                              transform.position.y <= centroCamara.y + altoCamara + 1f; 

        if (estaEnPantalla) 
        { 
            haEntradoEnPantalla = true; 
        } 

        if (haEntradoEnPantalla) 
        { 
            bool estaMuyLejos = transform.position.x < centroCamara.x - anchoCamara - margenPermitido || 
                                 transform.position.x > centroCamara.x + anchoCamara + margenPermitido || 
                                 transform.position.y < centroCamara.y - altoCamara - margenPermitido || 
                                 transform.position.y > centroCamara.y + altoCamara + margenPermitido; 

            if (estaMuyLejos) 
            { 
                Destroy(gameObject); 
            } 
        } 
    } 
}