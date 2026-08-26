using System.Drawing.Text;

namespace Haruka.Arcade.Apm.GameGuideEditor;

static class Program {
    public const String SEGA_FONT = "SEGA-NewRodinN B";

    [STAThread]
    static void Main() {
        ApplicationConfiguration.Initialize();

        if (!CheckFont(SEGA_FONT)) {
            MessageBox.Show("Could not find a required font. The preview might not be accurate.", "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        Application.Run(new FormMain());
    }

    private static bool CheckFont(string name) {
        InstalledFontCollection fontsCollection = new InstalledFontCollection();
        return fontsCollection.Families.Any(fontFamily => fontFamily.Name == name);
    }
}