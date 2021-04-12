using UnityEngine;

// Allows Mouse or Touch input to be used in the same way
public struct TranslatedInput {
    public static TranslatedInput zero;
    public Vector3 Direction { get; set; }
    public float Amount { get; set; }

    public int TouchCount { get; set; }

    public bool isZero() {
            return Mathf.Abs(Direction.magnitude) < float.Epsilon &&
                Mathf.Abs(Amount) < float.Epsilon &&
                TouchCount == 0;
    }
    public TranslatedInput(Vector2 direction, float amount, int touchCount) {
        Direction = new Vector3(direction.x, direction.y, 0);
        Amount = amount;
        TouchCount = touchCount;
    }

    public override string ToString() => $"({Direction.x}, {Direction.y}) @ {Amount}, {TouchCount} touch(es)";
}