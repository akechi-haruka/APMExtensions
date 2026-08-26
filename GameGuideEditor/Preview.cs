using System.Text.RegularExpressions;

namespace Haruka.Arcade.Apm.GameGuideEditor;

public partial class Preview : Form {
    private readonly Brush white;
    private readonly Brush buttonBlue;
    private readonly Font fontGuideText;
    private readonly Font fontButtonText;
    private readonly Dictionary<String, Image> imageCache = new Dictionary<string, Image>();
    private readonly StringFormat buttonTextFormat;

    private AppExConfig.GuidePage page;

    public Preview() {
        InitializeComponent();
        white = new SolidBrush(Color.White);
        buttonBlue = new SolidBrush(Color.Blue);
        fontGuideText = new Font(Program.SEGA_FONT, 24, GraphicsUnit.Pixel);
        fontButtonText = new Font(Program.SEGA_FONT, 16, GraphicsUnit.Pixel);
        buttonTextFormat = new StringFormat() {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
    }

    public void Redraw(AppExConfig.GuideInfo data, AppExConfig.GuidePage currentPage) {
        Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
        Width = data.width;
        Height = data.height + (screenRectangle.Top - Top);
        page = currentPage;
        Text = "Preview" + (currentPage != null ? ": " + currentPage.title : "");
        Refresh();
    }

    protected override void OnPaint(PaintEventArgs e) {
        if (page != null) {
            Rectangle screenRectangle = RectangleToScreen(ClientRectangle);

            int drawWidth = Width;
            int drawHeight = Height - (screenRectangle.Top - Top);
            ;

            if (page.file != null && !page.file.EndsWith(".txt")) {
                if (!imageCache.TryGetValue(page.file, out Image image)) {
                    try {
                        image = Image.FromFile(page.file);
                        imageCache[page.file] = image;
                    } catch (Exception ex) {
                        MessageBox.Show("Failed to read " + page.file + " in page " + page.title + ": " + ex.Message + "\nThe image will be removed.", "GameGuideEditor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        page.file = null;
                    }
                }

                if (image != null) {
                    e.Graphics.DrawImage(image, new Rectangle(0, 0, drawWidth, drawHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                }
            }

            StringFormat format = new StringFormat();
            if (page.align == "left") {
                format.Alignment = StringAlignment.Near;
            } else if (page.align == "right") {
                format.Alignment = StringAlignment.Far;
                drawWidth -= 24; // ???
            } else {
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
            }

            string text = StripUnityFormattingTags().Replace(page.text ?? "", "");
            SizeF textSize = e.Graphics.MeasureString(text, fontGuideText, PointF.Empty, format);

            PointF origin;
            if (page.align == "left") {
                origin = PointF.Empty;
            } else if (page.align == "right") {
                origin = new PointF(drawWidth - textSize.Width, 0);
            } else {
                origin = new PointF(drawWidth / 2F - textSize.Width / 2, drawHeight / 2F - textSize.Height / 2F);
            }

            e.Graphics.DrawString(text, fontGuideText, white, new RectangleF(origin, textSize), format);

            foreach (AppExConfig.GuideButton btn in page.content.buttons) {
                float x = btn.center ? btn.x - btn.width / 2F : btn.x;
                float y = btn.center ? btn.y - btn.height / 2F : btn.y;

                string buttonText = StripUnityFormattingTags().Replace(btn.text ?? "", "");

                e.Graphics.FillRectangle(buttonBlue, new RectangleF(x, y, btn.width, btn.height));
                e.Graphics.DrawString(buttonText, fontButtonText, white, new PointF(x + btn.width / 2F, y + btn.height / 2F), buttonTextFormat);
            }
        }
    }

    [GeneratedRegex(@"\<.*?\>")]
    private static partial Regex StripUnityFormattingTags();
}