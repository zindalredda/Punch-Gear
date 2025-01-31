using PunchGear.Entity;
using UnityEngine;
using UnityEngine.UI;

namespace PunchGear.Internal
{
    public class SliderEventBGM : MonoBehaviour
    {
        private Slider volumeSlider;
        private AudioManager audioManager;
        void Start()
        {
            audioManager = Object.FindFirstObjectByType<AudioManager>();
            volumeSlider = this.GetComponent<Slider>();

            // 오디오 마네자 알아서 만드셈
            volumeSlider.value = AudioManager.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(value =>
            {
                AudioManager.Instance.Volume = value;
            });
        }
    }
}
