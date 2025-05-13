using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoME : MaquinaEstados
{
    [SerializeField] private GameObject objetivo;
    [SerializeField] private float rangoDeteccion = 5f;
    [SerializeField] private float velVoltearAVer = 5f;
    [SerializeField] private float tiempoAgresion = 5f;

    public float RangoDeteccion { get { return rangoDeteccion; } private set { } }
    public float VelVoltearAVer { get { return velVoltearAVer; } private set { } }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public Transform TransformObjetivo { get; private set; }
    public float DistanciaAlObjetivo { get; private set; } = Mathf.Infinity;

    public float TiempoAgresion { get { return tiempoAgresion; } private set { } }
    [NonSerialized] public float ContTiempoAgresion;

    private void Awake()
    {
        var estados = new Dictionary<Type, EstadoBase>()
    {
        {typeof(EstadoReposo), new EstadoReposo(this)},
        {typeof(EstadoPerseguir), new EstadoPerseguir(this)},
        {typeof(EstadoAtacar), new EstadoAtacar(this)}
    };
        DefinirEstados(estados);

        NavMeshAgent = GetComponent<NavMeshAgent>();
        TransformObjetivo = objetivo.transform;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // if (TransformObjetivo != null)
        // {
        base.Update();

        DistanciaAlObjetivo = Vector3.Distance(TransformObjetivo.position, transform.position);
        // }
    }

    protected override Type ObtenerEstadoInicial()
    {
        return typeof(EstadoReposo);
    }

    public bool RevisarDistancia()
    {
        return (DistanciaAlObjetivo <= RangoDeteccion);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, rangoDeteccion);
    }

}