﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMovement : MovementComponent
{
    
    [SerializeField] private float ThrusterImpulse;
    [SerializeField] private float VelocityMax = -1.0f;

    [SerializeField] private float ThrottleSensitivity = 1; //meters per s per frame

    private float _VelocityMax;

    private Rigidbody _Rigidbody;
    private float Mass;

    private void CoupledTranslate(MovementController controller, Vector3 Input)
    {
        TargetVelocity = Input * _VelocityMax;
    }

    private void CruiseTranslate(MovementController controller, Vector3 Input)
    {
        TargetVelocity += Input *ThrottleSensitivity;
    }


    public override void Initialize(MovementController Controller)
    {
        if (VelocityMax <= 0){
            _VelocityMax = 9999999;
        }
        if (ThrottleSensitivity <=0) ThrottleSensitivity = 0.001f;//minimum throttle sensitivity that can be set
        _Rigidbody = Controller.gameObject.GetComponent<Rigidbody>();
        Debug.Assert(_Rigidbody == null); //Assert if rigid body is undefined

    }

    private Vector3 CalculateImpulse(MovementController Controller)
    {
        Vector3 Impulse = new Vector3();
        float MaxDeltaV = ThrusterImpulse /_Rigidbody.mass;
        Vector3 DeltaV = TargetVelocity-_Rigidbody.velocity;
        for (int i = 0; i < 3; i++)
        {
            



        }


    }

    public override void Translate(MovementController Controller,Vector3 Input,byte MovementSubMode)
    {
        switch (MovementSubMode)
        {
            case 0://coupled mode
            {
                CoupledTranslate(Controller,Input);
                break;
            }
            case 1://cruise mode
            {
                CruiseTranslate(Controller,Input);
                break;
            }
        }
    }
    public override void MovementUpdate(MovementController Controller,byte MovementSubMode)
    {
        
    }
}
