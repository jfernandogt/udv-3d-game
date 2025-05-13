using UnityEngine;

[RequireComponent(typeof(Equipo))]
public class ContactoAtaque : MonoBehaviour
{
    [SerializeField] private float cantAtaque = 1f;
    private Equipo equipo;

    void Awake()
    {
        equipo = GetComponent<Equipo>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent<Salud>(out Salud saludDelOtro))
        {
            Debug.Log("No tiene salud");
            return;
        }

        if (!collision.gameObject.TryGetComponent<Equipo>(out Equipo equipoDelOtro))
        {
            Debug.Log("No tiene equipo");
            return;
        }

        if (equipoDelOtro.EquipoActual == equipo.EquipoActual)
        {
            Debug.Log("Es el mismo equipo");
            return;
        }

        Debug.Log("Es el otro equipo");
        saludDelOtro.PerderSalud(cantAtaque);
    }
}
