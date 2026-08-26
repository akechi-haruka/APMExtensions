namespace Haruka.Arcade.Apm.GameGuideEditor;

partial class FormMain {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
        menuStrip1 = new System.Windows.Forms.MenuStrip();
        fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        guideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        saveFileDialog = new System.Windows.Forms.SaveFileDialog();
        openFileDialog = new System.Windows.Forms.OpenFileDialog();
        groupBox1 = new System.Windows.Forms.GroupBox();
        buttonPageDown = new System.Windows.Forms.Button();
        buttonPageUp = new System.Windows.Forms.Button();
        buttonAddPage = new System.Windows.Forms.Button();
        listBoxPages = new System.Windows.Forms.ListBox();
        listBoxElements = new System.Windows.Forms.ListBox();
        groupBox2 = new System.Windows.Forms.GroupBox();
        buttonButtonAdd = new System.Windows.Forms.Button();
        groupBoxPageData = new System.Windows.Forms.GroupBox();
        groupBoxElement = new System.Windows.Forms.GroupBox();
        label7 = new System.Windows.Forms.Label();
        comboBoxElementTarget = new System.Windows.Forms.ComboBox();
        checkBoxElementCenter = new System.Windows.Forms.CheckBox();
        label6 = new System.Windows.Forms.Label();
        textBoxElementText = new System.Windows.Forms.TextBox();
        numericUpDownElementH = new System.Windows.Forms.NumericUpDown();
        numericUpDownElementW = new System.Windows.Forms.NumericUpDown();
        label5 = new System.Windows.Forms.Label();
        numericUpDownElementY = new System.Windows.Forms.NumericUpDown();
        numericUpDownElementX = new System.Windows.Forms.NumericUpDown();
        label2 = new System.Windows.Forms.Label();
        textBoxTitle = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        buttonBrowseFile = new System.Windows.Forms.Button();
        textBoxFile = new System.Windows.Forms.TextBox();
        comboBoxAlignment = new System.Windows.Forms.ComboBox();
        label3 = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        textBoxText = new System.Windows.Forms.TextBox();
        openFileDialogPage = new System.Windows.Forms.OpenFileDialog();
        folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
        menuStrip1.SuspendLayout();
        groupBox1.SuspendLayout();
        groupBox2.SuspendLayout();
        groupBoxPageData.SuspendLayout();
        groupBoxElement.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementH).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementW).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementX).BeginInit();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
        menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, guideToolStripMenuItem });
        menuStrip1.Location = new System.Drawing.Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new System.Drawing.Size(1370, 33);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exportToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
        fileToolStripMenuItem.Text = "File";
        // 
        // newToolStripMenuItem
        // 
        newToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("newToolStripMenuItem.Image"));
        newToolStripMenuItem.Name = "newToolStripMenuItem";
        newToolStripMenuItem.Size = new System.Drawing.Size(165, 34);
        newToolStripMenuItem.Text = "New";
        newToolStripMenuItem.Click += newToolStripMenuItem_Click_1;
        // 
        // openToolStripMenuItem
        // 
        openToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("openToolStripMenuItem.Image"));
        openToolStripMenuItem.Name = "openToolStripMenuItem";
        openToolStripMenuItem.Size = new System.Drawing.Size(165, 34);
        openToolStripMenuItem.Text = "Open";
        openToolStripMenuItem.Click += openToolStripMenuItem_Click;
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("saveToolStripMenuItem.Image"));
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.Size = new System.Drawing.Size(165, 34);
        saveToolStripMenuItem.Text = "Save";
        saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
        // 
        // exportToolStripMenuItem
        // 
        exportToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("exportToolStripMenuItem.Image"));
        exportToolStripMenuItem.Name = "exportToolStripMenuItem";
        exportToolStripMenuItem.Size = new System.Drawing.Size(165, 34);
        exportToolStripMenuItem.Text = "Export";
        exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new System.Drawing.Size(162, 6);
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("exitToolStripMenuItem.Image"));
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        exitToolStripMenuItem.Size = new System.Drawing.Size(165, 34);
        exitToolStripMenuItem.Text = "Exit";
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // guideToolStripMenuItem
        // 
        guideToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { settingsToolStripMenuItem });
        guideToolStripMenuItem.Name = "guideToolStripMenuItem";
        guideToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
        guideToolStripMenuItem.Text = "Guide";
        // 
        // settingsToolStripMenuItem
        // 
        settingsToolStripMenuItem.Image = ((System.Drawing.Image)resources.GetObject("settingsToolStripMenuItem.Image"));
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
        settingsToolStripMenuItem.Size = new System.Drawing.Size(178, 34);
        settingsToolStripMenuItem.Text = "Settings";
        settingsToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
        settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click_1;
        // 
        // saveFileDialog
        // 
        saveFileDialog.Filter = "*.json|*.json";
        saveFileDialog.Title = "Save File";
        // 
        // openFileDialog
        // 
        openFileDialog.FileName = "*.json";
        openFileDialog.Filter = "*.json|*.json";
        openFileDialog.Title = "Open File";
        // 
        // groupBox1
        // 
        groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left));
        groupBox1.Controls.Add(buttonPageDown);
        groupBox1.Controls.Add(buttonPageUp);
        groupBox1.Controls.Add(buttonAddPage);
        groupBox1.Controls.Add(listBoxPages);
        groupBox1.Location = new System.Drawing.Point(12, 36);
        groupBox1.Name = "groupBox1";
        groupBox1.Size = new System.Drawing.Size(240, 452);
        groupBox1.TabIndex = 1;
        groupBox1.TabStop = false;
        groupBox1.Text = "Pages";
        // 
        // buttonPageDown
        // 
        buttonPageDown.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        buttonPageDown.AutoSize = true;
        buttonPageDown.Location = new System.Drawing.Point(166, 411);
        buttonPageDown.Name = "buttonPageDown";
        buttonPageDown.Size = new System.Drawing.Size(69, 35);
        buttonPageDown.TabIndex = 4;
        buttonPageDown.Text = "Down";
        buttonPageDown.UseVisualStyleBackColor = true;
        buttonPageDown.Click += buttonPageDown_Click;
        // 
        // buttonPageUp
        // 
        buttonPageUp.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        buttonPageUp.AutoSize = true;
        buttonPageUp.Location = new System.Drawing.Point(95, 411);
        buttonPageUp.Name = "buttonPageUp";
        buttonPageUp.Size = new System.Drawing.Size(65, 35);
        buttonPageUp.TabIndex = 3;
        buttonPageUp.Text = "Up";
        buttonPageUp.UseVisualStyleBackColor = true;
        buttonPageUp.Click += buttonPageUp_Click;
        // 
        // buttonAddPage
        // 
        buttonAddPage.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        buttonAddPage.AutoSize = true;
        buttonAddPage.Location = new System.Drawing.Point(6, 411);
        buttonAddPage.Name = "buttonAddPage";
        buttonAddPage.Size = new System.Drawing.Size(83, 35);
        buttonAddPage.TabIndex = 2;
        buttonAddPage.Text = "Add";
        buttonAddPage.UseVisualStyleBackColor = true;
        buttonAddPage.Click += buttonAddPage_Click_1;
        // 
        // listBoxPages
        // 
        listBoxPages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        listBoxPages.FormattingEnabled = true;
        listBoxPages.Location = new System.Drawing.Point(6, 30);
        listBoxPages.Name = "listBoxPages";
        listBoxPages.Size = new System.Drawing.Size(228, 379);
        listBoxPages.TabIndex = 1;
        listBoxPages.SelectedIndexChanged += listBoxPages_SelectedIndexChanged;
        listBoxPages.KeyUp += listBoxPages_KeyUp;
        // 
        // listBoxElements
        // 
        listBoxElements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right));
        listBoxElements.FormattingEnabled = true;
        listBoxElements.Location = new System.Drawing.Point(6, 30);
        listBoxElements.Name = "listBoxElements";
        listBoxElements.Size = new System.Drawing.Size(217, 329);
        listBoxElements.TabIndex = 0;
        listBoxElements.SelectedIndexChanged += listBoxElements_SelectedIndexChanged;
        listBoxElements.KeyUp += listBoxElements_KeyUp;
        // 
        // groupBox2
        // 
        groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left));
        groupBox2.Controls.Add(buttonButtonAdd);
        groupBox2.Controls.Add(listBoxElements);
        groupBox2.Location = new System.Drawing.Point(485, 30);
        groupBox2.Name = "groupBox2";
        groupBox2.Size = new System.Drawing.Size(229, 416);
        groupBox2.TabIndex = 2;
        groupBox2.TabStop = false;
        groupBox2.Text = "Elements";
        // 
        // buttonButtonAdd
        // 
        buttonButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        buttonButtonAdd.AutoSize = true;
        buttonButtonAdd.Location = new System.Drawing.Point(1, 375);
        buttonButtonAdd.Name = "buttonButtonAdd";
        buttonButtonAdd.Size = new System.Drawing.Size(222, 35);
        buttonButtonAdd.TabIndex = 3;
        buttonButtonAdd.Text = "Add";
        buttonButtonAdd.UseVisualStyleBackColor = true;
        buttonButtonAdd.Click += buttonButtonAdd_Click;
        // 
        // groupBoxPageData
        // 
        groupBoxPageData.Controls.Add(groupBoxElement);
        groupBoxPageData.Controls.Add(textBoxTitle);
        groupBoxPageData.Controls.Add(label4);
        groupBoxPageData.Controls.Add(buttonBrowseFile);
        groupBoxPageData.Controls.Add(textBoxFile);
        groupBoxPageData.Controls.Add(comboBoxAlignment);
        groupBoxPageData.Controls.Add(label3);
        groupBoxPageData.Controls.Add(label1);
        groupBoxPageData.Controls.Add(textBoxText);
        groupBoxPageData.Controls.Add(groupBox2);
        groupBoxPageData.Enabled = false;
        groupBoxPageData.Location = new System.Drawing.Point(258, 36);
        groupBoxPageData.Name = "groupBoxPageData";
        groupBoxPageData.Size = new System.Drawing.Size(1100, 452);
        groupBoxPageData.TabIndex = 3;
        groupBoxPageData.TabStop = false;
        groupBoxPageData.Text = "Page Data";
        // 
        // groupBoxElement
        // 
        groupBoxElement.Controls.Add(label7);
        groupBoxElement.Controls.Add(comboBoxElementTarget);
        groupBoxElement.Controls.Add(checkBoxElementCenter);
        groupBoxElement.Controls.Add(label6);
        groupBoxElement.Controls.Add(textBoxElementText);
        groupBoxElement.Controls.Add(numericUpDownElementH);
        groupBoxElement.Controls.Add(numericUpDownElementW);
        groupBoxElement.Controls.Add(label5);
        groupBoxElement.Controls.Add(numericUpDownElementY);
        groupBoxElement.Controls.Add(numericUpDownElementX);
        groupBoxElement.Controls.Add(label2);
        groupBoxElement.Enabled = false;
        groupBoxElement.Location = new System.Drawing.Point(720, 30);
        groupBoxElement.Name = "groupBoxElement";
        groupBoxElement.Size = new System.Drawing.Size(374, 416);
        groupBoxElement.TabIndex = 13;
        groupBoxElement.TabStop = false;
        groupBoxElement.Text = "Element Data";
        // 
        // label7
        // 
        label7.AutoSize = true;
        label7.Location = new System.Drawing.Point(6, 185);
        label7.Name = "label7";
        label7.Size = new System.Drawing.Size(47, 25);
        label7.TabIndex = 10;
        label7.Text = "Link:";
        // 
        // comboBoxElementTarget
        // 
        comboBoxElementTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxElementTarget.FormattingEnabled = true;
        comboBoxElementTarget.Location = new System.Drawing.Point(98, 182);
        comboBoxElementTarget.Name = "comboBoxElementTarget";
        comboBoxElementTarget.Size = new System.Drawing.Size(270, 33);
        comboBoxElementTarget.TabIndex = 9;
        comboBoxElementTarget.SelectedIndexChanged += comboBoxElementTarget_SelectedIndexChanged;
        // 
        // checkBoxElementCenter
        // 
        checkBoxElementCenter.AutoSize = true;
        checkBoxElementCenter.Location = new System.Drawing.Point(6, 147);
        checkBoxElementCenter.Name = "checkBoxElementCenter";
        checkBoxElementCenter.Size = new System.Drawing.Size(109, 29);
        checkBoxElementCenter.TabIndex = 8;
        checkBoxElementCenter.Text = "Centered";
        checkBoxElementCenter.UseVisualStyleBackColor = true;
        checkBoxElementCenter.CheckedChanged += checkBoxElementCenter_CheckedChanged;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Location = new System.Drawing.Point(6, 113);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(46, 25);
        label6.TabIndex = 7;
        label6.Text = "Text:";
        // 
        // textBoxElementText
        // 
        textBoxElementText.Location = new System.Drawing.Point(188, 110);
        textBoxElementText.Name = "textBoxElementText";
        textBoxElementText.Size = new System.Drawing.Size(180, 31);
        textBoxElementText.TabIndex = 6;
        textBoxElementText.TextChanged += textBoxElementText_TextChanged;
        // 
        // numericUpDownElementH
        // 
        numericUpDownElementH.Location = new System.Drawing.Point(281, 73);
        numericUpDownElementH.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownElementH.Name = "numericUpDownElementH";
        numericUpDownElementH.Size = new System.Drawing.Size(87, 31);
        numericUpDownElementH.TabIndex = 5;
        numericUpDownElementH.ValueChanged += numericUpDownElementH_ValueChanged;
        // 
        // numericUpDownElementW
        // 
        numericUpDownElementW.Location = new System.Drawing.Point(188, 73);
        numericUpDownElementW.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownElementW.Name = "numericUpDownElementW";
        numericUpDownElementW.Size = new System.Drawing.Size(87, 31);
        numericUpDownElementW.TabIndex = 4;
        numericUpDownElementW.ValueChanged += numericUpDownElementW_ValueChanged;
        // 
        // label5
        // 
        label5.AutoSize = true;
        label5.Location = new System.Drawing.Point(6, 75);
        label5.Name = "label5";
        label5.Size = new System.Drawing.Size(47, 25);
        label5.TabIndex = 3;
        label5.Text = "Size:";
        // 
        // numericUpDownElementY
        // 
        numericUpDownElementY.Location = new System.Drawing.Point(281, 30);
        numericUpDownElementY.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownElementY.Name = "numericUpDownElementY";
        numericUpDownElementY.Size = new System.Drawing.Size(87, 31);
        numericUpDownElementY.TabIndex = 2;
        numericUpDownElementY.ValueChanged += numericUpDownElementY_ValueChanged;
        // 
        // numericUpDownElementX
        // 
        numericUpDownElementX.Location = new System.Drawing.Point(188, 30);
        numericUpDownElementX.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownElementX.Name = "numericUpDownElementX";
        numericUpDownElementX.Size = new System.Drawing.Size(87, 31);
        numericUpDownElementX.TabIndex = 1;
        numericUpDownElementX.ValueChanged += numericUpDownElementX_ValueChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(6, 32);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(79, 25);
        label2.TabIndex = 0;
        label2.Text = "Position:";
        // 
        // textBoxTitle
        // 
        textBoxTitle.Location = new System.Drawing.Point(130, 33);
        textBoxTitle.Name = "textBoxTitle";
        textBoxTitle.Size = new System.Drawing.Size(349, 31);
        textBoxTitle.TabIndex = 12;
        textBoxTitle.TextChanged += textBoxTitle_TextChanged;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(6, 109);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(42, 25);
        label4.TabIndex = 11;
        label4.Text = "File:";
        // 
        // buttonBrowseFile
        // 
        buttonBrowseFile.Location = new System.Drawing.Point(405, 106);
        buttonBrowseFile.Name = "buttonBrowseFile";
        buttonBrowseFile.Size = new System.Drawing.Size(74, 31);
        buttonBrowseFile.TabIndex = 10;
        buttonBrowseFile.Text = "...";
        buttonBrowseFile.UseVisualStyleBackColor = true;
        buttonBrowseFile.Click += buttonBrowseFile_Click;
        // 
        // textBoxFile
        // 
        textBoxFile.Enabled = false;
        textBoxFile.Location = new System.Drawing.Point(130, 106);
        textBoxFile.Name = "textBoxFile";
        textBoxFile.Size = new System.Drawing.Size(269, 31);
        textBoxFile.TabIndex = 9;
        // 
        // comboBoxAlignment
        // 
        comboBoxAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxAlignment.FormattingEnabled = true;
        comboBoxAlignment.Items.AddRange(new object[] { "center", "left", "right" });
        comboBoxAlignment.Location = new System.Drawing.Point(130, 67);
        comboBoxAlignment.Name = "comboBoxAlignment";
        comboBoxAlignment.Size = new System.Drawing.Size(349, 33);
        comboBoxAlignment.TabIndex = 8;
        comboBoxAlignment.SelectedIndexChanged += comboBoxAlignment_SelectedIndexChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(6, 70);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(98, 25);
        label3.TabIndex = 7;
        label3.Text = "Alignment:";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(6, 33);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(48, 25);
        label1.TabIndex = 5;
        label1.Text = "Title:";
        // 
        // textBoxText
        // 
        textBoxText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left));
        textBoxText.Location = new System.Drawing.Point(6, 143);
        textBoxText.Multiline = true;
        textBoxText.Name = "textBoxText";
        textBoxText.Size = new System.Drawing.Size(473, 303);
        textBoxText.TabIndex = 4;
        textBoxText.TextChanged += textBoxText_TextChanged;
        // 
        // openFileDialogPage
        // 
        openFileDialogPage.Filter = ("Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png|Text files (*.txt" + ")|*.txt|All files (*.*)|*.*");
        openFileDialogPage.Title = "Select image or content file for page";
        // 
        // folderBrowserDialog
        // 
        folderBrowserDialog.Description = "Select directory to export guide to. (should be the AppEx directory of a game)";
        // 
        // FormMain
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1370, 500);
        Controls.Add(groupBoxPageData);
        Controls.Add(groupBox1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "GameGuideEditor";
        FormClosing += FormMain_FormClosing;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        groupBox1.ResumeLayout(false);
        groupBox1.PerformLayout();
        groupBox2.ResumeLayout(false);
        groupBox2.PerformLayout();
        groupBoxPageData.ResumeLayout(false);
        groupBoxPageData.PerformLayout();
        groupBoxElement.ResumeLayout(false);
        groupBoxElement.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementH).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementW).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementY).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownElementX).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.TextBox textBoxElementText;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox checkBoxElementCenter;
    private System.Windows.Forms.ComboBox comboBoxElementTarget;
    private System.Windows.Forms.Label label7;

    private System.Windows.Forms.NumericUpDown numericUpDownElementH;
    private System.Windows.Forms.NumericUpDown numericUpDownElementW;
    private System.Windows.Forms.Label label5;

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown numericUpDownElementX;
    private System.Windows.Forms.NumericUpDown numericUpDownElementY;

    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;

    private System.Windows.Forms.GroupBox groupBoxElement;

    private System.Windows.Forms.Button buttonButtonAdd;
    private System.Windows.Forms.Button buttonPageUp;
    private System.Windows.Forms.Button buttonPageDown;

    private System.Windows.Forms.OpenFileDialog openFileDialogPage;

    private System.Windows.Forms.TextBox textBoxTitle;

    private System.Windows.Forms.TextBox textBoxText;

    private System.Windows.Forms.Button buttonBrowseFile;
    private System.Windows.Forms.Label label4;

    private System.Windows.Forms.TextBox textBoxFile;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboBoxAlignment;

    private System.Windows.Forms.Button buttonAddPage;

    private System.Windows.Forms.GroupBox groupBoxPageData;

    private System.Windows.Forms.ListBox listBoxPages;

    private System.Windows.Forms.ListBox listBoxElements;

    private System.Windows.Forms.ToolStripMenuItem guideToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;

    private System.Windows.Forms.SaveFileDialog saveFileDialog;
    private System.Windows.Forms.OpenFileDialog openFileDialog;

    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

    #endregion
}