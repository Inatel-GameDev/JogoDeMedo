using UnityEngine;

public class SoundsLerdo : MonoBehaviour
{
    public static SoundsLerdo instance;

    
    public AudioClip explosao;
    public AudioClip monstroAndando;

    void Awake()
    {
        instance = this;
    }
}
