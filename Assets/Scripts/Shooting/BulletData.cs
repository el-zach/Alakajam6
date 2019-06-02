using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BulletData : ScriptableObject {
    public float speed = 25f;
    public float damage = 1f;
    public float lifeTime = 1.5f;
    public float colliderSize = 0.2f;
}
