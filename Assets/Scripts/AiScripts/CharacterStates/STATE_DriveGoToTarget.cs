using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterLogic;
using StateMachineLogic;
using UnityEngine;

public class STATE_DriveGoToTarget : AIState
{

    Transform target;
    float startDistance;
    float reachedDistance;

    CharacterBrain characterBrain;

    VehicleObject vehicle;

    public STATE_DriveGoToTarget(CharacterBrain brain,VehicleObject _vehicle, Transform _target, float _reachedDistance) : base(brain.character)
    {
        target = _target;
        characterBrain = brain;
        vehicle = _vehicle;
        reachedDistance = _reachedDistance;
        startDistance = Vector3.Distance(characterBrain.transform.position, target.position);
        
    }

    public override void EnterState()
    {
        base.EnterState();
        CarAiController aiController = vehicle.gameObject.GetComponent<CarAiController>();
        aiController.SetupNavigation(characterBrain,target);
        aiController.StartEngine();
    }
}

