using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private SOKitchenObject kitchenObjectData;

    public SOKitchenObject GetKitchenObjectData()
    {
        return kitchenObjectData;
    }
}
