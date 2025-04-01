using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    
    public AudioClip explosao;
    public AudioClip jogadorAndando;
    public AudioClip jogadorPulando;
    public AudioClip jogadorFerido;
    public AudioClip monstroAndando;

    void Awake()
    {
        instance = this;
    }
}
