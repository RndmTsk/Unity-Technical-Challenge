using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class DisplayObjectManager : MonoBehaviour {
    private GameObject[] targets;

    // Start is called before the first frame update
    void Start() {
        targets = transform.GetComponentsInChildren<Transform>()
            .Select(transform => transform.gameObject)
            .ToArray();
    }

    // Update is called once per frame
    void Update() {}

    public void ShowSphere() {
        Activate("Sphere");
    }

    public void ShowCube() {
        Activate("Cube");
    }

    public void ShowPaintBucket() {
        Activate("Paint 1G Bucket");
    }

    public void ShowJigsaw() {
        Activate("Jigsaw");
    }

    private void Activate(string child) {
        foreach(GameObject target in targets) {
            target.SetActive(false);
        }
        GameObject go = gameObject.transform.Find(child).gameObject;
        if (go != null) {
            go.SetActive(true);
        }

    }
}
