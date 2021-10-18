using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsSlider : MonoBehaviour
{
    private Slider slider = null;
    [SerializeField] private Text value;
    public string prefName;

    private void Update()
    {
        value.text = ((int)(slider.value * 100)).ToString();
    }

    private void OnEnable()
    {
        if(slider == null)
        {
            slider = GetComponentInChildren<Slider>();
            slider.value = PlayerPrefs.GetFloat(prefName);
        }
    }

    public void SaveValue()
    {
        PlayerPrefs.SetFloat(prefName, slider.value);
    }

    public void ChangeMixer(AudioMixer m)
    {
        m.SetFloat(prefName, Mathf.Log10(slider.value) * 20);
    }

    public Slider GetSlider
    {
        get { return slider; }
    }
}