using Haruka.Arcade.Apm.EMUICF;

namespace GameGuideEditor;

public partial class Preview : Form {
    public Preview() {
        InitializeComponent();
    }

    public void Redraw(AppExConfig.GuideInfo data) {
        Width = data.width;
        Height = data.height;
    }
}