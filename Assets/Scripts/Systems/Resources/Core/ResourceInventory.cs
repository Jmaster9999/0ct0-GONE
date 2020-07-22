﻿//Copyright Jesse Rougeau, 2020 ©

using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    [SerializeField] private ResourceModule ResourceManager = null;
    
    [SerializeField] private List<Resource> ActiveResources = new List<Resource>();
    public List<Resource> Resources{get =>ActiveResources;}

    [SerializeField] private bool UseOverrides = false;

    [SerializeField] private List<ResourceModule.ResourceData> OverrideValues;

    [SerializeField] private GameObject resourceAddedPopTxt = null;

    void Awake()
    {
        List<Resource> OverridenResources;
        if (UseOverrides)
        {
            OverridenResources = new List<Resource>();
            foreach (var OverrideData in OverrideValues)
            {
                if (!ActiveResources.Contains(OverrideData.resource)) Debug.LogError("Cannot override InActive Resource!");
                OverridenResources.Add(OverrideData.resource);
                ResourceManager.CreateResourceInstance(OverrideData,this);
            }
            foreach (var resource in ActiveResources)
            {
                if (!OverridenResources.Contains(resource))
                {
                    ResourceManager.CreateResourceInstance(resource,this); //create resource with default values
                }
            }
        }
        else
        {
            foreach (var resource in ActiveResources)
            {
                ResourceManager.CreateResourceInstance(resource,this); //create resource with default values
            }
        }
    }

    public bool CheckIfResourceList()
    {
        if(ActiveResources != null)
        {
            return true;
        }
        return false;
    }

    public List<Resource> GetActiveResourceList()
    {
        return ActiveResources;
    }

    public bool HasResource(Resource resourceToCheck)
    {
        return ActiveResources.Contains(resourceToCheck);
    }

    public void TryAdd(Resource resource, float amount)
    {
        //Debug.Log(!HasResource(resource));
        if (!HasResource(resource)) {
            ResourceManager.CreateResourceInstance(resource,this);//create the resource if it isn't present
            ActiveResources.Add(resource);
        }
        AddResource(resource,amount);
    }

    public void RemoveResource(Resource resource, float amount)
    {
        resource.SubInstanceValue(this,amount);
    }

    public void AddResource(Resource resource, float amount)
    {
        resource.AddInstanceValue(true, this,amount);
    }

    public void GenerateResourceText()
    {

    }

    public bool CanAdd(Resource resource, float amount)
    {
        return resource.CanAdd(this,amount);
    }


    public void SetResource(Resource resource, float amount)
    {
        resource.SetInstanceValue(this,amount);
    }

    public float GetResource(Resource resource)
    {
        if (HasResource(resource))
        {
            return resource.GetInstanceValue(this);
        }
        return 0;
    }


    // Update is called once per frame
    //void Update()
    //{
    //    //for viewing health
    //    if (3 <= ActiveResources.Count && this.name != "OctoHub")
    //    {
    //        Debug.Log(this.ToString() + ActiveResources[2].GetInstanceValue(this));
    //    }
    //}

    private void OnDestroy()//possibly implement cleanup in this function (May create race condition/coupling issues)
    {
        
    }
}

