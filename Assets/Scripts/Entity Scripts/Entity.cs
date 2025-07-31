using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Entity : MonoBehaviour
{
    /* State machine for subclasses who will have different abilities and movement based on entity state */
    public StateManager stateManager;

    #region Entity Physics
    /* It is important that the rigidbody component on an entity has interpolatation setting turned to interpolate, and that gravity is disabled since we're
    using our own */
    public Rigidbody rb;
    /* Strength of gravity uh DUH */
    [SerializeField] private float gravityStrength;
    /* Current gravity will be one of these enum values and change how gravity is applied to an entity */
    public enum GravityState
    {
        Regular,
        Tube
    }
    [SerializeField] private GravityState currentGravity;
    /* What direction gravitational force is being applied in */
    private Vector3 gravityDirection;
    /* How fast the player rotates to align with its current terrain */
    [SerializeField] private float rotationSpeed;
    /* Current Terrain is typically tube segments that have a line of gravity to be applied from their MainTerrain script */
    [SerializeField] private GameObject currentTerrain;

    #endregion
    #region Entity Stats
    [SerializeField] private float moveSpeed;
    private int health;
    #endregion

    #region Behaviors
    public IMovementBehavior movementBehavior;
    public AbilitiesBehavior abilitiesBehavior; //i made this an abstract class instead of an interface xd it just made a lil more sense
    #endregion

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (currentGravity == GravityState.Tube)
        {
            UpdateGravityVector();
            SurfaceAlignment();
        }
        abilitiesBehavior?.UseAbilities(this);
    }

    protected virtual void FixedUpdate()
    {
        ApplyGravity();
        movementBehavior?.Move(this);
    }

    void ApplyGravity()
    {
        //UpdateGravityVector();
        switch (currentGravity)
        {
            case GravityState.Regular:
                rb.AddForce(Vector3.down * gravityStrength, ForceMode.Acceleration);
                break;
            case GravityState.Tube:
                rb.AddForce(gravityDirection.normalized * gravityStrength, ForceMode.Acceleration);
                break;
        }
    }

    /* This method updates the gravityDirection variable to point towards the line of gravity on the currentTerrain from where the entity is currently */
    private void UpdateGravityVector()
    {
        Vector3 playerToOrigin = transform.position - currentTerrain.transform.position;
        Vector3 lineOfGravity = currentTerrain.GetComponent<MainTerrain>().lineOfGravity;
        Vector3 pointOfGravity = Vector3.Project(playerToOrigin, lineOfGravity) + currentTerrain.transform.position;

        Vector3 forceOfGravity = (pointOfGravity - transform.position);
        gravityDirection = forceOfGravity;
    }

    /* Aligns the transform of an entity to be standing upright in terms of the gravityDirection vector. (gravity points down irl, and you stand upwards)*/
    private void SurfaceAlignment()
    {
        Vector3 up = -gravityDirection.normalized;
        //Vector3 forward = Vector3.ProjectOnPlane(transform.forward, GetGravityVector().normalized).normalized;
        Vector3 forward = currentTerrain.GetComponent<MainTerrain>().lineOfGravity.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(forward, up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    #region Getters
    public float MoveSpeed()
    {
        return moveSpeed;
    }

    public GravityState GetGravityState()
    {
        return currentGravity;
    }

    public GameObject GetCurrentTerrain()
    {
        return currentTerrain;
    }
    
    public Vector3 GetGravityDirection()
    {
        return gravityDirection;
    }
    #endregion
}
