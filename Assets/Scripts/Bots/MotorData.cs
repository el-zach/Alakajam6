using UnityEngine;

[CreateAssetMenu]
public class MotorData : PartData
{

    public float speed = 1f;
    public float fireRate = 1f;
    public float rotationSpeed = 1f;

    public void ApplyMotorMultiplier(Bot bot)
    {
        bot.speed *= speed;
        bot.spurtDuration *= (1/speed);
        bot.fireRate *= fireRate;
        bot.salveCooldown *= 1 / fireRate;
        bot.rotationalSpeed *= rotationSpeed;
    }

}