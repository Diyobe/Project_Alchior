using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryButton : MonoBehaviour
{
    [SerializeField] GameObject selectedGlow;
    public bool selected = false;

    public void Select()
    {
        selected = true;
        selectedGlow.SetActive(true);
    }

    public void UnSelect()
    {
        selected = false;
        selectedGlow.SetActive(false);
    }

}
