using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterParameters", menuName = "Characters/ParametersSet")]
public class CharacterParametersSet : ScriptableObject
{
    public const string HEALTH = "health";
    public const string MAX_HEALTH = "max health";
    public const string ARMOR = "armor";
    public const string DAMAGE = "damage";
    public const string ATTACK_FREQUENCY = "attack frequency";
    public const string ATTACK_DISTANCE = "action distance";
    public const string SPEED = "speed";
    public const string CROUCH_MODIFICATOR = "crouch modificator";
    public const string OTHER = "other";


    public class EffectsSet : Dictionary<string, List<Effect>>
    {
        public EffectsSet()
        {
            Add(HEALTH, new List<Effect>());
            Add(MAX_HEALTH, new List<Effect>());
            Add(ARMOR, new List<Effect>());
            Add(DAMAGE, new List<Effect>());
            Add(ATTACK_FREQUENCY, new List<Effect>());
            Add(ATTACK_DISTANCE, new List<Effect>());
            Add(SPEED, new List<Effect>());
            Add(CROUCH_MODIFICATOR, new List<Effect>());
            Add(OTHER, new List<Effect>());
        }

        public void ApplyEffects(Effect[] new_effects)
        {
            foreach (Effect effect in new_effects)
            {
                ApplyEffect(effect);
            }
        }

        public void ApplyEffect(Effect effect)
        {
            try
            {
                this[effect.parameter].Add(effect);
            }
            catch (KeyNotFoundException)
            {
                this[OTHER].Add(effect);
            }
        }
    }


    public string character_name;

    public int max_health = 10;

    public int armor = 0;

    public int right_hand_damage = 1;
    public float right_hand_attack_frequency = 1.0f;
    public float right_hand_action_distance = 2.0f;

    public int left_hand_damage = 0;
    public float left_hand_attack_frequency = 1.0f;
    public float left_hand_action_distance = 2.0f;

    public float speed = 4.0f;
    public float croach_modificator = 0.5f;


    public void UseEffects(EffectsSet effects)
    {
        List<Effect> effect_list = effects[MAX_HEALTH];
        foreach (Effect effect in effect_list)
            max_health += (int)effect.effect;

        effect_list = effects[ARMOR];
        foreach (Effect effect in effect_list)
            armor += (int)effect.effect;

        effect_list = effects[DAMAGE];
        foreach (Effect effect in effect_list)
        {
            right_hand_damage += (int)effect.effect;
            left_hand_damage += (int)effect.effect;
        }

        float base_right_hand_attack_frequency = right_hand_attack_frequency;
        float base_left_hand_attack_frequency = left_hand_attack_frequency;

        effect_list = effects[ATTACK_FREQUENCY];
        foreach (Effect effect in effect_list)
        {
            right_hand_attack_frequency += (int)(effect.effect * base_right_hand_attack_frequency);
            left_hand_attack_frequency += (int)(effect.effect * base_left_hand_attack_frequency);
        }

        float base_right_hand_action_distance = right_hand_action_distance;
        float base_left_hand_action_distance = left_hand_attack_frequency;

        effect_list = effects[ATTACK_DISTANCE];
        foreach (Effect effect in effect_list)
        {
            right_hand_action_distance += (int)(effect.effect * base_right_hand_action_distance);
            left_hand_action_distance += (int)(effect.effect * base_left_hand_action_distance);
        }
    }
}
