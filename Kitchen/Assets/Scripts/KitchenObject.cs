using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParents kitchenObjectParents;
    
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return  kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParents kitchenObjectParents)
    {
        if (this.kitchenObjectParents != null)
        {
            this.kitchenObjectParents.ClearKitchenObject();
        }
        
        this.kitchenObjectParents = kitchenObjectParents;

        if (kitchenObjectParents.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParents already has a KitchenObject");
        }
        kitchenObjectParents.SetKitchenObject(this);
        
        transform.parent = kitchenObjectParents.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParents GetKitchenObjectParent()
    {
        return kitchenObjectParents;
    }

    public void DestroySelf()
    {
        kitchenObjectParents.ClearKitchenObject();
        Destroy(gameObject);
    }


    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParents kitchenObjectParents)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParents);
        
        return kitchenObject;
    }
    
}
