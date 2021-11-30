using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    protected bool canMoveCancel = false;
    public bool CanMoveCancel
    {
        get { return canMoveCancel; }
    }

    // Le dernier personnage qu'on a touch�
    protected CharacterBase characterHit;
    public CharacterBase CharacterHit
    {
        get { return characterHit; }
    }


    // Utilis� pour g�rer le bug d'animation event si on cancel frame perfect
    protected bool endAction = false;
    protected bool canEndAction = false;

    protected CharacterBase character;
    protected AttackManager attackID; // Les combo/target combo partage le meme attackID
    protected AttackManager currentAttackManager;
    public AttackManager CurrentAttackManager
    {
        get { return currentAttackManager; }
    }

    [SerializeField]
    Animator animator;

    public EventAttackManager OnAttack;
    public EventVoid OnAttackActive;

    // � virer 
    public Animator Animator
    {
        get { return animator; }
    }

    public bool movementCancelable = false;


    public void InitializeComponent(CharacterBase c)
    {
        character = c;
    }



    // A patcher pour prendre en compte les attack conditions
    public bool CanAct()
    {
        if (currentAttackManager != null && canMoveCancel == false)
            return false;
        //if (currentAttackManager != null && canMoveCancel == true/* && characterHit == null*/)
        //{
        //    return false;
        //}
        return true;
    }

    private AttackManager CheckCombo(AttackManager attack)
    {
        if (currentAttackManager != null)
        {
            if (attack == attackID)
            {
                if (currentAttackManager.AtkCombo != null)
                {
                    return currentAttackManager.AtkCombo;
                }
            }
        }
        return attack;
    }






    public bool Action(AttackManager attack)
    {
        if (CanAct() == false)
            return false;
        if (attack.CanUseAttack(character) == false)
            return false;

        endAction = false;
        canEndAction = false;
        canMoveCancel = false;
        characterHit = null;

        // Combo
        AttackManager attackToInstantiate = CheckCombo(attack);
        attackID = attack;

        // Animation de l'attaque
        animator.Play(attackToInstantiate.AttackAnim.name, 0, 0f);

        // On cr�er l'attaque et �a setup diff�rent param�tres
        if (currentAttackManager != null)
            currentAttackManager.CancelAction();
        currentAttackManager = Instantiate(attackToInstantiate, this.transform.position, Quaternion.identity);
        currentAttackManager.CreateAttack(character);

        OnAttack?.Invoke(currentAttackManager);

        return true;
    }



    // Cancel l'action mais ne reset pas le state
    public void CancelAction()
    {
        if (currentAttackManager != null)
            currentAttackManager.CancelAction();
        currentAttackManager = null;
        attackID = null;

        canMoveCancel = false;
        canEndAction = false;
        endAction = false;
        movementCancelable = false;
    }



    // Termine l'action et retourne en �tat idle
    public void FinishAction()
    {
        CancelAction();

        character.ResetToIdle();
    }





    // Appel� par les anims
    public void ActionActive(int subAttack = 0)
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionActive(subAttack);
            OnAttackActive?.Invoke();
        }
    }

    // Appel� par les anims
    public void ActionUnactive(int subAttack = 0)
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionUnactive(subAttack);
        }
    }
    public void ActionAllActive()
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionAllActive();
            OnAttackActive?.Invoke();
        }
    }

    // Appel� par les anims
    public void ActionAllUnactive()
    {
        if (currentAttackManager != null)
        {
            currentAttackManager.ActionAllUnactive();
        }
    }

    // Appel� par les anims
    // Cr�er une subaction de l'attaque (Si l'attaque n'a pas de subaction, ne fais rien)
    /* public void SubAction(int nb)
     {
         if (currentAttackManager != null)
         {
             //currentAttackManager.SubAction(nb);
         }
     }*/

    // Appel� par les anims
    public void MoveCancelable()
    {
        canMoveCancel = true;
    }

    // Appel� par les anims
    // active le bool pour Cancel l'action � la frame suivante via EndActionState
    public void EndAction()
    {
        if (canEndAction == true && currentAttackManager != null)
        {
            endAction = true;
        }
    }







    // Appel� par le State pour g�rer les cancel d'animation frame perfect
    public void CanEndAction()
    {
        if (canEndAction == false)
            canEndAction = true;
    }

    // Cancel l'action si le bool end action est toujours valid�
    public void EndActionState()
    {
        if (endAction == true)
        {
            FinishAction();
        }
    }




    // Appel� par les attack controller
    public void HasHit(CharacterBase target)
    {
        characterHit = target;
        // Event
    }




    public void SetAttackMotionSpeed(float newValue)
    {
        animator.speed = newValue;
        if (currentAttackManager != null)
            currentAttackManager.SetMotionSpeed(newValue);
    }
}
