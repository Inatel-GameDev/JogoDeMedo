using UnityEngine;

public class SoundsLerdo : MonoBehaviour
{
    public static SoundsLerdo instance;

    
    public AudioClip explosao;
    public AudioClip monstroAndando;
    public AudioClip aviso;

    void Awake()
    {
        instance = this;
    }
}
