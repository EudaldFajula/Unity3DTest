using System.Collections.Generic;
using UnityEngine;

public class InteractBehavior : MonoBehaviour
{
    private readonly List<IInteractable> interactables = new();

    public void Interact()
    {
        // Limpia referencias destruidas
        interactables.RemoveAll(i => i == null);

        if (interactables.Count == 0)
            return;

        IInteractable closest = null;
        float minDist = float.MaxValue;

        foreach (var i in interactables)
        {
            var mb = (MonoBehaviour)i;
            float dist = Vector3.Distance(transform.position, mb.transform.position);

            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }

        closest?.Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            if (!interactables.Contains(interactable))
                interactables.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
            interactables.Remove(interactable);
    }
}
