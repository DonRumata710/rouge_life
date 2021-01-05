using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastObject : MonoBehaviour
{
    static private float speed = 10.0f;
    static public float effectLength = 6.0f;

    public Effect effect;
    public GameObject visualEffect;

    private GameObject caster;
    private Transform target;

    public void SetCastData(GameObject spell_caster, Transform spell_target)
    {
        caster = spell_caster;
        target = spell_target;
    }

    public GameObject GetCaster()
    {
        return caster;
    }

    private void Update()
    {
        if (target == null)
        {
            OnTriggerEnter(null);
        }
        
        float delta = speed * Time.deltaTime;
        Vector3 moving = transform.position + (target.position - transform.position).normalized * delta;
        transform.position = moving;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider != null)
        {
            CommonCharacterController character = collider.gameObject.GetComponent<CommonCharacterController>();
            if (character != null)
                character.ApplyEffect(caster, effect);
        }

        Destroy(gameObject);

        if (visualEffect != null)
        {
            GameObject visual_effect = Instantiate(visualEffect, transform);
            Destroy(visual_effect, effectLength);
        }
    }
}
