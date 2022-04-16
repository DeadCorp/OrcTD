using Godot;
using System;
using System.Linq;
using Godot.Collections;

public class BaseHero : Spatial {
    
    public string HeroType { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float Damage { get; set; }
    public int Level { get; set; }

    private Dictionary _myData = new Dictionary() {
        {"HeroType", "Melee"},
        {"AttackSpeed", 10.0 },
        {"AttackRange", 10.0 },
        {"Damage", 10.0 },
        {"Level", 1 },
    };
    public override void _Ready() {
        InitiateHero(_myData);
    }

    public void InitiateHero(Dictionary data) {
        HeroType = (string) data["HeroType"];
        AttackSpeed = (float) data["AttackSpeed"];
        AttackRange = (float) data["AttackRange"];
        Damage = (float) data["Damage"];
        Level = (int) data["Level"];

        GD.Print($"HeroType : {HeroType} AttackSpeed : {AttackSpeed} AttackRange : {AttackRange} Damage : {Damage} Level : {Level}");
    }
}
