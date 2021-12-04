using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] CharacterBase character;
    [SerializeField] Image fillBar;

    // Start is called before the first frame update
    void Start()
    {
        character.CharacterData.onHpChanged += UpdateHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHP(float hp)
    {
        fillBar.fillAmount = hp / (float)character.CharacterData.GetMaxHP();
    }
}
