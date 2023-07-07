using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class AmmoText : MonoBehaviour
{
    private TMP_Text _text;
    private HoldWeapon hold;
    
    private Transform _player;
    [Inject]
    private void Construct(Player player)
    {
        hold = player.GetComponent<HoldWeapon>();
    }

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = hold.SelectedWeapon.type == WeaponType.Fists? "" : hold.SelectedWeapon.ammo.ToString();
    }
}
