using UnityEngine;

public abstract class BaseBehaviorScript
{
    public abstract void Movement();
    public abstract void Ability();
    //testing 
    public abstract void Update(); //in subclasses of Entity, this update method needs to be called
}
