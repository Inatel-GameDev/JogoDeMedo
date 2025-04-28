using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Item Info")]
    public Sprite icon;
    public GameObject worldPrefab;
    
    public abstract void Use(Jogador jogador);
}