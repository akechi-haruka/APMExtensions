using Newtonsoft.Json;

namespace Haruka.Arcade.Apm.GameGuideEditor;

public partial class FormMain : Form {
    private readonly Preview preview;
    private AppExConfig.GuideInfo data = new AppExConfig.GuideInfo();

    private bool blockEvents;

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

            // ReSharper disable CoVariantArrayConversion
            listBoxPages.Items.AddRange(data.pages.ToArray());
            comboBoxElementTarget.Items.AddRange(data.pages.ToArray());
            // ReSharper enable CoVariantArrayConversion

            foreach (AppExConfig.GuidePage page in listBoxPages.Items.Cast<AppExConfig.GuidePage>()) {
                foreach (AppExConfig.GuideButton element in page.content.buttons) {
                    element.LinkTargetToTargetObject(listBoxPages.Items.Cast<AppExConfig.GuidePage>().ToList());
                }
            }

            UpdateUi();
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
        if (saveFileDialog.ShowDialog(this) == DialogResult.OK) {
            try {
                UpdateTargetIds();
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(data));
            } catch (Exception ex) {
                MessageBox.Show(this, "Error saving file: " + ex.Message, "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void exportToolStripMenuItem_Click(object sender, EventArgs e) {
        if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK) {
            try {
                UpdateTargetIds();

                string filePath = Path.Combine(folderBrowserDialog.SelectedPath, "guide.json");

                foreach (AppExConfig.GuidePage page in listBoxPages.Items.Cast<AppExConfig.GuidePage>()) {
                    if (page.file != null) {
                        string relativeFilePath = Path.GetFileName(page.file);
                        File.Copy(page.file, Path.Combine(folderBrowserDialog.SelectedPath, relativeFilePath));
                        page.file = relativeFilePath;
                    }
                }

                File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
            } catch (Exception ex) {
                MessageBox.Show(this, "Error exporting guide: " + ex.Message, "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        groupBoxElement.Enabled = false;
        listBoxPages.Items.Clear();
        comboBoxElementTarget.Items.Clear();
    }

    private void UpdateUiAfterPageChange() {
        SuspendLayout();

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
        ResumeLayout(true);
    }

    private void UpdateUi() {
        preview.Redraw(data, listBoxPages.SelectedItem as AppExConfig.GuidePage);
    }

    #region Page controls

    private void listBoxPages_SelectedIndexChanged(object sender, EventArgs e) {
        if (blockEvents) {
            return;
        }

        groupBoxPageData.Enabled = listBoxPages.SelectedItem != null;
        UpdateUiAfterPageChange();
        UpdateUi();
    }

    private void listBoxPages_KeyUp(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Delete && listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage toDelete = (AppExConfig.GuidePage)listBoxPages.SelectedItem;

            foreach (AppExConfig.GuidePage page in listBoxPages.Items.Cast<AppExConfig.GuidePage>()) {
                foreach (AppExConfig.GuideButton element in page.content.buttons) {
                    if (element.targetObject == toDelete) {
                        element.targetObject = null;
                        element.target = 0;
                    }
                }
            }

            listBoxPages.Items.RemoveAt(listBoxPages.SelectedIndex);
            comboBoxElementTarget.Items.RemoveAt(listBoxPages.SelectedIndex);

            UpdateUiAfterPageChange();
        }
    }

    private void buttonAddPage_Click_1(object sender, EventArgs e) {
        AppExConfig.GuidePage i = new AppExConfig.GuidePage() {
            title = "New Page"
        };
        listBoxPages.Items.Add(i);
        comboBoxElementTarget.Items.Add(i);
        listBoxPages.SelectedIndex = listBoxPages.Items.Count - 1;
        UpdateUi();
    }

    private void textBoxTitle_TextChanged(object sender, EventArgs e) {
        if (listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.title = textBoxTitle.Text;

            blockEvents = true;
            listBoxPages.Items[listBoxPages.SelectedIndex] = listBoxPages.Items[listBoxPages.SelectedIndex];

            for (int i = 0; i < comboBoxElementTarget.Items.Count; i++) {
                comboBoxElementTarget.Items[i] = comboBoxElementTarget.Items[i];
            }

            blockEvents = false;
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
                text = "New Button",
                width = 100,
                height = 40,
            });
            UpdateUiAfterPageChange();
            listBoxElements.SelectedIndex = listBoxElements.Items.Count - 1;
        }
    }

    private void listBoxElements_KeyUp(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Delete && listBoxElements.SelectedItem != null && listBoxPages.SelectedItem != null) {
            AppExConfig.GuidePage page = (AppExConfig.GuidePage)listBoxPages.SelectedItem;
            page.content.buttons.Remove((AppExConfig.GuideButton)listBoxElements.SelectedItem);
            UpdateUiAfterPageChange();
        }
    }

    private void listBoxElements_SelectedIndexChanged(object sender, EventArgs e) {
        if (blockEvents) {
            return;
        }

        groupBoxElement.Enabled = listBoxElements.SelectedItem != null;
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            numericUpDownElementX.Value = btn.x;
            numericUpDownElementY.Value = btn.y;
            numericUpDownElementW.Value = btn.width;
            numericUpDownElementH.Value = btn.height;
            checkBoxElementCenter.Checked = btn.center;
            textBoxElementText.Text = btn.text;
            btn.LinkTargetToTargetObject(listBoxPages.Items.Cast<AppExConfig.GuidePage>().ToList());

            comboBoxElementTarget.SelectedItem = btn.targetObject;
        }
    }

    private void numericUpDownElementX_ValueChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.x = (int)numericUpDownElementX.Value;
            UpdateUi();
        }
    }

    private void numericUpDownElementY_ValueChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.y = (int)numericUpDownElementY.Value;
            UpdateUi();
        }
    }

    private void numericUpDownElementW_ValueChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.width = (int)numericUpDownElementW.Value;
            UpdateUi();
        }
    }

    private void numericUpDownElementH_ValueChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.height = (int)numericUpDownElementH.Value;
            UpdateUi();
        }
    }

    private void textBoxElementText_TextChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.text = textBoxElementText.Text;

            blockEvents = true;
            listBoxElements.Items[listBoxElements.SelectedIndex] = listBoxElements.Items[listBoxElements.SelectedIndex];
            blockEvents = false;

            UpdateUi();
        }
    }

    private void checkBoxElementCenter_CheckedChanged(object sender, EventArgs e) {
        if (listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.center = checkBoxElementCenter.Checked;
            UpdateUi();
        }
    }

    private void comboBoxElementTarget_SelectedIndexChanged(object sender, EventArgs e) {
        if (comboBoxElementTarget.SelectedItem != null && listBoxElements.SelectedItem != null) {
            AppExConfig.GuideButton btn = (AppExConfig.GuideButton)listBoxElements.SelectedItem;
            btn.targetObject = (AppExConfig.GuidePage)comboBoxElementTarget.SelectedItem;
        }
    }

    private void UpdateTargetIds() {
        List<AppExConfig.GuidePage> pages = listBoxPages.Items.Cast<AppExConfig.GuidePage>().ToList();
        foreach (AppExConfig.GuidePage page in pages) {
            foreach (AppExConfig.GuideButton btn in page.content.buttons) {
                if (btn.targetObject != null) {
                    for (int i = 0; i < pages.Count; i++) {
                        if (pages[i] == btn.targetObject) {
                            btn.target = i + 1;
                        }
                    }
                }
            }
        }
    }

    #endregion
}