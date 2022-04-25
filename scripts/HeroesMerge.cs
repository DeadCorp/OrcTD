using Godot;
using System;
using Array = Godot.Collections.Array;

public class HeroesMerge : Node
{

    public override void _Ready()
    {
        
    }

    public void OnHeroesMerge(Node node, Array collided) {
        
        var heroArea = collided[0] as Area;
        var hero = heroArea?.GetParentOrNull<BaseHero>();
        GD.Print(hero, collided);
        if (hero == null) {
            return;
        }
        else {
            var feedHero = node as BaseHero;
            if (hero.HeroType == feedHero?.HeroType && hero.Level == feedHero.Level) {
                if (hero.LevelUp())
                    node.QueueFree();
            }
        }
    }

    public void OnHeroesStartMerge(Node node) {
        var feedHero = node as BaseHero;
        foreach (var child in GetChildren()) {
            if (child is BaseHero hero && hero != feedHero)  {
                if (hero.HeroType == feedHero?.HeroType && hero.Level == feedHero.Level) {
                    hero.AvailableToMerge = true;
                }
            }
        }
    }

    public void OnHeroesEndMerge(Node node) {
        foreach (var child in GetChildren()) {
            if (child is BaseHero hero) {
                hero.AvailableToMerge = false;
            }
        }
    }
}
