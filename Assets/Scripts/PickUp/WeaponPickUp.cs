using UnityEngine;

public class WeaponPickUp : PickUpBase
{
    [SerializeField] private WeaponType type;
    [SerializeField] private int ammo;
    protected override bool PickUp()
    {
        if (!Player.TryGetComponent(out HoldWeapon hold)) return false;
        hold.PickUpWeapon(type, ammo);
        return true;
    }
}