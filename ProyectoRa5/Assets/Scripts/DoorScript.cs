using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorScript : MonoBehaviour, IInteractable
{
    private Animator _animator;
    private bool _isOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void RotateDoor()
    {
        if (!_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void Interact()
    {
        RotateDoor();
    }
    public void OpenDoor()
    {
        _isOpen = true;
        _animator.SetTrigger("Open");
    }
    public void CloseDoor()
    {
        _isOpen = false;
        _animator.SetTrigger("Close");
    }
}
