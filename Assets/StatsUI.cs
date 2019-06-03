using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public Slider aggression;
    public Slider speed;
    public Slider defense;
    public Text armorType;

    bool getStats = false;

    public void CollectStats()
    {
        getStats = true;
    }

    private void LateUpdate()
    {
        if (getStats)
        {
            GetStatsNextFrame();
            getStats = false;
        }
    }

    void GetStatsNextFrame()
    {
        Bot bot = BotConfiguator.singleton.activeBot.GetComponent<Bot>();
        Debug.Log("dam Mult:" + bot.damageMultiplier_Rock + ";" + bot.damageMultiplier_Paper + ";" + bot.damageMultiplier_Scissors);
        if ((bot.damageMultiplier_Paper > bot.damageMultiplier_Rock) && (bot.damageMultiplier_Paper > bot.damageMultiplier_Scissors))
        {
            armorType.text = "Rock";
        }
        else if ((bot.damageMultiplier_Rock > bot.damageMultiplier_Paper) && (bot.damageMultiplier_Rock > bot.damageMultiplier_Scissors))
        {
            armorType.text = "Scissors";
        }
        else if ((bot.damageMultiplier_Scissors > bot.damageMultiplier_Paper) && (bot.damageMultiplier_Scissors > bot.damageMultiplier_Rock))
        {
            armorType.text = "Paper";
        }
        else
        {
            armorType.text = "Neutral";
        }

        float aggro = (bot.fireRate + (bot.salveCount / bot.salveCooldown))*(1f+bot.damage);
        aggression.value = aggro;
        Debug.Log("agr: " + aggro);
        float spd = (bot.speed / bot.spurtDuration);
        speed.value = spd;
        Debug.Log("spd: " + spd);
        float def = bot.maxHealth + bot.weight;
        defense.value = def;
        Debug.Log("def: " + def);
    }

}
