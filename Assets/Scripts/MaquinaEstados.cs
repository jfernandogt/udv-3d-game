using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaquinaEstados : MonoBehaviour
{
    private Dictionary<Type, EstadoBase> estadosDisponibles;
    public EstadoBase EstadoActual { get; private set; }
    public void DefinirEstados(Dictionary<Type, EstadoBase> estados)
    {
        estadosDisponibles = estados;
    }
    protected virtual void Start()
    {
        Type TipoEstadoActual = ObtenerEstadoInicial();
        EstadoActual = estadosDisponibles[TipoEstadoActual];
        if (EstadoActual != null)
        {
            EstadoActual.Entrar();
        }
    }
    protected virtual void Update()
    {
        if (EstadoActual == null) { return; }
        EstadoActual.UpdateLogica();
    }
    protected virtual Type ObtenerEstadoInicial()
    {
        return null;
    }
    public void CambiarEstado(Type tipoNuevoEstado)
    {
        EstadoActual.Salir();
        EstadoActual = estadosDisponibles[tipoNuevoEstado];
        EstadoActual.Entrar();
    }
}
