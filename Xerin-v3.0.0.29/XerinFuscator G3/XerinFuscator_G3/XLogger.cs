using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using KoiVM.Internal;
using XCore.Embed;
using XCore.Logger;
using XCore.Protections;
using XCore.VirtHelper;
using XerinFuscator_G3.UI;
using XerinVM.Core;

namespace XerinFuscator_G3;

internal class XLogger : Form
{
	private bool dragging;

	private Point dragCursorPoint;

	private Point dragFormPoint;

	private IContainer components;

	private Guna2Elipse guna2Elipse1;

	private Panel header;

	private Guna2HtmlLabel headerTitle;

	private Guna2ProgressBar guna2ProgressBar1;

	private RichTextBox logBox;

	private Guna2HtmlLabel guna2HtmlLabel1;

	private Guna2ShadowForm guna2ShadowForm1;

	private Guna2ImageButton closeForm;

	public static bool soundAfter { get; set; }

	public static bool autoDir { get; set; }

	public static string dirPath { get; set; }

	public static int R { get; set; }

	public static int G { get; set; }

	public static int B { get; set; }

	public XLogger()
	{
		InitializeComponent();
	}

	private async void XLogger_Load(object sender, EventArgs e)
	{
		guna2ShadowForm1.SetShadowForm(this);
		headerTitle.ForeColor = UIColors.readColor(R, G, B);
		guna2ProgressBar1.ProgressColor = UIColors.readColor(R, G, B);
		guna2ProgressBar1.ProgressColor2 = UIColors.readColor(R, G, B);
		logBox.Clear();
		closeForm.Enabled = false;
		try
		{
			if (ProtectionManager.Protections.Count != 0)
			{
				Embeder.isEmptyList = Embeder.dlls.Count == 0;
				Cursor = Cursors.WaitCursor;
				await Task.Run(async delegate
				{
					XGui.protectionManager.ExecuteProtections(XGui.ctx, logBox);
					await XGui.ctx.Save();
					GC.Collect();
					GC.WaitForPendingFinalizers();
				});
				DateTime now = DateTime.Now;
				Logger.Log(logBox, $"Obfuscation process completed at: {now}, Saving assembly....");
				if (Helper.isEnabled && File.Exists(Usage.normal))
				{
					Logger.Log(logBox, "Executing Code virtualization....");
					await new Usage().ExceuteKoi(Usage.normal, Usage.vmOutput, null, null);
					Logger.Log(logBox, "Execution of Code virtualization completed.");
				}
			}
			else
			{
				closeForm.Enabled = true;
				Logger.Log(logBox, "At least select one protection!");
			}
		}
		catch (Exception ex)
		{
			closeForm.Enabled = true;
			guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
			Logger.Log(logBox, "Error: " + ex.Message);
		}
		finally
		{
			closeForm.Enabled = true;
			Cursor = Cursors.Default;
			guna2ProgressBar1.Style = ProgressBarStyle.Blocks;
			soundAfter = false;
			//if (soundAfter)
			//{
				
			//	//using SoundPlayer soundPlayer = new SoundPlayer(nigger1.sound);
			//	//soundPlayer.Play();
			//}
			if (autoDir)
			{
				Process.Start(dirPath);
			}
			Helper.names.Clear();
			InitializePhase.mnames.Clear();
			Usage.normal = null;
			Usage.vmOutput = null;
			XGui.ctx.Dispose();
			XGui.ctx = null;
		}
	}

	private void header_MouseDown(object sender, MouseEventArgs e)
	{
		dragging = true;
		dragCursorPoint = Cursor.Position;
		dragFormPoint = base.Location;
	}

	private void header_MouseMove(object sender, MouseEventArgs e)
	{
		if (dragging)
		{
			base.Opacity = 0.96;
			Cursor = Cursors.Hand;
			Point pt = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
			base.Location = Point.Add(dragFormPoint, new Size(pt));
		}
	}

	private void header_MouseUp(object sender, MouseEventArgs e)
	{
		base.Opacity = 1.0;
		Cursor = Cursors.Default;
		dragging = false;
	}

