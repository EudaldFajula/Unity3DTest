using UnityEngine;

public class FlareGunScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject FlareGunFloor;
    [SerializeField] private GameObject FlareGunHand;

    public void Interact()
    {
        FlareGunFloor.SetActive(false);
        FlareGunHand.SetActive(true);
    }
}
