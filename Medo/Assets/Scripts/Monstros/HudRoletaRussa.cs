using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class HudRoletaRussa : MonoBehaviour
{
    [SerializeField] public Russo monstro;
    [SerializeField] public Button botao;
    [SerializeField] public CameraPOV cameraPov;
    [SerializeField] public int balas = 6;

    private void Start()
    {
        botao.onClick.AddListener(tiro);
    }

    private void OnEnable()
    {
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        cameraPov.enabled = false;
    }
    
    // acho q ta pegando mais de um click
    public void tiro()
    {
        Random rnd = new Random();
        int n = rnd.Next(1, 7);
        Debug.Log(n);
        if (n == 1) {
            monstro.jogadorAlvo.Morte();
        }
        else
        {
            monstro.jogadorAlvo.MudarEstado(monstro.jogadorAlvo.EstadoAndando);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameraPov.enabled = true;
            
            n = rnd.Next(1, balas + 1);
            Debug.Log(n);
            if (n == 1) {
                monstro.Morte();
            }
            else
            {
                balas--;
                if (balas == 0)
                    balas = 1;
                monstro.Pausa();
            }
        }
        gameObject.SetActive(false);
    }
}
