using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public Image[] slots = new Image[4];
    public Inventario inventory;
    
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].sprite = null;
        }
        
        for (int i = 0; i < inventory.GetItems().Count; i++)
        {
            slots[i].sprite = inventory.GetItems()[i].icon;
        }
    }
}