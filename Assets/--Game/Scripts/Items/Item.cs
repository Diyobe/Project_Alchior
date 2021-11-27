using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    //public enum Type
    //{
    //    USABLE,
    //    CONSUMABLE,
    //    WEAPON,
    //    SHIRT,
    //    PANTS,
    //    ACCESSORY,
    //    MATERIAL,
    //    INGREDIENT,
    //    DISH,
    //    RARE,
    //}
    //public Type type;
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public bool isStackable = true;
    public string description;


    public virtual void Use()
    {
        //Use the item
        //Something might happen
        Debug.Log("Using " + name);
    }
}
