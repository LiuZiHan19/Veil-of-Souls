using UnityEngine.UI;

public class MenuSettingView : UIBehaviour
{
    private Slider _musicSlider;
    private Slider _soundSlider;
    private Button _closeBtn;
    private IDataProvider _dataProvider;

    public MenuSettingView(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    protected override void ParseComponent()
    {
        _musicSlider = FindComponent<Slider>("Middle/MusicSlider");
        _soundSlider = FindComponent<Slider>("Middle/SoundSlider");
        _closeBtn = FindComponent<Button>("Top/Button_Back");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RegisterButtonEvent(_closeBtn, OnClickCloseBtn);
        _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        _soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
    }

    public override void Show()
    {
        base.Show();
        _musicSlider.value = _dataProvider.MusicVolume;
        _soundSlider.value = _dataProvider.SoundVolume;
    }

    private void OnSoundSliderValueChanged(float arg0)
    {
        SoundManager.Instance.UpdateSfxVolume(arg0);
        _dataProvider.SoundVolume = arg0;
    }

    private void OnMusicSliderValueChanged(float arg0)
    {
        SoundManager.Instance.UpdateMusicVolume(arg0);
        _dataProvider.MusicVolume = arg0;
    }

    private void OnClickCloseBtn()
    {
        SoundManager.Instance.PlaySharedSfx(AudioID.SfxButtonClick);
        Hide();
    }

    protected override void RemoveEvent()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
        _soundSlider.onValueChanged.RemoveListener(OnSoundSliderValueChanged);
        UnRegisterButtonEvent(_closeBtn, OnClickCloseBtn);
        base.RemoveEvent();
    }
}