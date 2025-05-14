using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoAtacar : EstadoBase
{
    private EnemigoME enemigoME;
    private float tiempoUltimoAtaque;

    public EstadoAtacar(EnemigoME maquinaEstados) : base(maquinaEstados)
    {
        enemigoME = (EnemigoME)maquinaEstados;
    }
    public override void Entrar()
    {
        base.Entrar();
        Debug.Log("Entró a estado atacar");
    }
    public override void UpdateLogica()
    {
        base.UpdateLogica();

        if (enemigoME.TransformObjetivo == null) return;

        VoltearAVerObjetivo();

        if (enemigoME.DistanciaAlObjetivo > enemigoME.NavMeshAgent.stoppingDistance)
        {
            enemigoME.CambiarEstado(typeof(EstadoPerseguir));
            return;
        }

        if (Time.time - tiempoUltimoAtaque >= enemigoME.TiempoEntreAtaques)
        {
            Atacar();
            tiempoUltimoAtaque = Time.time;
        }
    }

    private void VoltearAVerObjetivo()
    {
        Vector3 direccion = (enemigoME.TransformObjetivo.position - enemigoME.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(
            new Vector3(direccion.x, 0, direccion.z)
        );
        enemigoME.transform.rotation = Quaternion.Slerp(enemigoME.transform.rotation, lookRotation, Time.deltaTime * enemigoME.VelVoltearAVer);
    }

    private void Atacar()
    {
        if (enemigoME.TransformObjetivo == null) return;

        Salud objetivoSalud = enemigoME.TransformObjetivo.GetComponent<Salud>();
        if (objetivoSalud != null)
        {
            if (objetivoSalud.EstaMuerto())
            {
                enemigoME.CambiarEstado(typeof(EstadoReposo));
                Debug.Log("El objetivo está muerto");
                return;
            }
            else
            {

                objetivoSalud.PerderSalud(enemigoME.Daño);
            }
        }
    }

}