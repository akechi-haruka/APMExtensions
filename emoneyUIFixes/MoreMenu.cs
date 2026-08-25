using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Apm.Emoney.Ui;
using Emoney.SharedMemory;
using Haruka.Arcade.Apm.BananaphoneLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Haruka.Arcade.Apm.EMUICF {
    public class MoreMenu : MonoBehaviour {
        private enum State {
            Initial,
            SelectAction,
            Wait,
            SpeakerVolume,
            SelectAudioMode,
            GameGuide,
            CabinetLeds
        }

        private const float TIMEOUT = 30f;

        private readonly Color normalColor = new Color(0.011764706F, 0.6627451F, 0.95686275F, 1);
        private readonly Color greenColor = new Color(0.01967995F, 0.6691177F, 0.25313914F, 1);
        private readonly Dictionary<string, GameObject> selectActionItems = new Dictionary<string, GameObject>();

        private float? timeRemaining;
        private State menuState;
        private int currentGuidePage;
        private Color currentLedColor;

        private State MenuState {
            set {
                if (menuState != value && (value == State.Initial || value - 1 == menuState)) {
                    ChangeState(value);
                }
            }
        }

        private string HeaderText {
            set { headerText.GetComponent<Text>().text = value; }
        }

        private string GuideText {
            set { guideMessage.transform.GetComponent<Text>().text = value; }
        }

        private TextAnchor GuideTextAlignment {
            set { guideMessage.transform.GetComponent<Text>().alignment = value; }
        }

        private bool IsStart {
            get { return menuState > State.Initial; }
        }

        public void StartMenu() {
            if (!IsStart) {
                Plugin.Log.LogDebug("Start SelectAction");
                MenuState = State.SelectAction;
            }
        }

        public void OnClickCancel() {
            timeRemaining = null;
            MenuState = State.Initial;
            sceneManager.GetComponent<SceneManager>().MenuState = SceneManager.State.EntryMenu;
        }

        [UsedImplicitly]
        private void Start() {
            ChangeState(State.Initial);
        }

        [UsedImplicitly]
        private void Update() {
            if (timeRemaining != null) {
                if (timeRemaining > 0f) {
                    timeRemaining -= Time.deltaTime;
                }

                if (timeRemaining <= 0f) {
                    OnClickCancel();
                }
            }
        }

        private void ChangeState(State next) {
            if (next != State.Initial) {
                timeRemaining = Plugin.ConfigDisableTimeout.Value ? Single.MaxValue : TIMEOUT;
            }

            menuState = next;
            switch (next) {
                case State.Initial:
                    ResetUi();
                    return;
                case State.SelectAction: {
                    header.GetComponent<Animator>().SetTrigger("Show");
                    HeaderText = "More Options";
                    itemButtons.SetActive(true);

                    bool atLeastOneAppExEnabled = Plugin.AppExConfig.exit.kill && Plugin.AppExConfig.exit.kill_process_name_list?.Length > 0;

                    SpawnButton(itemButtons, "Game Guide", Plugin.GuideData.pages?.Count > 0, ActionGameGuide);
                    SpawnButton(itemButtons, "Cabinet\nLights", Plugin.AllowLedControl, ActionCabinetLights);
                    SpawnButton(itemButtons, "Cabinet\nSpeakers", Plugin.ConfigSpeakerAdjustmentEnabled.Value, ActionSpeakerVolume);
                    SpawnButton(itemButtons, "Audio Mode", Plugin.ConfigSpeakerAdjustmentEnabled.Value, ActionAudioMode);
                    SpawnButton(itemButtons, "Exit Game", Plugin.ConfigAllowExit.Value && atLeastOneAppExEnabled, ActionKillGame);

                    return;
                }
                case State.Wait:
                    header.GetComponent<Animator>().SetTrigger("Show");
                    HeaderText = "Please wait...";

                    itemButtons.SetActive(false);
                    audioModeButtons.SetActive(false);
                    cancelMessage.SetActive(true);
                    return;
                case State.SelectAudioMode:
                    header.GetComponent<Animator>().SetTrigger("Show");
                    HeaderText = "Select Audio Mode";

                    Plugin.SaveCurrentAudioVolume();

                    SpawnButton(audioModeButtons, "Mute", true, ActionSpeakersMute);
                    SpawnButton(audioModeButtons, "Normal", true, ActionSpeakersNormal);
                    SpawnButton(audioModeButtons, "Speakers\nonly", true, ActionSpeakersOnly);
                    SpawnButton(audioModeButtons, "Headphones\nonly", true, ActionHeadphonesOnly);

                    itemButtons.SetActive(false);
                    audioModeButtons.SetActive(true);
                    return;
                case State.SpeakerVolume:
                    header.GetComponent<Animator>().SetTrigger("Show");
                    HeaderText = "Cabinet Speaker Volume";

                    itemButtons.SetActive(false);
                    speakerSubFrame.SetActive(true);

                    return;
                case State.GameGuide:
                    frame.GetComponent<Image>().sprite = greenFrameSprite;
                    header.GetComponent<Image>().color = greenColor;

                    itemButtons.SetActive(false);
                    guide.SetActive(true);
                    pinButton.SetActive(true);
                    prevGuidePageButton.SetActive(true);
                    nextGuidePageButton.SetActive(true);

                    ResizeMenu(Plugin.GuideData.width, Plugin.GuideData.height);

                    ChangeGuidePage(0);

                    return;
                case State.CabinetLeds:
                    header.GetComponent<Animator>().SetTrigger("Show");
                    HeaderText = "Cabinet Light Color";

                    AppExConfig.LedSettings settings = Plugin.AppExConfig.led;

                    SetLedUi(settings.r / 255F, settings.g / 255F, settings.b / 255F, true);

                    itemButtons.SetActive(false);
                    ledSubFrame.SetActive(true);

                    // not sure why I need to do this here instead of the init function
                    ledSliderR.transform.position = new Vector3(200, 94, 0);
                    ledSliderG.transform.position = new Vector3(500, 94, 0);
                    ledSliderB.transform.position = new Vector3(800, 94, 0);
                    ledValueR.transform.localPosition = new Vector3(0, -40, 0);
                    ledValueG.transform.localPosition = new Vector3(0, -40, 0);
                    ledValueB.transform.localPosition = new Vector3(0, -40, 0);

                    return;
                default:
                    return;
            }
        }

        private void ResetUi() {
            HeaderText = "";
            itemButtons.SetActive(false);
            audioModeButtons.SetActive(false);
            gameInfoText.SetActive(false);
            cancelMessage.SetActive(false);
            speakerSubFrame.SetActive(false);
            pinButton.SetActive(false);
            prevGuidePageButton.SetActive(false);
            nextGuidePageButton.SetActive(false);
            guide.SetActive(false);
            ledSubFrame.SetActive(false);
            header.GetComponent<Image>().color = normalColor;
            pinButton.transform.Find("Text").GetComponent<Text>().text = "Pin";

            foreach (KeyValuePair<string, GameObject> keyValuePair2 in selectActionItems) {
                Destroy(keyValuePair2.Value);
            }

            pinButton.GetComponent<Button>().interactable = true;

            selectActionItems.Clear();
            frame.GetComponent<Image>().sprite = blueFrameSprite;

            ResizeMenu(1000, 178, false, false);
        }

        private void SpawnButton(GameObject container, string buttonName, bool active, UnityAction<int> action) {
            GameObject obj = Instantiate(buttonPrefab, container.transform);
            obj.GetComponent<Button>().interactable = active;

            ItemButton ibtn = obj.GetComponent<ItemButton>();
            ibtn.Click.AddListener(action);
            ibtn.ItemName = buttonName;

            obj.transform.Find("Price").gameObject.SetActive(false);

            Transform label = obj.transform.Find("OtherItem/Name");
            label.localPosition = new Vector3(label.localPosition.x, -52, 0);
            label.gameObject.GetComponent<Text>().fontSize = 22;

            selectActionItems.Add(buttonName, obj);
        }

        private void ResizeMenu(int w, int h, bool moveWindow = true, bool addBorderMargins = true) {
            const int hmargin = 50;
            const int wmargin = 15;

            if (addBorderMargins) {
                h += hmargin;
                w += wmargin;
            }

            header.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, w);
            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            guideMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(addBorderMargins ? w - wmargin : w, addBorderMargins ? h - hmargin : h);

            int origin = -(w / 2) + 45;
            prevGuidePageButton.transform.localPosition = new Vector3(origin + 10, prevGuidePageButton.transform.localPosition.y, 0);
            pinButton.transform.localPosition = new Vector3(origin + 95, pinButton.transform.localPosition.y, 0);
            nextGuidePageButton.transform.localPosition = new Vector3(origin + 155, nextGuidePageButton.transform.localPosition.y, 0);

            if (moveWindow) {
                Resource data = Plugin.SceneManager.emoneyController.GetComponent<EmoneyController>().Data.Resource;
                Rect rect = Plugin.SceneManager.GetPosition(data.MainPosition, data.MainMarginX, data.MainMarginY, new Vector2(w, h));
                GameObject.Find("WindowManager").GetComponent<WindowManager>().MoveWindow(rect, Plugin.SceneManager.delta);

                RectTransform rt = frame.transform.GetComponent<RectTransform>();
                Plugin.Self.ChangePosAbsolute(rt, rt, new Rect(0, h, w, h), Plugin.SceneManager.delta);
            }
        }

        #region Primary menu button actions

        private void ActionSpeakerVolume(int _) {
            ChangeState(State.SpeakerVolume);
        }

        private void ActionAudioMode(int _) {
            ChangeState(State.SelectAudioMode);
        }

        private void ActionCabinetLights(int _) {
            ChangeState(State.CabinetLeds);
        }

        private void ActionKillGame(int arg0) {
            cancelButton.GetComponent<Button>().interactable = false;
            ChangeState(State.Wait);
            Plugin.ExitGame(arg0);
        }

        private void ActionGameGuide(int _) {
            ChangeState(State.GameGuide);
        }

        #endregion

        #region Speaker Mode Menu

        private void ActionSpeakersMute(int _) {
            ChangeState(State.Wait);
            StartCoroutine(ActionSpeakerChange(false, false));
        }

        private void ActionSpeakersNormal(int _) {
            ChangeState(State.Wait);
            StartCoroutine(ActionSpeakerChange(true, true));
        }

        private void ActionSpeakersOnly(int _) {
            ChangeState(State.Wait);
            StartCoroutine(ActionSpeakerChange(true, false));
        }

        private void ActionHeadphonesOnly(int _) {
            ChangeState(State.Wait);
            StartCoroutine(ActionSpeakerChange(false, true));
        }

        private IEnumerator ActionSpeakerChange(bool speakers, bool headphones) {
            Headbanana.SetSpeakerVolume(speakers ? Plugin.SavedSpeakerVolume : 0F);
            Headbanana.SetHeadphoneVolumeForDefault(headphones ? Plugin.SavedHeadphoneVolume : 0F);

            yield return new WaitForSeconds(0.25F);

            OnClickCancel();
        }

        public void UpdateSpeakerVolumeFromCurrent() {
            UpdateSpeakerVolumeDisplay(Headbanana.GetSpeakerVolume());
        }

        private void OnSpeakerVolumeChange(float vol) {
            Headbanana.SetSpeakerVolume(vol);
            UpdateSpeakerVolumeDisplay(vol);
        }

        public void UpdateSpeakerVolumeDisplay(float vol) {
            speakerSubFrame.transform.Find("Slider").GetComponent<Slider>().value = vol;
            speakerSubFrame.transform.Find("HeadphoneIcon/Right").GetComponent<HeadphoneVolumeIcon>().Volume = (int)vol;
            speakerSubFrame.transform.Find("Value").GetComponent<Text>().text = ((int)vol).ToString();
        }

        #endregion

        #region LEDs

        public void SetLedUi(float r, float g, float b, bool setSliders = false) {
            currentLedColor = new Color(r, g, b);
            header.GetComponent<Image>().color = currentLedColor;

            UpdateSlider(ledSliderR.transform, ledValueR.transform, r, setSliders);
            UpdateSlider(ledSliderG.transform, ledValueG.transform, g, setSliders);
            UpdateSlider(ledSliderB.transform, ledValueB.transform, b, setSliders);

            if (timeRemaining < TIMEOUT) {
                timeRemaining = TIMEOUT;
            }
        }

        private void UpdateSlider(Transform slider, Transform value, float vol, bool setPosition) {
            if (setPosition) {
                slider.GetComponent<Slider>().value = vol * 100;
            }

            value.GetComponent<Text>().text = ((int)(vol * 255)).ToString();
        }

        private void OnLedRgbChangeR(float value) {
            SetLedUi(value / 100F, currentLedColor.g, currentLedColor.b);
            Plugin.LedManager.Set(currentLedColor);
        }

        private void OnLedRgbChangeG(float value) {
            SetLedUi(currentLedColor.r, value / 100F, currentLedColor.b);
            Plugin.LedManager.Set(currentLedColor);
        }

        private void OnLedRgbChangeB(float value) {
            SetLedUi(currentLedColor.r, currentLedColor.g, value / 100F);
            Plugin.LedManager.Set(currentLedColor);
        }

        #endregion

        #region Game Guide

        private void ChangeGuidePage(int pageNumber) {
            List<AppExConfig.GuidePage> pages = Plugin.GuideData.pages;

            if (pageNumber < 0 || pageNumber >= pages.Count) {
                Plugin.Log.LogWarning("Guide page out of range: " + pageNumber + "/" + pages.Count);
                return;
            }

            AppExConfig.GuidePage page = pages[pageNumber];

            header.GetComponent<Animator>().SetTrigger("Show");
            HeaderText = page.title + " (" + (pageNumber + 1) + "/" + pages.Count + ")";

            GuideText = page.text ?? "";

            String alignment = page.align ?? "center";
            if (alignment == "left") {
                GuideTextAlignment = TextAnchor.UpperLeft;
            } else if (alignment == "right") {
                GuideTextAlignment = TextAnchor.UpperRight;
            } else {
                GuideTextAlignment = TextAnchor.MiddleCenter;
            }

            if (!String.IsNullOrWhiteSpace(page.file)) {
                string path = Path.Combine(Plugin.ConfigExDataPath.Value, page.file);
                if (path.EndsWith(".txt")) {
                    try {
                        GuideText = File.ReadAllText(path);
                    } catch (Exception ex) {
                        Plugin.Log.LogError("Failed to load guide file " + path + ": " + ex);
                        GuideText = "Failed to load page";
                    }

                    guideImage.SetActive(false);
                } else {
                    try {
                        ModdingUtil.ChangeImage(guideImage.GetComponent<Image>(), File.ReadAllBytes(path), 2, 2);
                        guideImage.SetActive(true);
                    } catch (Exception ex) {
                        Plugin.Log.LogError("Failed to load guide file " + path + ": " + ex);
                        GuideText = "Failed to load page";
                    }
                }
            } else {
                guideImage.SetActive(false);
            }

            foreach (GameObject button in guidePageButtons) {
                Destroy(button);
            }

            guidePageButtons.Clear();
            if (page.content.buttons != null) {
                const int borderMargin = 15;

                Vector2 guideArea = guideMessage.GetComponent<RectTransform>().sizeDelta;
                bool centerContent = page.content.align != "topleft";

                foreach (AppExConfig.GuideButton def in page.content.buttons) {
                    bool centerButton = def.center;
                    GameObject button = CloneAndRewireButton(guide.transform, "GuidePageButton", def.text, new Vector2(def.width, def.height), Images.BLUE_BUTTON_TEXTURE, () => ChangeGuidePage(def.target - 1));

                    if (centerContent) {
                        if (centerButton) {
                            button.transform.localPosition = new Vector3(def.x + def.width / 2F, -def.y - def.height + borderMargin / 2F, 0);
                        } else {
                            button.transform.localPosition = new Vector3(def.x + def.width, -def.y - def.height + borderMargin / 2F, 0);
                        }
                    } else {
                        if (centerButton) {
                            button.transform.localPosition = new Vector3(def.x - guideArea.x / 2F + def.width / 2F, -def.y + guideArea.y / 2F - def.height / 2F - borderMargin, 0);
                        } else {
                            button.transform.localPosition = new Vector3(def.x - guideArea.x / 2F + def.width, -def.y + guideArea.y / 2F - def.height - borderMargin, 0);
                        }
                    }

                    guidePageButtons.Add(button);
                }
            }

            prevGuidePageButton.GetComponent<Button>().interactable = pageNumber > 0;
            nextGuidePageButton.GetComponent<Button>().interactable = pageNumber < pages.Count - 1;

            currentGuidePage = pageNumber;

            if (timeRemaining < TIMEOUT) {
                timeRemaining = TIMEOUT;
            }
        }

        private void PinMenu() {
            timeRemaining = Single.MaxValue;
            pinButton.transform.Find("Text").GetComponent<Text>().text = "Pinned";
            pinButton.GetComponent<Button>().interactable = false;
        }

        private void PrevGuidePage() {
            ChangeGuidePage(currentGuidePage - 1);
        }

        private void NextGuidePage() {
            ChangeGuidePage(currentGuidePage + 1);
        }

        #endregion

        #region Unity objects

        private GameObject sceneManager;
        private GameObject contents;
        private GameObject header;
        private GameObject frame;
        private GameObject headerText;
        private GameObject itemButtons;
        private GameObject audioModeButtons;
        private GameObject gameInfoText;
        private GameObject cancelMessage;
        private GameObject buttonPrefab;
        private GameObject cancelButton;
        private Sprite blueFrameSprite;
        private Sprite greenFrameSprite;
        private GameObject guideMessage;
        private GameObject pinButton;
        private GameObject prevGuidePageButton;
        private GameObject nextGuidePageButton;
        private GameObject speakerSubFrame;
        private GameObject guide;
        private GameObject guideImage;
        private readonly List<GameObject> guidePageButtons = new List<GameObject>();
        private GameObject ledSubFrame;
        private GameObject ledSliderR;
        private GameObject ledSliderG;
        private GameObject ledSliderB;
        private GameObject ledValueR;
        private GameObject ledValueG;
        private GameObject ledValueB;

        public void InitializeModdedObjectsFromCopy(EmoneyMenu emenu) {
            // original stuff
            sceneManager = emenu.sceneManager;
            header = emenu.header;
            frame = emenu.frame;
            headerText = emenu.headerText;
            itemButtons = emenu.itemButtons;
            gameInfoText = emenu.waitMessage;
            cancelMessage = emenu.cancelMessage;
            buttonPrefab = emenu.buttonPrefab;
            cancelButton = emenu.cancelButton;
            blueFrameSprite = emenu.payToCoinFrameSprite;
            greenFrameSprite = emenu.balanceFrameSprite;

            // frame
            contents = frame.transform.Find("Contents").gameObject;

            Button cancelButtonComp = cancelButton.GetComponent<Button>();
            ModdingUtil.ChangeButton(cancelButtonComp, OnClickCancel);
            cancelButtonComp.interactable = true;

            // audio mode
            audioModeButtons = Instantiate(itemButtons, itemButtons.transform.parent);
            audioModeButtons.name = "AudioModeButtons";

            // speaker volume
            speakerSubFrame = Instantiate(GameObject.Find("Canvas/Headphone/Frame/Contents/Main"), contents.transform.Find("Main"));
            speakerSubFrame.name = "SpeakerSubFrame";
            speakerSubFrame.transform.localPosition = Vector3.zero;
            speakerSubFrame.transform.Find("HeadphoneIcon/Left").gameObject.SetActive(false);
            Transform rs = speakerSubFrame.transform.Find("HeadphoneIcon/Right");
            rs.localPosition = new Vector3(rs.localPosition.x, 0, 0);
            ModdingUtil.ChangeImage(speakerSubFrame.transform.Find("HeadphoneIcon/Icon").GetComponent<Image>(), Convert.FromBase64String(Images.SPEAKER_TEXTURE), 89, 95);
            ModdingUtil.ChangeSlider(speakerSubFrame.transform.Find("Slider").GetComponent<Slider>(), OnSpeakerVolumeChange);
            UpdateSpeakerVolumeFromCurrent();

            // game guide
            guide = new GameObject("GameGuide") {
                transform = {
                    parent = contents.transform.Find("Main"),
                    localPosition = Vector3.zero
                }
            };

            guideMessage = Instantiate(gameInfoText.transform.Find("View/Text").gameObject, guide.transform);
            guideMessage.name = "GuideText";
            guideMessage.transform.localPosition = new Vector3(0, -56, 0);
            guideMessage.GetComponent<Text>().supportRichText = true;

            guideImage = new GameObject("GuideImage", typeof(Image)) {
                transform = {
                    parent = guide.transform,
                    position = new Vector3(0, 0, 0),
                    localPosition = new Vector3(0, -51, 0)
                }
            };
            RectTransform rt = guideImage.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(Plugin.GuideData.width, Plugin.GuideData.height);

            // game guide frame
            prevGuidePageButton = CloneAndRewireButton(cancelButton.transform.parent, "PrevPageButton", "<<", new Vector2(50, 40), Images.BLUE_BUTTON_TEXTURE, PrevGuidePage);
            pinButton = CloneAndRewireButton(cancelButton.transform.parent, "PinButton", "Pin", new Vector2(75, 40), Images.GREEN_BUTTON_TEXTURE, PinMenu);
            nextGuidePageButton = CloneAndRewireButton(cancelButton.transform.parent, "NextPageButton", ">>", new Vector2(50, 40), Images.BLUE_BUTTON_TEXTURE, NextGuidePage);

            // leds
            ledSubFrame = Instantiate(GameObject.Find("Canvas/Headphone/Frame/Contents/Main"), contents.transform.Find("Main"));
            ledSubFrame.name = "LedSubFrame";
            ledSubFrame.transform.localPosition = Vector3.zero;

            GameObject originalSlider = ledSubFrame.transform.Find("Slider").gameObject;
            GameObject originalValue = ledSubFrame.transform.Find("Value").gameObject;
            ledSliderR = Instantiate(originalSlider, ledSubFrame.transform);
            ledSliderR.name = "SliderR";
            ledSliderR.GetComponent<RectTransform>().sizeDelta = new Vector2(250F, 20F);
            ledSliderG = Instantiate(originalSlider, ledSubFrame.transform);
            ledSliderG.name = "SliderG";
            ledSliderG.GetComponent<RectTransform>().sizeDelta = new Vector2(250F, 20F);
            ledSliderB = Instantiate(originalSlider, ledSubFrame.transform);
            ledSliderB.name = "SliderB";
            ledSliderB.GetComponent<RectTransform>().sizeDelta = new Vector2(250F, 20F);
            ledValueR = Instantiate(originalValue, ledSliderR.transform);
            ledValueR.name = "ValueR";
            ledValueR.GetComponent<Text>().color = new Color(1, 0, 0);
            ledValueG = Instantiate(originalValue, ledSliderG.transform);
            ledValueG.name = "ValueG";
            ledValueG.GetComponent<Text>().color = new Color(0, 1, 0);
            ledValueB = Instantiate(originalValue, ledSliderB.transform);
            ledValueB.name = "ValueB";
            ledValueB.GetComponent<Text>().color = new Color(0, 0, 1);


            ModdingUtil.ChangeSlider(ledSliderR.GetComponent<Slider>(), OnLedRgbChangeR);
            ModdingUtil.ChangeSlider(ledSliderG.GetComponent<Slider>(), OnLedRgbChangeG);
            ModdingUtil.ChangeSlider(ledSliderB.GetComponent<Slider>(), OnLedRgbChangeB);

            // cleanup
            DestroyImmediate(contents.transform.Find("Main/BrandButtons").gameObject);
            DestroyImmediate(contents.transform.Find("Main/ItemButtons/Check").gameObject);
            DestroyImmediate(contents.transform.Find("Main/AudioModeButtons/Check").gameObject);
            DestroyImmediate(ledSubFrame.transform.Find("HeadphoneIcon").gameObject);
            DestroyImmediate(originalSlider);
            DestroyImmediate(originalValue);
        }

        private GameObject CloneAndRewireButton(Transform parent, String objectName, String label, Vector2 size, String imageBase64, UnityAction action) {
            GameObject btn = Instantiate(cancelButton, parent);
            btn.name = objectName;

            btn.GetComponent<RectTransform>().sizeDelta = size;
            btn.transform.Find("ClickArea").GetComponent<RectTransform>().sizeDelta = size;

            ModdingUtil.ChangeImage(btn.GetComponent<Image>(), Convert.FromBase64String(imageBase64), (int)size.x, (int)size.y);
            ModdingUtil.ChangeButton(btn.GetComponent<Button>(), action);

            btn.transform.Find("Text").GetComponent<Text>().text = label;

            return btn;
        }

        #endregion
    }
}