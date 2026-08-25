using Haruka.Arcade.Apm.EMUICF;
using Newtonsoft.Json;

namespace GameGuideEditor;

public partial class FormMain : Form {
    private readonly Preview preview;
    private AppExConfig.GuideInfo data = new AppExConfig.GuideInfo();

    public FormMain() {
        InitializeComponent();
        preview = new Preview();
        preview.Show(this);
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
        preview.Close();
    }

    #region Menu

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {
        if (openFileDialog.ShowDialog(this) == DialogResult.OK) {
            ResetUi();
            try {
                data = JsonConvert.DeserializeObject<AppExConfig.GuideInfo>(File.ReadAllText(openFileDialog.FileName));
            } catch (Exception ex) {
                MessageBox.Show(this, "Error opening file: " + ex.Message, "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ReSharper disable once CoVariantArrayConversion
            listBoxPages.Items.AddRange(data.pages.ToArray());
            UpdateUi();
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
        if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
            try {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(data));
            } catch (Exception ex) {
                MessageBox.Show(this, "Error saving file: " + ex.Message, "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void exportToolStripMenuItem_Click(object sender, EventArgs e) {
        if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK) {
            // TODO: export
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        Close();
    }

    private void newToolStripMenuItem_Click_1(object sender, EventArgs e) {
        data = new AppExConfig.GuideInfo();
        ResetUi();
    }

    private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e) {
        Settings s = new Settings(data.width, data.height);
        if (s.ShowDialog(this) == DialogResult.OK) {
            data.width = s.SelectedWidth;
            data.height = s.SelectedHeight;
            UpdateUi();
        }
    }

    #endregion

    private void ResetUi() {
        groupBoxPageData.Enabled = false;
    }

    private void UpdateUi() {
        SuspendLayout();
        groupBoxPageData.Enabled = listBoxPages.SelectedItem != null;
        groupBoxElement.Enabled = false;

        listBoxElements.Items.Clear();
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;

            textBoxTitle.Text = page.title;
            textBoxText.Text = page.text;
            textBoxFile.Text = page.file;
            comboBoxAlignment.SelectedItem = page.align;

            foreach (AppExConfig.GuideButton element in page.content.buttons) {
                listBoxElements.Items.Add(element);
            }
        } else {
            textBoxTitle.Text = "";
            textBoxText.Text = "";
            textBoxFile.Text = "";
            comboBoxAlignment.SelectedItem = null;
        }

        data.pages = listBoxPages.Items.Cast<AppExConfig.GuidePage>().ToList();

        preview.Redraw(data);
        ResumeLayout(true);
    }

    #region Page controls

    private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e) {
        UpdateUi();
    }

    private void listBoxPages_KeyUp(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Delete && listBoxPages.SelectedItem != null) {
            listBoxPages.Items.RemoveAt(listBoxPages.SelectedIndex);
        }
    }

    private void buttonAddPage_Click_1(object sender, EventArgs e) {
        listBoxPages.Items.Add(new AppExConfig.GuidePage() {
            title = "New Page"
        });
        listBoxPages.SelectedIndex = listBoxPages.Items.Count - 1;
        UpdateUi();
    }

    private void textBoxTitle_TextChanged(object sender, EventArgs e) {
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.title = textBoxTitle.Text;
            listBoxPages.Items[listBoxPages.SelectedIndex] = listBoxPages.Items[listBoxPages.SelectedIndex];
        }
    }

    private void textBoxText_TextChanged(object sender, EventArgs e) {
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.text = textBoxText.Text;
            UpdateUi();
        }
    }

    private void comboBoxAlignment_SelectedIndexChanged(object sender, EventArgs e) {
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.align = comboBoxAlignment.SelectedItem?.ToString() ?? "center";
            UpdateUi();
        }
    }

    private void buttonBrowseFile_Click(object sender, EventArgs e) {
        if (openFileDialogPage.ShowDialog(this) == DialogResult.OK) {
            if (listBoxPages.SelectedItem != null) {
                AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
                page.file = textBoxFile.Text = openFileDialogPage.FileName;
                UpdateUi();
            }
        }
    }

    private void buttonPageUp_Click(object sender, EventArgs e) {
        if (listBoxPages.SelectedIndex > -1) {
            listBoxPages.MoveSelectedItemUp();
        }
    }

    private void buttonPageDown_Click(object sender, EventArgs e) {
        if (listBoxPages.SelectedIndex > -1) {
            listBoxPages.MoveSelectedItemDown();
        }
    }

    #endregion

    #region Element controls

    private void buttonButtonAdd_Click(object sender, EventArgs e) {
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.content.buttons.Add(new AppExConfig.GuideButton() {
                text = "New Button"
            });
            UpdateUi();
        }
    }

    private void listBoxElements_KeyUp(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Delete && listBoxElements.SelectedItem != null) {
            listBoxElements.Items.RemoveAt(listBoxElements.SelectedIndex);
        }
    }

    private void listBoxElements_SelectedIndexChanged(object sender, EventArgs e) {
        groupBoxElement.Enabled = listBoxElements.SelectedItem != null;
        if (listBoxElements.SelectedItem != null) {
        }
    }

    #endregion
}