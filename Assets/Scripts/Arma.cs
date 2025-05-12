using System.Collections;
using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float ataque = 5f;
    [SerializeField] private float tiempoEntreDisparo = 0.5f;
    [SerializeField] private float rango = 100f;
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
        ProcesarRaycast();
        yield return new WaitForSecondsRealtime(tiempoEntreDisparo);
        puedeDisparar = true;

    }

    private void ProcesarRaycast()
    {
        if (Physics.Raycast(cameraPrimeraPersona.position, CalcularDireccion(), out RaycastHit hit, rango, layerMask))
        {
            TrailRenderer trail = Instantiate(trailPrefab, origenProyectil.transform.position, Quaternion.identity);

            StartCoroutine(MoverTrail(trail, hit));

            if (hit.transform.TryGetComponent<Salud>(out Salud saludObjetivo))
            {
                saludObjetivo.PerderSalud(ataque);
            }
        }
    }

    private IEnumerator MoverTrail(TrailRenderer trail, RaycastHit hit)
    {
        float t = 0f;

        while (t < 1)
        {
            trail.transform.position = Vector3.Lerp(origenProyectil.transform.position, hit.point, t);
            t += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }

    private Vector3 CalcularDireccion()
    {
        Vector3 direction = cameraPrimeraPersona.forward;
        return direction;
    }
}
