using UnityEngine;

public class EstadoReposo : EstadoBase
{
    private EnemigoME enemigoME;

    public EstadoReposo(EnemigoME maquinaEstados) : base(maquinaEstados)
    {
        enemigoME = (EnemigoME)maquinaEstados;
    }

    public override void Entrar()
    {
        base.Entrar();
        Debug.Log("Entró a Estado Reposo");
    }

    public override void UpdateLogica()
    {
        base.UpdateLogica();

        if (enemigoME.RevisarDistancia())
        {
            enemigoME.CambiarEstado(typeof(EstadoPerseguir));
            return;
        }
    }
}