using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruka.Arcade.Apm.EMUICF {
    public static class ModdingUtil {
        public static void ChangeButton(Button obj, UnityAction ev) {
            obj.onClick = new Button.ButtonClickedEvent();
            obj.onClick.AddListener(ev);
        }

        public static void ChangeImage(Image image, byte[] data, int width, int height) {
            Texture2D modTexture = new Texture2D(width, height);
            modTexture.LoadImage(data);
            image.sprite = Sprite.Create(modTexture, new Rect(0.0f, 0.0f, modTexture.width, modTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static void ChangeSlider(Slider obj, UnityAction<float> ev) {
            obj.onValueChanged = new Slider.SliderEvent();
            obj.onValueChanged.AddListener(ev);
        }
    }
}