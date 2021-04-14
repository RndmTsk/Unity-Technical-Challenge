using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeValueChanged : MonoBehaviour
{
    private Volume urpVolume;
    private Bloom urpBloom;
    private Vignette urpVignette;

    // Start is called before the first frame update
    void Start() {
        urpVolume = GetComponent<Volume>();
        urpVolume.profile.TryGet(out urpBloom);
        urpVolume.profile.TryGet(out urpVignette);
    }

    // Update is called once per frame
    void Update() {}

    public void BloomThresholdUpdated(float newValue) {
        if (urpBloom != null) {
            urpBloom.threshold.value = newValue;
        }
    }

    public void BloomIntensityUpdated(float newValue) {
        if (urpBloom != null) {
            urpBloom.intensity.value = newValue;
        }
    }

    public void VignetteIntensityUpdated(float newValue) {
        if (urpVignette != null) {
            urpVignette.intensity.value = newValue;
        }
    }

    public void VignetteSmoothnessUpdated(float newValue) {
        if (urpVignette != null) {
            urpVignette.smoothness.value = newValue;
        }
    }
}
