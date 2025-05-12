using UnityEngine;

public class EstadoBase : MonoBehaviour
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
