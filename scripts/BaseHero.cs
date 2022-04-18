using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

[Tool]
public class BaseHero : Spatial {
	
	private const int Maxlevel = 5;

	public enum HeroTypes {
		Melee,
		Range,
		Mage,
		Mannequin
	}
	private HeroTypes _heroType = HeroTypes.Mannequin;
	public float AttackSpeed { get; set; }
	public float AttackRange { get; set; }
	public float Damage { get; set; }
	public int Level { get; set; }
	
	[Export()] // for test now
	public HeroTypes HeroType {
		get => _heroType;
		set {
			_heroType = value;
			ChangeHeroType();
		}
	}
	public bool IsMaxLevel { get; set; }


	private readonly Dictionary _myData = new Dictionary() {
		{"HeroType", HeroTypes.Mannequin}, //melee range mage
		{"AttackSpeed", 10.0 },
		{"AttackRange", 10.0 },
		{"Damage", 10.0 },
		{"Level", 1 },
	};

	private DragAndDrop _dragAndDrop = new DragAndDrop();
	public override void _Ready() {
		InitiateHero(_myData);
	}

	public void InitiateHero(Dictionary data) {
		HeroType = (HeroTypes) data["HeroType"] == HeroTypes.Mannequin ? HeroType : (HeroTypes) data["HeroType"];
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
		Material correctMaterial =
			ResourceLoader.Load<Material>($"res://materials/test/mannequin_matirial_{HeroType}.tres");
		if (GetNode("hero/skeleton/mesh") is MeshInstance myMesh && correctMaterial != null) {
			if (myMesh.GetActiveMaterial(0) != correctMaterial)
				myMesh.SetSurfaceMaterial(0, correctMaterial);
		}
		
	}

	public void _on_DragableObject_DragMove(Node node, Dictionary cast) {
		var pos = (Vector3) cast["position"];
		Translation = new Vector3(pos.x, Translation.y, pos.z);
		
	}
	
}
