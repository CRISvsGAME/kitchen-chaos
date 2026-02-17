using System.Collections;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IInteractable, IKitchenObjectParent
{
    private Renderer _renderer;
    private Material _material;
    private Color _baseEmission;
    private Color _glowEmission;

    [SerializeField] private float _glowIntensity = 2f;
    [SerializeField] private Color _glowColor = Color.white;
    [SerializeField] private float _fadeDuration = 1f;
    private Coroutine _fadeCoroutine;
    private float _fadeTimer;

    [SerializeField] private SOKitchenObject _kitchenObjectData;
    [SerializeField] private Transform _kitchenObjectFollowTransform;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;
        _material.EnableKeyword("_EMISSION");
        _baseEmission = _material.GetColor("_EmissionColor");
        _glowEmission = _glowColor * _glowIntensity;
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

    public void OnLookEnter()
    {
        Fade(1f);
    }

    public void OnLookExit()
    {
        Fade(0f);
    }

    // #########################################################################
    // Fade Emission
    // #########################################################################

    private void Fade(float target)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        _fadeCoroutine = StartCoroutine(FadeEmission(target));
    }

    private IEnumerator FadeEmission(float target)
    {
        while (_fadeTimer != target)
        {
            _fadeTimer = Mathf.MoveTowards(_fadeTimer, target, Time.deltaTime / _fadeDuration);
            _material.SetColor("_EmissionColor", Color.Lerp(_baseEmission, _glowEmission, _fadeTimer));

            yield return null;
        }

        _fadeCoroutine = null;
    }
}
