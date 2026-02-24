using UnityEngine;

public class ClearCounter : MonoBehaviour, IInteractable, IKitchenObjectParent
{
    [SerializeField] private SOKitchenObject _kitchenObjectData;
    [SerializeField] private Transform _kitchenObjectFollowTransform;
    private KitchenObject _kitchenObject;

    // #########################################################################
    // IKitchenObjectParent Implementation
    // #########################################################################

    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectFollowTransform;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    // #########################################################################
    // IInteractable Implementation
    // #########################################################################

    public void Interact(Player player)
    {
        if (_kitchenObject == null)
        {
            if (player.HasKitchenObject())
            {
                _kitchenObject = player.GetKitchenObject();
                _kitchenObject.SetKitchenObjectParent(this);
                player.ClearKitchenObject();
            }
            else
            {
                Transform kitchenObjectTransform = Instantiate(_kitchenObjectData.prefab, _kitchenObjectFollowTransform);
                _kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
                _kitchenObject.SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                player.SetKitchenObject(_kitchenObject);
                _kitchenObject.SetKitchenObjectParent(player);
                _kitchenObject = null;
            }
        }
    }
}
