using UnityEngine;

[CreateAssetMenu(fileName = "SOKitchenObject", menuName = "Scriptable Objects/SOKitchenObject")]
public class SOKitchenObject : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
