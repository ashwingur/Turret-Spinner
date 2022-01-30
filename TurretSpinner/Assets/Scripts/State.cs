using System.Collections;
using System.Collections.Generic;

public class State<T>
{
    protected T machine;
    
    public State(T machine)
    {
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
