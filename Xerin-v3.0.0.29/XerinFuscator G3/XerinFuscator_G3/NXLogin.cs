using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
namespace XerinFuscator_G3;

internal class NXLogin : Form
{
	private string fullText = "";

	private int currentIndex;

	private bool dragging;

	private Point dragCursorPoint;

	private Point dragFormPoint;

	private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private IContainer components;

	private Guna2Elipse guna2Elipse1;

	private Guna2Panel guna2Panel_0;

	private Guna2Panel guna2Panel_1;

	private Guna2Button validateButton;

	private Guna2TextBox passwordBox;

	private Guna2Separator guna2Separator1;

	private Guna2TextBox usernameBox;

	private Guna2HtmlLabel title2;

	private Guna2HtmlLabel userName;

	private Guna2HtmlLabel bonjour;

	private Guna2VSeparator guna2VSeparator1;

	private Guna2CirclePictureBox guna2CirclePictureBox1;

	private Guna2HtmlLabel guna2HtmlLabel1;

	private Guna2ImageButton guna2ImageButton3;

	private Guna2ImageButton guna2ImageButton2;

	private Guna2ImageButton guna2ImageButton1;

	private Guna2ShadowForm guna2ShadowForm1;

	private Guna2ImageButton guna2ImageButton5;

	private Guna2ImageButton guna2ImageButton4;

	private Guna2HtmlLabel guna2HtmlLabel2;

	private Guna2HtmlLabel guna2HtmlLabel4;

	private Guna2HtmlLabel guna2HtmlLabel3;

	private Guna2HtmlLabel guna2HtmlLabel5;

	private Guna2HtmlLabel guna2HtmlLabel6;

	public NXLogin()
	{
		
		InitializeComponent();
		DoubleBuffered = true;
	}

	private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
	{
		dragging = true;
		dragCursorPoint = Cursor.Position;
		dragFormPoint = base.Location;
	}

	private void guna2Panel1_MouseMove(object sender, MouseEventArgs e)
	{
		if (dragging)
		{
			base.Opacity = 0.94;
			Cursor = Cursors.Hand;
			Point pt = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
			base.Location = Point.Add(dragFormPoint, new Size(pt));
		}
	}

	private void guna2Panel1_MouseUp(object sender, MouseEventArgs e)
	{
		base.Opacity = 0.98;
		Cursor = Cursors.Default;
		dragging = false;
	}

	private async void AnimateText()
	{
		fullText = "BONJOUR, ";
		string text = fullText;
		foreach (char c in text)
		{
			bonjour.Text += c;
			await Task.Delay(80);
		}
		currentIndex = 0;
		fullText = "";
		userName.Text = Environment.UserName.ToUpper();
		AnimateText2();
	}

	private async void fadeIn()
	{
		while (base.Opacity < 0.98)
		{
			base.Opacity += 0.02;
			await Task.Delay(5);
		}
	}

	private async void AnimateText2()
	{
		fullText = "PLEASE LOGIN TO CONTINUE";
		string text = fullText;
		foreach (char c in text)
		{
			title2.Text += c;
			await Task.Delay(20);
		}
		currentIndex = 0;
		fullText = "";
		visibleCtrls(new Control[4] { usernameBox, passwordBox, guna2Separator1, validateButton });
	}

	private async void visibleCtrls(Control[] controls)
	{
		for (int i = 0; i < controls.Length; i++)
		{
			controls[i].Visible = true;
			await Task.Delay(100);
		}
	}

	private void NXLogin_Load(object sender, EventArgs e)
	{
		Invalidate();
		fadeIn();
		guna2ShadowForm1.SetShadowForm(this);
		if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Config", "Account.ini")))
		{
			string[] array = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Config", "Account.ini"));
			if (array != null)
			{
				usernameBox.Text = array[0];
				passwordBox.Text = array[1];
			}
		}
		AnimateText();
	}

	private async void NXLogin_Shown(object sender, EventArgs e)
	{
		await Shake.ShakeIT(this);
	}

