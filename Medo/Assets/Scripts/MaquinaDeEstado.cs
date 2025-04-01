using UnityEngine;

public abstract class MaquinaDeEstado : MonoBehaviour
{
    public abstract void MudarEstado(Estado novoEstado);
    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void LateUpdate();
    
}
