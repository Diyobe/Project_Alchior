using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCondition
{
    public virtual bool CheckCondition(CharacterBase character)
    {
        return true;
    }

}

// Utilis� comme relais pour les CharacterCondition
// Cr�er � la base pour ne pas faire de AttackManager un objet de Odin pour essayer de rester le plus pur 'possible'
public class CharacterConditionGameObject : SerializedMonoBehaviour
{
    [SerializeField]
    [ListDrawerSettings(Expanded = true, AlwaysAddDefaultValue = true)]
    CharacterCondition[] conditions;

    public bool CheckConditions(CharacterBase character)
    {
        if (conditions.Length == 0)
            return true;
        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i].CheckCondition(character) == false)
                return false;
        }
        return true;
    }
}
