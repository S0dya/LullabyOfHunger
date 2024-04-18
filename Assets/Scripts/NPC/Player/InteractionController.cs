using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : SingletonSubject<InteractionController>
{
    [Header("settings")]
    public float InteractionAngle;

    //local
    List<Interaction> _interactions = new List<Interaction>();

    //threshold
    Interaction _curNearestInteraction;

    Vector3 _curDirection;
    float _curAngle;
    float _curDistance;

    float _curNearestDistance;

    protected override void Awake()
    {
        base.Awake();

        CreateInstance();
    }

    //outisde methods
    public void Interact()
    {
        if (_interactions.Count == 0) return;

        FindNearestInteraction();

        if (_curNearestInteraction == null) return;

        switch (_curNearestInteraction)
        {
            case InteractionDoor interactionDoor: interactionDoor.Interact(); break;
            case InteractionItem interactionItem: interactionItem.Interact(); break;
            default: break;
        }
    }

    public void AddInteraction(Interaction interaction) => _interactions.Add(interaction);
    public void RemoveInteraction(Interaction interaction) => _interactions.Remove(interaction);

    void FindNearestInteraction()
    {
        _curNearestInteraction = null; _curNearestDistance = float.MaxValue;

        foreach (var interaction in _interactions)
        {
            _curDirection = interaction.transform.position - transform.position; _curDirection.y = 0f;

            _curAngle = Vector3.Angle(transform.forward, _curDirection);

            if (_curAngle > InteractionAngle) continue;

            _curDistance = Vector3.Distance(interaction.transform.position, transform.position);

            if (_curDistance < _curNearestDistance)
            {
                _curNearestInteraction = interaction; _curNearestDistance = _curDistance;
            }
        }
    }
}
