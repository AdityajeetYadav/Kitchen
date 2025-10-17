using System;
using UnityEditor.Rendering;
using UnityEngine;

public class ClearCounter : BaseCounterClass
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen Object Is Here
            if (player.HasKitchenObject())
            {
                //Player Carrying Something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //Player not Carrying anything
            }
        }
        else
        {
            // There is a Kitchen Object
            if (player.HasKitchenObject())
            {
                //Player is Carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    else
                    {
                        // Player is not carrying the plate but something else
                        if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                        {
                            //Counter is holding a plate
                            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                            {
                                player.GetKitchenObject().DestroySelf();
                            }
                        }
                    }
                }
            }
            else
            {
                //Player is not Carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
