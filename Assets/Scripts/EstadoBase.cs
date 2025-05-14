using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoBase
{
    protected MaquinaEstados maquinaEstados;
    public EstadoBase(MaquinaEstados maquinaEstados)
    {
        this.maquinaEstados = maquinaEstados;
    }
    public virtual void Entrar() { }
    public virtual void UpdateLogica() { }
    public virtual void Salir() { }
}