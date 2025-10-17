using System;
using UnityEngine;

public class PlatesCounter : BaseCounterClass
{
    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;
    
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    
    

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is Empty Handed
            if (platesSpawnedAmount > 0)
            {
                //There's atleast one plate here
                platesSpawnedAmount--;
                
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                
                OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
