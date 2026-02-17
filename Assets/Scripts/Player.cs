using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    private const float EPSILON = 1e-5f;
    private const float PLAYER_HEIGHT = 2f;
    private const float PLAYER_RADIUS = 0.7f;
    private const int MAX_ITERATIONS = 2;

    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private LayerMask _counterLayerMask;

    [SerializeField] private Transform _kitchenObjectFollowTransform;
    private KitchenObject _kitchenObject;

    private bool _isWalking;
    private IInteractable _currentInteractable;

    public void Start()
    {
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        _currentInteractable?.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    private void OnDestroy()
    {
        _gameInput.OnInteractAction -= GameInput_OnInteractAction;
    }

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
    // Interaction and Movement Handling
    // #########################################################################

    private void HandleInteraction()
    {
        float interactionDistance = 2f;
        bool hit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactionDistance, _counterLayerMask);

        IInteractable interactable = hit ? hitInfo.transform.GetComponent<IInteractable>() : null;

        if (_currentInteractable != interactable)
        {
            _currentInteractable?.OnLookExit();
            _currentInteractable = interactable;
            _currentInteractable?.OnLookEnter();
        }
    }

    private void HandleMovement()
    {
        Vector2 input = _gameInput.GetMovementVectorNormalized();

        if (input.sqrMagnitude < EPSILON)
        {
            _isWalking = false;
            return;
        }

        bool isWalking = false;
        float distance = _movementSpeed * Time.deltaTime;
        Vector3 direction = new(input.x, 0f, input.y);
        Vector3 position = transform.position;
        Vector3 capsuleBot = position;
        Vector3 capsuleTop = position + Vector3.up * PLAYER_HEIGHT;

        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            bool hit = Physics.CapsuleCast(capsuleBot, capsuleTop, PLAYER_RADIUS, direction, out RaycastHit hitInfo, distance);

            if (!hit)
            {
                position += distance * direction;
                isWalking = true;
                break;
            }

            float allowedDistance = Mathf.Max(hitInfo.distance - EPSILON, 0f);

            if (allowedDistance > EPSILON)
            {
                position += allowedDistance * direction;
                distance -= allowedDistance;
                isWalking = true;
            }

            direction = Vector3.ProjectOnPlane(direction, hitInfo.normal);

            if (direction.sqrMagnitude < EPSILON)
                break;

            direction.Normalize();
            capsuleBot = position;
            capsuleTop = position + Vector3.up * PLAYER_HEIGHT;
        }

        _isWalking = isWalking;
        transform.position = position;
        Vector3 rotation = isWalking ? direction : new Vector3(input.x, 0f, input.y);
        transform.forward = Vector3.Slerp(transform.forward, rotation, Time.deltaTime * _rotationSpeed);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}
