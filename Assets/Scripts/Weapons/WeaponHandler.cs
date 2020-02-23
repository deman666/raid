using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{

    Animator animator;
    [System.Serializable]
    public class UserSettings
    {
        public Transform rightHand;
        public Transform pistolUnequipSpot;
        public Transform rifleUnequipSpot;
    }
    [SerializeField]
    public UserSettings userSettings;

    [System.Serializable]
    public class Animations
    {
        public string weaponTypeInt = "WeaponType";
        public string reloadingBool = "isReloading";
        public string aimingBool = "Aiming";
    }
    [SerializeField]
    public Animations animations;
    public Weapon currentWeapon;
    public List<Weapon> weaponsList = new List<Weapon>();
    public int maxWeapons = 2;
    bool aim;
    bool reload;
    int weaponType;
    bool settingWeapon;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
        if (currentWeapon)
        {
            currentWeapon.SetEquipped(true);
            currentWeapon.SetOwner(this);
            AddWeaponToList(currentWeapon);

            if (currentWeapon.ammo.clipAmmo <= 0)
                Reload();
        }
        if (reload)
        {
            if (settingWeapon)
            {
                reload = false;
            }
        }
        if (weaponsList.Count > 0)
        {
            for(int i = 0; i < weaponsList.Count; i++)
            {
                if (weaponsList[i] != currentWeapon)
                {
                    weaponsList[i].SetEquipped(false);
                    weaponsList[i].SetOwner(this);
                }
            }
        }
    }

    //Animates character
    void Animate()
    {
        if (!animator)
            return;
        // #Adrian temp comment-out
        animator.SetBool(animations.aimingBool, aim);
        animator.SetBool(animations.reloadingBool, reload);
        animator.SetInteger(animations.weaponTypeInt, weaponType);

        if (!currentWeapon)
        {
            weaponType = 0;
            return;
        }
        switch (currentWeapon.weaponType)
        {
            case Weapon.WeaponType.Pistol:
                weaponType = 1;
                break;
            case Weapon.WeaponType.Rifle:
                weaponType = 2;
                break;
        }
    }

    //Adds a weapon to the weaponsList
    void AddWeaponToList(Weapon weapon)
    {
        if (weaponsList.Contains(weapon))
            return;
        weaponsList.Add(weapon);
    }

    public void FingerOnTrigger(bool pulling)
    {
        if (!currentWeapon)
            return;

        currentWeapon.PullTrigger(pulling && !reload);
    }

    public void Reload()
    {
        if (reload || !currentWeapon)
            return;
        if (currentWeapon.ammo.carryingAmmo <= 0 || currentWeapon.ammo.clipAmmo == currentWeapon.ammo.maxClipAmmo)
            return;

        reload = true;
        StartCoroutine(StopReload());
    }
    IEnumerator StopReload()
    {
        yield return new WaitForSeconds(currentWeapon.weaponSettings.reloadDuration);
        currentWeapon.LoadClip();
        reload = false;
    }

    public void Aim(bool aiming)
    {
        aim = aiming;
    }

    //Drop current weapon
    public void DropCurrentWeapon()
    {
        if (!currentWeapon)
            return;
        currentWeapon.SetEquipped(false);
        currentWeapon.SetOwner(null);
        weaponsList.Remove(currentWeapon);
        currentWeapon = null;
    }

    //Switches to next weapon
    public void SwtichWeapons()
    {
        if (settingWeapon)
            return;

        if (currentWeapon)
        {
            int currentWeaponIndex = weaponsList.IndexOf(currentWeapon);
            int nextWeaponIndex = (currentWeaponIndex + 1) % weaponsList.Count;

            currentWeapon = weaponsList[nextWeaponIndex];
        }
        else
        {
            currentWeapon = weaponsList[0];
        }
        settingWeapon = true;
        StartCoroutine(StopSettingWeapon());
    }
    IEnumerator StopSettingWeapon()
    {
        yield return new WaitForSeconds(0.7f);
        settingWeapon = false;
    }
    
}
