using UnityEngine;

[CreateAssetMenu]
public class MantleData : PartData
{
    public float defenseRock=1f, defensePaper=1f, defenseScissors=1f;

    public void ApplyMultiplier(Bot _bot)
    {
        _bot.damageMultiplier_Rock *= (1f / defenseRock);
        _bot.damageMultiplier_Paper *= (1f / defensePaper);
        _bot.damageMultiplier_Scissors *= (1f / defenseScissors);
    }

}