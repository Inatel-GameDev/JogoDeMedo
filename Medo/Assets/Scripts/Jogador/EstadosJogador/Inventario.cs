using System;
using System.Collections;
using UnityEngine;


/// <summary>
/// Responsavel por tudo de itens
///  * Armazenar e ler a lista
///  * Marcar o item atual ?
/// </summary>
public class Inventario : MonoBehaviour
{
    [SerializeField] private Jogador jogador;
    //  Possuir uma lista de prefabs, salvar apenas uma string na lista, quando soltar o item instancia
    public String[] inventario;
    public Item itemPerto ;
    [SerializeField] private GameObject[] itemsPrefabs;
    

    // Atualizar a tela com escrita

    private void Start()
    {
        inventario = new string[jogador.capacidade];
    }


    void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Item");
            // habilitar escrito na tela             
            itemPerto = other.gameObject.GetComponent<Item>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Sem Item");
            if(itemPerto == other.gameObject.GetComponent<Item>()){
                itemPerto = null;
            }            
        }
    }

    public void AdicionarItem(){
        
        // acho que tem q trocar de item pra gameobject 
        for (int i = 0; i < inventario.Length; i++){
            
            if(inventario[i] == null){
                inventario[i] = itemPerto.gameObject.name;                
                Destroy(itemPerto.gameObject);
                // Atualizar tela 
                break;
            }
        }                
    }

    public void SoltarItem(int selecionado)
    {
        // trocar para estrutura de dicionario se o for ficar lerdo 
        for (int i = 0; i < itemsPrefabs.Length; i++)
        {
            if (inventario[selecionado] == itemsPrefabs[i].name)
            {
                Instantiate(itemsPrefabs[i]  , transform.position, Quaternion.identity);
                inventario[selecionado] = null;
                break;
            }
            else
            {
                Debug.Log("Item não encontrado para soltar");
            }
        }
    }
    
}
