using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCategoryButton : CategoryButton
{
    public Image characterFace;

    public void UpdateCharacterFace(Sprite sprite)
    {
        characterFace.sprite = sprite;
    }
}
