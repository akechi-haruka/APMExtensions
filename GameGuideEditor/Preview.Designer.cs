using System.ComponentModel;

namespace Haruka.Arcade.Apm.GameGuideEditor;

partial class Preview {
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
        SuspendLayout();
        // 
        // Preview
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.Black;
        ClientSize = new System.Drawing.Size(800, 450);
        ControlBox = false;
        DoubleBuffered = true;
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        ShowInTaskbar = false;
        Text = "Preview";
        ResumeLayout(false);
    }

    #endregion
}