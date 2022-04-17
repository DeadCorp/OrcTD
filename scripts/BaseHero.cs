using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

public class BaseHero : Spatial {
    
    private const int Maxlevel = 5;
    
    private string _heroType = "no_type";
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float Damage { get; set; }
    public int Level { get; set; }
    
    [Export()] // for test now
    public string HeroType {
        get => _heroType;
        set {
            _heroType = value;
            ChangeHeroType();
        }
    }
    public bool IsMaxLevel { get; set; }


    private readonly Dictionary _myData = new Dictionary() {
        {"HeroType", "range"}, //melee range mage
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

        GD.Print($" hero : {Name} {{ \n \t HeroType : {HeroType} \n \t AttackSpeed : {AttackSpeed} \n \t AttackRange : {AttackRange}  \n \t Damage : {Damage} \n \t Level : {Level} \n }}");
    }

    private void LevelUp() {
        if (Level == Maxlevel) return;
        AttackSpeed *= 1.35f;
        AttackRange *= 1.15f;
        Damage *= 2.0f;

        Level++;
        if (Level == Maxlevel) 
            IsMaxLevel = true;
        
    }
    
    private void ChangeHeroType() {
        if (IsValidType(HeroType)) {
            Material correctMaterial =
                ResourceLoader.Load<Material>($"res://materials/test/mannequin_matirial_{HeroType}.tres");
            if (GetNode("hero/skeleton/mesh") is MeshInstance myMesh && correctMaterial != null) {
                if (myMesh.GetActiveMaterial(0) != correctMaterial)
                    myMesh.SetSurfaceMaterial(0, correctMaterial);
            }
        }
    }

    private bool IsValidType(string heroType) {
        
        var types = new List<string>() {"melee", "range", "mage", "no_type"};
        foreach (var type in types) {
            if (type == heroType) return true;
        }

        return false;
    }
}
