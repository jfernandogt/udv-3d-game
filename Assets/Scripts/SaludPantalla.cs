using UnityEngine;
using UnityEngine.UI;

public class SaludPantalla : MonoBehaviour
{
    [SerializeField] private Salud salud;
    [SerializeField] private Image barraSalud;


    private void OnEnable()
    {
        // Subscribe to the event
        salud.alActualizarSalud += ActualizarBarra;
    }

    private void OnDisable()
    {
        salud.alActualizarSalud -= ActualizarBarra;
    }

    private void ActualizarBarra()
    {
        barraSalud.fillAmount = salud.ObtenerFraccion();
    }
}
