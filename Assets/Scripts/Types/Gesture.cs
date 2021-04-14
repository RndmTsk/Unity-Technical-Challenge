using UnityEngine;
using System.Linq;

public struct Gesture {
    public enum GestureKind {
        None,
        Rotation,
        Compression,
        Expansion,
        Translation
    }

    // A convenience property to access the first 'Location' (most cases we only care about the first one)
    public Vector2 Location { get { return locations[0]; } }

    // A convenience property to access the first 'Direction' (most cases we only care about the first one)
    public Vector2 Direction { get { return directions[0]; } }

    // A convenience property to access the first 'Location' (most cases we only care about the first one)
    public Vector2 FirstLocation { get { return Location; } }

    // A convenience property to access the first 'Direction' (most cases we only care about the first one)
    public Vector2 FirstDirection { get { return Direction; } }

    // A convenience property to access the last 'Location', to avoid running off the end of the array,
    // we index based on size
    public Vector2 LastLocation { get { return locations[locations.Length - 1]; } }

    // A convenience property to access the last 'Direction', to avoid running off the end of the array,
    // we index based on size
    public Vector2 LastDirection { get { return directions[directions.Length - 1]; } }
    
    // The overall "strength" of the gesture
    public float Magnitude;

    // What type of gesture it was - used so that the gesture "calculation" is isolated to one spot
    public GestureKind Kind;

    // Internally, we store multiple 'Location' entries in this array
    private readonly Vector2[] locations;

    // Internally, we store multiple 'Direction' entries in this array
    private readonly Vector2[] directions;

    // This initializer is private as it's not immediately obvious how the values are to be used.
    //
    // Given the values are transposed (based on our project setup), it's important that we isolate
    // this action here in case we change our project
    private Gesture(Vector2[] screenLocations, Vector2[] screenDirections, float magnitude, GestureKind kind) {
        Kind = kind;
        locations = Transpose(screenLocations);
        directions = Transpose(screenDirections);
        Magnitude = magnitude;
    }

    // This initializer is private as it's not immediately obvious how the values are to be used.
    //
    // Given the values are transposed (based on our project setup), it's important that we isolate
    // this action here in case we change our project
    private Gesture(Vector2 screenLocation, Vector2 screenDirection, float magnitude, GestureKind kind) {
        locations = new Vector2[] { Transpose(screenLocation) };
        directions = new Vector2[] { Transpose(screenDirection) };
        Magnitude = magnitude;
        Kind = kind;
    }

    // A convenience function to create a 'None' type of gesture
    public static Gesture Empty() {
        return new Gesture(
            Vector2.zero,
            Vector2.zero,
            0,
            GestureKind.None
        );
    }

    // A convenience function to create a 'Rotation' type of gesture given a keyboard, or mouse input
    public static Gesture RotationFrom(Vector2 direction, float magnitude) {
        return new Gesture(
            Vector2.zero,
            direction,
            magnitude,
            GestureKind.Rotation
        );
    }

    // A convenience function to create a 'Rotation' type of gesture given a history of 'Touch'es
    public static Gesture RotationFrom(Touch currentTouch, Touch previousTouch) {
        Vector2 touchDelta = previousTouch.position - currentTouch.position;
        return new Gesture(
            currentTouch.position,
            touchDelta.normalized,
            touchDelta.magnitude,
            GestureKind.Rotation
        );
    }

    // A convenience function to create a 'Scale' type of gesture given a history of 'Touch'es from multiple fingers
    public static Gesture ScaleFrom(Touch currentTouch1, Touch previousTouch1, Touch currentTouch2, Touch previousTouch2) {
        float currentDistance = Vector2.Distance(currentTouch1.position, currentTouch2.position);
        float previousDistance = Vector2.Distance(previousTouch1.position, previousTouch2.position);
        Vector2[] screenLocations = new Vector2[] {
            currentTouch1.position,
            currentTouch2.position
        };
        Vector2[] screenDirections = new Vector2[] {
            (previousTouch1.position - currentTouch1.position).normalized,
            (previousTouch2.position - currentTouch2.position).normalized
        };
        if (currentDistance < previousDistance) { // Expanding
            return new Gesture(screenLocations, screenDirections, currentDistance - previousDistance, GestureKind.Expansion);
        } else {
            return new Gesture(screenLocations, screenDirections, previousDistance - currentDistance, GestureKind.Compression);
        }
    }

    // A convenience function to create a 'Translation' type of gesture given a history of 'Touch'es
    public static Gesture TranslationFrom(Touch currentTouch, Touch previousTouch) {
        Vector2 touchDelta = previousTouch.position - currentTouch.position;
        return new Gesture(
            currentTouch.position,
            touchDelta.normalized,
            touchDelta.magnitude,
            GestureKind.Translation
        );
    }

    // All touches will be translated based on the initial touch that began the sequence
    // We also transpose the X and Y elements as our camera doesn't match our screen layout
    private static Vector2 Transpose(Vector2 input) {
        return new Vector2(x: input.y, y: input.x);
    }

    private static Vector2[] Transpose(Vector2[] inputs) {
        return inputs.Select(input => Transpose(input)).ToArray();
    }

    public override string ToString() => $"@ ({Location.x}, {Location.y}) in ({Direction.x}, {Direction.y}), measuring: {Magnitude} [{Kind}]";
}
