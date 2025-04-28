using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject itemSlotPrefab;

    private Inventario inventory;

    private void Start()
    {
        inventory = GetComponent<Inventario>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach(Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        foreach(Item item in inventory.GetItems())
        {
            GameObject slot = Instantiate(itemSlotPrefab, itemsParent);
            slot.GetComponent<Image>().sprite = item.icon;
        }
    }
}