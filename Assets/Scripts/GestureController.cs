using System;
using UnityEngine;

// Listens for input events on Keyboards, Mice, and Touch Screens
public class GestureController : MonoBehaviour {
    // The 'GameObject' that will be manipulated by gestures
    public GameObject target;

    // A standard rotation speed, updatable in the Unity Editor
    public float rotationSpeed = 300.0f;

    public float translationSpeed = 30.0f;

    public float scaleAmount = 0.05f;

    // Our gestures are based on a distance delta, this is the
    // largest length a touch will travel to get a full application
    // of 'rotationSpeed'
    public float idealMagnitude;

    // When receiving a single, or multi-touch, this represents the
    // touch that is at index '0', and is in the 'Begin' phase
    private Touch previousTouch1;

    // When receiving a multi-touch, this represents the
    // touch that is at index '1', and is in the 'Begin' phase
    private Touch previousTouch2;

    // Mouse input is instantaneous, not continuous, use a separate
    // property to keep track based on changes
    private bool isMouseDown = false;

    Func<Gesture> HandleInputs = null;

    // Start is called before the first frame update
    void Start() {
        // Consider 1/2 of the smaller screen dimension a "full-speed" swipe
        idealMagnitude = Mathf.Min(Screen.width, Screen.height) / 2;
        if (Input.touchSupported) {
            HandleInputs = HandleTouchInput;
        } else {
            HandleInputs = HandleAxialInput;
        }
    }

    // Update is called once per frame
    void Update() {
        Gesture gesture = HandleInputs();

        // TODO: (TL) Most of these might benefit from LERP
        switch (gesture.Kind) {
        case Gesture.GestureKind.Rotation:
            float speed = gesture.Magnitude / idealMagnitude * rotationSpeed;
            target.transform.RotateAround(target.transform.position, gesture.Direction, speed * Time.deltaTime);
            break;

        case Gesture.GestureKind.Compression:
            // Clamp at a maximum size to avoid taking up the entire screen
            // We're scaling uniformly, so we can cheat and look at only the 'x' property
            if (target.transform.localScale.x < 5.0f) {
                target.transform.localScale += new Vector3(scaleAmount, scaleAmount, scaleAmount);
            }
            break;

        case Gesture.GestureKind.Expansion:
            // Clamp at a minimum size to avoid inverting the item or something weird
            // We're scaling uniformly, so we can cheat and look at only the 'x' property
            if (target.transform.localScale.x > 0.01f) {
                target.transform.localScale -= new Vector3(scaleAmount, scaleAmount, scaleAmount);
            }
            break;

        case Gesture.GestureKind.Translation:
            target.transform.Translate(gesture.Direction * translationSpeed * Time.deltaTime);
            break;

        default:
            break;
        }
    }

    // Touch inputs are in a somewhat different structure than other inputs, we need
    // to translate them into a position, direction, and size, based on how many there are
    private Gesture HandleTouchInput() {
        if (Input.touchCount == 0) {
            return new Gesture();
        } else if (Input.touchCount == 1) { // Rotation
            Touch currentTouch = Input.GetTouch(0);
            if (currentTouch.phase == TouchPhase.Began) {
                previousTouch1 = currentTouch;
            } else if (currentTouch.phase == TouchPhase.Moved || currentTouch.phase == TouchPhase.Stationary) {
                return Gesture.RotationFrom(currentTouch, previousTouch1);
            }
            // touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled - TODO: (TL) Finish - deceleration
            return new Gesture();
        } else { // Pinch, or Drag
            Touch currentTouch1 = Input.GetTouch(0);
            Touch currentTouch2 = Input.GetTouch(1);
            // Ensure we've updated our `firstTouch` and `firstOtherTouch`
            // properties when we start a new gesture
            if (currentTouch1.phase == TouchPhase.Began) {
                previousTouch1 = currentTouch1;
            }
            if (currentTouch2.phase == TouchPhase.Began) {
                previousTouch2 = currentTouch2;
            }
            Vector2 direction1 = (previousTouch1.position - currentTouch1.position).normalized;
            Vector2 direction2 = (previousTouch2.position - currentTouch2.position).normalized;
            float gestureDirection = Vector3.Dot(direction1, direction2);

            Gesture gesture;
            if (gestureDirection > 0) { // Co-linear, translate
                // While the 'Translation' gesture is initiated by two touches, given they
                // occur on similar lines, we really only care about the first one to establish
                // a direction for the overall movement
                gesture = Gesture.TranslationFrom(currentTouch1, previousTouch1);
            } else if (gestureDirection < 0) { // Opposing directions, scale
                // For a 'Scale' gesture, we need to know lots of info to determine whether we should
                // increase, or reduce the scale.
                gesture = Gesture.ScaleFrom(currentTouch1, previousTouch1, currentTouch2, previousTouch2);
            } else {
                gesture = Gesture.Empty();
            }
            // For pinch/expand gestures, we should keep track of
            // discrete changes, rather than the overall change, as in
            // the translation and rotation movement tracking.
            //
            // This means, we only care about the most recent touches
            previousTouch1 = currentTouch1;
            previousTouch2 = currentTouch2;
            return gesture;
        }
    }

    // To improve development speed, Axial input (Keyboard, and Mouse) was what I started with
    // To ensure the components were reading from the same info, 'TranslatedInput' receives
    // values from either the Mouse, or Keyboard
    private Gesture HandleAxialInput() {
        // 1. Mouse
        float mouseXComponent = Input.GetAxis("Mouse X");
        float mouseYComponent = Input.GetAxis("Mouse Y");
        Vector2 mouseDirection = new Vector2(x: -mouseXComponent, y: -mouseYComponent);

        // 2. Keyboard
        float keyboardXComponent = Input.GetAxis("Horizontal");
        float keyboardYComponent = Input.GetAxis("Vertical");
        Vector2 keyboardDirection = new Vector2(x: -keyboardXComponent, y: -keyboardYComponent);

        // Mouse input is instantaneous, not continuous, need to keep track
        if (Input.GetMouseButtonDown(0)) {
            isMouseDown = true;
        } else if (Input.GetMouseButtonUp(0)) {
            isMouseDown = false;
        }

        // Prefer Keyboard to Mouse input
        if (keyboardDirection != Vector2.zero) {
            return Gesture.RotationFrom(
                keyboardDirection,
                idealMagnitude
            );
        } else if (isMouseDown && mouseDirection != Vector2.zero) {
            return Gesture.RotationFrom(
                mouseDirection,
                idealMagnitude
            );
        } else {
            return Gesture.Empty();
        }
    }
}