using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitiesBehavior
{
    public List<IAbility> abilities = new(); //every entity can have a list of abilities
    public abstract void UseAbilities(Entity entity); //how that list of abilities is managed is controlled here?
}
