using EcsDemo;
using ME.BECS;
using ME.BECS.Views;
using TMPro;
using UnityEngine;

public class HeroView : EntityView
{
    [SerializeField] private TMP_Text _health;

    protected override void ApplyState(in EntRO ent)
    {
        gameObject.name = ent.Read<Name>().Value.ToString();
        _health.text = ent.Read<Health>().Value.ToString();
    }
}