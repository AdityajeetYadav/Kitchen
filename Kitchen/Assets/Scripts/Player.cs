using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParents
{
    
    public static Player Instance{get; private set;}

    public event EventHandler OnPickedSomething;
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounterClass selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    
    private bool isWalking;
    private Vector2 lastInteractDir;
    private BaseCounterClass selectedCounter;
    private KitchenObject kitchenObject;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player Instance");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractionAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        //if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractionAction(object snyder, System.EventArgs e)
    {
        //if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        var raydirection = (moveDir != Vector3.zero) ? moveDir : transform.forward;
        
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, raydirection , out RaycastHit rayCastHit, interactDistance,  counterLayerMask))
        {
            if (rayCastHit.transform.TryGetComponent(out BaseCounterClass baseCounterClass))
            {
                if (baseCounterClass != selectedCounter)
                {
                    selectedCounter = baseCounterClass;
                    SetSelectedCounter(baseCounterClass);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x,0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir, moveDistance);

        if (!canMove)
        {
            // cannot move towards moveDir
            // Attempting to move towards x
            Vector3 moveDirX = new Vector3( moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX, moveDistance);

            if (canMove)
            {
                //can move only on the x
                moveDir = moveDirX;
            }
            else
            {
                // Attempting to move towards z
                Vector3 moveDirZ = new Vector3( 0f,0f,moveDir.z).normalized;
                canMove = moveDir.z !=0 && !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move in any direction
                }
            }
        }
        
        
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        
        isWalking = moveDir != Vector3.zero;
        
        float rotatespeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatespeed);
    }

    private void SetSelectedCounter(BaseCounterClass selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
