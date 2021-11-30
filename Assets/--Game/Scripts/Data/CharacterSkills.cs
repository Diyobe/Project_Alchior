using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Skills", menuName = "Data/Character Skills")]
public class CharacterSkills : ScriptableObject
{
    public List<AttackManager> learnableSkills = new List<AttackManager>();
    public List<AttackManager> learnedSkills = new List<AttackManager>();

    public AttackManager firstEquipedSkill;
    public AttackManager secondEquipedSkill;
    public AttackManager thirdEquipedSkill;
    public AttackManager fourtEquipedhSkill;
}
