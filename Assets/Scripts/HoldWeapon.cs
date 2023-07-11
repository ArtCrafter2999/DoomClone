using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum WeaponType
{
    Fists,
    Pistol,
    Shotgun,
    Chainsaw
}

public class HoldWeapon : MonoBehaviour
{
    [Serializable]
    public class Weapon
    {
        public KeyCode keyCode;
        public WeaponType type;
        public int ammo;
        public bool Available => type == WeaponType.Fists || ammo > 0;
        public AudioClip sound;
        public float reloadSpeed;
        public float damage;
    }

    [SerializeField] private AudioSource source;

    [FormerlySerializedAs("weaponSet")] [SerializeField]
    private List<Weapon> weaponSeter;

    private readonly Dictionary<WeaponType, Weapon> _weaponSet = new Dictionary<WeaponType, Weapon>();
    public Weapon SelectedWeapon;

    [Header("Shoot")] [SerializeField] private new Camera camera;
    [SerializeField] private GameObject bulletHole;
    [SerializeField] private Animator animator;
    [SerializeField] private float reloadSpeed = 2f;

    private float _reloadCoolDown = 0;

    private void Awake()
    {
        weaponSeter.ForEach(w => _weaponSet.Add(w.type, w));
        SelectedWeapon = _weaponSet[WeaponType.Fists];
    }

    private void Update()
    {
        if (!SelectedWeapon.Available)
        {
            try
            {
                ChangeWeapon(_weaponSet.Values.First(w => w.Available && w.type != WeaponType.Fists));
            }
            catch (Exception)
            {
                ChangeWeapon(_weaponSet[WeaponType.Fists]);
            }
        }

        foreach (var keyWeapon in _weaponSet.Values.Where(keyWeapon =>
                     Input.GetKeyDown(keyWeapon.keyCode) && keyWeapon.Available))
        {
            ChangeWeapon(keyWeapon);
        }

        if (Input.GetMouseButton(0) && _reloadCoolDown <= 0) Attack();
        _reloadCoolDown -= Time.deltaTime;
    }

    public void PickUpWeapon(WeaponType type, int ammo)
    {
        _weaponSet[type].ammo += ammo;
        ChangeWeapon(_weaponSet[type]);
    }

    private void ChangeWeapon(Weapon weapon)
    {
        SelectedWeapon = weapon;
        animator.SetInteger("Type", (int)SelectedWeapon.type);
        animator.SetTrigger("Change");
    }

    private void Attack()
    {
        if (camera == null) return;
        if (SelectedWeapon.sound != null && (SelectedWeapon.type != WeaponType.Chainsaw || !source.isPlaying)) source.PlayOneShot(SelectedWeapon.sound);
        animator.SetTrigger("Attack");
        _reloadCoolDown = SelectedWeapon.reloadSpeed;
        SelectedWeapon.ammo--;

        Ray ray;
        RaycastHit hit;
        HasHealth health;        
        switch (SelectedWeapon.type)
        {
            case WeaponType.Fists:
                ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit, 1.5f) &&
                    hit.transform.TryGetComponent(out health))
                    health.TakeDamage(SelectedWeapon.damage);
                break;
            case WeaponType.Pistol:
                ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit) &&
                    hit.transform.TryGetComponent(out health))
                    health.TakeDamage(SelectedWeapon.damage);
                if (!hit.transform.TryGetComponent<Rigidbody>(out _))
                {
                    Instantiate(bulletHole, hit.point + hit.normal * 0.01f,
                        Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
                break;
            case WeaponType.Shotgun:
                var raysAmount = Random.Range(5, 7);
                for (int i = 0; i < raysAmount; i++)
                {
                    ray = camera.ViewportPointToRay(new Vector3(0.5f + Random.Range(-10, 10) * 0.01f, 0.5f, 0));
                    if (!Physics.Raycast(ray, out hit)) continue;

                    if (!hit.transform.TryGetComponent<Rigidbody>(out _))
                    {
                        Instantiate(bulletHole, hit.point + hit.normal * 0.01f,
                            Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }

                    if (!hit.transform.TryGetComponent(out health)) continue;
                    health.TakeDamage(SelectedWeapon.damage);
                }
                break;
            case WeaponType.Chainsaw:
                ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit, 1.5f) &&
                    hit.transform.TryGetComponent(out health))
                    health.TakeDamage(SelectedWeapon.damage);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}