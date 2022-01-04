using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stats
{
    int health;

    CharacterParametersSet dynamic_params = ScriptableObject.CreateInstance<CharacterParametersSet>();
    CharacterParametersSet base_params = ScriptableObject.CreateInstance<CharacterParametersSet>();


    public event CommonCharacterController.SimpleEvent OnDeath;


    public float AttackDistance
    {
        get
        {
            float attack_distance = dynamic_params.right_hand_action_distance;
            if (dynamic_params.left_hand_damage > 0 && attack_distance > dynamic_params.left_hand_action_distance)
                attack_distance = dynamic_params.left_hand_action_distance;
            return attack_distance;
        }
    }

    public int RightHandDamage
    {
        get
        {
            return dynamic_params.right_hand_damage;
        }
    }

    public float RightHandFrequency
    {
        get
        {
            return dynamic_params.right_hand_attack_frequency;
        }
    }

    public int LeftHandDamage
    {
        get
        {
            return dynamic_params.left_hand_damage;
        }
    }

    public float LeftHandFrequency
    {
        get
        {
            return dynamic_params.left_hand_attack_frequency;
        }
    }


    public int MaxHealth
    {
        get
        {
            return dynamic_params.max_health;
        }
    }

    public int Armor
    {
        get
        {
            return dynamic_params.armor;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            Debug.Log("Health changed to " + value + " " + base_params.character_name);

            if (health <= 0  && OnDeath != null)
                OnDeath();
        }
    }

    public float Speed
    {
        get
        {
            return dynamic_params.speed;
        }
    }

    public float CroachModificator
    {
        get
        {
            return dynamic_params.croach_modificator;
        }
    }

    public CharacterParametersSet EquipmentParameters
    {
        set
        {
            value.character_name = base_params.character_name;
            base_params = value;
            CalcEffects();
        }
    }


    CharacterParametersSet.EffectsSet effects = new CharacterParametersSet.EffectsSet();

    
	public EquipmentSet equipments;


    public Stats(CharacterParametersSet param)
    {
        base_params = param;
        health = param.max_health;
    }

    
    public void EffectTact(float delta)
    {
        List<Effect> effect_list = effects[CharacterParametersSet.HEALTH];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];
            health += (int)(effect.effect * delta);

            effect.duration -= delta;
            if (effect.duration <= 0.0)
                effect_list.RemoveAt(i);
        }

        effect_list = effects[CharacterParametersSet.MAX_HEALTH];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];

            effect.duration -= delta;
            if (effect.duration <= 0.0)
            {
                effect_list.RemoveAt(i);
                effects[effect.parameter].Remove(effect);
            }
        }

        effect_list = effects[CharacterParametersSet.ARMOR];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];

            effect.duration -= delta;
            if (effect.duration <= 0.0)
            {
                effect_list.RemoveAt(i);
                effects[effect.parameter].Remove(effect);
            }
        }

        effect_list = effects[CharacterParametersSet.DAMAGE];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];

            effect.duration -= delta;
            if (effect.duration <= 0.0)
            {
                effect_list.RemoveAt(i);
                effects[effect.parameter].Remove(effect);
            }
        }

        effect_list = effects[CharacterParametersSet.ATTACK_FREQUENCY];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];

            effect.duration -= delta;
            if (effect.duration <= 0.0)
            {
                effect_list.RemoveAt(i);
                effects[effect.parameter].Remove(effect);
            }
        }

        effect_list = effects[CharacterParametersSet.ATTACK_DISTANCE];
        for (int i = 0; i < effect_list.Count; ++i)
        {
            Effect effect = effect_list[i];

            effect.duration -= delta;
            if (effect.duration <= 0.0)
            {
                effect_list.RemoveAt(i);
                effects[effect.parameter].Remove(effect);
            }
        }

        CalcEffects();
    }

    void CalcEffects()
    {
        ResetEquipmentEffects();

        dynamic_params.UseEffects(effects);
    }
    
    void ResetEquipmentEffects()
    {
        dynamic_params.max_health = base_params.max_health;
        dynamic_params.armor = base_params.armor;
        dynamic_params.right_hand_damage = base_params.right_hand_damage;
        dynamic_params.left_hand_damage = base_params.left_hand_damage;
        dynamic_params.right_hand_attack_frequency = base_params.right_hand_attack_frequency;
        dynamic_params.left_hand_attack_frequency = base_params.left_hand_attack_frequency;
        dynamic_params.right_hand_action_distance = base_params.right_hand_action_distance;
        dynamic_params.left_hand_action_distance = base_params.left_hand_action_distance;
    }


    public void ApplyEffect(Effect effect)
    {
        if (effect.parameter == CharacterParametersSet.HEALTH && effect.duration == 0)
        {
            Health += (int)effect.effect;
        }
        else
        {
            effects.ApplyEffect(effect);
            CalcEffects();
        }
    }


	public bool IsWeaponTwoHanded (Weapon weapon)
	{
        // TODO: check if weapon is twohanded
		return false;
	}


    public bool CheckCompability(Equipment equipment)
    {
        // TODO: add param checking
        return true;
    }
}
