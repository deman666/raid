using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Weapon : MonoBehaviour
{
    private Camera aimingCamera;

    Collider col;
    Animator animator;

    public enum WeaponType
    {
        Pistol, Rifle
    }
    public WeaponType weaponType;

    [System.Serializable]
    public class UserSettings
    {
        public Transform LeftHandIKTarget;
        public Vector3 spineRotation;
    }
    [SerializeField]
    public UserSettings userSettings;

    [System.Serializable]
    public class WeaponSettings
    {
        [Header("-Bullet Options-")]
        public Transform bulletSpawn;
        public float damage = 5f;
        public float bulletSpread = 5f;
        public float fireRate = 0.2f;
        public float range = 200f;

        [Header("-Effects-")]
        public GameObject bullet;
        public GameObject clip;

        [Header("-Other-")]
        public float reloadDuration = 2f;
        public GameObject clipGO;

        [Header("-Positioning-")]
        public Vector3 equipPosition;
        public Vector3 equipRotation;
        public Vector3 unEquipPosition;
        public Vector3 unEquipRotation;

        [Header("-Animation-")]
        public bool useAnimation;
        public int fireAnimationLayer = 0;
        public string fireAnimationName = "Fire";
    }
    [SerializeField]
    public WeaponSettings weaponSettings;

    [System.Serializable]
    public class Ammintion
    {
        public int carryingAmmo;
        public int clipAmmo;
        public int maxClipAmmo;
    }
    [SerializeField]
    public Ammintion ammo;

    WeaponHandler owner;
    bool equiped;
    bool pullingTrigger;
    bool resettingCartridge;

    void Start()
    {
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        aimingCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner)
        {
            DisableEnableComponents(false);
            if (equiped)
            {
                if (owner.userSettings.rightHand)
                {
                    Equip();
                    if (pullingTrigger)
                    {
                        Fire();
                    }
                }
            }
            else
            {
                UnEquip(weaponType);
            }
        }
        else
        {
            DisableEnableComponents(true);
            transform.SetParent(null);
        }
    }
    //Fires the weapon

    void Fire()
    {
        if (ammo.clipAmmo <= 0 || resettingCartridge || !weaponSettings.bulletSpawn)
            return;
        
        #region bullet spawn
        if (weaponSettings.bullet)
        {
            GameObject bullet = Instantiate(weaponSettings.bullet, weaponSettings.bulletSpawn.transform.position, weaponSettings.bulletSpawn.transform.rotation);
            BulletAdrian bulletAdrian = bullet.GetComponent<BulletAdrian>();

            Vector3 hitPoint = CalcHitPoint();

            hitPoint += (Vector3)Random.insideUnitCircle * weaponSettings.bulletSpread; // spread

            bulletAdrian.Init(weaponSettings.bulletSpawn.transform.position, hitPoint);
        }
        #endregion
        if (weaponSettings.useAnimation)
            animator.Play(weaponSettings.fireAnimationName, weaponSettings.fireAnimationLayer);

        ammo.clipAmmo--;
        resettingCartridge = true;
        StartCoroutine(LoadNextBullet());
    }
    Vector3 CalcHitPoint()
    {
        Transform cam = aimingCamera.transform;
        //int layerMask = LayerMask.NameToLayer("TestRaycastLayerMask");
        int layerMask = LayerMask.GetMask(new string[] { "TestRaycastLayerMask" });
        layerMask = ~layerMask;
        bool isHit = Physics.Raycast(cam.position + cam.forward * 1f, cam.forward, out RaycastHit hitInfo, weaponSettings.range, layerMask);
        if (isHit)
        {
            return hitInfo.point;
        }
        else
        {
            return cam.position + cam.forward * (1f + weaponSettings.range);
        }
    }
    IEnumerator LoadNextBullet()
    {
        yield return new WaitForSeconds(weaponSettings.fireRate);
        resettingCartridge = false;
    }
    void DisableEnableComponents(bool enabled)
    {
        if (!enabled)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }
    }
    void Equip()
    {
        if (!owner)
            return;
        else if (!owner.userSettings.rightHand)
            return;

        transform.SetParent(owner.userSettings.rightHand);
        transform.localPosition = weaponSettings.equipPosition;
        Quaternion equipRot = Quaternion.Euler(weaponSettings.equipRotation);
        transform.localRotation = equipRot;
    }
    void UnEquip(WeaponType wpType)
    {
        if (!owner)
            return;
        switch(wpType){
            case WeaponType.Pistol:
                transform.SetParent(owner.userSettings.pistolUnequipSpot);
            break;
            case WeaponType.Rifle:
            transform.SetParent(owner.userSettings.rifleUnequipSpot);
            break;
        }
        transform.localPosition = weaponSettings.unEquipPosition;
        Quaternion unEquipRot = Quaternion.Euler(weaponSettings.unEquipRotation);
        transform.localRotation = unEquipRot;
    }
    public void LoadClip()
    {
        int ammoNeeded = ammo.maxClipAmmo - ammo.clipAmmo;

        if(ammoNeeded >= ammo.carryingAmmo)
        {
            ammo.clipAmmo = ammo.carryingAmmo;
            ammo.carryingAmmo = 0;
        }
        else
        {
            ammo.carryingAmmo -= ammoNeeded;
            ammo.clipAmmo = ammo.maxClipAmmo;
        }
    }
    //Sets the weapons equip state
    public void SetEquipped(bool equip)
    {
        equiped = equip;

    }
    // pullse the trigger
    public void PullTrigger(bool isPulling)
    {
        pullingTrigger = isPulling;
    }
    public void SetOwner(WeaponHandler wp)
    {
        owner = wp;
    }
    private void OnDrawGizmos()
    {
        Vector3 hitPoint = CalcHitPoint();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint, 0.5f);

        Transform cam = aimingCamera.transform;
        Gizmos.DrawLine(cam.position + cam.forward * 1f, hitPoint);
    }
}
