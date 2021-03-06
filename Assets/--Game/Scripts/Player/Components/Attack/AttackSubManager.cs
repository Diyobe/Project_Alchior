using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider))]
public class AttackSubManager : MonoBehaviour
{
    [SerializeField]
    private Collider hitBox;
    public Collider HitBox
    {
        get { return hitBox; }
    }

    [Title("Components")]
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    private List<AttackComponent> atkCompList;

    [HideInInspector]
    public PackageCreator.Event.GameEventCharacters playerHitEvent;






    [Title("Parry Settings")]
    [SerializeField]
    private int clashLevel = 1;
    public int ClashLevel
    {
        get { return clashLevel; }
    }

    [SerializeField]
    [SuffixLabel("L'attaque ne proc jamais de clash")]
    private bool noClash = false;
    public bool NoClash
    {
        get { return noClash; }
    }
    [SerializeField]
    [SuffixLabel("L'attaque provoque une parade mais ne repousse pas")]
    private bool noParryCancel = false;
    public bool NoParryCancel
    {
        get { return noParryCancel; }
    }

    // Si l'attack est disjoint on ne peut pas se faire repousser
    [SerializeField]
    [SuffixLabel("Si il y a Parade/Garde, l'utilisateur de cette attaque n'est pas impliqu?")]
    private bool disjoint = false;
    public bool Disjoint
    {
        get { return disjoint; }
    }

    // On peut Garder l'attaque juste en courant
    [SerializeField]
    [SuffixLabel("L'attaque peut etre gard? si on Dash")]
    private bool guardOnDash = false;
    public bool GuardOnDash
    {
        get { return guardOnDash; }
    }

    // Si guardWin est On l'attaque ne repousse pas le joueur
    [SerializeField]
    [SuffixLabel("La garde gagne sur cette attaque")]
    private bool guardWin = false;
    public bool GuardWin
    {
        get { return guardWin; }
    }

    [HorizontalGroup("Break")]
    [SerializeField]
    private bool parryBreak = false;
    public bool BreakParry
    {
        get { return parryBreak; }
    }
    [HorizontalGroup("Break")]
    [SerializeField]
    private bool guardBreak = false;
    public bool BreakGuard
    {
        get { return guardBreak; }
    }







    CharacterBase user;
    public CharacterBase User
    {
        get { return user; }
    }



    // Ces deux flags sont utilis?s pour les collisions et g?rer leur priorit? (Clash prio sur Hit)
    /*bool hasHit;
    public bool HasHit
    {
        get { return hasHit; }
    }
    bool hasClash;
    public bool HasClash
    {
        get { return hasClash; }
    }*/

    private List<string> playerHitList = new List<string>();

    // Utilis? pour identifier l'attaque
    string attackID = "";
    public string AttackID
    {
        get { return attackID; }
    }



    [Button]
    public void UpdateComponents()
    {
        hitBox = GetComponent<Collider>();
        atkCompList = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>());

    }



    [Title("Events")]
    [SerializeField]

    private UnityEvent<string> onHitColliderEvents;
    private bool eventReceived;

    // ===============================================================================


    public void Update()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.UpdateComponent(user);
        }
    }

    public void InitAttack(CharacterBase character, string attackName)
    {
        tag = character.tag;
        user = character;
        hitBox.enabled = false;
        attackID = attackName;

        for (int i = 0; i < atkCompList.Count; i++)
        {
            atkCompList[i].StartComponent(character);
        }
    }

    /// <summary>
    /// Reinitialise une attack mais n'appelle pas les Start Components, utilis? principalement par le renvoi de projectile
    /// </summary>
    /// <param name="character"></param>
    /// <param name="attackName"></param>
    public void ReInitAttack(CharacterBase character, string attackName)
    {
        tag = character.tag;
        user = character;
        attackID = attackName;
        playerHitList.Clear();
    }

    public void ActionActive()
    {
        playerHitList.Clear();
        hitBox.enabled = true;
    }

    public void ActionUnactive()
    {
        hitBox.enabled = false;
    }

    public void CancelAction()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.EndComponent(user);
        }
    }

    public void AddPlayerHitList(string targetTag)
    {
        playerHitList.Add(targetTag);
    }

    public bool IsInHitList(string targetTag)
    {
        return playerHitList.Contains(targetTag);
    }





    public void Hit(CharacterBase target)
    {
        //if (BattleManager.Instance.gameData.GameMode == GameModeStateEnum.Volley_Mode && target.gameObject.tag != "Ball")
        //    return;

        //if (target.TeamID == TeamEnum.No_Team || target.TeamID != user.TeamID)
        //{
            string targetTag = target.transform.root.tag;

            if (!playerHitList.Contains(targetTag))
            {
                playerHitList.Add(targetTag);
                if (onHitColliderEvents != null && !eventReceived)
                {
                    // Event pour eviter le multi hit
                    onHitColliderEvents.Invoke(targetTag);
                    // Event qui envoie le user et la target quand hit
                    if (playerHitEvent != null)
                    {
                        playerHitEvent.Raise(user, target);
                    }
                    eventReceived = true;
                }

                foreach (AttackComponent atkC in atkCompList)
                {
                    atkC.OnHit(user, target);
                }

                user.Action.HasHit(target);
            }
        //}
    }

    public void Guard(CharacterBase target)
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            // Si la garde ne repousse pas on doit le signaler pour les composants
            atkC.OnGuard(user, target, !guardWin);
        }
    }

    public void Parry(CharacterBase target)
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.OnParry(user, target);
        }
    }

    public void Clash(CharacterBase target)
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.OnClash(user, target);
        }
    }



    AttackSubManager attackClashed;
    public AttackSubManager AttackClashed
    {
        get { return attackClashed; }
    }


    // Clash
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;
        if (noClash == true)
            return;

        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            //if (atkMan.User.TeamID == user.TeamID && atkMan.User.TeamID != TeamEnum.No_Team) // Pour emp?cher les joueurs dans la m?me ?quipes de clash
            //    return;

            if (atkMan.noClash == true)
                return;

            attackClashed = atkMan;
            user.Knockback.ContactPoint = (atkMan.HitBox.bounds.center + user.CenterPoint.position) * 0.5f;
            atkMan.User.Knockback.RegisterHit(this);
        }

    }

}
