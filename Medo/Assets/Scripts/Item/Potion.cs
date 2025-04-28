using UnityEngine;


[CreateAssetMenu(menuName = "Items/Potion")]
public class Potion : Item
{
    public override void Use(Jogador jogador)
    {
        Debug.Log("Potion used");    
    }
}
