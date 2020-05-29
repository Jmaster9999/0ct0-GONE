﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : ScriptableObject
{
    [SerializeField] public bool CanTick;
    [SerializeField] public long TickRate;

    public virtual bool ConditionCheck(GameFrameworkManager GameManager)
    {
        return true;
    }

    public virtual bool TransitionCheck(GameFrameworkManager GameManager)
    {
        return true;
    }

    public abstract void OnActivate(GameState LastState);

    public abstract void OnDeactivate(GameState NewState);

    public abstract void Reset();

    public virtual void OnUpdate(){}

    public virtual void Initalize(){}
}
