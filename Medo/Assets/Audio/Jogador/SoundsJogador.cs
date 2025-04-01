using UnityEngine;

public class SoundsJogador : MonoBehaviour
{
    public static SoundsJogador instance;

    
    public AudioClip explosao;
    public AudioClip andando;
    public AudioClip atingido;
    public AudioClip morte;

    void Awake()
    {
        instance = this;
    }
}

