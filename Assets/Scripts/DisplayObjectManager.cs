using UnityEngine;
using UnityEngine.UI;

public class DisplayObjectManager : MonoBehaviour {
    // Update is called once per frame
    void Update() {}

    // Handles hiding all objects, and activating the rest
    public void ShowChildAtIndex(Dropdown dropdown) {
        Debug.Log("Displaying child @" + dropdown.value);
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++) {
            Transform ct = transform.GetChild(i);
            if (ct == null) {
                continue;
            }
            GameObject go = ct.gameObject;
            if (go == null) {
                continue;
            }
            go.SetActive(i == dropdown.value);
        }
    }
}
