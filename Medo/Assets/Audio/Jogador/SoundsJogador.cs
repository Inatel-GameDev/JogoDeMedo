using UnityEngine;

public class SoundsJogador : MonoBehaviour
{
    public static SoundsJogador instance;

    
    public AudioClip explosao;
    public AudioClip andando;
    public AudioClip atingido;

    void Awake()
    {
        instance = this;
    }
}

