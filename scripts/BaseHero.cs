using Godot;
using Godot.Collections;

[Tool]
public class BaseHero : Spatial {
	private const int MaxLevel = 5;

	public enum HeroTypes {
		Melee,
		Range,
		Mage,
		Mannequin
	}

	private HeroTypes _heroType = HeroTypes.Mannequin;
	public float Speed { get; set; }
	public float AttackRange { get; set; }
	public float Damage { get; set; }

	public int Level {
		get => _level;
		set {
			_level = value;
			var label = GetNodeOrNull<Label>("Sprite3D/Viewport/Label");
			if (label != null) label.Text = $"{value}";
		}
	}

	[Export()] // for test now
	public HeroTypes HeroType {
		get => _heroType;
		set {
			_heroType = value;
			ChangeHeroType();
		}
	}

	public bool IsMaxLevel { get; set; }
	
	//[Export()]
	private Dictionary MyData { get; set; } = new Dictionary() {
		{"HeroType", HeroTypes.Mannequin}, //melee range mage
		{"Speed", 10.0},
		{"AttackRange", 10.0},
		{"Damage", 10.0},
		{"Level", 1},
	};

	private bool _lvlup = false;
	[Export()]
	public bool F {
		get => _lvlup;
		set {
			_lvlup = value;
			LevelUp();
			_lvlup = false;
			
		} 
	}

	public bool AvailableToMerge {
		get => _availableToMerge;
		set {
			_availableToMerge = value;
			var indicator = GetNodeOrNull<CPUParticles>("MergeIndicator");
			if (indicator != null) indicator.Emitting = value;
		}
	}

	private bool _availableToMerge = false;
	private int _level;

	public readonly string myGroupName = "Heroes";

	private HeroCombatLogic _combatLogic;

	public override void _Ready() {
		InitiateHero(MyData);
		var myDragableObject = GetNodeOrNull<DragableObject>("PickUpArea/DragableObject");
		
		var dragableArea = GetNodeOrNull<Area>("DragArea");
		var dragLogic = new HeroDragAndDropLogic(this, dragableArea);
	
		myDragableObject?.Connect("DragStart", dragLogic, nameof(dragLogic.OnDragStart));
		myDragableObject?.Connect("DragStop", dragLogic, nameof(dragLogic.OnDragStop));
		myDragableObject?.Connect("DragMove", dragLogic, nameof(dragLogic.OnDrag));

		_combatLogic = new HeroCombatLogic(this);

	}

	private void InitiateHero(Dictionary data) {
		HeroType = (HeroTypes) data["HeroType"] == HeroTypes.Mannequin ? HeroType : (HeroTypes) data["HeroType"];
		Speed = (float) data["Speed"];
		AttackRange = (float) data["AttackRange"];
		Damage = (float) data["Damage"];
		Level = (int) data["Level"];

		AddToGroup(myGroupName);
	}
	public bool LevelUp() {
		if (Level == MaxLevel) return false;
		Speed *= 1.35f;
		AttackRange *= 1.15f;
		Damage *= 2.0f;

		Level++;
		if (Level == MaxLevel) 
			IsMaxLevel = true;
		
		MyData["Speed"] = Speed;
		MyData["AttackRange"] = AttackRange;
		MyData["Damage"] = Damage;
		MyData["Level"] = Level;

		
		return true;

	}
	private void ChangeHeroType() {
		Material correctMaterial =
			ResourceLoader.Load<Material>($"res://materials/test/mannequin_matirial_{HeroType}.tres");
		if (GetNode("mesh") is MeshInstance myMesh && correctMaterial != null) {
			if (myMesh.GetActiveMaterial(0) != correctMaterial)
				myMesh.SetSurfaceMaterial(0, correctMaterial);
		}
		
	}

	
	
}

