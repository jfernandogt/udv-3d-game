using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPerseguir : EstadoBase
{
    private EnemigoME enemigoME;

    public EstadoPerseguir(EnemigoME maquinaEstados) : base(maquinaEstados)
    {
        enemigoME = (EnemigoME)maquinaEstados;
    }

    public override void Entrar()
    {
        base.Entrar();
        Debug.Log("Entró a Estado Perseguir");

        enemigoME.ContTiempoAgresion = enemigoME.TiempoAgresion;
    }

    public override void UpdateLogica()
    {
        base.UpdateLogica();

        if (enemigoME.RevisarDistancia())
        {
            enemigoME.ContTiempoAgresion = enemigoME.TiempoAgresion;
        }

        enemigoME.ContTiempoAgresion -= Time.deltaTime;

        if (enemigoME.ContTiempoAgresion > 0)
        {
            if (enemigoME.DistanciaAlObjetivo > enemigoME.NavMeshAgent.stoppingDistance)
            {
                enemigoME.NavMeshAgent.SetDestination(enemigoME.TransformObjetivo.transform.position);
            }
            else
            {
                enemigoME.CambiarEstado(typeof(EstadoAtacar));
                return;
            }
        }
    }
}
