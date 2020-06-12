﻿using UnityEngine;


[CreateAssetMenu(menuName = "Systems/Events/Resource Collection Event")]
public class ResourceCollectionEvent : Event
{
    public bool EventTrigger = false;
    [SerializeField] Resource CollectResource;
    [SerializeField] float ResourceAmount;
    [SerializeField] private string audioLog = null;
    [SerializeField] public GameObject soundPlayer = null;

    private float previousAmount = -1000f;
    private float totalAdded;

    public override bool Condition(GameObject target)
    {
        var currentAmount = target.GetComponent<ResourceInventory>().GetResource(CollectResource);
        
        //initialize previousAmount if we're looping through for the first time
        if(previousAmount == -1000f)
        {
            previousAmount = currentAmount;
        }

        //exit early if no change
        if (currentAmount == previousAmount)
        {
            return EventTrigger;
        }
        else
        {
            //find out if the amount we have now is higher than the previous amount to see if any resource was added
            var delta = currentAmount - previousAmount;
            if(delta > 0)
            {
                //if resource was added, add to the total added amount
                totalAdded = totalAdded + delta;
            }

            //update previous amount for next tick
            previousAmount = currentAmount;
        }

        //quest complete?
        if(totalAdded >= ResourceAmount)
        {
            Debug.Log("EVENT COMPLETE");
            EventTrigger = true;
            //reset the scriptableobject values
            totalAdded = 0;
            previousAmount = -1000f;
        }

        return EventTrigger;
    }

    protected override void Effect(GameObject target)
    {
        //no effect, this uses unity events for any effects
        if(audioLog != null)
        {
            AkSoundEngine.PostEvent(audioLog, soundPlayer);
        }
    }
}
