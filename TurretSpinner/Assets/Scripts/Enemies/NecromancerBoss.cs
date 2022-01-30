using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBoss : Enemy
{
    [SerializeField] private float idleRotationSpeed;
    private State<NecromancerBoss> currentState;

    private State<NecromancerBoss> summonState;
    private State<NecromancerBoss> idleState;

    private class IdleState: State<NecromancerBoss>
    {
        public IdleState(NecromancerBoss machine): base(machine) { }

    }

    private class SummonState : State<NecromancerBoss>
    {
        public SummonState(NecromancerBoss machine) : base(machine) { }
    }

    private void Awake()
    {
        idleState = new IdleState(this);
        summonState = new SummonState(this);
        currentState = idleState;
    }

    // Update is called once per aframe
    void Update()
    {
        
    }
}


