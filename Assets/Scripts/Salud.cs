using UnityEngine;
using System;
using UnityEngine.Events;
using System.Collections;

public class Salud : MonoBehaviour
{
    [SerializeField] private float saludMax = 3f;
    [SerializeField] private bool destruirAlMorir = true;
    [SerializeField] private float tiempoEnDestruirse = 0f;
    [SerializeField] private float tiempoInvencible = 2f;
    [SerializeField] private UnityEvent<float> alPerderSalud;
    [SerializeField] private UnityEvent alMorir;

    private float saludActual;
    private Animator animador;
    private bool estaMuerto = false;
    private bool esInvencible = false;

    public event Action alActualizarSalud;


    private void Awake()
    {
        animador = GetComponent<Animator>();
        saludActual = saludMax;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alActualizarSalud?.Invoke();
    }

    public bool EstaMuerto()
    {
        return estaMuerto;
    }

    public float ObtenerFraccion()
    {
        return saludActual / saludMax;
    }

    public float ObtenerSalud()
    {
        return saludActual;
    }

    public void AjustarSalud(float salud)
    {
        saludActual = salud;
        alActualizarSalud?.Invoke();
    }

    public void PerderSalud(float saludPerdida)
    {
        if (esInvencible) // Comprobar si es invencible
        {
            return; // No perder salud si es invencible
        }
        animador.ResetTrigger("perderSalud");
        saludActual = Mathf.Max(saludActual - saludPerdida, 0);
        alPerderSalud?.Invoke(saludPerdida);
        alActualizarSalud?.Invoke();
        if (saludActual == 0)
        {
            Morir();
        }
        else
        {
            animador.SetTrigger("perderSalud");
        }
    }

    private void Morir()
    {
        if (estaMuerto)
        {
            return;
        }

        alMorir?.Invoke();
        estaMuerto = true;
        animador.SetTrigger("morir");
        if (destruirAlMorir)
        {
            Destroy(gameObject, tiempoEnDestruirse);
        }
    }

    public void ActivarInvencibilidad()
    {
        esInvencible = true;
        StartCoroutine(DesactivarInvencibilidad(tiempoInvencible));
    }

    private IEnumerator DesactivarInvencibilidad(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        esInvencible = false;
    }
}
