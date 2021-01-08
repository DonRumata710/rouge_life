using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EquipmentSet))]
[RequireComponent(typeof(Motor))]
public class CommonCharacterController : MonoBehaviour
{
    public enum Action
    {
        NONE,
        ATTACK,
        CAST,
        TALK
    }

    public delegate void SimpleEvent();
    public delegate void CommunicationEvent(CommonCharacterController character);

    public event CommunicationEvent OnAttacked;
    public event CommunicationEvent OnAttack;
    public event SimpleEvent OnDeath;

    public string characterType;


    public Stats Parameters
    {
        get;
        private set;
    }

    public EquipmentSet equipment {
        private set;
        get;
    }

    public Inventory inventory {
        private set;
        get;
    }


    public void SetAction(Action act)
    {
        if (action == act)
            action = Action.NONE;
        else
            action = act;
        Target = null;
    }

    public void MakeDamage(int damage, CommonCharacterController subject)
    {
        if (OnAttacked != null)
            OnAttacked(subject);

        Parameters.Health -= (int)damage;
    }

    public bool CheckCompability(Equipment equipment)
    {
        return Parameters.CheckCompability(equipment);
    }

    public void ApplyEffect(GameObject caster, Effect effect)
    {
        Debug.Log("Applying effect " + effect.effectName + " to " + characterType);
        Parameters.ApplyEffect(effect);

        if (effect.effect < 0.0f)
        {
            OnAttacked(caster.GetComponent<CommonCharacterController>());
        }
    }


    protected Animator anim;
    protected Action action = Action.NONE;

    protected float ActionDistance
    {
        get
        {
            switch (action)
            {
                case Action.ATTACK:
                    return Parameters.AttackDistance;
                case Action.CAST:
                    return 20.0f;
                case Action.TALK:
                    return 2.0f;
                default:
                    return 2.0f;
            }
        }
    }

    protected Motor Motor
    {
        private set;
        get;
    }


    protected virtual void Start()
    {
        Motor = GetComponent<Motor>();
        anim = GetComponent<Animator>();
        Parameters = new Stats(CharacterManager.instance.GetParameters(characterType));

        equipment = GetComponent<EquipmentSet>();
        inventory = GetComponent<Inventory>();

        Parameters.OnDeath += Death;
        equipment.CalcEquipment();
    }

    protected virtual void Update()
    {
        right_hand_attack_timeout -= Time.deltaTime;
        left_hand_attack_timeout -= Time.deltaTime;
        cast_time -= Time.deltaTime;

        if (Target != null && action != Action.NONE)
        {
            if (Vector3.Distance(Target.gameObject.transform.position, transform.position) < ActionDistance)
            {
                switch (action)
                {
                    case Action.ATTACK:
                        if (Target != null)
                            Attack();
                        break;
                    case Action.CAST:
                        if (Target != null)
                            Cast();
                        break;
                    case Action.TALK:
                        Talk();
                        break;
                    case Action.NONE:
                        break;
                }
            }
            else
            {
                Motor.FollowTarget(Target.gameObject, ActionDistance);
            }
        }
    }

    private void FixedUpdate()
    {
        Parameters.EffectTact(Time.fixedDeltaTime);
    }


    private CommonCharacterController target;
    private GameObject targetObject;


    protected GameObject TargetObject
    {
        get
        {
            return targetObject;
        }
    }


    public CommonCharacterController Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;

            if (target == null)
                return;

            targetObject = value.gameObject;
            if (action == Action.ATTACK)
            {
                Target.OnDeath += ResetTarget;
            }
        }
    }

    public void SetTarget(GameObject obj)
    {
        targetObject = obj;
        Target = obj.GetComponent<CommonCharacterController>();
    }

    protected void ResetTarget()
    {
        Target = null;
        action = Action.NONE;
    }

    protected virtual void ReactOnEnemyDeath() { }


    float right_hand_attack_timeout = 0.0f;
    float left_hand_attack_timeout = 0.0f;


    void Attack()
    {
        if (OnAttack != null)
            OnAttack(Target);

        if (right_hand_attack_timeout <= 0.0f)
        {
            anim.SetTrigger("attack");
            Target.MakeDamage(Parameters.RightHandDamage, this);
            right_hand_attack_timeout = 1.0f / Parameters.RightHandFrequency;
        }
        if (left_hand_attack_timeout <= 0.0f && equipment.rightHand.item is Weapon)
        {
            anim.SetTrigger("left hand attack");
            Target.MakeDamage(Parameters.LeftHandDamage, this);
            left_hand_attack_timeout = 1.0f / Parameters.LeftHandFrequency;
        }
    }


    float cast_time = 0.0f;

    public Spell CurrentSpell
    {
        get
        {
            return spell;
        }
        set
        {
            spell = value;
            if (spell)
            {
                cast_time = value.readingTime;
                action = Action.CAST;
            }
            else
            {
                action = Action.NONE;
            }
        }
    }

    private Spell spell;

    void Cast()
    {
        if (cast_time <= 0.0f && CurrentSpell != null)
        {
            if (CurrentSpell.moveForCast)
            {
                Vector3 offset = new Vector3(2.0f, 1.0f, 0.0f);
                GameObject cast_object = Instantiate(CurrentSpell.castObject);
                cast_object.transform.position = transform.position + offset;

                CastObject cast = cast_object.GetComponent<CastObject>();
                cast.effect = CurrentSpell.effect;
                cast.SetCastData(gameObject, Target.transform);
            }
            else
            {
                GameObject cast_object = Instantiate(CurrentSpell.resultVisualEffect, Target.transform.position, new Quaternion());
                Target.Parameters.ApplyEffect(CurrentSpell.effect);
                Destroy(cast_object, CastObject.effectLength);
            }

            anim.SetTrigger("cast");
            CurrentSpell = null;
        }
    }


    public event CommunicationEvent OnDialog;


    private void Talk()
    {
        Debug.Log("Hisohiso");
        if (OnDialog != null)
            OnDialog(Target);
        action = Action.NONE;
    }


    void Death()
    {
        Debug.Log(characterType + " is dead");

        if (OnDeath != null)
            OnDeath();

        SetAction(Action.NONE);
        anim.SetTrigger("death");
        Motor.enabled = false;
        Destroy(gameObject, 6.0f);
    }
}
