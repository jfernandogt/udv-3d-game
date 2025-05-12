using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtacar : EstadoBase
{
    private EnemigoME enemigoME;

    public EstadoAtacar(EnemigoME maquinaEstados) : base(maquinaEstados)
    {
        enemigoME = (EnemigoME)maquinaEstados;
    }

    public override void Entrar()
    {
        base.Entrar();
        Debug.Log("Entró a Estado Atacar");
    }

    public override void UpdateLogica()
    {
        base.UpdateLogica();
        VoltearAVerObjetivo();

        if (enemigoME.DistanciaAlObjetivo > enemigoME.NavMeshAgent.stoppingDistance)
        {
            enemigoME.CambiarEstado(typeof(EstadoPerseguir));
            return;
        }
        else
        {
            Debug.Log("Atacar");
        }
    }

    private void VoltearAVerObjetivo()
    {
        Vector3 direccion = (enemigoME.TransformObjetivo.position - enemigoME.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direccion.x, 0, direccion.z));
        enemigoME.transform.rotation = Quaternion.Slerp(enemigoME.transform.rotation, lookRotation, Time.deltaTime * enemigoME.VelVoltearAVer);
    }

}