	private async Task animateLogin(Control control, CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			control.Visible = !control.Visible;
			await Task.Delay(100);
		}
	}

	private void validateButton_Click(object sender, EventArgs e)
	{
		validateButton.Enabled = false;
        Hide();
        new XGui().ShowDialog();
        Close();
    }

	private void guna2ImageButton1_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void guna2ImageButton3_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void guna2ImageButton4_Click(object sender, EventArgs e)
	{
		Process.Start(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9kaXNjb3JkLmdnL252dVQ4V3RBRlU=")));
	}

	private void guna2ImageButton5_Click(object sender, EventArgs e)
	{
		Process.Start(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly90Lm1lL1hlcmluRnVzY2F0b3I=")));
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
		//System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XerinFuscator_G3.NXLogin));
		this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
		this.guna2Panel_0 = new Guna.UI2.WinForms.Guna2Panel();
		this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2ImageButton3 = new Guna.UI2.WinForms.Guna2ImageButton();
		this.guna2ImageButton2 = new Guna.UI2.WinForms.Guna2ImageButton();
		this.guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
		this.guna2Panel_1 = new Guna.UI2.WinForms.Guna2Panel();
		this.guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2ImageButton5 = new Guna.UI2.WinForms.Guna2ImageButton();
		this.guna2ImageButton4 = new Guna.UI2.WinForms.Guna2ImageButton();
		this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.validateButton = new Guna.UI2.WinForms.Guna2Button();
		this.passwordBox = new Guna.UI2.WinForms.Guna2TextBox();
		this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
		this.usernameBox = new Guna.UI2.WinForms.Guna2TextBox();
		this.title2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.userName = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.bonjour = new Guna.UI2.WinForms.Guna2HtmlLabel();
		this.guna2VSeparator1 = new Guna.UI2.WinForms.Guna2VSeparator();
		this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
		this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
		this.guna2Panel_0.SuspendLayout();
		this.guna2Panel_1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.guna2CirclePictureBox1).BeginInit();
		base.SuspendLayout();
		this.guna2Elipse1.BorderRadius = 10;
		this.guna2Elipse1.TargetControl = this;
		this.guna2Panel_0.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.guna2Panel_0.Controls.Add(this.guna2HtmlLabel1);
		this.guna2Panel_0.Controls.Add(this.guna2ImageButton3);
		this.guna2Panel_0.Controls.Add(this.guna2ImageButton2);
		this.guna2Panel_0.Controls.Add(this.guna2ImageButton1);
		this.guna2Panel_0.Location = new System.Drawing.Point(0, 0);
		this.guna2Panel_0.Name = "guna2Panel1";
		this.guna2Panel_0.Size = new System.Drawing.Size(1000, 40);
		this.guna2Panel_0.TabIndex = 0;
		this.guna2Panel_0.MouseDown += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseDown);
		this.guna2Panel_0.MouseMove += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseMove);
		this.guna2Panel_0.MouseUp += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseUp);
		this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.White;
		this.guna2HtmlLabel1.IsContextMenuEnabled = false;
		this.guna2HtmlLabel1.IsSelectionEnabled = false;
		this.guna2HtmlLabel1.Location = new System.Drawing.Point(10, 11);
		this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
		this.guna2HtmlLabel1.Size = new System.Drawing.Size(64, 18);
		this.guna2HtmlLabel1.TabIndex = 15;
		this.guna2HtmlLabel1.Text = "LOGIN XERIN";
		this.guna2HtmlLabel1.MouseDown += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseDown);
		this.guna2HtmlLabel1.MouseMove += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseMove);
		this.guna2HtmlLabel1.MouseUp += new System.Windows.Forms.MouseEventHandler(guna2Panel1_MouseUp);
		this.guna2ImageButton3.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.guna2ImageButton3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.guna2ImageButton3.HoverState.ImageSize = new System.Drawing.Size(14, 14);
		this.guna2ImageButton3.Image = nigger4.guna2ImageButton3_Image;
        this.guna2ImageButton3.ImageOffset = new System.Drawing.Point(0, 0);
		this.guna2ImageButton3.ImageRotate = 0f;
		this.guna2ImageButton3.ImageSize = new System.Drawing.Size(15, 15);
		this.guna2ImageButton3.Location = new System.Drawing.Point(932, 11);
		this.guna2ImageButton3.Name = "guna2ImageButton3";
		this.guna2ImageButton3.PressedState.ImageSize = new System.Drawing.Size(13, 13);
		this.guna2ImageButton3.Size = new System.Drawing.Size(18, 18);
		this.guna2ImageButton3.TabIndex = 14;
		this.guna2ImageButton3.Click += new System.EventHandler(guna2ImageButton3_Click);
		this.guna2ImageButton2.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.guna2ImageButton2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.guna2ImageButton2.HoverState.ImageSize = new System.Drawing.Size(14, 14);
		this.guna2ImageButton2.Image = nigger4.guna2ImageButton2_Image;
		this.guna2ImageButton2.ImageOffset = new System.Drawing.Point(0, 0);
		this.guna2ImageButton2.ImageRotate = 0f;
		this.guna2ImageButton2.ImageSize = new System.Drawing.Size(15, 15);
		this.guna2ImageButton2.Location = new System.Drawing.Point(951, 11);
		this.guna2ImageButton2.Name = "guna2ImageButton2";
		this.guna2ImageButton2.PressedState.ImageSize = new System.Drawing.Size(13, 13);
		this.guna2ImageButton2.Size = new System.Drawing.Size(18, 18);
		this.guna2ImageButton2.TabIndex = 13;
		this.guna2ImageButton1.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.guna2ImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.guna2ImageButton1.HoverState.ImageSize = new System.Drawing.Size(14, 14);
		this.guna2ImageButton1.Image = nigger4.guna2ImageButton1_Image;
		this.guna2ImageButton1.ImageOffset = new System.Drawing.Point(0, 0);
		this.guna2ImageButton1.ImageRotate = 0f;
		this.guna2ImageButton1.ImageSize = new System.Drawing.Size(15, 15);
		this.guna2ImageButton1.Location = new System.Drawing.Point(970, 11);
		this.guna2ImageButton1.Name = "guna2ImageButton1";
		this.guna2ImageButton1.PressedState.ImageSize = new System.Drawing.Size(13, 13);
		this.guna2ImageButton1.Size = new System.Drawing.Size(18, 18);
		this.guna2ImageButton1.TabIndex = 12;
		this.guna2ImageButton1.Click += new System.EventHandler(guna2ImageButton1_Click);
		this.guna2Panel_1.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.guna2Panel_1.Controls.Add(this.guna2HtmlLabel6);
		this.guna2Panel_1.Controls.Add(this.guna2HtmlLabel5);
		this.guna2Panel_1.Controls.Add(this.guna2HtmlLabel4);
		this.guna2Panel_1.Controls.Add(this.guna2HtmlLabel3);
		this.guna2Panel_1.Controls.Add(this.guna2ImageButton5);
		this.guna2Panel_1.Controls.Add(this.guna2ImageButton4);
		this.guna2Panel_1.Controls.Add(this.guna2HtmlLabel2);
		this.guna2Panel_1.Controls.Add(this.validateButton);
		this.guna2Panel_1.Controls.Add(this.passwordBox);
		this.guna2Panel_1.Controls.Add(this.guna2Separator1);
		this.guna2Panel_1.Controls.Add(this.usernameBox);
		this.guna2Panel_1.Controls.Add(this.title2);
		this.guna2Panel_1.Controls.Add(this.userName);
		this.guna2Panel_1.Controls.Add(this.bonjour);
		this.guna2Panel_1.Controls.Add(this.guna2VSeparator1);
		this.guna2Panel_1.Controls.Add(this.guna2CirclePictureBox1);
		this.guna2Panel_1.Location = new System.Drawing.Point(0, 39);
		this.guna2Panel_1.Name = "guna2Panel2";
		this.guna2Panel_1.Size = new System.Drawing.Size(1000, 460);
		this.guna2Panel_1.TabIndex = 16;
		this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel6.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel6.ForeColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.guna2HtmlLabel6.IsContextMenuEnabled = false;
		this.guna2HtmlLabel6.IsSelectionEnabled = false;
		this.guna2HtmlLabel6.Location = new System.Drawing.Point(894, 421);
		this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
		this.guna2HtmlLabel6.Size = new System.Drawing.Size(84, 18);
		this.guna2HtmlLabel6.TabIndex = 24;
		this.guna2HtmlLabel6.Text = "XERIN COMPANY";
		this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel5.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel5.ForeColor = System.Drawing.Color.White;
		this.guna2HtmlLabel5.IsContextMenuEnabled = false;
		this.guna2HtmlLabel5.IsSelectionEnabled = false;
		this.guna2HtmlLabel5.Location = new System.Drawing.Point(764, 421);
		this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
		this.guna2HtmlLabel5.Size = new System.Drawing.Size(128, 18);
		this.guna2HtmlLabel5.TabIndex = 23;
		this.guna2HtmlLabel5.Text = "ALL RIGHTS RESERVED TO";
		this.guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel4.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel4.ForeColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.guna2HtmlLabel4.IsContextMenuEnabled = false;
		this.guna2HtmlLabel4.IsSelectionEnabled = false;
		this.guna2HtmlLabel4.Location = new System.Drawing.Point(190, 400);
		this.guna2HtmlLabel4.Name = "guna2HtmlLabel4";
		this.guna2HtmlLabel4.Size = new System.Drawing.Size(20, 18);
		this.guna2HtmlLabel4.TabIndex = 22;
		this.guna2HtmlLabel4.Text = "ON!";
		this.guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel3.ForeColor = System.Drawing.Color.White;
		this.guna2HtmlLabel3.IsContextMenuEnabled = false;
		this.guna2HtmlLabel3.IsSelectionEnabled = false;
		this.guna2HtmlLabel3.Location = new System.Drawing.Point(22, 400);
		this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
		this.guna2HtmlLabel3.Size = new System.Drawing.Size(168, 18);
		this.guna2HtmlLabel3.TabIndex = 21;
		this.guna2HtmlLabel3.Text = "OBFUSCATOR THAT YOU CAN RELY";
		this.guna2ImageButton5.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.guna2ImageButton5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.guna2ImageButton5.HoverState.ImageSize = new System.Drawing.Size(17, 17);
		this.guna2ImageButton5.Image = nigger1.telegram_app_18px;
		this.guna2ImageButton5.ImageOffset = new System.Drawing.Point(0, 0);
		this.guna2ImageButton5.ImageRotate = 0f;
		this.guna2ImageButton5.ImageSize = new System.Drawing.Size(18, 18);
		this.guna2ImageButton5.Location = new System.Drawing.Point(116, 421);
		this.guna2ImageButton5.Name = "guna2ImageButton5";
		this.guna2ImageButton5.PressedState.ImageSize = new System.Drawing.Size(16, 16);
		this.guna2ImageButton5.Size = new System.Drawing.Size(18, 18);
		this.guna2ImageButton5.TabIndex = 20;
		this.guna2ImageButton5.Click += new System.EventHandler(guna2ImageButton5_Click);
		this.guna2ImageButton4.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
		this.guna2ImageButton4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.guna2ImageButton4.HoverState.ImageSize = new System.Drawing.Size(17, 17);
		this.guna2ImageButton4.Image = nigger1.discord_new_18px;
		this.guna2ImageButton4.ImageOffset = new System.Drawing.Point(0, 0);
		this.guna2ImageButton4.ImageRotate = 0f;
		this.guna2ImageButton4.ImageSize = new System.Drawing.Size(18, 18);
		this.guna2ImageButton4.Location = new System.Drawing.Point(92, 421);
		this.guna2ImageButton4.Name = "guna2ImageButton4";
		this.guna2ImageButton4.PressedState.ImageSize = new System.Drawing.Size(16, 16);
		this.guna2ImageButton4.Size = new System.Drawing.Size(18, 18);
		this.guna2ImageButton4.TabIndex = 19;
		this.guna2ImageButton4.Click += new System.EventHandler(guna2ImageButton4_Click);
		this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
		this.guna2HtmlLabel2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.White;
		this.guna2HtmlLabel2.IsContextMenuEnabled = false;
		this.guna2HtmlLabel2.IsSelectionEnabled = false;
		this.guna2HtmlLabel2.Location = new System.Drawing.Point(22, 421);
		this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
		this.guna2HtmlLabel2.Size = new System.Drawing.Size(62, 18);
		this.guna2HtmlLabel2.TabIndex = 18;
		this.guna2HtmlLabel2.Text = "NEED HELP?";
		this.validateButton.AutoRoundedCorners = true;
		this.validateButton.BackColor = System.Drawing.Color.Transparent;
		this.validateButton.BorderColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.validateButton.BorderRadius = 21;
		this.validateButton.BorderThickness = 1;
		this.validateButton.Cursor = System.Windows.Forms.Cursors.Hand;
		this.validateButton.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.validateButton.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.validateButton.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.validateButton.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.validateButton.FillColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.validateButton.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f);
		this.validateButton.ForeColor = System.Drawing.Color.White;
		this.validateButton.HoverState.FillColor = System.Drawing.Color.FromArgb(28, 28, 28);
		this.validateButton.Image = nigger1.fingerprint_accepted_24px;
		this.validateButton.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.validateButton.ImageOffset = new System.Drawing.Point(6, 0);
		this.validateButton.ImageSize = new System.Drawing.Size(24, 24);
		this.validateButton.Location = new System.Drawing.Point(399, 291);
		this.validateButton.Name = "validateButton";
		this.validateButton.Size = new System.Drawing.Size(180, 44);
		this.validateButton.TabIndex = 17;
		this.validateButton.Text = "VALIDATE DATA";
		this.validateButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.validateButton.TextOffset = new System.Drawing.Point(6, 0);
		this.validateButton.Visible = false;
		this.validateButton.Click += new System.EventHandler(validateButton_Click);
		this.passwordBox.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.passwordBox.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.passwordBox.DefaultText = "";
		this.passwordBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
		this.passwordBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
		this.passwordBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.passwordBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.passwordBox.FillColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.passwordBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.passwordBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.passwordBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.passwordBox.IconLeftSize = new System.Drawing.Size(24, 24);
		this.passwordBox.Location = new System.Drawing.Point(399, 236);
		this.passwordBox.Name = "passwordBox";
		this.passwordBox.PasswordChar = '\0';
		this.passwordBox.PlaceholderText = "ENTER YOUR PASSWORD HERE";
		this.passwordBox.SelectedText = "";
		this.passwordBox.Size = new System.Drawing.Size(257, 36);
		this.passwordBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
		this.passwordBox.TabIndex = 16;
		this.passwordBox.Visible = false;
		this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.guna2Separator1.Location = new System.Drawing.Point(399, 173);
		this.guna2Separator1.Name = "guna2Separator1";
		this.guna2Separator1.Size = new System.Drawing.Size(40, 10);
		this.guna2Separator1.TabIndex = 15;
		this.guna2Separator1.Visible = false;
		this.usernameBox.BorderColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.usernameBox.Cursor = System.Windows.Forms.Cursors.IBeam;
		this.usernameBox.DefaultText = "";
		this.usernameBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
		this.usernameBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
		this.usernameBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.usernameBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.usernameBox.FillColor = System.Drawing.Color.FromArgb(18, 18, 18);
		this.usernameBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.usernameBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.usernameBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.usernameBox.IconLeftSize = new System.Drawing.Size(24, 24);
		this.usernameBox.Location = new System.Drawing.Point(399, 192);
		this.usernameBox.Name = "usernameBox";
		this.usernameBox.PasswordChar = '\0';
		this.usernameBox.PlaceholderText = "ENTER YOUR USERNAME HERE";
		this.usernameBox.SelectedText = "";
		this.usernameBox.Size = new System.Drawing.Size(257, 36);
		this.usernameBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
		this.usernameBox.TabIndex = 14;
		this.usernameBox.Visible = false;
		this.title2.BackColor = System.Drawing.Color.Transparent;
		this.title2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.title2.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
		this.title2.IsContextMenuEnabled = false;
		this.title2.IsSelectionEnabled = false;
		this.title2.Location = new System.Drawing.Point(399, 149);
		this.title2.Name = "title2";
		this.title2.Size = new System.Drawing.Size(3, 2);
		this.title2.TabIndex = 13;
		this.title2.Text = null;
		this.userName.BackColor = System.Drawing.Color.Transparent;
		this.userName.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.userName.ForeColor = System.Drawing.Color.FromArgb(41, 230, 124);
		this.userName.IsContextMenuEnabled = false;
		this.userName.IsSelectionEnabled = false;
		this.userName.Location = new System.Drawing.Point(477, 125);
		this.userName.Name = "userName";
		this.userName.Size = new System.Drawing.Size(3, 2);
		this.userName.TabIndex = 12;
		this.userName.Text = null;
		this.bonjour.BackColor = System.Drawing.Color.Transparent;
		this.bonjour.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bonjour.ForeColor = System.Drawing.Color.White;
		this.bonjour.IsContextMenuEnabled = false;
		this.bonjour.IsSelectionEnabled = false;
		this.bonjour.Location = new System.Drawing.Point(399, 125);
		this.bonjour.Name = "bonjour";
		this.bonjour.Size = new System.Drawing.Size(3, 2);
		this.bonjour.TabIndex = 11;
		this.bonjour.Text = null;
		this.guna2VSeparator1.FillColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.guna2VSeparator1.Location = new System.Drawing.Point(342, 110);
		this.guna2VSeparator1.Name = "guna2VSeparator1";
		this.guna2VSeparator1.Size = new System.Drawing.Size(10, 240);
		this.guna2VSeparator1.TabIndex = 10;
		this.guna2CirclePictureBox1.Image = nigger1.fingerprint_512px;
		this.guna2CirclePictureBox1.ImageRotate = 0f;
		this.guna2CirclePictureBox1.Location = new System.Drawing.Point(173, 168);
		this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
		this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
		this.guna2CirclePictureBox1.Size = new System.Drawing.Size(125, 125);
		this.guna2CirclePictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.guna2CirclePictureBox1.TabIndex = 9;
		this.guna2CirclePictureBox1.TabStop = false;
		this.guna2ShadowForm1.BorderRadius = 10;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
		base.ClientSize = new System.Drawing.Size(1000, 500);
		base.Controls.Add(this.guna2Panel_1);
		base.Controls.Add(this.guna2Panel_0);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = nigger3.iconap;
		base.MaximizeBox = false;
		this.MaximumSize = new System.Drawing.Size(1000, 500);
		base.Name = "NXLogin";
		base.Opacity = 0.0;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Login xerin";
		base.Load += new System.EventHandler(NXLogin_Load);
		base.Shown += new System.EventHandler(NXLogin_Shown);
		this.guna2Panel_0.ResumeLayout(false);
		this.guna2Panel_0.PerformLayout();
		this.guna2Panel_1.ResumeLayout(false);
		this.guna2Panel_1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.guna2CirclePictureBox1).EndInit();
		base.ResumeLayout(false);
	}

	static NXLogin()
	{
	
	}
}
