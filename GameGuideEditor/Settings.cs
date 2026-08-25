namespace GameGuideEditor;

public partial class Settings : Form {
    public int SelectedWidth { get; private set; }
    public int SelectedHeight { get; private set; }

    public Settings(int w, int h) {
        InitializeComponent();
        numericUpDownWidth.Value = w;
        numericUpDownHeight.Value = h;
    }

    private void buttonOK_Click(object sender, EventArgs e) {
        SelectedWidth = (int)numericUpDownWidth.Value;
        SelectedHeight = (int)numericUpDownHeight.Value;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void Settings_Load(object sender, EventArgs e) {
    }
}