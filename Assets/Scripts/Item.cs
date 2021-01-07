using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : IDamagable
{
    [SerializeField] protected HealthSystem healthSystem = new HealthSystem(1, 1);
    public float TotalHealthPoints { get => healthSystem.TotalHealthPoints; set => healthSystem.TotalHealthPoints = value; }
    public float HealthPoints { get => healthSystem.HealthPoints; set => healthSystem.HealthPoints = value; }

    public virtual void Use()
    {
        Debug.Log("Used Item");
    }

    public abstract int GetItemID();

    public virtual void Damage(float damage)
    {
        if (healthSystem.Destructable == false)
        {
            Debug.Log("Indestructable");
            return;
        }
        HealthPoints = HealthPoints - damage;
        if (HealthPoints <= 0) Kill();
    }

    public virtual void Kill()
    {

        Debug.Log("Killed");
    }

    public virtual void ForceKill()
    {

        Debug.Log("ForcrKilled");
    }

}

public abstract class ItemUsable : Item
{
    private int uses = 1;
    public int Uses { get => uses; set => uses = value; }
    
    public abstract override int GetItemID();
}

public class ItemManager
{

    public Item GetItemByID(int ID)
    {
        switch (ID)
        {
            case 0:
                Item_Soup soup = new Item_Soup();
                return soup;
            default:
                Debug.Log("No item with ID");
                break;
        }

        return null;
    }


}

public class Inventory
{
    public Item[,] items;

    public void ResizeInventory(Vector2Int size)
    {

    }

    public void DropItem(Vector2Int itemPos)
    {

    }

    public Item GetItem(Vector2Int itemPos)
    {
        Item item = null;

        return item;
    }

    public void UseItem(Vector2Int itemPos)
    {

    }
}

public class Item_Soup : ItemUsable
{
    public int itemID = 0;

    public override void Use()
    {
        base.Use();

    }
    public override int GetItemID() { return itemID; }
}
