using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAbilities : AbilitiesBehavior
{
    private PlayerManager playerManager;
    private Dictionary<string, IAbility> abilityMapping; //idk this is the best way i could think of mapping inputs to abilities
    public PlayerAbilities(Dictionary<string, IAbility> am, PlayerManager pm) //abilities should be instantiated from the playermanager class
    {
        playerManager = pm;
        abilityMapping = am;
        abilities = abilityMapping.Values.ToList();
    }
    public override void UseAbilities(Entity entity) //every frame this is being ran in the super class update of PlayerManager, checking if ability buttons are pressed
    {
        foreach (string abilityButton in abilityMapping.Keys) {
            if (InputManager.buttonMap[abilityButton].PressedThisFrame())
            {
                abilityMapping[abilityButton].Execute(entity);
            }
        }
    }
}

