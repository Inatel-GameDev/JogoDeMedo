using UnityEngine;

public class Inventario : MonoBehaviour
{
    [SerializeField] private Jogador jogador;

    //  Possuir uma lista de prefabs, salvar apenas uma string na lista, quando soltar o item instancia 


    // Atualizar a tela com escrita


    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Item");
            // habilitar escrito na tela             
            jogador.itemPerto = other.gameObject.GetComponent<Item>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Sem Item");
            if(jogador.itemPerto == other.gameObject.GetComponent<Item>()){
                jogador.itemPerto = null;
            }            
        }
    }

}
