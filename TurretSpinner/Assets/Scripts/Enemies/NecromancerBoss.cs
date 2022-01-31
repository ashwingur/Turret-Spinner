using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBoss : Enemy
{
    [SerializeField] private float idleRotationSpeed;
    [SerializeField] private int summonAmount;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float minIdleTime;
    [SerializeField] private Transform minionPrefab;
    private State<NecromancerBoss> currentState;

    private State<NecromancerBoss> summonState;
    private State<NecromancerBoss> idleState;
    private State<NecromancerBoss> movingState;

    private void Awake()
    {
        idleState = new IdleState(this);
        summonState = new SummonState(this);
        movingState = new MovingState(this);
        currentState = idleState;
    }

    private class IdleState: State<NecromancerBoss>
    {
        float idleTimeLeft;

        public IdleState(NecromancerBoss machine): base(machine) { }

        public override void Enter()
        {
            idleTimeLeft = Random.Range(machine.minIdleTime, machine.maxIdleTime);
        }

        public override void Update()
        {
            idleTimeLeft -= Time.deltaTime;
            if (idleTimeLeft <= 0)
            {
                machine.ChangeState(machine.movingState);
            }
            machine.transform.Rotate(new Vector3(0, 0, machine.idleRotationSpeed * Time.deltaTime));
        }

    }

    private class SummonState : State<NecromancerBoss>
    {
        public SummonState(NecromancerBoss machine) : base(machine) { }

        public override void Enter()
        {
            machine.SummonMinions();
            machine.ChangeState(machine.idleState);
        }
    }

    private class MovingState : State<NecromancerBoss>
    {
        private float xTarget;
        private float yTarget;

        public MovingState(NecromancerBoss machine) : base(machine) { }

        public override void Enter()
        {
            // Choose a random location to go to (a bit of padding added so it doesnt go straight to the walls
            xTarget = Random.Range(-PlayerMovement.horizontalBorder + 2, PlayerMovement.horizontalBorder + 2);
            yTarget = Random.Range(-PlayerMovement.verticalBorder + 2, PlayerMovement.verticalBorder + 2);
        }

        public override void Update()
        {
            Vector3 target = new Vector3(xTarget, yTarget, 0);
            machine.MoveToLocation(target, true);

            if (Vector3.Distance(machine.transform.position, target) < 0.3f)
            {
                machine.ChangeState(machine.summonState);
            }
        }

    }

    protected override void Update()
    {
        currentState.Update();
    }

    private void ChangeState(State<NecromancerBoss> state)
    {
        currentState = state;
        currentState.Enter();
    }

    private void SummonMinions()
    {
        for (int i = 0; i < summonAmount; i++)
        {
            Instantiate(minionPrefab, transform.position, transform.rotation);
        }
    }


}


