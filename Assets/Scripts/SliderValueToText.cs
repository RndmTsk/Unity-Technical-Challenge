using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    private Text sliderText;
    // Start is called before the first frame update
    void Start() {
        sliderText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {}

    public void SliderValueChanged(float newValue) {
        sliderText.text = newValue.ToString("F2");
    }
}
