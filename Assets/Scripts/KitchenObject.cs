using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private SOKitchenObject _kitchenObjectData;

    private ClearCounter _clearCounter;

    public SOKitchenObject GetKitchenObjectData()
    {
        return _kitchenObjectData;
    }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        _clearCounter = clearCounter;
    }

    public ClearCounter GetClearCounter()
    {
        return _clearCounter;
    }
}
