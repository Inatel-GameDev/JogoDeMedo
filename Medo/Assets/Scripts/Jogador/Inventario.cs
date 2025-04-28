using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsavel por tudo de itens
///  * Armazenar e ler a lista
///  * Marcar o item atual ?
/// </summary>
public class Inventario : MonoBehaviour
{
    [SerializeField] private Jogador jogador;
    public WorldItem itemPerto;
    public int selecionado = 0;
    private List<Item> items = new List<Item>(); 
    public List<Item> GetItems() => new List<Item>(items);
    public Image image;
    
    public void AddItem()
    {
        items.Add(itemPerto.item);
        image.sprite = itemPerto.item.icon;
        itemPerto.DestroyItem();
    }

    public void RemoveItem()
    {
        ItemSpawner.Instance.SpawnItem(items[selecionado],transform.position);
        items.Remove(items[selecionado]);
        image.sprite = null;
    }

    // index do inventário vai ser o mesmo index da UI e do item "selecionado" 
    public void UseItem(int index, Jogador player)
    {
        if(index >= 0 && index < items.Count)
        {
            items[index].Use(player);
            RemoveItem();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Item");
            // habilitar escrito na tela             
            itemPerto = other.gameObject.GetComponent<WorldItem>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Item")){
            Debug.Log("Sem Item");
            if(itemPerto == other.gameObject.GetComponent<WorldItem>()){
                // Atualizar a tela com escrita
                itemPerto = null;
            }            
        }
    }
}
