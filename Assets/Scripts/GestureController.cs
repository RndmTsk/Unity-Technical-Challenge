using UnityEngine;

public class GestureController : MonoBehaviour {
    public GameObject target;
    public float rotationSpeed = 300.0f;

    private float screenWidth;
    private float screenHeight;

    private Touch initialTouch;

    // Start is called before the first frame update
    void Start() {
        screenWidth = Screen.currentResolution.width;
        screenHeight = Screen.currentResolution.height;

        // float inch = Mathf.Sqrt((Screen.width * Screen.width) + (Screen.height * Screen.height));
        // float ratio = inch / Screen.dpi;
        // Debug.Log("Screen Ratio: " + ratio);
    }

    // Update is called once per frame
    void Update() {
        TranslatedInput input = HandleInput();
        if (input.isZero()) {
            return;
        }
        if (input.TouchCount == 1) {
            target.transform.RotateAround(target.transform.position, input.Direction, input.Amount * Time.deltaTime);
        }
    }

    private TranslatedInput HandleInput() {
        if (Input.touchSupported) {
            return HandleTouchInput();
        } else {
            return HandleAxialInput();
        }
    }

    private TranslatedInput HandleTouchInput() {
        if (Input.touchCount == 0) {
            return new TranslatedInput();
        } else if (Input.touchCount == 1) { // Rotation
            Touch touch = Input.GetTouch(0);
            Debug.Log("TouchPhase: " + touch.phase);
            switch (touch.phase) {
            case TouchPhase.Began:
                initialTouch = touch;
                return new TranslatedInput();
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
                return new TranslatedInput();
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                Debug.Log("Initial Touch: " + initialTouch.position);
                Debug.Log("Touch.Moved: " + touch);
                Vector2 rotationDirection = (initialTouch.position - touch.position).normalized;
                float rotationSpeed = touch.radius * this.rotationSpeed;
                return new TranslatedInput(rotationDirection, rotationSpeed, 1);

            default:
                // TODO: (TL) Fall-off
                return new TranslatedInput();
            }
        } else { // Pinch, or Drag
            return new TranslatedInput();
        }
    }

    private TranslatedInput HandleAxialInput() {
        // 1. Mouse
        float mouseXComponent = Input.GetAxis("Mouse X");
        float mouseYComponent = Input.GetAxis("Mouse Y");
        Vector2 mouseDirection = new Vector2(x: mouseYComponent, y: mouseXComponent);

        // 2. Keyboard
        float keyboardXComponent = Input.GetAxis("Horizontal");
        float keyboardYComponent = Input.GetAxis("Vertical");
        Vector2 keyboardDirection = new Vector2(x: keyboardYComponent, y: keyboardXComponent);

        // Prefer Keyboard to Mouse input
        if (keyboardDirection != Vector2.zero) {
            Debug.Log("KEYBOARD");
            return new TranslatedInput(keyboardDirection, keyboardDirection.magnitude * rotationSpeed, 1);
        } else
        if (mouseDirection != Vector2.zero) {
            Debug.Log("MOUSE");
            return new TranslatedInput(mouseDirection, mouseDirection.magnitude * rotationSpeed, 1);
        } else {
            return new TranslatedInput();
        }
    }
}