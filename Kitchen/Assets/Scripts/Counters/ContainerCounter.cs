using System;
using UnityEngine;

public class ContainerCounter : BaseCounterClass
{
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public event EventHandler OnPlayerGrabbedObject;
    
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is not Carrying Anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
 
}