	private void closeApp_Click(object sender, EventArgs e)
	{
		base.Owner = null;
		BringToFront();
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
		//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XerinFuscator_G3.XLogger));
		this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
		this.header = new System.Windows.Forms.Panel();
		this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.headerTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.logBox = new System.Windows.Forms.RichTextBox();
		this.guna2ProgressBar1 = new Guna.UI2.WinForms.Guna2ProgressBar();
		this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
		this.closeForm = new Guna.UI2.WinForms.Guna2ImageButton();
		this.header.SuspendLayout();
		base.SuspendLayout();
		this.guna2Elipse1.BorderRadius = 0;
		this.guna2Elipse1.TargetControl = this;
		this.header.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.header.Controls.Add(this.closeForm);
		this.header.Controls.Add(this.guna2HtmlLabel1);
		this.header.Controls.Add(this.headerTitle);
		this.header.Location = new System.Drawing.Point(0, 0);
		this.header.Name = "header";
		this.header.Size = new System.Drawing.Size(440, 40);
		this.header.TabIndex = 7;
		this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(header_MouseDown);
		this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(header_MouseMove);
		this.header.MouseUp += new System.Windows.Forms.MouseEventHandler(header_MouseUp);
		this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.White;
		this.guna2HtmlLabel1.IsContextMenuEnabled = false;
		this.guna2HtmlLabel1.IsSelectionEnabled = false;
		this.guna2HtmlLabel1.Location = new System.Drawing.Point(43, 11);
		this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
		this.guna2HtmlLabel1.Size = new System.Drawing.Size(43, 18);
		this.guna2HtmlLabel1.TabIndex = 4;
		this.guna2HtmlLabel1.Text = "LOGGER";
		this.guna2HtmlLabel1.MouseDown += new System.Windows.Forms.MouseEventHandler(header_MouseDown);
		this.guna2HtmlLabel1.MouseMove += new System.Windows.Forms.MouseEventHandler(header_MouseMove);
		this.guna2HtmlLabel1.MouseUp += new System.Windows.Forms.MouseEventHandler(header_MouseUp);
		this.headerTitle.BackColor = System.Drawing.Color.Transparent;
		this.headerTitle.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.headerTitle.ForeColor = System.Drawing.Color.FromArgb(222, 85, 85);
		this.headerTitle.IsContextMenuEnabled = false;
		this.headerTitle.IsSelectionEnabled = false;
		this.headerTitle.Location = new System.Drawing.Point(10, 11);
		this.headerTitle.Name = "headerTitle";
		this.headerTitle.Size = new System.Drawing.Size(32, 18);
		this.headerTitle.TabIndex = 2;
		this.headerTitle.Text = "XERIN";
		this.headerTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(header_MouseDown);
		this.headerTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(header_MouseMove);
		this.headerTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(header_MouseUp);
		this.logBox.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
		this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.logBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.logBox.ForeColor = System.Drawing.Color.White;
		this.logBox.Location = new System.Drawing.Point(12, 53);
		this.logBox.Name = "logBox";
		this.logBox.ReadOnly = true;
		this.logBox.Size = new System.Drawing.Size(416, 292);
		this.logBox.TabIndex = 8;
		this.logBox.Text = "";
		this.guna2ProgressBar1.FillColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.guna2ProgressBar1.Location = new System.Drawing.Point(0, 358);
		this.guna2ProgressBar1.Name = "guna2ProgressBar1";
		this.guna2ProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(222, 85, 85);
		this.guna2ProgressBar1.ProgressColor2 = System.Drawing.Color.FromArgb(222, 85, 85);
		this.guna2ProgressBar1.Size = new System.Drawing.Size(440, 2);
		this.guna2ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
		this.guna2ProgressBar1.TabIndex = 9;
		this.guna2ProgressBar1.Text = "guna2ProgressBar1";
		this.guna2ProgressBar1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
		this.guna2ShadowForm1.BorderRadius = 0;
		this.guna2ShadowForm1.TargetForm = this;
		this.closeForm.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.closeForm.Cursor = System.Windows.Forms.Cursors.Hand;
		this.closeForm.HoverState.ImageSize = new System.Drawing.Size(14, 14);
		this.closeForm.Image = nigger3.closeForm_Image;
		this.closeForm.ImageOffset = new System.Drawing.Point(0, 0);
		this.closeForm.ImageRotate = 0f;
		this.closeForm.ImageSize = new System.Drawing.Size(15, 15);
		this.closeForm.Location = new System.Drawing.Point(410, 11);
		this.closeForm.Name = "closeForm";
		this.closeForm.PressedState.ImageSize = new System.Drawing.Size(13, 13);
		this.closeForm.Size = new System.Drawing.Size(18, 18);
		this.closeForm.TabIndex = 18;
		this.closeForm.Click += new System.EventHandler(closeApp_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
		base.ClientSize = new System.Drawing.Size(440, 360);
		base.Controls.Add(this.guna2ProgressBar1);
		base.Controls.Add(this.logBox);
		base.Controls.Add(this.header);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = nigger3.iconap;
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(440, 360);
		base.Name = "XLogger";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		base.Load += new System.EventHandler(XLogger_Load);
		this.header.ResumeLayout(false);
		this.header.PerformLayout();
		base.ResumeLayout(false);
	}
}
