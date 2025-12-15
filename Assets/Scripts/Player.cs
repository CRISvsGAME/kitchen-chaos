using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameInput _gameInput;

    [SerializeField]
    private float _moveSpeed = 10f;
    private bool _isWalking = false;

    private void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveVector = new(inputVector.x, 0f, inputVector.y);
        Vector3 moveDelta = _moveSpeed * Time.deltaTime * moveVector;
        transform.position += moveDelta;

        if (moveVector.sqrMagnitude > 0f)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveVector, Time.deltaTime * 10f);
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}
