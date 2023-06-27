
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField] private GameObject SettingUI = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Text VolumeText = null;
    // Start is called before the first frame update
    void Start()
    {
        LoadValues();
        SettingUI.SetActive(false);
    }
 
    public void VolumeSlider(float volume)
    {
        VolumeText.text = volume.ToString("0.0");
        volumeSlider.value = volume;
        AudioListener.volume = volume;
    }
    public void saveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
        DeactivateSetting();
    }
    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }
    public void ActivateSetting()
    {
        SettingUI.SetActive(true);
    }
    public void DeactivateSetting()
    {
        SettingUI.SetActive(false);
    }
}
