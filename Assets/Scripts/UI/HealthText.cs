using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class HealthText : MonoBehaviour
{
    private TMP_Text _text;
    private HasHealth _health;
    
    private Transform _player;
    [Inject]
    private void Construct(Player player)
    {
        _health = player.GetComponent<HasHealth>();
    }

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _health.healthChanged.AddListener(_ => HealthChanged());
        HealthChanged();
    }

    private void HealthChanged()
    {
        _text.text = _health.health + "%";
    }
}
