using System.ComponentModel;

namespace Haruka.Arcade.Apm.GameGuideEditor;

partial class Settings {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
        numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
        buttonOK = new System.Windows.Forms.Button();
        buttonCancel = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)numericUpDownWidth).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 14);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(64, 25);
        label1.TabIndex = 0;
        label1.Text = "Width:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(12, 51);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(69, 25);
        label2.TabIndex = 1;
        label2.Text = "Height:";
        // 
        // numericUpDownWidth
        // 
        numericUpDownWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        numericUpDownWidth.Location = new System.Drawing.Point(198, 12);
        numericUpDownWidth.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownWidth.Minimum = new decimal(new int[] { 300, 0, 0, 0 });
        numericUpDownWidth.Name = "numericUpDownWidth";
        numericUpDownWidth.Size = new System.Drawing.Size(120, 31);
        numericUpDownWidth.TabIndex = 2;
        numericUpDownWidth.Value = new decimal(new int[] { 300, 0, 0, 0 });
        // 
        // numericUpDownHeight
        // 
        numericUpDownHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
        numericUpDownHeight.Location = new System.Drawing.Point(198, 49);
        numericUpDownHeight.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        numericUpDownHeight.Minimum = new decimal(new int[] { 300, 0, 0, 0 });
        numericUpDownHeight.Name = "numericUpDownHeight";
        numericUpDownHeight.Size = new System.Drawing.Size(120, 31);
        numericUpDownHeight.TabIndex = 3;
        numericUpDownHeight.Value = new decimal(new int[] { 300, 0, 0, 0 });
        // 
        // buttonOK
        // 
        buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
        buttonOK.AutoSize = true;
        buttonOK.Location = new System.Drawing.Point(12, 139);
        buttonOK.Name = "buttonOK";
        buttonOK.Size = new System.Drawing.Size(86, 35);
        buttonOK.TabIndex = 4;
        buttonOK.Text = "OK";
        buttonOK.UseVisualStyleBackColor = true;
        buttonOK.Click += buttonOK_Click;
        // 
        // buttonCancel
        // 
        buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
        buttonCancel.AutoSize = true;
        buttonCancel.Location = new System.Drawing.Point(232, 139);
        buttonCancel.Name = "buttonCancel";
        buttonCancel.Size = new System.Drawing.Size(86, 35);
        buttonCancel.TabIndex = 5;
        buttonCancel.Text = "Cancel";
        buttonCancel.UseVisualStyleBackColor = true;
        buttonCancel.Click += buttonCancel_Click;
        // 
        // Settings
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(330, 186);
        Controls.Add(buttonCancel);
        Controls.Add(buttonOK);
        Controls.Add(numericUpDownHeight);
        Controls.Add(numericUpDownWidth);
        Controls.Add(label2);
        Controls.Add(label1);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new System.Drawing.Size(352, 242);
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Settings";
        Load += Settings_Load;
        ((System.ComponentModel.ISupportInitialize)numericUpDownWidth).EndInit();
        ((System.ComponentModel.ISupportInitialize)numericUpDownHeight).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.Button buttonCancel;

    private System.Windows.Forms.NumericUpDown numericUpDownHeight;
    private System.Windows.Forms.Button buttonOK;

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown numericUpDownWidth;

    #endregion
}