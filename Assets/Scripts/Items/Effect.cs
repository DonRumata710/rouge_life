using UnityEngine;


[CreateAssetMenu(fileName = "Magic", menuName = "Effect")]
public class Effect : ScriptableObject
{
	public string effectName;
    public string parameter;
    public float effect = 0;

    public float duration = float.PositiveInfinity;

    public Effect ()
	{}

	public Effect (string param, int value)
	{
        parameter = param;
		effect = value;
	}
}
