using System.Collections;
using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float ataque = 5f;
    [SerializeField] private float tiempoEntreDisparo = 0.5f; // Usado para cadencia de ráfaga y cooldown general
    [SerializeField] private float rango = 100f;
    [SerializeField] private bool rafaga = false;
    [SerializeField] private int cantidadProyectiles = 1; // Para escopeta o número base para ráfaga
    [SerializeField] private float dispersionMaxima = 0f; // Ángulo máximo de dispersión para escopeta
    [SerializeField] private LayerMask layerMask;

    [Header("GameObjects")]
    [SerializeField] private Transform cameraPrimeraPersona;
    [SerializeField] private Transform origenProyectil;
    [SerializeField] private TrailRenderer trailPrefab;

    private bool puedeDisparar = true;

    public void ProcesarEntrada(bool value)
    {
        if (puedeDisparar && value)
        {
            StartCoroutine(Disparar());
        }
    }

    private IEnumerator Disparar()
    {
        puedeDisparar = false;

        if (rafaga)
        {
            Debug.Log("Disparando en ráfaga");
            // En modo ráfaga, disparar al menos 3 proyectiles, o cantidadProyectiles si es mayor.
            // Todos los proyectiles de la ráfaga van rectos.
            int balasEnRafaga = Mathf.Max(3, cantidadProyectiles);

            for (int i = 0; i < balasEnRafaga; i++)
            {

                Vector3 origenDelRayo = cameraPrimeraPersona.position;
                Vector3 origenDelTrail = origenProyectil.position;
                Vector3 direccionRecta = cameraPrimeraPersona.forward.normalized;
                DispararUnProyectil(origenDelRayo, origenDelTrail, direccionRecta);

                // Si no es la última bala de la ráfaga, esperar antes de la siguiente.
                if (i < balasEnRafaga - 1)
                {
                    yield return new WaitForSecondsRealtime(tiempoEntreDisparo);
                }
            }
            // Cooldown después de que la ráfaga completa ha terminado.
            yield return new WaitForSecondsRealtime(tiempoEntreDisparo);
        }
        else
        {
            // Modo de disparo normal (único o escopeta)
            ProcesarDisparoModoNormal();
            // Cooldown después del disparo.
            yield return new WaitForSecondsRealtime(tiempoEntreDisparo);
        }

        puedeDisparar = true;
    }

    // Nueva función para procesar disparos no-ráfaga (único o escopeta)
    private void ProcesarDisparoModoNormal()
    {
        Vector3 origenDelRayo = cameraPrimeraPersona.position;
        Vector3 origenDelTrail = origenProyectil.position;

        for (int i = 0; i < cantidadProyectiles; i++)
        {
            Vector3 direccionDisparo;
            if (cantidadProyectiles == 1) // Disparo único estándar
            {
                direccionDisparo = cameraPrimeraPersona.forward.normalized;
            }
            else // Modo escopeta (cantidadProyectiles > 1)
            {
                // El primer proyectil (i=0) de la escopeta va al centro, los demás con dispersión.
                direccionDisparo = CalcularDireccionProyectilEscopeta(i == 0);
            }
            DispararUnProyectil(origenDelRayo, origenDelTrail, direccionDisparo);
        }
    }

    // Función para calcular la dirección con dispersión para el modo escopeta
    private Vector3 CalcularDireccionProyectilEscopeta(bool esProyectilCentral)
    {
        Vector3 direccionBase = cameraPrimeraPersona.forward.normalized;

        // Aplicar dispersión solo si no es el proyectil central Y la dispersión está configurada
        if (!esProyectilCentral && dispersionMaxima > 0f)
        {
            float maxAnguloDispersionRad = dispersionMaxima * Mathf.Deg2Rad;
            Vector2 circuloAleatorio = Random.insideUnitCircle * Mathf.Tan(maxAnguloDispersionRad * Random.value);
            Vector3 direccionRelativa = new Vector3(circuloAleatorio.x, circuloAleatorio.y, 1.0f).normalized;
            return cameraPrimeraPersona.rotation * direccionRelativa;
        }

        return direccionBase; // Proyectil central o sin dispersión configurada
    }

    // Función genérica para disparar un solo proyectil
    private void DispararUnProyectil(Vector3 origenDelRayo, Vector3 origenDelTrail, Vector3 direccionDisparo)
    {
        TrailRenderer trail = Instantiate(trailPrefab, origenDelTrail, Quaternion.identity);

        if (Physics.Raycast(origenDelRayo, direccionDisparo, out RaycastHit hit, rango, layerMask))
        {
            StartCoroutine(MoverTrail(trail, origenDelTrail, hit.point));

            if (hit.transform.TryGetComponent<Salud>(out Salud saludObjetivo))
            {
                saludObjetivo.PerderSalud(ataque);
            }
        }
        else
        {
            // Si no hay impacto, el trail viaja la distancia máxima en la dirección del disparo
            Vector3 puntoFinalTrailSinHit = origenDelTrail + (direccionDisparo * rango);
            StartCoroutine(MoverTrail(trail, origenDelTrail, puntoFinalTrailSinHit));
        }
    }

    private IEnumerator MoverTrail(TrailRenderer trail, Vector3 puntoInicio, Vector3 puntoFin)
    {
        float t = 0f;
        float duracionMovimiento = trail.time > 0 ? trail.time : 0.1f;

        while (t < 1)
        {
            trail.transform.position = Vector3.Lerp(puntoInicio, puntoFin, t);
            t += Time.deltaTime / duracionMovimiento;
            yield return null;
        }

        trail.transform.position = puntoFin;
        Destroy(trail.gameObject, trail.time);
    }
}
