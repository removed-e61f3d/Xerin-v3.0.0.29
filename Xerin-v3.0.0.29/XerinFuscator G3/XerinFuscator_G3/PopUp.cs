using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using XerinFuscator_G3.UI;

namespace XerinFuscator_G3;

internal class PopUp : Form
{
	private IContainer components;

	private Guna2Elipse guna2Elipse1;

	private Panel header;

	private Guna2HtmlLabel headerTitle;

	private Guna2ProgressBar guna2ProgressBar1;

	private Guna2CirclePictureBox guna2CirclePictureBox1;

	private Guna2HtmlLabel content;

	private Guna2ShadowForm guna2ShadowForm1;

	public static int R { get; set; }

	public static int G { get; set; }

	public static int B { get; set; }

	public static string Title { get; set; }

	public static string Content { get; set; }

	public PopUp()
	{
		InitializeComponent();
	}

	private async void PopUp_Load(object sender, EventArgs e)
	{
		guna2ShadowForm1.SetShadowForm(this);
		headerTitle.ForeColor = UIColors.readColor(R, G, B);
		guna2ProgressBar1.ProgressColor = UIColors.readColor(R, G, B);
		guna2ProgressBar1.ProgressColor2 = UIColors.readColor(R, G, B);
		NotificationPopUp.Position(this);
		headerTitle.Text = Title;
		content.Text = Content;
		await Task.Delay(1500);
		Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XerinFuscator_G3.PopUp));
		this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
		this.header = new System.Windows.Forms.Panel();
		this.headerTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2ProgressBar1 = new Guna.UI2.WinForms.Guna2ProgressBar();
		this.content = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
		this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
		this.header.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.guna2CirclePictureBox1).BeginInit();
		base.SuspendLayout();
		this.guna2Elipse1.BorderRadius = 10;
		this.guna2Elipse1.TargetControl = this;
		this.header.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.header.Controls.Add(this.headerTitle);
		this.header.Location = new System.Drawing.Point(0, 0);
		this.header.Name = "header";
		this.header.Size = new System.Drawing.Size(440, 40);
		this.header.TabIndex = 8;
		this.headerTitle.BackColor = System.Drawing.Color.Transparent;
		this.headerTitle.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.headerTitle.ForeColor = System.Drawing.Color.FromArgb(222, 85, 85);
		this.headerTitle.IsContextMenuEnabled = false;
		this.headerTitle.IsSelectionEnabled = false;
		this.headerTitle.Location = new System.Drawing.Point(10, 11);
		this.headerTitle.Name = "headerTitle";
		this.headerTitle.Size = new System.Drawing.Size(28, 18);
		this.headerTitle.TabIndex = 2;
		this.headerTitle.Text = "TITLE";
		this.guna2ProgressBar1.FillColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.guna2ProgressBar1.Location = new System.Drawing.Point(0, 118);
		this.guna2ProgressBar1.Name = "guna2ProgressBar1";
		this.guna2ProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(222, 85, 85);
		this.guna2ProgressBar1.ProgressColor2 = System.Drawing.Color.FromArgb(222, 85, 85);
		this.guna2ProgressBar1.Size = new System.Drawing.Size(440, 2);
		this.guna2ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		this.guna2ProgressBar1.TabIndex = 10;
		this.guna2ProgressBar1.Text = "guna2ProgressBar1";
		this.guna2ProgressBar1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
		this.content.AutoSize = false;
		this.content.BackColor = System.Drawing.Color.Transparent;
		this.content.Font = new System.Drawing.Font("Bahnschrift", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.content.ForeColor = System.Drawing.Color.White;
		this.content.IsContextMenuEnabled = false;
		this.content.IsSelectionEnabled = false;
		this.content.Location = new System.Drawing.Point(55, 54);
		this.content.Name = "content";
		this.content.Size = new System.Drawing.Size(371, 47);
		this.content.TabIndex = 11;
		this.content.Text = "CONTENT";
		this.content.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
		this.guna2CirclePictureBox1.Image = nigger1.info_32px;
		this.guna2CirclePictureBox1.ImageRotate = 0f;
		this.guna2CirclePictureBox1.Location = new System.Drawing.Point(17, 63);
		this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
		this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
		this.guna2CirclePictureBox1.Size = new System.Drawing.Size(32, 32);
		this.guna2CirclePictureBox1.TabIndex = 12;
		this.guna2CirclePictureBox1.TabStop = false;
		this.guna2ShadowForm1.BorderRadius = 10;
		this.guna2ShadowForm1.TargetForm = this;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
		base.ClientSize = new System.Drawing.Size(440, 120);
		base.Controls.Add(this.guna2CirclePictureBox1);
		base.Controls.Add(this.content);
		base.Controls.Add(this.guna2ProgressBar1);
		base.Controls.Add(this.header);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        base.Icon = nigger3.iconap;
        base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(440, 120);
		base.Name = "PopUp";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "PopUp";
		base.TopMost = true;
		base.Load += new System.EventHandler(PopUp_Load);
		this.header.ResumeLayout(false);
		this.header.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.guna2CirclePictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
