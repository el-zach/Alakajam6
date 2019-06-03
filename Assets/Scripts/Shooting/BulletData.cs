using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BulletData : ScriptableObject {
    public float speed = 25f;
    public float damage = 1f;
    public DamageType damageType;
    public float lifeTime = 1.5f;
    public float colliderSize = 0.2f;
    public float mass = 1f;
    public Mesh graphic;
    public Material material;
    public bool useGravity = false;
}

public enum DamageType { None, Rock, Paper, Scissors}