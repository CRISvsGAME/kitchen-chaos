using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private SOKitchenObject _kitchenObjectData;

    private IKitchenObjectParent _kitchenObjectParent;

    public SOKitchenObject GetKitchenObjectData()
    {
        return _kitchenObjectData;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        _kitchenObjectParent = kitchenObjectParent;
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return _kitchenObjectParent;
    }
}
