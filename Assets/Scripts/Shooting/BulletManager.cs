using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager singleton;
    private void Awake()
    {
        if (!singleton) singleton = this;
        else Debug.Log("Too many BulletManager");
    }

    public GameObject bulletPrefab;
    public List<GameObject> useableBullets = new List<GameObject>();

    public void InstantiateBullet(BulletData _bullet, Vector3 _pos, Quaternion _rot,float _damage, GameObject _owner)
    {
        _bullet.damage = _damage;
        if (useableBullets.Count != 0)
        {
            //use old bullet
            useableBullets[0].transform.position = _pos;
            useableBullets[0].transform.rotation = _rot;

            SetupBullet(useableBullets[0],_bullet,_owner);

            useableBullets[0].SetActive(true);
            useableBullets.RemoveAt(0);
        }
        else
        {
            //create new Bullet
            GameObject newBullet = Instantiate(bulletPrefab, _pos, _rot, transform);
            SetupBullet(newBullet,_bullet,_owner);
        }
    }

    void SetupBullet(GameObject _bulletObject,BulletData _bullet, GameObject _owner)
    {
        var bulletScript = _bulletObject.GetComponent<Bullet>();
        bulletScript.data = _bullet;
        bulletScript.SetMeUp();
        //LayerMask.NameToLayer("PlayerBullets")

        Collider coll = _owner.GetComponent<Collider>(); //_owner.GetComponentInParent<Collider>();
        if(coll==null)
            coll = _owner.GetComponentInChildren<Collider>();
        if (coll == null)
            coll = _owner.GetComponentInParent<Collider>();
        Physics.IgnoreCollision(_bulletObject.GetComponent<Collider>(), coll);

        //_bulletObject.layer = _owner == Owner.Player ? 14 : _owner == Owner.Enemy ? 15 : 0;
        
    }
    
}

public enum Owner { Default, Player, Enemy}
