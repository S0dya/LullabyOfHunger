using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : SingletonSubject<InteractionController>
{

    //local
    List<Interaction> _interactions = new List<Interaction>();

    //threshold
    Interaction _curNearestInteraction;

    protected override void Awake()
    {
        base.Awake();

        CreateInstance();
    }

    //outisde methods
    public void Interact()
    {
        if (_interactions.Count == 0) return;

        _curNearestInteraction = _interactions.OrderBy(interaction => Vector3.Distance(interaction.transform.position, transform.position)).FirstOrDefault();

        switch (_curNearestInteraction)
        {
            case InteractionDoor interactionDoor: interactionDoor.Interact(); break;
            case InteractionItem interactionItem: interactionItem.Interact(); break;
            default: break;
        }
    }

    public void AddInteraction(Interaction interaction) => _interactions.Add(interaction);
    public void RemoveInteraction(Interaction interaction) => _interactions.Remove(interaction);
}
