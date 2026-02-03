using MoreMountains.Feedbacks;
using UnityEngine;

public interface IReactive
{
    public abstract void ReactToTheBeat(MMF_Player p);

    public void ChangeBeatsPerMinute();
}

//
// public class Door : MonoBehaviour, IInteractable
// {
//     public void Interact(Transform interactor)
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public string GetInteractableName()
//     {
//         throw new System.NotImplementedException();
//     }
//
//     public Transform GetInteractableTransform()
//     {
//         throw new System.NotImplementedException();
//     }
// }