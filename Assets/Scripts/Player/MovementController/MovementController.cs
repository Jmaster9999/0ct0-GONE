﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MovementController : MonoBehaviour
{

    
    private Vector3 _RawInput = new Vector3();

    private Vector3  _RotationTarget = new Vector3();
    private Vector3 _NormalizedInput= new Vector3();

    private Vector3 TargetVelocityLocal  = new Vector3(); //target velocity in m/s in world space

    private Vector3 TargetVelocityWorld = new Vector3(); //target velocity in m/s in local space

    private Vector3 TargetAngle  = new Vector3(); //target angular in degrees

    private byte MovementMode = 0;

    [SerializeField] private bool NormalizeInput = true;

    [SerializeField] private float ThrusterImpulse = 10;

    [SerializeField] private float ThrusterTorque = 10f;
    [SerializeField] private float VelocityMax = -1.0f;
    [SerializeField] private float ThrottleSensitivity = 1; //meters per s per frame

    
    [SerializeField] private float FuelPerImpulseUnit = 0.2f;
    [SerializeField] private float FuelPerTorqueUnit = 0.2f;
    [SerializeField] private float FuelEfficency = 100;
    [SerializeField] private float ScrollMult = 0.05f;

    [SerializeField] private Throttle ThrottleUI;
    
    
    private float _VelocityMax;
    private float _SetVelocityMax;
    public Vector3 Throttle = new Vector3();

  
    private float Mass;

    private float ScrollValue = 100;

    [SerializeField] private PlayerCamera _CameraScript;
    public PlayerCamera CameraScript{get=>_CameraScript;}
    [SerializeField] private Rigidbody _Rigidbody;
    [SerializeField] private ResourceInventory LinkedResourceBehavior;
    [SerializeField] private Resource FuelResource = null;





    public Vector3 RawInput{get => _RawInput;}
    public Vector3 NormalizedInput{get => _NormalizedInput;}

    public void SetRotationTarget(Vector3 NewRotationIn)
    {
        _RotationTarget = NewRotationIn;
    }

    public void OnHorizontalTranslate(InputValue value)
    {
        _RawInput.x = value.Get<Vector2>().x;
        _RawInput.z = value.Get<Vector2>().y;
    }
    public void OnVerticalTranslate(InputValue value)
    {
        _RawInput.y = value.Get<float>();
    }

    public void OnScroll(InputValue value)
    {
        
        ScrollValue += value.Get<float>()* ScrollMult;
        ScrollValue = Mathf.Clamp(ScrollValue,5,100);
        _SetVelocityMax = _VelocityMax * (ScrollValue/100);
        ThrottleUI.UpdateUI(ScrollValue, _SetVelocityMax); 
    }

    private void NormalizeInputs()
    {
        _NormalizedInput = _RawInput.normalized;
    }

    private void Start()
    {
        if (VelocityMax <= 0) 
        {
            _VelocityMax = 99999;
        }
        else
        {
            _VelocityMax = VelocityMax;
        }
        _SetVelocityMax = _VelocityMax;
        //used to get the starting maxvelocity - do not alter max velocity
        ThrottleUI.SetMaxVelocity(_VelocityMax);
        if (ThrottleSensitivity <=0) ThrottleSensitivity = 0.001f;//minimum throttle sensitivity that can be set
        Debug.Assert(_Rigidbody != null); //Assert if rigid body is undefined

    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        NormalizeInputs();
        if (NormalizeInput)
        {
            Translate(_NormalizedInput,MovementMode);
        }
        else
        {
            Translate(_RawInput,MovementMode);
        }
        Rotate(_RotationTarget);
        MovementUpdate(MovementMode);
    }


    private void CoupledTranslate( Vector3 Input)
    {
        TargetVelocityLocal = Input * _SetVelocityMax;
    }

    private void CruiseTranslate(Vector3 Input)
    {
        TargetVelocityLocal += Input *ThrottleSensitivity;

        for (int i = 0; i < 3; i++) //componentwise max speed clamp
        {
              TargetVelocityLocal[i] = Mathf.Clamp(TargetVelocityLocal[i], -_SetVelocityMax, _SetVelocityMax);
        }
      
    }



      private Vector3 CalculateImpulse()
    {
        Vector3 Impulse = new Vector3();
       
        //get the difference between our desired velocity and our current velocity (relative to the camera)
        Vector3 DeltaV = TargetVelocityLocal - CameraScript.GetRootTransform().InverseTransformVector(_Rigidbody.velocity); 
        float fuelUsage = 0;
        
        //calculate unadjusted impulse
        Impulse = _Rigidbody.mass * DeltaV;

       
        for (int i = 0; i < 3; i++)
        { //componentwise clamp of impulse to maximum thruster values
            Impulse[i] = Mathf.Clamp(Impulse[i],-ThrusterImpulse,ThrusterImpulse);

            Throttle[i] = Impulse[i]/ThrusterImpulse; //get the throttle value for sound and vfx
            fuelUsage += Mathf.Abs(Impulse[i])*(FuelPerImpulseUnit/FuelEfficency);
        }
        LinkedResourceBehavior.RemoveResource(FuelResource,fuelUsage);
        return Impulse;
    }

    public void Rotate(Vector3 Angle)
    {
        TargetAngle = Angle;
    }

    private Vector3 CalculateRotation()
    {
        Vector3 AngularVelocity = new Vector3();
        Vector3 DeltaAngle = (TargetAngle - transform.rotation.eulerAngles) * Mathf.Deg2Rad;
        AngularVelocity = (DeltaAngle * ThrusterTorque)/50;
        return AngularVelocity;
    }


    //private CalculateVelocityChange(MovementController Controller)
    //{
    //}

    private void ApplyTorque()
    {
        Vector3 Torques = new Vector3();
        float AngleDelta = 0;
        float fuelUsage = 0;        
        var TargetRot = new Quaternion();
        TargetRot.eulerAngles = TargetAngle;
        var DeltaRot = Quaternion.Inverse(_Rigidbody.rotation)*TargetRot;

        for (int i = 0; i < 3; i++)
        {
            AngleDelta = DeltaRot.eulerAngles[i];
            if (AngleDelta > 180) AngleDelta = -((AngleDelta)-180);
            Torques[i] = Mathf.Clamp(AngleDelta,-ThrusterTorque,ThrusterTorque);
            fuelUsage += Mathf.Abs(Torques[i])*(FuelPerTorqueUnit/FuelEfficency);
        }

        _Rigidbody.AddRelativeTorque(new Vector3(1,0,0) * Torques.x,ForceMode.Impulse);
        _Rigidbody.AddRelativeTorque(new Vector3(0,1,0) * Torques.y,ForceMode.Impulse);
        _Rigidbody.AddRelativeTorque(new Vector3(0,0,1) * Torques.z,ForceMode.Impulse);
        LinkedResourceBehavior.RemoveResource(FuelResource,fuelUsage);
    }


    public void Translate(Vector3 Input,byte MovementSubMode)
    {
        switch (MovementSubMode)
        {
            case 0://coupled mode
            {
                CoupledTranslate(Input);
                break;
            }
            case 1://cruise mode
            {
                CruiseTranslate(Input);
                break;
            }
        }
    }

    public void OnFrameUpdate()
    {
    
    }


    public void MovementUpdate(byte MovementSubMode)
    {
        _Rigidbody.AddRelativeForce(CalculateImpulse(), ForceMode.Impulse);
        ApplyTorque();
    }

}
