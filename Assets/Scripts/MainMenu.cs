using UnityEngine;

public class MainMenu : MonoBehaviour {
    #region Configurables

    [Header("Configurables")]
    [Tooltip("The color to apply when selecting the 'Color' option")]
    public Color color;

    [Tooltip("The texture to apply when selecting the 'Texture 2D' button")]
    public Texture2D texture;

    [Tooltip("A collection of objects from which to select")]
    public GameObject[] objects;

    #endregion


    #region Active Element
    private GameObject activeObject;
    private Renderer activeRenderer;

    #endregion

    #region Reset Values
    private Color defaultColor;
    private Quaternion defaultRotation;
    private Vector3 defaultPosition;

    #endregion


    // Start is called before the first frame update
    void Start() {
        defaultRotation = transform.rotation;
        defaultPosition = transform.position;
        defaultColor = Color.white;

        if (objects.Length == 0) {
            Debug.LogError("No 'objects' provided to App!");
            return;
        }
        HideAllObjects();
        Activate(objects[0]);
    }

    // Update is called once per frame
    void Update() {}

    void OnGUI() {
        DrawModelConfigButtons();
        DrawModelSelectButtons();
        DrawResetButtons();
    }

    private void HideAllObjects() {
        foreach(GameObject go in objects) {
            go.SetActive(false);
        }
    }

    private void Activate(GameObject go) {
        go.SetActive(true);
        activeObject = go;
        activeRenderer = activeObject.GetComponent<Renderer>();
    }

    private void DrawModelConfigButtons() {
        if (GUI.Button(new Rect(10, 10, 160, 50), "Color")) {
            Debug.Log("Apply a color");
            ResetActiveRenderer();
            activeRenderer.material.color = color;
        }

        if (GUI.Button(new Rect(180, 10, 160, 50), "2D Texture")) {
            Debug.Log("Apply a texture");
            ResetActiveRenderer();
            activeRenderer.material.mainTexture = texture;
        }

        if (GUI.Button(new Rect(350, 10, 160, 50), "Option 3")) {
            Debug.Log("Apply Option #3");
        }
    }

    private void DrawModelSelectButtons() {
        float buttonSpacing = 10 + 60; // Spacing of 10, height of 60
        float buttonY = 10 + buttonSpacing;
        foreach(GameObject go in objects) {
            if (GUI.Button(new Rect(10, buttonY, 160, 50), go.name)) {
                Debug.Log("Enabling " + go.name);
                ResetActiveRenderer();
                HideAllObjects();
                Activate(go);
            }
            buttonY += buttonSpacing;
        }
    }

    private void DrawResetButtons() {
        if (GUI.Button(new Rect(10, 300, 160, 50), "Reset Orientation")) {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }
        if (GUI.Button(new Rect(180, 300, 160, 50), "Reset Object")) {
            ResetActiveRenderer();
        }
    }

    private void ResetActiveRenderer() {
        if (activeRenderer == null) {
            Debug.LogError("Invoked 'ResetActiveRenderer()' with no 'activeRenderer'!");
            return;
        }
        Debug.Log("Applying default color: " + defaultColor);
        activeRenderer.material.mainTexture = null;
        activeRenderer.material.color = defaultColor;
    }
}
