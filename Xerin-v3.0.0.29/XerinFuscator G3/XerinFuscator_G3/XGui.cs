using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cfex.Renamer;
using Confuser.Protections.ReferenceProxy;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using OLD.Renamer;
using XCore;
using XCore.Context;
using XCore.Embed;
using XCore.Generator;
using XCore.Optimizer;
using XCore.Protections;
using XCore.Utils;
using XCore.VirtHelper;
using XerinFuscator_G3.UI;
using XerinVM.Core;
using XProtections;
using XProtections.AntiCrack.Globals;
using XProtections.CodeEncryption;
using XProtections.ControlFlow;

namespace XerinFuscator_G3;

internal class XGui : Form
{
	private bool dragging;

	private Point dragCursorPoint;

	private Point dragFormPoint;

	private Panel dimPanel;

	public static XContext ctx;

	public static ProtectionManager protectionManager;

	private ControlProj controlProj;

	private bool allmeths;

	public static int ParentX;

	public static int ParentY;

	private bool togemall;

	private IContainer components;

	private Guna2Elipse guna2Elipse1;

	private Panel header;

	private Guna2HtmlLabel headerTitle2;

	private Guna2HtmlLabel headerTitle1;

	private Panel panel2;

	private Panel panel3;

	private Guna2ShadowForm guna2ShadowForm1;

	private Guna2PictureBox guna2PictureBox1;

	private Guna2HtmlLabel guna2HtmlLabel5;

	private Guna2HtmlLabel duration;

	private Guna2HtmlLabel userName;

	private Guna2CirclePictureBox guna2CirclePictureBox1;

	private Guna2Button gotoAssembly;

	private Guna2Button gotoProtections;

	private Guna2VSeparator pagesSeparator;

	private Guna2Button gotoColors;

	private Guna2PictureBox guna2PictureBox2;

	private Guna2HtmlLabel guna2HtmlLabel6;

	private Guna2Button gotoRenamer;

	private Guna2Button gotoCodeEnc;

	private Guna2Button gotoCodeVirt;

	private Guna2Button gotoProject;

	private Panel assemblyPage;

	private Guna2HtmlLabel guna2HtmlLabel7;

	private Guna2TextBox assemblyBox;

	private Guna2HtmlLabel guna2HtmlLabel8;

	private Guna2TextBox destinationBox;

	private Guna2ImageButton changeDestination;

	private Guna2ImageButton addAssembly;

	private Guna2HtmlLabel asmArch;

	private Guna2HtmlLabel asmName;

	private Guna2HtmlLabel guna2HtmlLabel9;

	private Guna2HtmlLabel guna2HtmlLabel10;

	private Panel protectionsPage;

	private Panel anticrackPanel;

	private Guna2HtmlLabel guna2HtmlLabel13;

	private Guna2VSeparator guna2VSeparator3;

	private Guna2ImageButton anticrackSettings;

	private Guna2ToggleSwitch anticrackSwitch;

	private Panel l2fPanel;

	private Guna2ToggleSwitch localtofieldSwitch;

	private Guna2HtmlLabel guna2HtmlLabel23;

	private Guna2VSeparator guna2VSeparator13;

	private Panel refproxyPanel;

	private Guna2ToggleSwitch referenceproxySwitch;

	private Guna2HtmlLabel guna2HtmlLabel22;

	private Guna2VSeparator guna2VSeparator12;

	private Panel resourcesPanel;

	private Guna2ToggleSwitch encoderesourcesSwitch;

	private Guna2HtmlLabel guna2HtmlLabel21;

	private Guna2VSeparator guna2VSeparator11;

	private Panel stringsPanel;

	private Guna2ToggleSwitch encodestringsSwitch;

	private Guna2HtmlLabel guna2HtmlLabel20;

	private Guna2VSeparator guna2VSeparator10;

	private Panel codemutPanel;

	private Guna2ImageButton codemutationSettings;

	private Guna2ToggleSwitch codemutationSwitch;

	private Guna2HtmlLabel guna2HtmlLabel19;

	private Guna2VSeparator guna2VSeparator9;

	private Panel cflowPanel;

	private Guna2ImageButton controlflowSettings;

	private Guna2ToggleSwitch controlflowSwitch;

	private Guna2HtmlLabel guna2HtmlLabel18;

	private Guna2VSeparator guna2VSeparator8;

	private Panel antivirtPanel;

	private Guna2ToggleSwitch antivirtualmachineSwitch;

	private Guna2HtmlLabel guna2HtmlLabel17;

	private Guna2VSeparator guna2VSeparator7;

	private Panel antidumpPanel;

	private Guna2ToggleSwitch antidumpSwitch;

	private Guna2HtmlLabel guna2HtmlLabel16;

	private Guna2VSeparator guna2VSeparator6;

	private Panel antidebugPanel;

	private Guna2ToggleSwitch antidebugSwitch;

	private Guna2HtmlLabel guna2HtmlLabel15;

	private Guna2VSeparator guna2VSeparator5;

	private Panel antidecPanel;

	private Guna2ToggleSwitch antidecompilerSwitch;

	private Guna2HtmlLabel guna2HtmlLabel14;

	private Guna2VSeparator guna2VSeparator4;

	private Panel integrityPanel;

	private Guna2ToggleSwitch integritycheckSwitch;

	private Guna2HtmlLabel guna2HtmlLabel24;

	private Guna2VSeparator guna2VSeparator14;

	private Guna2ImageButton appSettings;

	private Guna2VSeparator guna2VSeparator2;

	private Panel anticrackPage;

	private Panel panel6;

	private Guna2ToggleSwitch anticrackExclude;

	private Guna2HtmlLabel guna2HtmlLabel26;

	private Guna2VSeparator guna2VSeparator17;

	private Panel panel5;

	private Guna2ToggleSwitch anticrackSilentMsg;

	private Guna2HtmlLabel guna2HtmlLabel25;

	private Guna2VSeparator guna2VSeparator16;

	private Panel panel4;

	private Guna2ToggleSwitch anticrackNormalMode;

	private Guna2HtmlLabel guna2HtmlLabel12;

	private Guna2VSeparator guna2VSeparator15;

	private Guna2HtmlLabel guna2HtmlLabel11;

	private Guna2HtmlLabel guna2HtmlLabel28;

	private Guna2TextBox excludeBox;

	private Guna2TextBox webhookBox;

	private Panel controlflowPage;

	private Panel panel9;

	private Guna2ToggleSwitch performancecflowMode;

	private Guna2HtmlLabel guna2HtmlLabel29;

	private Guna2VSeparator guna2VSeparator19;

	private Panel panel10;

	private Guna2ToggleSwitch strongcflowMode;

	private Guna2HtmlLabel guna2HtmlLabel30;

	private Guna2VSeparator guna2VSeparator20;

	private Guna2HtmlLabel guna2HtmlLabel31;

	private Guna2HtmlLabel guna2HtmlLabel32;

	private Panel codemutationPage;

	private Panel panel11;

	private Guna2ToggleSwitch performancemutationMode;

	private Guna2HtmlLabel guna2HtmlLabel27;

	private Guna2VSeparator guna2VSeparator18;

	private Panel panel12;

	private Guna2ToggleSwitch strongmutationMode;

	private Guna2HtmlLabel guna2HtmlLabel33;

	private Guna2VSeparator guna2VSeparator21;

	private Guna2HtmlLabel guna2HtmlLabel35;

	private Panel codeencPage;

	private Panel codeencPanel;

	private Guna2ToggleSwitch codeencSwitch;

	private Guna2HtmlLabel guna2HtmlLabel37;

	private Guna2VSeparator guna2VSeparator23;

	private Guna2HtmlLabel guna2HtmlLabel39;

	private Panel renamerPage;

	private Panel renamePanel;

	private Guna2ToggleSwitch renamerSwitch;

	private Guna2HtmlLabel guna2HtmlLabel36;

	private Guna2VSeparator guna2VSeparator22;

	private Guna2HtmlLabel guna2HtmlLabel41;

	private Guna2ToggleSwitch customRenamer;

	private Guna2TextBox customBox;

	private Guna2HtmlLabel guna2HtmlLabel43;

	private CheckedListBox renamingOptions;

	private Guna2HtmlLabel guna2HtmlLabel42;

	private Panel virtualizationPage;

	private Guna2TextBox methodBox;

	private Guna2HtmlLabel guna2HtmlLabel44;

	private Panel virtPanel;

	private Guna2ToggleSwitch virtSwitch;

	private Guna2HtmlLabel guna2HtmlLabel46;

	private Guna2VSeparator guna2VSeparator24;

	private Guna2HtmlLabel guna2HtmlLabel48;

	private TreeView methodsList;

	private Guna2ImageButton searchMethod;

	private Panel projectPage;

	private Guna2HtmlLabel guna2HtmlLabel51;

	private Guna2Button saveProject;

	private Guna2Button loadProject;

	private Panel colorsPage;

	private Guna2HtmlLabel guna2HtmlLabel2;

	private Guna2HtmlLabel guna2HtmlLabel50;

	private Guna2HtmlLabel guna2HtmlLabel47;

	private Guna2HtmlLabel guna2HtmlLabel40;

	private Guna2HtmlLabel guna2HtmlLabel38;

	private Guna2HtmlLabel guna2HtmlLabel34;

	private Guna2ImageButton backfromMutation;

	private Guna2ImageButton backfromCflow;

	private Guna2ImageButton backfromACrack;

	private Panel settingsPage;

	private Panel panel15;

	private Guna2ToggleSwitch autoDir;

	private Guna2HtmlLabel guna2HtmlLabel53;

	private Guna2VSeparator guna2VSeparator25;

	private Panel panel16;

	private Guna2ToggleSwitch soundReminder;

	private Guna2HtmlLabel guna2HtmlLabel54;

	private Guna2VSeparator guna2VSeparator26;

	private Guna2HtmlLabel guna2HtmlLabel55;

	private Guna2HtmlLabel guna2HtmlLabel56;

	private Guna2HtmlLabel guna2HtmlLabel49;

	private Guna2HtmlLabel guna2HtmlLabel45;

	private Guna2HtmlLabel guna2HtmlLabel1;

	private Guna2ImageButton backfromSettings;

	private Guna2TextBox bVal;

	private Guna2TextBox gVal;

	private Guna2TextBox rVal;

	private Guna2ImageButton setinRGB;

	private Guna2HtmlToolTip guna2HtmlToolTip1;

	private Guna2ImageButton guna2ImageButton_0;

	private Panel panel1;

	private Guna2ToggleSwitch bsodSwitch;

	private Guna2HtmlLabel guna2HtmlLabel52;

	private Guna2VSeparator guna2VSeparator1;

	private Guna2TextBox customMsg;

	private Guna2ImageButton guna2ImageButton3;

	private Panel embedPage;

	private ListBox dllsList;

	private Panel panel17;

	private Guna2ToggleSwitch embedderSwitch;

	private Guna2HtmlLabel guna2HtmlLabel60;

	private Guna2VSeparator guna2VSeparator30;

	private Guna2HtmlLabel guna2HtmlLabel61;

	private Guna2HtmlLabel guna2HtmlLabel62;

	private Guna2ImageButton removeDll;

	private Guna2ImageButton addDlls;

	private Guna2ImageButton backfromProject;

	private Guna2ImageButton clearList;

	private Guna2ContextMenuStrip guna2ContextMenuStrip1;

	private ToolStripMenuItem aDDMETHODToolStripMenuItem;

	private ToolStripMenuItem rEMOVEMETHODToolStripMenuItem;

	private ImageList imageList1;

	private Guna2ImageButton obfuscateAssembly;

	private Guna2VSeparator guna2VSeparator27;

	private Guna2ImageButton selectAll;

	private Guna2ImageButton gotoAbout;

	private Panel rightsPage;

	private Guna2HtmlLabel guna2HtmlLabel3;

	private Guna2HtmlLabel guna2HtmlLabel63;

	private Guna2CirclePictureBox guna2CirclePictureBox2;

	private Guna2HtmlLabel guna2HtmlLabel4;

	private Guna2HtmlLabel guna2HtmlLabel57;

	private Label label1;

	private Guna2HtmlLabel appVersion;

	private Guna2ImageButton backfromRights;

	private Guna2ImageButton addAllMethods;

	private Guna2Elipse guna2Elipse2;

	private Panel UnverifiableCodeAttributePanel;

	private Guna2ToggleSwitch UnverifiableCodeAttributeSwitch;

	private Guna2HtmlLabel guna2HtmlLabel64;

	private Guna2VSeparator guna2VSeparator31;

	private Panel cfexPanel;

	private Guna2ToggleSwitch cfexRenamerSwitch;

	private Guna2HtmlLabel guna2HtmlLabel65;

	private Guna2VSeparator guna2VSeparator32;

	private Panel intencodingPanel;

	private Guna2ToggleSwitch intencodeSwitch;

	private Guna2HtmlLabel guna2HtmlLabel66;

	private Guna2VSeparator guna2VSeparator33;

	private Panel antitamperPanel;

	private Guna2ToggleSwitch antiTamperSwitch;

	private Guna2HtmlLabel guna2HtmlLabel67;

	private Guna2VSeparator guna2VSeparator34;

	private Panel OptimizePanel;

	private Guna2ToggleSwitch optimizierSwitch;

	private Guna2HtmlLabel guna2HtmlLabel59;

	private Guna2VSeparator guna2VSeparator29;

	private Panel simplePanel;

	private Guna2ToggleSwitch simpilifierSwitch;

	private Guna2HtmlLabel guna2HtmlLabel58;

	private Guna2VSeparator guna2VSeparator28;

	private Panel preserveridsPanel;

	private Guna2ToggleSwitch preserveRids;

	private Guna2HtmlLabel guna2HtmlLabel68;

	private Guna2VSeparator guna2VSeparator35;

	private Guna2PictureBox guna2PictureBox3;

	private Panel antiEmulatePanel;

	private Guna2ToggleSwitch antikauthEmulateSwitch;

	private Guna2HtmlLabel guna2HtmlLabel69;

	private Guna2VSeparator guna2VSeparator36;

	private Guna2ImageButton minimizeApp;

	private Guna2ImageButton maximizeApp;

	private Guna2ImageButton closeApp;

	private Timer killSwitch;

	private Guna2Button editProcList;




    byte[] image0 = {
    0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D,
    0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10,
    0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0xF3, 0xFF, 0x61, 0x00, 0x00, 0x00,
    0x01, 0x73, 0x52, 0x47, 0x42, 0x00, 0xAE, 0xCE, 0x1C, 0xE9, 0x00, 0x00,
    0x00, 0x04, 0x67, 0x41, 0x4D, 0x41, 0x00, 0x00, 0xB1, 0x8F, 0x0B, 0xFC,
    0x61, 0x05, 0x00, 0x00, 0x00, 0x09, 0x70, 0x48, 0x59, 0x73, 0x00, 0x00,
    0x0E, 0xC3, 0x00, 0x00, 0x0E, 0xC3, 0x01, 0xC7, 0x6F, 0xA8, 0x64, 0x00,
    0x00, 0x00, 0xAD, 0x49, 0x44, 0x41, 0x54, 0x38, 0x4F, 0x63, 0xA0, 0x1B,
    0x08, 0x0D, 0x0D, 0xDD, 0xF0, 0x1F, 0x09, 0x84, 0x84, 0x84, 0xAC, 0x87,
    0x4A, 0x11, 0x07, 0x40, 0x9A, 0xA0, 0x4C, 0x30, 0x40, 0xE7, 0x13, 0x04,
    0x64, 0x1B, 0x20, 0x2C, 0x2C, 0x2C, 0x15, 0x16, 0x16, 0xB6, 0x11, 0xAF,
    0x01, 0x38, 0xFC, 0xC7, 0xE4, 0xE2, 0xE2, 0x52, 0x9E, 0x95, 0x95, 0xF5,
    0x16, 0x44, 0x47, 0x45, 0x45, 0xED, 0x82, 0x4A, 0x83, 0x01, 0x90, 0xBF,
    0x1D, 0xA2, 0x1B, 0x08, 0x40, 0x02, 0x50, 0x26, 0x18, 0x80, 0xF8, 0xB1,
    0xB1, 0xB1, 0x47, 0x23, 0x23, 0x23, 0xF7, 0xC8, 0xCA, 0xCA, 0x2A, 0x43,
    0x85, 0x71, 0x03, 0x6C, 0x06, 0x44, 0x47, 0x47, 0x1F, 0x06, 0x32, 0x59,
    0x21, 0x22, 0x04, 0x00, 0x36, 0x03, 0x80, 0xDE, 0x5A, 0x9B, 0x98, 0x98,
    0x78, 0x51, 0x55, 0x55, 0xD5, 0x0C, 0x2A, 0x8C, 0x1B, 0x00, 0x9D, 0xBA,
    0x13, 0xA4, 0x09, 0x06, 0x60, 0xFE, 0x33, 0x33, 0x33, 0x0B, 0xCD, 0xCC,
    0xCC, 0x7C, 0xE1, 0xE7, 0xE7, 0x37, 0x13, 0x18, 0x90, 0x5B, 0xA0, 0xD2,
    0x60, 0x40, 0x74, 0x3A, 0x20, 0x2A, 0x16, 0x88, 0x01, 0x03, 0x6F, 0x00,
    0xAE, 0x70, 0x1A, 0x68, 0xC0, 0xC0, 0x00, 0x00, 0x11, 0x36, 0xBB, 0xBC,
    0x1F, 0x04, 0xE9, 0x10, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44,
    0xAE, 0x42, 0x60, 0x82
};


    byte[] image1 = {
    0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D,
    0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10,
    0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0xF3, 0xFF, 0x61, 0x00, 0x00, 0x00,
    0x01, 0x73, 0x52, 0x47, 0x42, 0x00, 0xAE, 0xCE, 0x1C, 0xE9, 0x00, 0x00,
    0x00, 0x04, 0x67, 0x41, 0x4D, 0x41, 0x00, 0x00, 0xB1, 0x8F, 0x0B, 0xFC,
    0x61, 0x05, 0x00, 0x00, 0x00, 0x09, 0x70, 0x48, 0x59, 0x73, 0x00, 0x00,
    0x0E, 0xC3, 0x00, 0x00, 0x0E, 0xC3, 0x01, 0xC7, 0x6F, 0xA8, 0x64, 0x00,
    0x00, 0x00, 0xCC, 0x49, 0x44, 0x41, 0x54, 0x38, 0x4F, 0x63, 0x18, 0x3C,
    0xC0, 0xCB, 0xCB, 0xAB, 0x33, 0x3F, 0x3F, 0xFF, 0xFB, 0x7F, 0x02, 0x00,
    0xA4, 0xC6, 0xD3, 0xD3, 0xB3, 0x03, 0xAA, 0x0D, 0x01, 0xB2, 0xB2, 0xB2,
    0x7E, 0x0A, 0x0B, 0x0B, 0xF3, 0x42, 0xB9, 0x38, 0x81, 0xA0, 0xA0, 0x20,
    0x7F, 0x4D, 0x4D, 0xCD, 0x0F, 0x28, 0x17, 0x01, 0x40, 0xA6, 0x43, 0x99,
    0x04, 0x01, 0x56, 0xB5, 0x84, 0x0C, 0xD0, 0xD0, 0xD0, 0xB0, 0x0F, 0x0A,
    0x0A, 0x5A, 0x08, 0x62, 0x93, 0x6C, 0x80, 0x91, 0x91, 0x91, 0x4F, 0x4A,
    0x4A, 0xCA, 0x73, 0x65, 0x65, 0x65, 0x23, 0x10, 0x1F, 0xAF, 0x01, 0x20,
    0x5B, 0x40, 0xB6, 0x81, 0x05, 0x81, 0x40, 0x5B, 0x5B, 0xDB, 0x31, 0x33,
    0x33, 0xF3, 0x85, 0x9A, 0x9A, 0x9A, 0x0D, 0x54, 0x08, 0xBF, 0x01, 0x20,
    0x5B, 0x40, 0xB6, 0x81, 0x6C, 0x35, 0x34, 0x34, 0xF4, 0x45, 0xB6, 0x19,
    0x06, 0xF0, 0x1A, 0x00, 0x02, 0x9A, 0x9A, 0x9A, 0xB6, 0x49, 0x49, 0x49,
    0x2F, 0x41, 0x18, 0xC4, 0x86, 0x0A, 0xC3, 0x01, 0x41, 0x03, 0x40, 0x00,
    0xA8, 0xD1, 0x0E, 0x84, 0xA1, 0x5C, 0x14, 0x40, 0x94, 0x01, 0xF8, 0x00,
    0x56, 0xB5, 0xA0, 0x84, 0x24, 0x24, 0x24, 0xC4, 0x07, 0xE5, 0xE2, 0x04,
    0x38, 0x13, 0x92, 0xB7, 0xB7, 0x77, 0x77, 0x5D, 0x5D, 0xDD, 0x1F, 0x90,
    0xE9, 0xF8, 0x40, 0x63, 0x63, 0xE3, 0x6F, 0xAC, 0x49, 0x79, 0x80, 0x00,
    0x03, 0x03, 0x00, 0x2A, 0xF1, 0xB2, 0xC7, 0x09, 0x6B, 0xB2, 0x9B, 0x00,
    0x00, 0x00, 0x00, 0x49, 0x45, 0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82
};




    byte[] image2 = {
    0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D,
    0x49, 0x48, 0x44, 0x52, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x10,
    0x08, 0x06, 0x00, 0x00, 0x00, 0x1F, 0xF3, 0xFF, 0x61, 0x00, 0x00, 0x00,
    0x01, 0x73, 0x52, 0x47, 0x42, 0x00, 0xAE, 0xCE, 0x1C, 0xE9, 0x00, 0x00,
    0x00, 0x04, 0x67, 0x41, 0x4D, 0x41, 0x00, 0x00, 0xB1, 0x8F, 0x0B, 0xFC,
    0x61, 0x05, 0x00, 0x00, 0x00, 0x09, 0x70, 0x48, 0x59, 0x73, 0x00, 0x00,
    0x0E, 0xC3, 0x00, 0x00, 0x0E, 0xC3, 0x01, 0xC7, 0x6F, 0xA8, 0x64, 0x00,
    0x00, 0x00, 0x67, 0x49, 0x44, 0x41, 0x54, 0x38, 0x4F, 0x63, 0x18, 0x3C,
    0xC0, 0xCB, 0xCB, 0xAB, 0x33, 0x3F, 0x3F, 0xFF, 0xFB, 0x7F, 0x02, 0x00,
    0xA4, 0xC6, 0xD3, 0xD3, 0xB3, 0x03, 0xAA, 0x0D, 0x01, 0xB2, 0xB2, 0xB2,
    0x7E, 0x0A, 0x0B, 0x0B, 0xF3, 0x42, 0xB9, 0x38, 0x81, 0xA0, 0xA0, 0x20,
    0x7F, 0x4D, 0x4D, 0xCD, 0x0F, 0x28, 0x17, 0x01, 0x40, 0xA6, 0x43, 0x99,
    0x04, 0x01, 0x56, 0xB5, 0xA3, 0x06, 0x0C, 0x0B, 0x03, 0x40, 0x09, 0x49,
    0x48, 0x48, 0x88, 0x0F, 0xCA, 0xC5, 0x09, 0x70, 0x26, 0x24, 0x6F, 0x6F,
    0xEF, 0xEE, 0xBA, 0xBA, 0xBA, 0x3F, 0x20, 0xD3, 0xF1, 0x81, 0xC6, 0xC6,
    0xC6, 0xDF, 0x58, 0x93, 0xF2, 0x00, 0x01, 0x06, 0x06, 0x00, 0x04, 0x48,
    0x98, 0xEA, 0x33, 0x3B, 0x91, 0xB6, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45,
    0x4E, 0x44, 0xAE, 0x42, 0x60, 0x82
};









    public XGui()
	{
		controlProj = new ControlProj();
		InitializeComponent();
        this.imageList1.Images.Add("tree_structure_16px.png", Image.FromStream(new MemoryStream(image0)));
        this.imageList1.Images.Add("checked_checkbox_16px", Image.FromStream(new MemoryStream(image1)));
        this.imageList1.Images.Add("unchecked_checkbox_16px.png", Image.FromStream(new MemoryStream(image2)));
        this.imageList1.Images.SetKeyName(0, "tree_structure_16px.png");
        this.imageList1.Images.SetKeyName(1, "checked_checkbox_16px.png");
        this.imageList1.Images.SetKeyName(2, "unchecked_checkbox_16px.png");

        DoubleBuffered = true;
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
			base.Opacity = 0.94;
			Cursor = Cursors.Hand;
			Point pt = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
			base.Location = Point.Add(dragFormPoint, new Size(pt));
		}
	}

	private void header_MouseUp(object sender, MouseEventArgs e)
	{
		base.Opacity = 0.98;
		Cursor = Cursors.Default;
		dragging = false;
	}

	private void XGui_Load(object sender, EventArgs e)
	{
	}

	private async void XGui_Shown(object sender, EventArgs e)
	{
		await Shake.ShakeIT(this);
	}

	private string expirydaysleft()
	{
		return "Until the inx stops skidding";
	}

	private void minimizeApp_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	private void maximizeApp_Click(object sender, EventArgs e)
	{
	}

	private void closeApp_Click(object sender, EventArgs e)
	{
		Environment.Exit(0);
	}

	private void gotoAssembly_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "assembly";
		Pages.changeButtons(this, panel2, assemblyPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoProtections_Click(object sender, EventArgs e)
	{
		selectAll.Visible = true;
		Pages.current = "protections";
		Pages.changeButtons(this, panel2, protectionsPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoRenamer_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "renamer";
		Pages.changeButtons(this, panel2, renamerPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoCodeEnc_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "codeenc";
		Pages.changeButtons(this, panel2, codeencPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoCodeVirt_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "codevirt";
		Pages.changeButtons(this, panel2, virtualizationPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoProject_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "emped";
		Pages.changeButtons(this, panel2, embedPage, (Guna2Button)sender, pagesSeparator);
	}

	private void gotoColors_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		Pages.current = "colors";
		Pages.changeButtons(this, panel2, colorsPage, (Guna2Button)sender, pagesSeparator);
	}

	private void backfromMutation_Click(object sender, EventArgs e)
	{
		protectionsPage.BringToFront();
	}

	private void backfromCflow_Click(object sender, EventArgs e)
	{
		protectionsPage.BringToFront();
	}

	private void backfromACrack_Click(object sender, EventArgs e)
	{
		protectionsPage.BringToFront();
	}

	private void anticrackSettings_Click(object sender, EventArgs e)
	{
		anticrackPage.BringToFront();
	}

	private void controlflowSettings_Click(object sender, EventArgs e)
	{
		controlflowPage.BringToFront();
	}

	private void codemutationSettings_Click(object sender, EventArgs e)
	{
		codemutationPage.BringToFront();
	}

	private void appSettings_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		settingsPage.BringToFront();
	}

	private void guna2ImageButton3_Click(object sender, EventArgs e)
	{
		selectAll.Visible = false;
		projectPage.BringToFront();
	}

	private void gotoAbout_Click(object sender, EventArgs e)
	{
		rightsPage.BringToFront();
	}

	private void backfromSettings_Click(object sender, EventArgs e)
	{
		string current = Pages.current;
		if (current == null)
		{
			return;
		}
		switch (current.Length)
		{
		case 5:
			if (current == "emped")
			{
				embedPage.BringToFront();
			}
			break;
		case 6:
			if (current == "colors")
			{
				colorsPage.BringToFront();
			}
			break;
		case 7:
			switch (current[0])
			{
			case 'r':
				if (current == "renamer")
				{
					renamerPage.BringToFront();
				}
				break;
			case 'c':
				if (current == "codeenc")
				{
					codeencPage.BringToFront();
				}
				break;
			}
			break;
		case 8:
			switch (current[0])
			{
			case 'c':
				if (current == "codevirt")
				{
					virtualizationPage.BringToFront();
				}
				break;
			case 'a':
				if (current == "assembly")
				{
					assemblyPage.BringToFront();
				}
				break;
			}
			break;
		case 11:
			if (current == "protections")
			{
				selectAll.Visible = true;
				protectionsPage.BringToFront();
			}
			break;
		case 9:
		case 10:
			break;
		}
	}

	private void backfromProject_Click(object sender, EventArgs e)
	{
		string current = Pages.current;
		if (current == null)
		{
			return;
		}
		switch (current.Length)
		{
		case 5:
			if (current == "emped")
			{
				embedPage.BringToFront();
			}
			break;
		case 6:
			if (current == "colors")
			{
				colorsPage.BringToFront();
			}
			break;
		case 7:
			switch (current[0])
			{
			case 'r':
				if (current == "renamer")
				{
					renamerPage.BringToFront();
				}
				break;
			case 'c':
				if (current == "codeenc")
				{
					codeencPage.BringToFront();
				}
				break;
			}
			break;
		case 8:
			switch (current[0])
			{
			case 'c':
				if (current == "codevirt")
				{
					virtualizationPage.BringToFront();
				}
				break;
			case 'a':
				if (current == "assembly")
				{
					assemblyPage.BringToFront();
				}
				break;
			}
			break;
		case 11:
			if (current == "protections")
			{
				selectAll.Visible = true;
				protectionsPage.BringToFront();
			}
			break;
		case 9:
		case 10:
			break;
		}
	}

	private void backfromRights_Click(object sender, EventArgs e)
	{
		string current = Pages.current;
		if (current == null)
		{
			return;
		}
		switch (current.Length)
		{
		case 5:
			if (current == "emped")
			{
				embedPage.BringToFront();
			}
			break;
		case 6:
			if (current == "colors")
			{
				colorsPage.BringToFront();
			}
			break;
		case 7:
			switch (current[0])
			{
			case 'r':
				if (current == "renamer")
				{
					renamerPage.BringToFront();
				}
				break;
			case 'c':
				if (current == "codeenc")
				{
					codeencPage.BringToFront();
				}
				break;
			}
			break;
		case 8:
			switch (current[0])
			{
			case 'c':
				if (current == "codevirt")
				{
					virtualizationPage.BringToFront();
				}
				break;
			case 'a':
				if (current == "assembly")
				{
					assemblyPage.BringToFront();
				}
				break;
			}
			break;
		case 11:
			if (current == "protections")
			{
				selectAll.Visible = true;
				protectionsPage.BringToFront();
			}
			break;
		case 9:
		case 10:
			break;
		}
	}

	private void randomRGB_Click(object sender, EventArgs e)
	{
		Random random = new Random();
		int num = random.Next(0, 256);
		rVal.Text = Convert.ToString(num);
		XLogger.R = num;
		PopUp.R = num;
		int num2 = random.Next(0, 256);
		gVal.Text = Convert.ToString(num2);
		XLogger.G = num2;
		PopUp.G = num2;
		int num3 = random.Next(0, 256);
		bVal.Text = Convert.ToString(num3);
		XLogger.B = num3;
		PopUp.B = num3;
		UIColors.setColor(header, new Control[5] { guna2HtmlLabel57, guna2HtmlLabel10, headerTitle1, userName, pagesSeparator }, new Control[12]
		{
			rightsPage, embedPage, colorsPage, settingsPage, assemblyPage, protectionsPage, renamerPage, codeencPage, virtualizationPage, anticrackPage,
			codemutationPage, controlflowPage
		}, pagesSeparator, UIColors.readColor(num, num2, num3));
		guna2HtmlToolTip1.BorderColor = UIColors.readColor(num, num2, num3);
	}

	private void setinRGB_Click(object sender, EventArgs e)
	{
		try
		{
			XLogger.R = Convert.ToInt32(rVal.Text);
			XLogger.G = Convert.ToInt32(gVal.Text);
			XLogger.B = Convert.ToInt32(bVal.Text);
			PopUp.R = Convert.ToInt32(rVal.Text);
			PopUp.G = Convert.ToInt32(gVal.Text);
			PopUp.B = Convert.ToInt32(bVal.Text);
			UIColors.setColor(header, new Control[5] { guna2HtmlLabel57, guna2HtmlLabel10, headerTitle1, userName, pagesSeparator }, new Control[12]
			{
				rightsPage, embedPage, colorsPage, settingsPage, assemblyPage, protectionsPage, renamerPage, codeencPage, virtualizationPage, anticrackPage,
				codemutationPage, controlflowPage
			}, pagesSeparator, UIColors.readColor(Convert.ToInt32(rVal.Text), Convert.ToInt32(gVal.Text), Convert.ToInt32(bVal.Text)));
			guna2HtmlToolTip1.BorderColor = UIColors.readColor(Convert.ToInt32(rVal.Text), Convert.ToInt32(gVal.Text), Convert.ToInt32(bVal.Text));
		}
		catch
		{
			NotificationPopUp.PopUp("ERROR", "CHECK RGB VALUE !");
		}
	}

	private void rVal_TextChanged(object sender, EventArgs e)
	{
		ControlProj.R = Convert.ToInt32(rVal.Text);
	}

	private void gVal_TextChanged(object sender, EventArgs e)
	{
		ControlProj.G = Convert.ToInt32(gVal.Text);
	}

	private void bVal_TextChanged(object sender, EventArgs e)
	{
		ControlProj.B = Convert.ToInt32(bVal.Text);
	}

	private void assemblyBox_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		foreach (string path in array)
		{
			if (!Path.GetExtension(path).Equals(".exe", StringComparison.OrdinalIgnoreCase))
			{
				if (Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase))
				{
					assemblyBox.Text = path;
					destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
					ctx = new XContext(assemblyBox.Text)
					{
						DirPath = destinationBox.Text,
						OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
					};
					methodsList.Nodes.Clear();
					virtSwitch.Checked = false;
					virtSwitch.Enabled = false;
					ControlProj.path = assemblyBox.Text;
				}
			}
			else
			{
				assemblyBox.Text = path;
				destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
				ctx = new XContext(assemblyBox.Text)
				{
					DirPath = destinationBox.Text,
					OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
				};
				ControlProj.path = assemblyBox.Text;
				Usage.normal = ctx.OutPutPath;
				Usage.vmOutput = Usage.normal.Replace(".exe", "-VM.exe");
				virtSwitch.Enabled = true;
				Helper.loadMethods(methodsList, ctx);
				if (allmeths)
				{
					addAllMethods.PerformClick();
					addAllMethods.PerformClick();
				}
			}
			asmName.Text = "NAME: " + asmInfo.asmName(assemblyBox.Text);
			asmArch.Text = "ARCHITECTURE: " + asmInfo.asmArch(assemblyBox.Text);
		}
	}

	private void assemblyBox_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void addAssembly_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = "Load .NET assembly",
			Filter = ".NET Assembly (*.exe)|*.exe|(*.dll)|*.dll",
			Multiselect = false
		};
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		string fileName = openFileDialog.FileName;
		int num = fileName.LastIndexOf(".");
		if (num == -1)
		{
			return;
		}
		string text = fileName.Substring(num);
		text = text.ToLower();
		if (text == ".exe")
		{
			assemblyBox.Text = fileName;
			destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
			ctx = new XContext(assemblyBox.Text)
			{
				DirPath = destinationBox.Text,
				OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
			};
			ControlProj.path = assemblyBox.Text;
			Usage.normal = ctx.OutPutPath;
			Usage.vmOutput = Usage.normal.Replace(".exe", "-VM.exe");
			virtSwitch.Enabled = true;
			Helper.loadMethods(methodsList, ctx);
			if (allmeths)
			{
				addAllMethods.PerformClick();
				addAllMethods.PerformClick();
			}
		}
		else if (text == ".dll")
		{
			assemblyBox.Text = fileName;
			destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
			ctx = new XContext(assemblyBox.Text)
			{
				DirPath = destinationBox.Text,
				OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
			};
			methodsList.Nodes.Clear();
			virtSwitch.Checked = false;
			virtSwitch.Enabled = false;
			ControlProj.path = assemblyBox.Text;
		}
		asmName.Text = "NAME: " + asmInfo.asmName(assemblyBox.Text);
		asmArch.Text = "ARCHITECTURE: " + asmInfo.asmArch(assemblyBox.Text);
	}

	private void changeDestination_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
		{
			Description = "Select destination"
		};
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			destinationBox.Text = folderBrowserDialog.SelectedPath;
		}
	}

	private void antiEmulatePanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antiEmulatePanel, "PREVENTS EMULATING PROCESS ON KEYAUTH");
	}

	private void preserveridsPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(preserveridsPanel, "ONLY DISABLE IF YOU FACE SOME ISSUES AFTER OBFUSCATION!<br>THIS IS IMPORTANT FOR CODE VIRTUALIZATION<br>BUT IT MAY CORRUPT MONO ASSEMBLIES OR OTHER ASSEMBLIES");
	}

	private void UnverifiableCodeAttributePanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(UnverifiableCodeAttributePanel, "IT CAN BE USEFUL IN SCENARIOS THAT INVOLVE PERFORMANCE-CRITICAL CODE<br>OR UNSAFE CODE, OR INTEROPERABILITY WITH UNMANAGED RESOURCES");
	}

	private void anticrackPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(anticrackPanel, "ADVANCED ANTI CRACK THAT DETECTS CRACKING PROCESSES WITH MANY OPTIONS");
	}

	private void antidecPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antidecPanel, "CAN STOP SOME DECOMPILERS FROM DECOMBILING YOUR ASSEMBLY");
	}

	private void antidebugPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antidebugPanel, "PREVENTS DEBUGGING PROCESSES ON ASSEMBLY");
	}

	private void antidumpPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antidumpPanel, "PREVENTS DUMPING PROCESS ON ASSEMBLY FROM MEMORY");
	}

	private void antivirtPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antivirtPanel, "PREVENTS ASSEMBLY FROM BEING RAN ON VM");
	}

	private void cflowPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(cflowPanel, "MANGLES CODE INSIDE METHODS TO CONFUSE CODE");
	}

	private void codemutPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(codemutPanel, "MUTATE CODE INSIDE METHODS");
	}

	private void stringsPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(stringsPanel, "ENCODE, COMPRESS & CACHE STRINGS FOR BEST PERFORMANCE");
	}

	private void resourcesPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(resourcesPanel, "ENCODE & COMPRESS ASSEMBLY RESOURCES");
	}

	private void refproxyPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(refproxyPanel, "REDIRECT CALLS / REFERENCES INTO METHODS TO CONFUSE IT");
	}

	private void l2fPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(l2fPanel, "CONVERTS LOCALS TO FIELD");
	}

	private void integrityPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(l2fPanel, "HASH MODULE TO PREVENT FILE MODIFICATIONS");
	}

	private void virtPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(virtPanel, "VIRTUALIZING CODE IN METHOD");
	}

	private void renamePanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(renamePanel, "RENAME SYMBOLS { METHODS / FIELDS / ETC }");
	}

	private void cfexPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(cfexPanel, "RENAME SYMBOLS { METHODS / FIELDS / ETC } BUT NEEDS TO RESOLVE DEPENDENCIES");
	}

	private void codeencPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(codeencPanel, "ENCRYPT & STORE DATA IN METHODS");
	}

	private void intencodingPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(intencodingPanel, "ENCODING & SECURING INTEGERS INSIDE ASSEMBLY");
	}

	private void antitamperPanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(antitamperPanel, " IF ANY TAMPERING IS DETECTED (E.G., METHOD MODIFICATIONS OR DYNAMIC CALLS FROM UNEXPECTED ASSEMBLIES)<br>IT TERMINATES THE EXECUTION TO PREVENT FURTHER OPERATIONS");
	}

	private void simplePanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(simplePanel, "SIMPLIFYING METHOD BRANCHES AND MACROS, THIS CAN LEAD TO BETTER APP PERFORMANCE");
	}

	private void OptimizePanel_MouseDown(object sender, MouseEventArgs e)
	{
		guna2HtmlToolTip1.SetToolTip(OptimizePanel, "OPTIMIZING METHOD MACROS, REMOVING USELESS VALUES & OTHER THINGS, THIS CAN LEAD TO BETTER APP PERFORMANCE");
	}

	private void antikauthEmulateSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (antikauthEmulateSwitch.Checked)
		{
			protectionManager.AddProtection(new AntiEmulate());
		}
		else if (!antikauthEmulateSwitch.Checked)
		{
			protectionManager.RemoveProtection(new AntiEmulate());
		}
	}

	private void preserveRids_CheckedChanged(object sender, EventArgs e)
	{
		if (preserveRids.Checked)
		{
			XContext.presreve = true;
		}
		else
		{
			XContext.presreve = false;
		}
	}

	private void UnverifiableCodeAttributeSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!UnverifiableCodeAttributeSwitch.Checked)
		{
			UnverifiableCodeAttributeAttr.attr = false;
		}
		else
		{
			UnverifiableCodeAttributeAttr.attr = true;
		}
	}

	private void simpilifierSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (simpilifierSwitch.Checked)
		{
			XContext.Simpilify = true;
		}
		else
		{
			XContext.Simpilify = false;
		}
	}

	private void optimizierSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (optimizierSwitch.Checked)
		{
			XContext.Optimize = true;
		}
		else
		{
			XContext.Optimize = false;
		}
	}

	private void anticrackSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (anticrackSwitch.Checked)
		{
			protectionManager.AddProtection(new DetectCrackersYHook());
		}
		else if (!anticrackSwitch.Checked)
		{
			protectionManager.RemoveProtection(new DetectCrackersYHook());
		}
	}

	private void antidecompilerSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (antidecompilerSwitch.Checked)
		{
			XContext.AD = true;
			protectionManager.AddProtection(new AntiDecompiler());
		}
		else if (!antidecompilerSwitch.Checked)
		{
			XContext.AD = false;
			protectionManager.RemoveProtection(new AntiDecompiler());
		}
	}

	private void antidebugSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!antidebugSwitch.Checked)
		{
			if (!antidebugSwitch.Checked)
			{
				protectionManager.RemoveProtection(new AntiDebug());
			}
		}
		else
		{
			protectionManager.AddProtection(new AntiDebug());
		}
	}

	private void antidumpSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (antidumpSwitch.Checked)
		{
			protectionManager.AddProtection(new AntiDump());
		}
		else if (!antidumpSwitch.Checked)
		{
			protectionManager.RemoveProtection(new AntiDump());
		}
	}

	private void antivirtualmachineSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!antivirtualmachineSwitch.Checked)
		{
			if (!antivirtualmachineSwitch.Checked)
			{
				protectionManager.RemoveProtection(new AntiVM());
			}
		}
		else
		{
			protectionManager.AddProtection(new AntiVM());
		}
	}

	private void controlflowSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!controlflowSwitch.Checked)
		{
			protectionManager.RemoveProtection(new ControlFlow());
		}
	}

	private void codemutationSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!codemutationSwitch.Checked)
		{
			if (!codemutationSwitch.Checked)
			{
				protectionManager.RemoveProtection(new SecondMutationStage());
			}
		}
		else
		{
			protectionManager.AddProtection(new SecondMutationStage());
		}
	}

	private void encodestringsSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!encodestringsSwitch.Checked)
		{
			if (!encodestringsSwitch.Checked)
			{
				protectionManager.RemoveProtection(new eConstants());
			}
		}
		else
		{
			protectionManager.AddProtection(new eConstants());
		}
	}

	private void encoderesourcesSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!encoderesourcesSwitch.Checked)
		{
			if (!encoderesourcesSwitch.Checked)
			{
				protectionManager.RemoveProtection(new ResourcesEncoder());
			}
		}
		else
		{
			protectionManager.AddProtection(new ResourcesEncoder());
		}
	}

	private void referenceproxySwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (referenceproxySwitch.Checked)
		{
			protectionManager.AddProtection(new ReferenceProxyPhase());
		}
		else if (!referenceproxySwitch.Checked)
		{
			protectionManager.RemoveProtection(new ReferenceProxyPhase());
		}
	}

	private void localtofieldSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (localtofieldSwitch.Checked)
		{
			protectionManager.AddProtection(new L2F());
		}
		else if (!localtofieldSwitch.Checked)
		{
			protectionManager.RemoveProtection(new L2F());
		}
	}

	private void integritycheckSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (integritycheckSwitch.Checked)
		{
			virtSwitch.Checked = false;
			protectionManager.AddProtection(new IntegrityCheck());
		}
		else if (!integritycheckSwitch.Checked)
		{
			protectionManager.RemoveProtection(new IntegrityCheck());
		}
	}

	private void virtSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (virtSwitch.Checked)
		{
			cfexRenamerSwitch.Checked = false;
			codeencSwitch.Checked = false;
			integritycheckSwitch.Checked = false;
			Helper.isEnabled = true;
		}
		else
		{
			Helper.isEnabled = false;
		}
	}

	private void renamerSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!renamerSwitch.Checked)
		{
			if (!renamerSwitch.Checked)
			{
				protectionManager.RemoveProtection(new Renamer());
			}
		}
		else
		{
			cfexRenamerSwitch.Checked = false;
			renamingOptions.Enabled = true;
			customBox.Enabled = true;
			customRenamer.Enabled = true;
			protectionManager.AddProtection(new Renamer());
		}
	}

	private void cfexRenamerSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!cfexRenamerSwitch.Checked)
		{
			if (!cfexRenamerSwitch.Checked)
			{
				protectionManager.RemoveProtection(new cfexRenamer());
			}
			return;
		}
		virtSwitch.Checked = false;
		renamerSwitch.Checked = false;
		renamingOptions.Enabled = false;
		customBox.Enabled = false;
		customRenamer.Enabled = false;
		protectionManager.AddProtection(new cfexRenamer());
	}

	private void codeencSwitch_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (codeencSwitch.Checked)
			{
				XContext.CE = true;
				virtSwitch.Checked = false;
				protectionManager.AddProtection(new CodeEncryption());
			}
			else
			{
				XContext.CE = false;
				protectionManager.RemoveProtection(new CodeEncryption());
			}
		}
		catch
		{
		}
	}

	private void intencodeSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!intencodeSwitch.Checked)
		{
			if (!intencodeSwitch.Checked)
			{
				protectionManager.RemoveProtection(new IntEncoding());
			}
		}
		else
		{
			protectionManager.AddProtection(new IntEncoding());
		}
	}

	private void strongmutationMode_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void performancemutationMode_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void strongcflowMode_CheckedChanged(object sender, EventArgs e)
	{
		if (strongcflowMode.Checked)
		{
			performancecflowMode.Checked = false;
			ControlFlow.isPerformance = false;
			ControlFlow.isStrong = true;
			protectionManager.AddProtection(new ControlFlow());
		}
		else if (!strongcflowMode.Checked)
		{
			protectionManager.RemoveProtection(new ControlFlow());
		}
	}

	private void performancecflowMode_CheckedChanged(object sender, EventArgs e)
	{
		if (performancecflowMode.Checked)
		{
			strongcflowMode.Checked = false;
			ControlFlow.isPerformance = true;
			ControlFlow.isStrong = false;
			protectionManager.AddProtection(new ControlFlow());
		}
		else if (!performancecflowMode.Checked)
		{
			protectionManager.RemoveProtection(new ControlFlow());
		}
	}

	private void embedderSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (embedderSwitch.Checked)
		{
			Embeder.isEmbed = true;
			protectionManager.AddProtection(new Embeder());
		}
		else
		{
			Embeder.isEmbed = false;
			protectionManager.RemoveProtection(new Embeder());
		}
	}

	private void antiTamperSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!antiTamperSwitch.Checked)
		{
			if (!antiTamperSwitch.Checked)
			{
				protectionManager.RemoveProtection(new AntiTamper());
			}
		}
		else
		{
			protectionManager.AddProtection(new AntiTamper());
		}
	}

	private void editProcList_Click(object sender, EventArgs e)
	{
		Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Config", "Processes list.ini"));
	}

	private void anticrackNormalMode_CheckedChanged(object sender, EventArgs e)
	{
		if (anticrackNormalMode.Checked)
		{
			Global.Normal = true;
		}
		else
		{
			Global.Normal = false;
		}
	}

	private void anticrackSilentMsg_CheckedChanged(object sender, EventArgs e)
	{
		if (anticrackSilentMsg.Checked)
		{
			Global.Silent = true;
			ControlProj.silentMsg = true;
		}
		else
		{
			Global.Silent = false;
			ControlProj.silentMsg = false;
		}
	}

	private void anticrackExclude_CheckedChanged(object sender, EventArgs e)
	{
		if (!anticrackExclude.Checked)
		{
			Global.Exclude = false;
		}
		else
		{
			Global.Exclude = true;
		}
	}

	private void bsodSwitch_CheckedChanged(object sender, EventArgs e)
	{
		if (!bsodSwitch.Checked)
		{
			Global.Bsod = false;
			ControlProj.bsod = false;
		}
		else
		{
			Global.Bsod = true;
			ControlProj.bsod = true;
		}
	}

	private void webhookBox_TextChanged(object sender, EventArgs e)
	{
		Global.webhookLink = webhookBox.Text.e_e();
		ControlProj.webhook = webhookBox.Text.e_e();
	}

	private void excludeBox_TextChanged(object sender, EventArgs e)
	{
		Global.excludeString = excludeBox.Text;
		ControlProj.exclude = excludeBox.Text;
	}

	private void customMsg_TextChanged(object sender, EventArgs e)
	{
		Global.customMessage = customMsg.Text;
		ControlProj.customMsg = customMsg.Text;
	}

	private void dllsList_DragDrop(object sender, DragEventArgs e)
	{
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: false);
		foreach (string text in array)
		{
			if (Path.GetExtension(text).Equals(".dll", StringComparison.OrdinalIgnoreCase) && XCore.Utils.Utils.isAssemblyDotNet(text))
			{
				if (!dllsList.Items.Contains(text))
				{
					dllsList.Items.Add(text);
					Embeder.dlls.Add(text);
				}
				else
				{
					NotificationPopUp.PopUp("ERROR", "THIS LIBRARY IS ATTACHED BEFORE !");
				}
			}
			else
			{
				NotificationPopUp.PopUp("ERROR", "THIS LIBRARY IS NOT .NET OR NOT DLL!");
			}
		}
	}

	private void dllsList_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			e.Effect = DragDropEffects.Copy;
		}
	}

	private void addDlls_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			FileName = string.Empty,
			Title = "Select assembly to embed",
			Filter = "Assembly (*.dll)|*.dll",
			CheckFileExists = true,
			Multiselect = true
		};
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string[] fileNames = openFileDialog.FileNames;
			foreach (string text in fileNames)
			{
				try
				{
					Stream stream;
					if ((stream = openFileDialog.OpenFile()) == null)
					{
						continue;
					}
					using (stream)
					{
						if (text.Length > 0 && XCore.Utils.Utils.isAssemblyDotNet(text))
						{
							if (!dllsList.Items.Contains(text))
							{
								dllsList.Items.Add(text);
								Embeder.dlls.Add(text);
							}
							else
							{
								NotificationPopUp.PopUp("ERROR", "THIS LIBRARY IS ATTACHED BEFORE !");
							}
						}
						else
						{
							NotificationPopUp.PopUp("ERROR", "THIS LIBRARY IS NOT .NET !");
						}
					}
				}
				catch
				{
				}
			}
		}
		dllsList.Focus();
	}

	private void removeDll_Click(object sender, EventArgs e)
	{
		try
		{
			dllsList.Items.Remove(dllsList.SelectedItem);
			Embeder.dlls.Remove(dllsList.SelectedItem.ToString());
		}
		catch
		{
		}
	}

	private void clearList_Click(object sender, EventArgs e)
	{
		dllsList.Items.Clear();
		Embeder.dlls.Clear();
	}

	private void CheckNodesForName(TreeNode parentNode, List<string> names)
	{
		foreach (TreeNode node in parentNode.Nodes)
		{
			if (names.Contains(node.Text))
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
			}
			CheckNodesForName(node, names);
		}
	}

	private bool SearchRecursive(IEnumerable nodes, string searchFor)
	{
		foreach (TreeNode node in nodes)
		{
			if (node.Text.ToUpper().Contains(searchFor))
			{
				methodsList.SelectedNode = node;
			}
			if (SearchRecursive(node.Nodes, searchFor))
			{
				return true;
			}
		}
		return false;
	}

	private void searchMethod_Click(object sender, EventArgs e)
	{
		methodsList.Focus();
		string text = methodBox.Text.Trim().ToUpper();
		if (text != "" && methodsList.Nodes.Count > 0 && SearchRecursive(methodsList.Nodes, text))
		{
			methodsList.SelectedNode.Expand();
			methodsList.Focus();
			methodsList.SelectedNode = methodsList.SelectedNode.NextVisibleNode;
		}
	}

	private void methodsList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		methodsList.SelectedNode = e.Node;
	}

	private void methodsList_MouseClick(object sender, MouseEventArgs e)
	{
		methodsList.SelectedNode = methodsList.GetNodeAt(e.X, e.Y);
	}

	private void aDDMETHODToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string item = methodsList.SelectedNode.Text;
		if (!Helper.names.Contains(item))
		{
			Helper.names.Add(methodsList.SelectedNode.Text);
			Helper.AddProtection(methodsList.SelectedNode.Text);
			methodsList.SelectedNode.ImageIndex = 1;
			methodsList.SelectedNode.SelectedImageIndex = 1;
		}
	}

	private void rEMOVEMETHODToolStripMenuItem_Click(object sender, EventArgs e)
	{
		string item = methodsList.SelectedNode.Text;
		if (Helper.names.Contains(item))
		{
			Helper.names.Remove(item);
			Helper.RemoveProtection(methodsList.SelectedNode.Text);
			methodsList.SelectedNode.ImageIndex = 2;
			methodsList.SelectedNode.SelectedImageIndex = 2;
		}
	}

	private void guna2ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
	{
		if (Convert.ToInt16(methodsList.SelectedNode.Tag) == 0)
		{
			guna2ContextMenuStrip1.Enabled = false;
		}
		else if (Convert.ToInt16(methodsList.SelectedNode.Tag) == 1)
		{
			guna2ContextMenuStrip1.Enabled = false;
		}
		else if (Convert.ToInt16(methodsList.SelectedNode.Tag) == 2)
		{
			guna2ContextMenuStrip1.Enabled = true;
		}
		else if (methodsList.SelectedNode.ImageIndex == 1)
		{
			guna2ContextMenuStrip1.Enabled = false;
		}
	}

	public static List<string> GetAllNodes(TreeView treeView)
	{
		List<string> list = new List<string>();
		foreach (TreeNode node in treeView.Nodes)
		{
			AddChildNodesToList(node, list);
		}
		return list;
	}

	private static void AddChildNodesToList(TreeNode treeNode, List<string> nodeList)
	{
		nodeList.Add(treeNode.Text);
		foreach (TreeNode node in treeNode.Nodes)
		{
			AddChildNodesToList(node, nodeList);
		}
	}

	private void addAllMethods_Click(object sender, EventArgs e)
	{
		if (!allmeths)
		{
			allmeths = true;
			methodsList.Enabled = false;
			addAllMethods.Image = nigger1.check_all_18px;
			{
				foreach (string allNode in GetAllNodes(methodsList))
				{
					Helper.names.Remove(allNode);
					Helper.RemoveProtection(allNode);
					ControlProj.isall = true;
					Helper.names.Add(allNode);
					Helper.AddProtection(allNode);
				}
				return;
			}
		}
		allmeths = false;
		methodsList.Enabled = true;
		addAllMethods.Image = nigger1.uncheck_all_18px;
		foreach (string allNode2 in GetAllNodes(methodsList))
		{
			ControlProj.isall = false;
			Helper.names.Remove(allNode2);
			Helper.RemoveProtection(allNode2);
		}
	}

	private void renamingOptions_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		switch (e.Index)
		{
		case 0:
			Globals.events = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[0] = true;
			break;
		case 1:
			Globals.flds = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[1] = true;
			break;
		case 2:
			Globals.methods = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[2] = true;
			break;
		case 3:
			Globals.parameters = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[3] = true;
			break;
		case 4:
			Globals.props = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[4] = true;
			break;
		case 5:
			Globals.types = e.NewValue == CheckState.Checked;
			ControlProj.rChecked[5] = true;
			break;
		}
	}

	private void customRenamer_CheckedChanged(object sender, EventArgs e)
	{
		if (customRenamer.Checked)
		{
			ControlProj.CustomRN = true;
			GGeneration.CustomRN = true;
		}
		else
		{
			ControlProj.CustomRN = false;
			GGeneration.CustomRN = false;
		}
	}

	private void customBox_TextChanged(object sender, EventArgs e)
	{
		ControlProj.Custom = customBox.Text;
		GGeneration.Custom = customBox.Text;
	}

	private void loadProject_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			FileName = string.Empty,
			Title = "Open Xerin's Project File",
			Filter = "Project File(*.xml)|*.xml",
			CheckFileExists = true
		};
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		using (MemoryStream xerinConfig = new MemoryStream(File.ReadAllBytes(openFileDialog.FileName)))
		{
			controlProj.Load(xerinConfig);
		}
		if (ControlProj.path != string.Empty)
		{
			assemblyBox.Text = ControlProj.path;
		}
		try
		{
			if (!assemblyBox.Text.EndsWith(".exe"))
			{
				if (assemblyBox.Text.EndsWith(".dll"))
				{
					destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
					ctx = new XContext(assemblyBox.Text)
					{
						DirPath = destinationBox.Text,
						OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
					};
					methodsList.Nodes.Clear();
					virtSwitch.Checked = false;
					virtSwitch.Enabled = false;
					ControlProj.path = assemblyBox.Text;
				}
			}
			else
			{
				destinationBox.Text = Path.Combine(Path.GetDirectoryName(assemblyBox.Text) + "\\Secured");
				ctx = new XContext(assemblyBox.Text)
				{
					DirPath = destinationBox.Text,
					OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
				};
				ControlProj.path = assemblyBox.Text;
				Usage.normal = ctx.OutPutPath;
				Usage.vmOutput = Usage.normal.Replace(".exe", "-VM.exe");
				virtSwitch.Enabled = true;
				Helper.loadMethods(methodsList, ctx);
				if (allmeths)
				{
					addAllMethods.PerformClick();
					addAllMethods.PerformClick();
				}
			}
			asmName.Text = "NAME: " + asmInfo.asmName(assemblyBox.Text);
			asmArch.Text = "ARCHITECTURE: " + asmInfo.asmArch(assemblyBox.Text);
		}
		catch
		{
		}
		rVal.Text = Convert.ToString(ControlProj.R);
		gVal.Text = Convert.ToString(ControlProj.G);
		bVal.Text = Convert.ToString(ControlProj.B);
		XLogger.R = Convert.ToInt32(rVal.Text);
		XLogger.G = Convert.ToInt32(gVal.Text);
		XLogger.B = Convert.ToInt32(bVal.Text);
		PopUp.R = Convert.ToInt32(rVal.Text);
		PopUp.G = Convert.ToInt32(gVal.Text);
		PopUp.B = Convert.ToInt32(bVal.Text);
		UIColors.setColor(header, new Control[4] { guna2HtmlLabel10, headerTitle1, userName, pagesSeparator }, new Control[10] { colorsPage, settingsPage, assemblyPage, protectionsPage, renamerPage, codeencPage, virtualizationPage, anticrackPage, codemutationPage, controlflowPage }, pagesSeparator, UIColors.readColor(Convert.ToInt32(rVal.Text), Convert.ToInt32(gVal.Text), Convert.ToInt32(bVal.Text)));
		guna2HtmlToolTip1.BorderColor = UIColors.readColor(Convert.ToInt32(rVal.Text), Convert.ToInt32(gVal.Text), Convert.ToInt32(bVal.Text));
		if (ControlProj.cfexrenamer)
		{
			cfexRenamerSwitch.Checked = true;
		}
		if (ControlProj.renamer)
		{
			renamerSwitch.Checked = true;
		}
		if (ControlProj.antiCrack)
		{
			anticrackSwitch.Checked = true;
		}
		if (ControlProj.antiDebug)
		{
			antidebugSwitch.Checked = true;
		}
		if (ControlProj.antiDecombiler)
		{
			antidecompilerSwitch.Checked = true;
		}
		if (ControlProj.antiDump)
		{
			antidumpSwitch.Checked = true;
		}
		if (ControlProj.antiVM)
		{
			antivirtualmachineSwitch.Checked = true;
		}
		if (ControlProj.antiemulate)
		{
			antikauthEmulateSwitch.Checked = true;
		}
		if (ControlProj.antitamper)
		{
			antiTamperSwitch.Checked = true;
		}
		if (ControlProj.stringsEncoder)
		{
			encodestringsSwitch.Checked = true;
		}
		if (ControlProj.intencoding)
		{
			intencodeSwitch.Checked = true;
		}
		if (ControlProj.balancedcodemutation)
		{
			codemutationSwitch.Checked = true;
			performancemutationMode.Checked = true;
		}
		if (ControlProj.controlFlow)
		{
			controlflowSwitch.Checked = true;
		}
		if (ControlProj.cflowPerformance)
		{
			ControlFlow.isPerformance = true;
			ControlFlow.isStrong = true;
			controlflowSwitch.Checked = true;
			performancecflowMode.Checked = true;
		}
		if (ControlProj.cflowStrong)
		{
			ControlFlow.isStrong = true;
			ControlFlow.isPerformance = false;
			controlflowSwitch.Checked = true;
			strongcflowMode.Checked = true;
		}
		if (ControlProj.localtofield)
		{
			localtofieldSwitch.Checked = true;
		}
		if (ControlProj.integrityCheck)
		{
			integritycheckSwitch.Checked = true;
		}
		if (ControlProj.refProxy)
		{
			referenceproxySwitch.Checked = true;
		}
		if (ControlProj.resourcesEncoder)
		{
			encoderesourcesSwitch.Checked = true;
		}
		if (ControlProj.codeEncryption)
		{
			codeencSwitch.Checked = true;
		}
		if (ControlProj.codeVirt)
		{
			virtSwitch.Checked = true;
		}
		if (ControlProj.webhook != string.Empty)
		{
			webhookBox.Text = Encoding.UTF8.GetString(Convert.FromBase64String(ControlProj.webhook));
		}
		if (ControlProj.exclude != string.Empty)
		{
			excludeBox.Text = ControlProj.exclude;
		}
		if (ControlProj.customMsg != string.Empty)
		{
			customMsg.Text = ControlProj.customMsg;
		}
		if (ControlProj.excludep)
		{
			if (!string.IsNullOrEmpty(ControlProj.exclude) && !string.IsNullOrWhiteSpace(ControlProj.exclude))
			{
				anticrackExclude.Checked = true;
			}
			else
			{
				anticrackExclude.Checked = false;
			}
		}
		if (ControlProj.normal)
		{
			anticrackNormalMode.Checked = true;
		}
		if (ControlProj.silentMsg)
		{
			anticrackSilentMsg.Checked = true;
		}
		if (ControlProj.bsod)
		{
			bsodSwitch.Checked = true;
		}
		if (ControlProj.CustomRN)
		{
			customRenamer.Checked = true;
			customBox.Text = ControlProj.Custom;
		}
		if (ControlProj.RenameEvents)
		{
			renamingOptions.SetItemChecked(0, value: true);
		}
		if (ControlProj.RenameFields)
		{
			renamingOptions.SetItemChecked(1, value: true);
		}
		if (ControlProj.RenameMethods)
		{
			renamingOptions.SetItemChecked(2, value: true);
		}
		if (ControlProj.RenameParameters)
		{
			renamingOptions.SetItemChecked(3, value: true);
		}
		if (ControlProj.RenameProperties)
		{
			renamingOptions.SetItemChecked(4, value: true);
		}
		if (ControlProj.RenameTypes)
		{
			renamingOptions.SetItemChecked(5, value: true);
		}
		if (ControlProj.embeder)
		{
			embedderSwitch.Checked = true;
		}
		foreach (string item in ControlProj.dlls2)
		{
			dllsList.Items.Add(item);
			Embeder.dlls.Add(item);
		}
		if (ControlProj.isall)
		{
			allmeths = true;
			methodsList.Enabled = false;
			addAllMethods.Image = nigger1.check_all_18px;
			{
				foreach (string allNode in GetAllNodes(methodsList))
				{
					Helper.names.Remove(allNode);
					Helper.RemoveProtection(allNode);
					Helper.names.Add(allNode);
					Helper.AddProtection(allNode);
				}
				return;
			}
		}
		foreach (TreeNode node in methodsList.Nodes)
		{
			if (Helper.names.Contains(node.Text))
			{
				node.ImageIndex = 1;
				node.SelectedImageIndex = 1;
			}
			CheckNodesForName(node, Helper.names);
		}
	}

	private void saveProject_Click(object sender, EventArgs e)
	{
		MemoryStream xerinConfig = new MemoryStream();
		controlProj.Save(xerinConfig);
	}

	private void obfuscateAssembly_Click_1(object sender, EventArgs e)
	{
		if (ctx == null)
		{
			ctx = new XContext(assemblyBox.Text)
			{
				DirPath = destinationBox.Text,
				OutPutPath = destinationBox.Text + "\\" + Path.GetFileName(assemblyBox.Text)
			};
			ControlProj.path = assemblyBox.Text;
		}
		new XLogger().Show();
	}

	private void soundReminder_CheckedChanged(object sender, EventArgs e)
	{
		if (!soundReminder.Checked)
		{
			XLogger.soundAfter = false;
		}
		else
		{
			XLogger.soundAfter = true;
		}
	}

	private void autoDir_CheckedChanged(object sender, EventArgs e)
	{
		if (autoDir.Checked)
		{
			XLogger.autoDir = true;
		}
		else
		{
			XLogger.autoDir = false;
		}
	}

	private IEnumerable<Control> GetAllControls(Control control)
	{
		IEnumerable<Control> enumerable = control.Controls.Cast<Control>();
		return enumerable.SelectMany((Control ctrl) => GetAllControls(ctrl)).Concat(enumerable);
	}

	private void selectAll_Click(object sender, EventArgs e)
	{
		if (togemall)
		{
			togemall = false;
			selectAll.Image = nigger1.uncheck_all_18px;
			for (int i = 0; i < renamingOptions.Items.Count; i++)
			{
				renamingOptions.SetItemChecked(i, value: false);
			}
			Guna2ToggleSwitch[] array = GetAllControls(protectionsPage).OfType<Guna2ToggleSwitch>().ToArray();
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Checked = false;
			}
			strongcflowMode.Checked = false;
			intencodeSwitch.Checked = false;
			anticrackNormalMode.Checked = false;
			cfexRenamerSwitch.Checked = false;
			renamerSwitch.Checked = false;
			codeencSwitch.Checked = false;
			embedderSwitch.Checked = false;
			virtSwitch.Checked = false;
		}
		else
		{
			togemall = true;
			selectAll.Image = nigger1.check_all_18px;
			for (int k = 0; k < renamingOptions.Items.Count; k++)
			{
				renamingOptions.SetItemChecked(k, value: true);
			}
			Guna2ToggleSwitch[] array2 = GetAllControls(protectionsPage).OfType<Guna2ToggleSwitch>().ToArray();
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l].Checked = true;
			}
			strongcflowMode.Checked = true;
			intencodeSwitch.Checked = true;
			anticrackNormalMode.Checked = true;
			cfexRenamerSwitch.Checked = true;
			renamerSwitch.Checked = true;
			codeencSwitch.Checked = true;
			embedderSwitch.Checked = false;
			virtSwitch.Checked = false;
		}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XGui));
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.header = new System.Windows.Forms.Panel();
            this.minimizeApp = new Guna.UI2.WinForms.Guna2ImageButton();
            this.maximizeApp = new Guna.UI2.WinForms.Guna2ImageButton();
            this.closeApp = new Guna.UI2.WinForms.Guna2ImageButton();
            this.gotoAbout = new Guna.UI2.WinForms.Guna2ImageButton();
            this.obfuscateAssembly = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2VSeparator27 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2ImageButton3 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.appSettings = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2VSeparator2 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.headerTitle2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.headerTitle1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.selectAll = new Guna.UI2.WinForms.Guna2ImageButton();
            this.gotoProject = new Guna.UI2.WinForms.Guna2Button();
            this.gotoCodeVirt = new Guna.UI2.WinForms.Guna2Button();
            this.gotoCodeEnc = new Guna.UI2.WinForms.Guna2Button();
            this.gotoRenamer = new Guna.UI2.WinForms.Guna2Button();
            this.gotoColors = new Guna.UI2.WinForms.Guna2Button();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2HtmlLabel6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.gotoProtections = new Guna.UI2.WinForms.Guna2Button();
            this.pagesSeparator = new Guna.UI2.WinForms.Guna2VSeparator();
            this.gotoAssembly = new Guna.UI2.WinForms.Guna2Button();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.duration = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.userName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2CirclePictureBox1 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.assemblyPage = new System.Windows.Forms.Panel();
            this.preserveridsPanel = new System.Windows.Forms.Panel();
            this.guna2PictureBox3 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.preserveRids = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel68 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator35 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.OptimizePanel = new System.Windows.Forms.Panel();
            this.optimizierSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel59 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator29 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.simplePanel = new System.Windows.Forms.Panel();
            this.simpilifierSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel58 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator28 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.UnverifiableCodeAttributePanel = new System.Windows.Forms.Panel();
            this.UnverifiableCodeAttributeSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel64 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator31 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.asmArch = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.asmName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel9 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel10 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.changeDestination = new Guna.UI2.WinForms.Guna2ImageButton();
            this.addAssembly = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2HtmlLabel8 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.destinationBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2HtmlLabel7 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.assemblyBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.protectionsPage = new System.Windows.Forms.Panel();
            this.antiEmulatePanel = new System.Windows.Forms.Panel();
            this.antikauthEmulateSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel69 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator36 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.antitamperPanel = new System.Windows.Forms.Panel();
            this.antiTamperSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel67 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator34 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.intencodingPanel = new System.Windows.Forms.Panel();
            this.intencodeSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel66 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator33 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.integrityPanel = new System.Windows.Forms.Panel();
            this.integritycheckSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel24 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator14 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.l2fPanel = new System.Windows.Forms.Panel();
            this.localtofieldSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel23 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator13 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.refproxyPanel = new System.Windows.Forms.Panel();
            this.referenceproxySwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel22 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator12 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.resourcesPanel = new System.Windows.Forms.Panel();
            this.encoderesourcesSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel21 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator11 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.stringsPanel = new System.Windows.Forms.Panel();
            this.encodestringsSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel20 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator10 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.codemutPanel = new System.Windows.Forms.Panel();
            this.codemutationSettings = new Guna.UI2.WinForms.Guna2ImageButton();
            this.codemutationSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel19 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator9 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.cflowPanel = new System.Windows.Forms.Panel();
            this.controlflowSettings = new Guna.UI2.WinForms.Guna2ImageButton();
            this.controlflowSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel18 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator8 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.antivirtPanel = new System.Windows.Forms.Panel();
            this.antivirtualmachineSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel17 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator7 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.antidumpPanel = new System.Windows.Forms.Panel();
            this.antidumpSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel16 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator6 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.antidebugPanel = new System.Windows.Forms.Panel();
            this.antidebugSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel15 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator5 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.antidecPanel = new System.Windows.Forms.Panel();
            this.antidecompilerSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel14 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator4 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.anticrackPanel = new System.Windows.Forms.Panel();
            this.anticrackSettings = new Guna.UI2.WinForms.Guna2ImageButton();
            this.anticrackSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel13 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator3 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.anticrackPage = new System.Windows.Forms.Panel();
            this.editProcList = new Guna.UI2.WinForms.Guna2Button();
            this.customMsg = new Guna.UI2.WinForms.Guna2TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bsodSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel52 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator1 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.backfromACrack = new Guna.UI2.WinForms.Guna2ImageButton();
            this.excludeBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.webhookBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.anticrackExclude = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel26 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator17 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.panel5 = new System.Windows.Forms.Panel();
            this.anticrackSilentMsg = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel25 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator16 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.panel4 = new System.Windows.Forms.Panel();
            this.anticrackNormalMode = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel12 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator15 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel11 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel28 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.controlflowPage = new System.Windows.Forms.Panel();
            this.backfromCflow = new Guna.UI2.WinForms.Guna2ImageButton();
            this.panel9 = new System.Windows.Forms.Panel();
            this.performancecflowMode = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel29 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator19 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.panel10 = new System.Windows.Forms.Panel();
            this.strongcflowMode = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel30 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator20 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel31 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel32 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.codemutationPage = new System.Windows.Forms.Panel();
            this.backfromMutation = new Guna.UI2.WinForms.Guna2ImageButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.performancemutationMode = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel27 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator18 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.panel12 = new System.Windows.Forms.Panel();
            this.strongmutationMode = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel33 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator21 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel34 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel35 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.codeencPage = new System.Windows.Forms.Panel();
            this.codeencPanel = new System.Windows.Forms.Panel();
            this.codeencSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel37 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator23 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel38 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel39 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.renamerPage = new System.Windows.Forms.Panel();
            this.cfexPanel = new System.Windows.Forms.Panel();
            this.cfexRenamerSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel65 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator32 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.customRenamer = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.customBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2HtmlLabel43 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.renamingOptions = new System.Windows.Forms.CheckedListBox();
            this.guna2HtmlLabel42 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.renamePanel = new System.Windows.Forms.Panel();
            this.renamerSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel36 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator22 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel40 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel41 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.virtualizationPage = new System.Windows.Forms.Panel();
            this.addAllMethods = new Guna.UI2.WinForms.Guna2ImageButton();
            this.methodsList = new System.Windows.Forms.TreeView();
            this.guna2ContextMenuStrip1 = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            this.aDDMETHODToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rEMOVEMETHODToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.searchMethod = new Guna.UI2.WinForms.Guna2ImageButton();
            this.methodBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2HtmlLabel44 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.virtPanel = new System.Windows.Forms.Panel();
            this.virtSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel46 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator24 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel47 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel48 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.projectPage = new System.Windows.Forms.Panel();
            this.backfromProject = new Guna.UI2.WinForms.Guna2ImageButton();
            this.saveProject = new Guna.UI2.WinForms.Guna2Button();
            this.loadProject = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel50 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel51 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.colorsPage = new System.Windows.Forms.Panel();
            this.guna2ImageButton_0 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.setinRGB = new Guna.UI2.WinForms.Guna2ImageButton();
            this.bVal = new Guna.UI2.WinForms.Guna2TextBox();
            this.gVal = new Guna.UI2.WinForms.Guna2TextBox();
            this.rVal = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel49 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel45 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.settingsPage = new System.Windows.Forms.Panel();
            this.backfromSettings = new Guna.UI2.WinForms.Guna2ImageButton();
            this.panel15 = new System.Windows.Forms.Panel();
            this.autoDir = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel53 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator25 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.panel16 = new System.Windows.Forms.Panel();
            this.soundReminder = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel54 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator26 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel55 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel56 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlToolTip1 = new Guna.UI2.WinForms.Guna2HtmlToolTip();
            this.embedPage = new System.Windows.Forms.Panel();
            this.clearList = new Guna.UI2.WinForms.Guna2ImageButton();
            this.removeDll = new Guna.UI2.WinForms.Guna2ImageButton();
            this.addDlls = new Guna.UI2.WinForms.Guna2ImageButton();
            this.panel17 = new System.Windows.Forms.Panel();
            this.embedderSwitch = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.guna2HtmlLabel60 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2VSeparator30 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2HtmlLabel61 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel62 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dllsList = new System.Windows.Forms.ListBox();
            this.rightsPage = new System.Windows.Forms.Panel();
            this.backfromRights = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2HtmlLabel4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel57 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.appVersion = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2CirclePictureBox2 = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.guna2HtmlLabel3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel63 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Elipse2 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.killSwitch = new System.Windows.Forms.Timer(this.components);
            this.header.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).BeginInit();
            this.assemblyPage.SuspendLayout();
            this.preserveridsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).BeginInit();
            this.OptimizePanel.SuspendLayout();
            this.simplePanel.SuspendLayout();
            this.UnverifiableCodeAttributePanel.SuspendLayout();
            this.protectionsPage.SuspendLayout();
            this.antiEmulatePanel.SuspendLayout();
            this.antitamperPanel.SuspendLayout();
            this.intencodingPanel.SuspendLayout();
            this.integrityPanel.SuspendLayout();
            this.l2fPanel.SuspendLayout();
            this.refproxyPanel.SuspendLayout();
            this.resourcesPanel.SuspendLayout();
            this.stringsPanel.SuspendLayout();
            this.codemutPanel.SuspendLayout();
            this.cflowPanel.SuspendLayout();
            this.antivirtPanel.SuspendLayout();
            this.antidumpPanel.SuspendLayout();
            this.antidebugPanel.SuspendLayout();
            this.antidecPanel.SuspendLayout();
            this.anticrackPanel.SuspendLayout();
            this.anticrackPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.controlflowPage.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.codemutationPage.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.codeencPage.SuspendLayout();
            this.codeencPanel.SuspendLayout();
            this.renamerPage.SuspendLayout();
            this.cfexPanel.SuspendLayout();
            this.renamePanel.SuspendLayout();
            this.virtualizationPage.SuspendLayout();
            this.guna2ContextMenuStrip1.SuspendLayout();
            this.virtPanel.SuspendLayout();
            this.projectPage.SuspendLayout();
            this.colorsPage.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.panel15.SuspendLayout();
            this.panel16.SuspendLayout();
            this.embedPage.SuspendLayout();
            this.panel17.SuspendLayout();
            this.rightsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 10;
            this.guna2Elipse1.TargetControl = this;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.header.Controls.Add(this.minimizeApp);
            this.header.Controls.Add(this.maximizeApp);
            this.header.Controls.Add(this.closeApp);
            this.header.Controls.Add(this.gotoAbout);
            this.header.Controls.Add(this.obfuscateAssembly);
            this.header.Controls.Add(this.guna2VSeparator27);
            this.header.Controls.Add(this.guna2ImageButton3);
            this.header.Controls.Add(this.appSettings);
            this.header.Controls.Add(this.guna2VSeparator2);
            this.header.Controls.Add(this.headerTitle2);
            this.header.Controls.Add(this.headerTitle1);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1000, 39);
            this.header.TabIndex = 6;
            this.header.MouseDown += new System.Windows.Forms.MouseEventHandler(this.header_MouseDown);
            this.header.MouseMove += new System.Windows.Forms.MouseEventHandler(this.header_MouseMove);
            this.header.MouseUp += new System.Windows.Forms.MouseEventHandler(this.header_MouseUp);
            // 
            // minimizeApp
            // 
            this.minimizeApp.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.minimizeApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeApp.HoverState.ImageSize = new System.Drawing.Size(14, 14);
            this.minimizeApp.Image = global::nigger2.minimizeApp_Image;
            this.minimizeApp.ImageOffset = new System.Drawing.Point(0, 0);
            this.minimizeApp.ImageRotate = 0F;
            this.minimizeApp.ImageSize = new System.Drawing.Size(15, 15);
            this.minimizeApp.Location = new System.Drawing.Point(930, 10);
            this.minimizeApp.Name = "minimizeApp";
            this.minimizeApp.PressedState.ImageSize = new System.Drawing.Size(13, 13);
            this.minimizeApp.Size = new System.Drawing.Size(18, 18);
            this.minimizeApp.TabIndex = 19;
            this.minimizeApp.Click += new System.EventHandler(this.minimizeApp_Click);
            // 
            // maximizeApp
            // 
            this.maximizeApp.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.maximizeApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.maximizeApp.HoverState.ImageSize = new System.Drawing.Size(14, 14);
            this.maximizeApp.Image = global::nigger2.maximizeApp_Image;
            this.maximizeApp.ImageOffset = new System.Drawing.Point(0, 0);
            this.maximizeApp.ImageRotate = 0F;
            this.maximizeApp.ImageSize = new System.Drawing.Size(15, 15);
            this.maximizeApp.Location = new System.Drawing.Point(950, 10);
            this.maximizeApp.Name = "maximizeApp";
            this.maximizeApp.PressedState.ImageSize = new System.Drawing.Size(13, 13);
            this.maximizeApp.Size = new System.Drawing.Size(18, 18);
            this.maximizeApp.TabIndex = 18;
            this.maximizeApp.Click += new System.EventHandler(this.maximizeApp_Click);
            // 
            // closeApp
            // 
            this.closeApp.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.closeApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeApp.HoverState.ImageSize = new System.Drawing.Size(14, 14);
            this.closeApp.Image = global::nigger2.closeApp_Image;
            this.closeApp.ImageOffset = new System.Drawing.Point(0, 0);
            this.closeApp.ImageRotate = 0F;
            this.closeApp.ImageSize = new System.Drawing.Size(15, 15);
            this.closeApp.Location = new System.Drawing.Point(970, 10);
            this.closeApp.Name = "closeApp";
            this.closeApp.PressedState.ImageSize = new System.Drawing.Size(13, 13);
            this.closeApp.Size = new System.Drawing.Size(18, 18);
            this.closeApp.TabIndex = 17;
            this.closeApp.Click += new System.EventHandler(this.closeApp_Click);
            // 
            // gotoAbout
            // 
            this.gotoAbout.AnimatedGIF = true;
            this.gotoAbout.BackColor = System.Drawing.Color.Transparent;
            this.gotoAbout.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.gotoAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoAbout.HoverState.ImageSize = new System.Drawing.Size(15, 15);
            this.gotoAbout.Image = global::nigger1.copyright_16px;
            this.gotoAbout.ImageOffset = new System.Drawing.Point(0, 0);
            this.gotoAbout.ImageRotate = 0F;
            this.gotoAbout.ImageSize = new System.Drawing.Size(16, 16);
            this.gotoAbout.Location = new System.Drawing.Point(806, -1);
            this.gotoAbout.Name = "gotoAbout";
            this.gotoAbout.PressedState.ImageSize = new System.Drawing.Size(14, 14);
            this.gotoAbout.Size = new System.Drawing.Size(30, 40);
            this.gotoAbout.TabIndex = 16;
            this.gotoAbout.Click += new System.EventHandler(this.gotoAbout_Click);
            // 
            // obfuscateAssembly
            // 
            this.obfuscateAssembly.AnimatedGIF = true;
            this.obfuscateAssembly.BackColor = System.Drawing.Color.Transparent;
            this.obfuscateAssembly.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.obfuscateAssembly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.obfuscateAssembly.HoverState.ImageSize = new System.Drawing.Size(15, 15);
            this.obfuscateAssembly.Image = global::nigger1.shield_16px;
            this.obfuscateAssembly.ImageOffset = new System.Drawing.Point(0, 0);
            this.obfuscateAssembly.ImageRotate = 0F;
            this.obfuscateAssembly.ImageSize = new System.Drawing.Size(16, 16);
            this.obfuscateAssembly.Location = new System.Drawing.Point(759, -1);
            this.obfuscateAssembly.Name = "obfuscateAssembly";
            this.obfuscateAssembly.PressedState.ImageSize = new System.Drawing.Size(14, 14);
            this.obfuscateAssembly.Size = new System.Drawing.Size(30, 40);
            this.obfuscateAssembly.TabIndex = 15;
            this.obfuscateAssembly.Click += new System.EventHandler(this.obfuscateAssembly_Click_1);
            // 
            // guna2VSeparator27
            // 
            this.guna2VSeparator27.BackColor = System.Drawing.Color.Transparent;
            this.guna2VSeparator27.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.guna2VSeparator27.Location = new System.Drawing.Point(797, 14);
            this.guna2VSeparator27.Name = "guna2VSeparator27";
            this.guna2VSeparator27.Size = new System.Drawing.Size(1, 10);
            this.guna2VSeparator27.TabIndex = 14;
            // 
            // guna2ImageButton3
            // 
            this.guna2ImageButton3.AnimatedGIF = true;
            this.guna2ImageButton3.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton3.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2ImageButton3.HoverState.ImageSize = new System.Drawing.Size(15, 15);
            this.guna2ImageButton3.Image = global::nigger1.edit_file_16px;
            this.guna2ImageButton3.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton3.ImageRotate = 0F;
            this.guna2ImageButton3.ImageSize = new System.Drawing.Size(16, 16);
            this.guna2ImageButton3.Location = new System.Drawing.Point(842, -1);
            this.guna2ImageButton3.Name = "guna2ImageButton3";
            this.guna2ImageButton3.PressedState.ImageSize = new System.Drawing.Size(14, 14);
            this.guna2ImageButton3.Size = new System.Drawing.Size(30, 40);
            this.guna2ImageButton3.TabIndex = 13;
            this.guna2ImageButton3.Click += new System.EventHandler(this.guna2ImageButton3_Click);
            // 
            // appSettings
            // 
            this.appSettings.AnimatedGIF = true;
            this.appSettings.BackColor = System.Drawing.Color.Transparent;
            this.appSettings.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.appSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.appSettings.HoverState.ImageSize = new System.Drawing.Size(15, 15);
            this.appSettings.Image = global::nigger1.gear_16px;
            this.appSettings.ImageOffset = new System.Drawing.Point(0, 0);
            this.appSettings.ImageRotate = 0F;
            this.appSettings.ImageSize = new System.Drawing.Size(16, 16);
            this.appSettings.Location = new System.Drawing.Point(878, -1);
            this.appSettings.Name = "appSettings";
            this.appSettings.PressedState.ImageSize = new System.Drawing.Size(14, 14);
            this.appSettings.Size = new System.Drawing.Size(30, 40);
            this.appSettings.TabIndex = 12;
            this.appSettings.Click += new System.EventHandler(this.appSettings_Click);
            // 
            // guna2VSeparator2
            // 
            this.guna2VSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.guna2VSeparator2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.guna2VSeparator2.Location = new System.Drawing.Point(918, 14);
            this.guna2VSeparator2.Name = "guna2VSeparator2";
            this.guna2VSeparator2.Size = new System.Drawing.Size(1, 10);
            this.guna2VSeparator2.TabIndex = 8;
            // 
            // headerTitle2
            // 
            this.headerTitle2.BackColor = System.Drawing.Color.Transparent;
            this.headerTitle2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle2.ForeColor = System.Drawing.Color.White;
            this.headerTitle2.IsContextMenuEnabled = false;
            this.headerTitle2.IsSelectionEnabled = false;
            this.headerTitle2.Location = new System.Drawing.Point(42, 10);
            this.headerTitle2.Name = "headerTitle2";
            this.headerTitle2.Size = new System.Drawing.Size(142, 18);
            this.headerTitle2.TabIndex = 2;
            this.headerTitle2.Text = "FUSCATOR @Code2Reverse";
            this.headerTitle2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.header_MouseDown);
            this.headerTitle2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.header_MouseMove);
            this.headerTitle2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.header_MouseUp);
            // 
            // headerTitle1
            // 
            this.headerTitle1.BackColor = System.Drawing.Color.Transparent;
            this.headerTitle1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.headerTitle1.IsContextMenuEnabled = false;
            this.headerTitle1.IsSelectionEnabled = false;
            this.headerTitle1.Location = new System.Drawing.Point(10, 10);
            this.headerTitle1.Name = "headerTitle1";
            this.headerTitle1.Size = new System.Drawing.Size(32, 18);
            this.headerTitle1.TabIndex = 1;
            this.headerTitle1.Text = "XERIN";
            this.headerTitle1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.header_MouseDown);
            this.headerTitle1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.header_MouseMove);
            this.headerTitle1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.header_MouseUp);
            // 
            // guna2ShadowForm1
            // 
            this.guna2ShadowForm1.BorderRadius = 10;
            this.guna2ShadowForm1.TargetForm = this;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel2.Controls.Add(this.selectAll);
            this.panel2.Controls.Add(this.gotoProject);
            this.panel2.Controls.Add(this.gotoCodeVirt);
            this.panel2.Controls.Add(this.gotoCodeEnc);
            this.panel2.Controls.Add(this.gotoRenamer);
            this.panel2.Controls.Add(this.gotoColors);
            this.panel2.Controls.Add(this.guna2PictureBox2);
            this.panel2.Controls.Add(this.guna2HtmlLabel6);
            this.panel2.Controls.Add(this.gotoProtections);
            this.panel2.Controls.Add(this.pagesSeparator);
            this.panel2.Controls.Add(this.gotoAssembly);
            this.panel2.Controls.Add(this.guna2PictureBox1);
            this.panel2.Controls.Add(this.guna2HtmlLabel5);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(280, 481);
            this.panel2.TabIndex = 7;
            // 
            // selectAll
            // 
            this.selectAll.AnimatedGIF = true;
            this.selectAll.BackColor = System.Drawing.Color.Transparent;
            this.selectAll.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.selectAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectAll.HoverState.ImageSize = new System.Drawing.Size(17, 17);
            this.selectAll.Image = global::nigger1.uncheck_all_18px;
            this.selectAll.ImageOffset = new System.Drawing.Point(0, 0);
            this.selectAll.ImageRotate = 0F;
            this.selectAll.ImageSize = new System.Drawing.Size(18, 18);
            this.selectAll.Location = new System.Drawing.Point(236, 102);
            this.selectAll.Name = "selectAll";
            this.selectAll.PressedState.ImageSize = new System.Drawing.Size(16, 16);
            this.selectAll.Size = new System.Drawing.Size(30, 40);
            this.selectAll.TabIndex = 14;
            this.selectAll.Visible = false;
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // gotoProject
            // 
            this.gotoProject.Animated = true;
            this.gotoProject.AnimatedGIF = true;
            this.gotoProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoProject.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoProject.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoProject.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoProject.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoProject.FillColor = System.Drawing.Color.Transparent;
            this.gotoProject.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoProject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoProject.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoProject.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoProject.Image = global::nigger1.merge_24px2;
            this.gotoProject.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoProject.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoProject.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoProject.Location = new System.Drawing.Point(13, 274);
            this.gotoProject.Name = "gotoProject";
            this.gotoProject.PressedDepth = 0;
            this.gotoProject.Size = new System.Drawing.Size(264, 40);
            this.gotoProject.TabIndex = 16;
            this.gotoProject.Text = "EMBEDDER";
            this.gotoProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoProject.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoProject.Click += new System.EventHandler(this.gotoProject_Click);
            // 
            // gotoCodeVirt
            // 
            this.gotoCodeVirt.Animated = true;
            this.gotoCodeVirt.AnimatedGIF = true;
            this.gotoCodeVirt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoCodeVirt.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoCodeVirt.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoCodeVirt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoCodeVirt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoCodeVirt.FillColor = System.Drawing.Color.Transparent;
            this.gotoCodeVirt.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoCodeVirt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoCodeVirt.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoCodeVirt.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoCodeVirt.Image = global::nigger1.data_protection_24px2;
            this.gotoCodeVirt.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoCodeVirt.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoCodeVirt.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoCodeVirt.Location = new System.Drawing.Point(13, 231);
            this.gotoCodeVirt.Name = "gotoCodeVirt";
            this.gotoCodeVirt.PressedDepth = 0;
            this.gotoCodeVirt.Size = new System.Drawing.Size(264, 40);
            this.gotoCodeVirt.TabIndex = 15;
            this.gotoCodeVirt.Text = "CODE VIRTUALIZATION";
            this.gotoCodeVirt.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoCodeVirt.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoCodeVirt.Click += new System.EventHandler(this.gotoCodeVirt_Click);
            // 
            // gotoCodeEnc
            // 
            this.gotoCodeEnc.Animated = true;
            this.gotoCodeEnc.AnimatedGIF = true;
            this.gotoCodeEnc.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoCodeEnc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoCodeEnc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoCodeEnc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoCodeEnc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoCodeEnc.FillColor = System.Drawing.Color.Transparent;
            this.gotoCodeEnc.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoCodeEnc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoCodeEnc.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoCodeEnc.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoCodeEnc.Image = global::nigger1.shield_24px2;
            this.gotoCodeEnc.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoCodeEnc.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoCodeEnc.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoCodeEnc.Location = new System.Drawing.Point(13, 188);
            this.gotoCodeEnc.Name = "gotoCodeEnc";
            this.gotoCodeEnc.PressedDepth = 0;
            this.gotoCodeEnc.Size = new System.Drawing.Size(264, 40);
            this.gotoCodeEnc.TabIndex = 14;
            this.gotoCodeEnc.Text = "CODE ENCRYPTION";
            this.gotoCodeEnc.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoCodeEnc.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoCodeEnc.Click += new System.EventHandler(this.gotoCodeEnc_Click);
            // 
            // gotoRenamer
            // 
            this.gotoRenamer.Animated = true;
            this.gotoRenamer.AnimatedGIF = true;
            this.gotoRenamer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoRenamer.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoRenamer.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoRenamer.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoRenamer.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoRenamer.FillColor = System.Drawing.Color.Transparent;
            this.gotoRenamer.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoRenamer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoRenamer.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoRenamer.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoRenamer.Image = global::nigger1.translation_24px2;
            this.gotoRenamer.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoRenamer.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoRenamer.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoRenamer.Location = new System.Drawing.Point(13, 145);
            this.gotoRenamer.Name = "gotoRenamer";
            this.gotoRenamer.PressedDepth = 0;
            this.gotoRenamer.Size = new System.Drawing.Size(264, 40);
            this.gotoRenamer.TabIndex = 13;
            this.gotoRenamer.Text = "RENAMER";
            this.gotoRenamer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoRenamer.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoRenamer.Click += new System.EventHandler(this.gotoRenamer_Click);
            // 
            // gotoColors
            // 
            this.gotoColors.Animated = true;
            this.gotoColors.AnimatedGIF = true;
            this.gotoColors.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoColors.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoColors.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoColors.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoColors.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoColors.FillColor = System.Drawing.Color.Transparent;
            this.gotoColors.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoColors.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoColors.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoColors.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoColors.Image = global::nigger1.invert_colors_24px2;
            this.gotoColors.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoColors.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoColors.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoColors.Location = new System.Drawing.Point(13, 364);
            this.gotoColors.Name = "gotoColors";
            this.gotoColors.PressedDepth = 0;
            this.gotoColors.Size = new System.Drawing.Size(264, 40);
            this.gotoColors.TabIndex = 12;
            this.gotoColors.Text = "COLORS";
            this.gotoColors.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoColors.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoColors.Click += new System.EventHandler(this.gotoColors_Click);
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = global::nigger2.guna2PictureBox2_Image;
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(245, 330);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(18, 18);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.guna2PictureBox2.TabIndex = 11;
            this.guna2PictureBox2.TabStop = false;
            // 
            // guna2HtmlLabel6
            // 
            this.guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel6.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel6.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel6.IsContextMenuEnabled = false;
            this.guna2HtmlLabel6.IsSelectionEnabled = false;
            this.guna2HtmlLabel6.Location = new System.Drawing.Point(18, 330);
            this.guna2HtmlLabel6.Name = "guna2HtmlLabel6";
            this.guna2HtmlLabel6.Size = new System.Drawing.Size(71, 18);
            this.guna2HtmlLabel6.TabIndex = 10;
            this.guna2HtmlLabel6.Text = "APPEARANCE";
            // 
            // gotoProtections
            // 
            this.gotoProtections.Animated = true;
            this.gotoProtections.AnimatedGIF = true;
            this.gotoProtections.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoProtections.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoProtections.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoProtections.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoProtections.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoProtections.FillColor = System.Drawing.Color.Transparent;
            this.gotoProtections.Font = new System.Drawing.Font("Bahnschrift", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gotoProtections.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.gotoProtections.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoProtections.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoProtections.Image = global::nigger1.security_configuration_24px2;
            this.gotoProtections.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoProtections.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoProtections.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoProtections.Location = new System.Drawing.Point(13, 102);
            this.gotoProtections.Name = "gotoProtections";
            this.gotoProtections.PressedDepth = 0;
            this.gotoProtections.Size = new System.Drawing.Size(264, 40);
            this.gotoProtections.TabIndex = 9;
            this.gotoProtections.Text = "PROTECTIONS";
            this.gotoProtections.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoProtections.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoProtections.Click += new System.EventHandler(this.gotoProtections_Click);
            // 
            // pagesSeparator
            // 
            this.pagesSeparator.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.pagesSeparator.FillThickness = 4;
            this.pagesSeparator.Location = new System.Drawing.Point(0, 59);
            this.pagesSeparator.Name = "pagesSeparator";
            this.pagesSeparator.Size = new System.Drawing.Size(4, 40);
            this.pagesSeparator.TabIndex = 8;
            // 
            // gotoAssembly
            // 
            this.gotoAssembly.Animated = true;
            this.gotoAssembly.AnimatedGIF = true;
            this.gotoAssembly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gotoAssembly.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.gotoAssembly.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.gotoAssembly.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.gotoAssembly.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.gotoAssembly.FillColor = System.Drawing.Color.Transparent;
            this.gotoAssembly.Font = new System.Drawing.Font("Bahnschrift", 9F);
            this.gotoAssembly.ForeColor = System.Drawing.Color.White;
            this.gotoAssembly.HoverState.FillColor = System.Drawing.Color.Transparent;
            this.gotoAssembly.HoverState.ForeColor = System.Drawing.Color.White;
            this.gotoAssembly.Image = global::nigger1.file_24px;
            this.gotoAssembly.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoAssembly.ImageOffset = new System.Drawing.Point(10, 0);
            this.gotoAssembly.ImageSize = new System.Drawing.Size(24, 24);
            this.gotoAssembly.Location = new System.Drawing.Point(13, 59);
            this.gotoAssembly.Name = "gotoAssembly";
            this.gotoAssembly.PressedDepth = 0;
            this.gotoAssembly.Size = new System.Drawing.Size(264, 40);
            this.gotoAssembly.TabIndex = 8;
            this.gotoAssembly.Text = "ASSEMBLY";
            this.gotoAssembly.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.gotoAssembly.TextOffset = new System.Drawing.Point(14, 0);
            this.gotoAssembly.Click += new System.EventHandler(this.gotoAssembly_Click);
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Image = global::nigger2.guna2PictureBox1_Image;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(245, 25);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(18, 18);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.guna2PictureBox1.TabIndex = 8;
            this.guna2PictureBox1.TabStop = false;
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel5.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel5.IsContextMenuEnabled = false;
            this.guna2HtmlLabel5.IsSelectionEnabled = false;
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(18, 25);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(54, 18);
            this.guna2HtmlLabel5.TabIndex = 5;
            this.guna2HtmlLabel5.Text = "FEATURES";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.panel3.Controls.Add(this.duration);
            this.panel3.Controls.Add(this.userName);
            this.panel3.Controls.Add(this.guna2CirclePictureBox1);
            this.panel3.Location = new System.Drawing.Point(0, 424);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(280, 57);
            this.panel3.TabIndex = 6;
            // 
            // duration
            // 
            this.duration.AutoSize = false;
            this.duration.BackColor = System.Drawing.Color.Transparent;
            this.duration.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.duration.ForeColor = System.Drawing.Color.White;
            this.duration.IsContextMenuEnabled = false;
            this.duration.IsSelectionEnabled = false;
            this.duration.Location = new System.Drawing.Point(50, 28);
            this.duration.Name = "duration";
            this.duration.Size = new System.Drawing.Size(151, 15);
            this.duration.TabIndex = 9;
            this.duration.Text = "DURATION";
            // 
            // userName
            // 
            this.userName.AutoSize = false;
            this.userName.BackColor = System.Drawing.Color.Transparent;
            this.userName.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.userName.IsContextMenuEnabled = false;
            this.userName.IsSelectionEnabled = false;
            this.userName.Location = new System.Drawing.Point(50, 14);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(151, 15);
            this.userName.TabIndex = 5;
            this.userName.Text = "USER";
            // 
            // guna2CirclePictureBox1
            // 
            this.guna2CirclePictureBox1.Image = global::nigger1.fingerprint_30px;
            this.guna2CirclePictureBox1.ImageRotate = 0F;
            this.guna2CirclePictureBox1.Location = new System.Drawing.Point(12, 13);
            this.guna2CirclePictureBox1.Name = "guna2CirclePictureBox1";
            this.guna2CirclePictureBox1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox1.Size = new System.Drawing.Size(30, 30);
            this.guna2CirclePictureBox1.TabIndex = 8;
            this.guna2CirclePictureBox1.TabStop = false;
            // 
            // assemblyPage
            // 
            this.assemblyPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.assemblyPage.Controls.Add(this.preserveridsPanel);
            this.assemblyPage.Controls.Add(this.OptimizePanel);
            this.assemblyPage.Controls.Add(this.simplePanel);
            this.assemblyPage.Controls.Add(this.UnverifiableCodeAttributePanel);
            this.assemblyPage.Controls.Add(this.asmArch);
            this.assemblyPage.Controls.Add(this.asmName);
            this.assemblyPage.Controls.Add(this.guna2HtmlLabel9);
            this.assemblyPage.Controls.Add(this.guna2HtmlLabel10);
            this.assemblyPage.Controls.Add(this.changeDestination);
            this.assemblyPage.Controls.Add(this.addAssembly);
            this.assemblyPage.Controls.Add(this.guna2HtmlLabel8);
            this.assemblyPage.Controls.Add(this.destinationBox);
            this.assemblyPage.Controls.Add(this.guna2HtmlLabel7);
            this.assemblyPage.Controls.Add(this.assemblyBox);
            this.assemblyPage.Location = new System.Drawing.Point(279, 39);
            this.assemblyPage.Name = "assemblyPage";
            this.assemblyPage.Size = new System.Drawing.Size(721, 481);
            this.assemblyPage.TabIndex = 8;
            // 
            // preserveridsPanel
            // 
            this.preserveridsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.preserveridsPanel.Controls.Add(this.guna2PictureBox3);
            this.preserveridsPanel.Controls.Add(this.preserveRids);
            this.preserveridsPanel.Controls.Add(this.guna2HtmlLabel68);
            this.preserveridsPanel.Controls.Add(this.guna2VSeparator35);
            this.preserveridsPanel.Location = new System.Drawing.Point(42, 273);
            this.preserveridsPanel.Name = "preserveridsPanel";
            this.preserveridsPanel.Size = new System.Drawing.Size(410, 40);
            this.preserveridsPanel.TabIndex = 13;
            this.preserveridsPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.preserveridsPanel_MouseDown);
            // 
            // guna2PictureBox3
            // 
            this.guna2PictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.guna2PictureBox3.Image = global::nigger1.warning_shield_24px;
            this.guna2PictureBox3.ImageRotate = 0F;
            this.guna2PictureBox3.Location = new System.Drawing.Point(12, 10);
            this.guna2PictureBox3.Name = "guna2PictureBox3";
            this.guna2PictureBox3.Size = new System.Drawing.Size(20, 20);
            this.guna2PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox3.TabIndex = 14;
            this.guna2PictureBox3.TabStop = false;
            this.guna2PictureBox3.UseTransparentBackground = true;
            // 
            // preserveRids
            // 
            this.preserveRids.Animated = true;
            this.preserveRids.BackColor = System.Drawing.Color.Transparent;
            this.preserveRids.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.preserveRids.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.preserveRids.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.preserveRids.CheckedState.InnerColor = System.Drawing.Color.White;
            this.preserveRids.Cursor = System.Windows.Forms.Cursors.Hand;
            this.preserveRids.Location = new System.Drawing.Point(361, 10);
            this.preserveRids.Name = "preserveRids";
            this.preserveRids.Size = new System.Drawing.Size(35, 20);
            this.preserveRids.TabIndex = 10;
            this.preserveRids.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.preserveRids.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.preserveRids.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.preserveRids.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.preserveRids.CheckedChanged += new System.EventHandler(this.preserveRids_CheckedChanged);
            // 
            // guna2HtmlLabel68
            // 
            this.guna2HtmlLabel68.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel68.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel68.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel68.IsContextMenuEnabled = false;
            this.guna2HtmlLabel68.IsSelectionEnabled = false;
            this.guna2HtmlLabel68.Location = new System.Drawing.Point(38, 11);
            this.guna2HtmlLabel68.Name = "guna2HtmlLabel68";
            this.guna2HtmlLabel68.Size = new System.Drawing.Size(109, 18);
            this.guna2HtmlLabel68.TabIndex = 8;
            this.guna2HtmlLabel68.Text = "PreserveMethodRids";
            // 
            // guna2VSeparator35
            // 
            this.guna2VSeparator35.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator35.FillThickness = 3;
            this.guna2VSeparator35.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator35.Name = "guna2VSeparator35";
            this.guna2VSeparator35.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator35.TabIndex = 9;
            // 
            // OptimizePanel
            // 
            this.OptimizePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.OptimizePanel.Controls.Add(this.optimizierSwitch);
            this.OptimizePanel.Controls.Add(this.guna2HtmlLabel59);
            this.OptimizePanel.Controls.Add(this.guna2VSeparator29);
            this.OptimizePanel.Location = new System.Drawing.Point(42, 411);
            this.OptimizePanel.Name = "OptimizePanel";
            this.OptimizePanel.Size = new System.Drawing.Size(410, 40);
            this.OptimizePanel.TabIndex = 13;
            this.OptimizePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OptimizePanel_MouseDown);
            // 
            // optimizierSwitch
            // 
            this.optimizierSwitch.Animated = true;
            this.optimizierSwitch.BackColor = System.Drawing.Color.Transparent;
            this.optimizierSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.optimizierSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.optimizierSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.optimizierSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.optimizierSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optimizierSwitch.Location = new System.Drawing.Point(361, 10);
            this.optimizierSwitch.Name = "optimizierSwitch";
            this.optimizierSwitch.Size = new System.Drawing.Size(35, 20);
            this.optimizierSwitch.TabIndex = 10;
            this.optimizierSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.optimizierSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.optimizierSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.optimizierSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.optimizierSwitch.CheckedChanged += new System.EventHandler(this.optimizierSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel59
            // 
            this.guna2HtmlLabel59.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel59.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel59.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel59.IsContextMenuEnabled = false;
            this.guna2HtmlLabel59.IsSelectionEnabled = false;
            this.guna2HtmlLabel59.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel59.Name = "guna2HtmlLabel59";
            this.guna2HtmlLabel59.Size = new System.Drawing.Size(100, 18);
            this.guna2HtmlLabel59.TabIndex = 8;
            this.guna2HtmlLabel59.Text = "Optimize assembly";
            // 
            // guna2VSeparator29
            // 
            this.guna2VSeparator29.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator29.FillThickness = 3;
            this.guna2VSeparator29.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator29.Name = "guna2VSeparator29";
            this.guna2VSeparator29.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator29.TabIndex = 9;
            // 
            // simplePanel
            // 
            this.simplePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.simplePanel.Controls.Add(this.simpilifierSwitch);
            this.simplePanel.Controls.Add(this.guna2HtmlLabel58);
            this.simplePanel.Controls.Add(this.guna2VSeparator28);
            this.simplePanel.Location = new System.Drawing.Point(42, 365);
            this.simplePanel.Name = "simplePanel";
            this.simplePanel.Size = new System.Drawing.Size(410, 40);
            this.simplePanel.TabIndex = 13;
            this.simplePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simplePanel_MouseDown);
            // 
            // simpilifierSwitch
            // 
            this.simpilifierSwitch.Animated = true;
            this.simpilifierSwitch.BackColor = System.Drawing.Color.Transparent;
            this.simpilifierSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.simpilifierSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.simpilifierSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.simpilifierSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.simpilifierSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.simpilifierSwitch.Location = new System.Drawing.Point(361, 10);
            this.simpilifierSwitch.Name = "simpilifierSwitch";
            this.simpilifierSwitch.Size = new System.Drawing.Size(35, 20);
            this.simpilifierSwitch.TabIndex = 10;
            this.simpilifierSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.simpilifierSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.simpilifierSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.simpilifierSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.simpilifierSwitch.CheckedChanged += new System.EventHandler(this.simpilifierSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel58
            // 
            this.guna2HtmlLabel58.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel58.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel58.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel58.IsContextMenuEnabled = false;
            this.guna2HtmlLabel58.IsSelectionEnabled = false;
            this.guna2HtmlLabel58.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel58.Name = "guna2HtmlLabel58";
            this.guna2HtmlLabel58.Size = new System.Drawing.Size(97, 18);
            this.guna2HtmlLabel58.TabIndex = 8;
            this.guna2HtmlLabel58.Text = "Simplify assembly";
            // 
            // guna2VSeparator28
            // 
            this.guna2VSeparator28.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator28.FillThickness = 3;
            this.guna2VSeparator28.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator28.Name = "guna2VSeparator28";
            this.guna2VSeparator28.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator28.TabIndex = 9;
            // 
            // UnverifiableCodeAttributePanel
            // 
            this.UnverifiableCodeAttributePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.UnverifiableCodeAttributePanel.Controls.Add(this.UnverifiableCodeAttributeSwitch);
            this.UnverifiableCodeAttributePanel.Controls.Add(this.guna2HtmlLabel64);
            this.UnverifiableCodeAttributePanel.Controls.Add(this.guna2VSeparator31);
            this.UnverifiableCodeAttributePanel.Location = new System.Drawing.Point(42, 319);
            this.UnverifiableCodeAttributePanel.Name = "UnverifiableCodeAttributePanel";
            this.UnverifiableCodeAttributePanel.Size = new System.Drawing.Size(410, 40);
            this.UnverifiableCodeAttributePanel.TabIndex = 12;
            this.UnverifiableCodeAttributePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UnverifiableCodeAttributePanel_MouseDown);
            // 
            // UnverifiableCodeAttributeSwitch
            // 
            this.UnverifiableCodeAttributeSwitch.Animated = true;
            this.UnverifiableCodeAttributeSwitch.BackColor = System.Drawing.Color.Transparent;
            this.UnverifiableCodeAttributeSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.UnverifiableCodeAttributeSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.UnverifiableCodeAttributeSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.UnverifiableCodeAttributeSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.UnverifiableCodeAttributeSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UnverifiableCodeAttributeSwitch.Location = new System.Drawing.Point(361, 10);
            this.UnverifiableCodeAttributeSwitch.Name = "UnverifiableCodeAttributeSwitch";
            this.UnverifiableCodeAttributeSwitch.Size = new System.Drawing.Size(35, 20);
            this.UnverifiableCodeAttributeSwitch.TabIndex = 10;
            this.UnverifiableCodeAttributeSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.UnverifiableCodeAttributeSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.UnverifiableCodeAttributeSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.UnverifiableCodeAttributeSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.UnverifiableCodeAttributeSwitch.CheckedChanged += new System.EventHandler(this.UnverifiableCodeAttributeSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel64
            // 
            this.guna2HtmlLabel64.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel64.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel64.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel64.IsContextMenuEnabled = false;
            this.guna2HtmlLabel64.IsSelectionEnabled = false;
            this.guna2HtmlLabel64.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel64.Name = "guna2HtmlLabel64";
            this.guna2HtmlLabel64.Size = new System.Drawing.Size(134, 18);
            this.guna2HtmlLabel64.TabIndex = 8;
            this.guna2HtmlLabel64.Text = "UnverifiableCodeAttribute";
            // 
            // guna2VSeparator31
            // 
            this.guna2VSeparator31.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator31.FillThickness = 3;
            this.guna2VSeparator31.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator31.Name = "guna2VSeparator31";
            this.guna2VSeparator31.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator31.TabIndex = 9;
            // 
            // asmArch
            // 
            this.asmArch.BackColor = System.Drawing.Color.Transparent;
            this.asmArch.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asmArch.ForeColor = System.Drawing.Color.White;
            this.asmArch.IsContextMenuEnabled = false;
            this.asmArch.IsSelectionEnabled = false;
            this.asmArch.Location = new System.Drawing.Point(42, 234);
            this.asmArch.Name = "asmArch";
            this.asmArch.Size = new System.Drawing.Size(77, 18);
            this.asmArch.TabIndex = 11;
            this.asmArch.Text = "ARCHITECTURE";
            // 
            // asmName
            // 
            this.asmName.BackColor = System.Drawing.Color.Transparent;
            this.asmName.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asmName.ForeColor = System.Drawing.Color.White;
            this.asmName.IsContextMenuEnabled = false;
            this.asmName.IsSelectionEnabled = false;
            this.asmName.Location = new System.Drawing.Point(42, 210);
            this.asmName.Name = "asmName";
            this.asmName.Size = new System.Drawing.Size(32, 18);
            this.asmName.TabIndex = 10;
            this.asmName.Text = "NAME";
            // 
            // guna2HtmlLabel9
            // 
            this.guna2HtmlLabel9.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel9.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel9.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel9.IsContextMenuEnabled = false;
            this.guna2HtmlLabel9.IsSelectionEnabled = false;
            this.guna2HtmlLabel9.Location = new System.Drawing.Point(101, 186);
            this.guna2HtmlLabel9.Name = "guna2HtmlLabel9";
            this.guna2HtmlLabel9.Size = new System.Drawing.Size(26, 18);
            this.guna2HtmlLabel9.TabIndex = 9;
            this.guna2HtmlLabel9.Text = "INFO";
            // 
            // guna2HtmlLabel10
            // 
            this.guna2HtmlLabel10.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel10.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2HtmlLabel10.IsContextMenuEnabled = false;
            this.guna2HtmlLabel10.IsSelectionEnabled = false;
            this.guna2HtmlLabel10.Location = new System.Drawing.Point(42, 186);
            this.guna2HtmlLabel10.Name = "guna2HtmlLabel10";
            this.guna2HtmlLabel10.Size = new System.Drawing.Size(58, 18);
            this.guna2HtmlLabel10.TabIndex = 8;
            this.guna2HtmlLabel10.Text = "ASSEMBLY";
            // 
            // changeDestination
            // 
            this.changeDestination.AnimatedGIF = true;
            this.changeDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.changeDestination.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.changeDestination.Cursor = System.Windows.Forms.Cursors.Hand;
            this.changeDestination.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.changeDestination.Image = global::nigger1.folder_20px1;
            this.changeDestination.ImageOffset = new System.Drawing.Point(0, 0);
            this.changeDestination.ImageRotate = 0F;
            this.changeDestination.ImageSize = new System.Drawing.Size(20, 20);
            this.changeDestination.Location = new System.Drawing.Point(440, 107);
            this.changeDestination.Name = "changeDestination";
            this.changeDestination.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.changeDestination.Size = new System.Drawing.Size(30, 30);
            this.changeDestination.TabIndex = 7;
            this.changeDestination.Click += new System.EventHandler(this.changeDestination_Click);
            // 
            // addAssembly
            // 
            this.addAssembly.AnimatedGIF = true;
            this.addAssembly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.addAssembly.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.addAssembly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addAssembly.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.addAssembly.Image = global::nigger1.add_20px1;
            this.addAssembly.ImageOffset = new System.Drawing.Point(0, 0);
            this.addAssembly.ImageRotate = 0F;
            this.addAssembly.ImageSize = new System.Drawing.Size(20, 20);
            this.addAssembly.Location = new System.Drawing.Point(440, 38);
            this.addAssembly.Name = "addAssembly";
            this.addAssembly.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.addAssembly.Size = new System.Drawing.Size(30, 30);
            this.addAssembly.TabIndex = 6;
            this.addAssembly.Click += new System.EventHandler(this.addAssembly_Click);
            // 
            // guna2HtmlLabel8
            // 
            this.guna2HtmlLabel8.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel8.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel8.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel8.IsContextMenuEnabled = false;
            this.guna2HtmlLabel8.IsSelectionEnabled = false;
            this.guna2HtmlLabel8.Location = new System.Drawing.Point(42, 107);
            this.guna2HtmlLabel8.Name = "guna2HtmlLabel8";
            this.guna2HtmlLabel8.Size = new System.Drawing.Size(67, 18);
            this.guna2HtmlLabel8.TabIndex = 5;
            this.guna2HtmlLabel8.Text = "DESTINATION";
            // 
            // destinationBox
            // 
            this.destinationBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.destinationBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.destinationBox.DefaultText = "";
            this.destinationBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.destinationBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.destinationBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.destinationBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.destinationBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.destinationBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.destinationBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.destinationBox.ForeColor = System.Drawing.Color.White;
            this.destinationBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.destinationBox.Location = new System.Drawing.Point(42, 131);
            this.destinationBox.Name = "destinationBox";
            this.destinationBox.PasswordChar = '\0';
            this.destinationBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.destinationBox.PlaceholderText = "OBFUSCATED ASSEMBLY WILL BE SAVED HERE";
            this.destinationBox.SelectedText = "";
            this.destinationBox.Size = new System.Drawing.Size(427, 34);
            this.destinationBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.destinationBox.TabIndex = 4;
            // 
            // guna2HtmlLabel7
            // 
            this.guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel7.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel7.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel7.IsContextMenuEnabled = false;
            this.guna2HtmlLabel7.IsSelectionEnabled = false;
            this.guna2HtmlLabel7.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel7.Name = "guna2HtmlLabel7";
            this.guna2HtmlLabel7.Size = new System.Drawing.Size(87, 18);
            this.guna2HtmlLabel7.TabIndex = 3;
            this.guna2HtmlLabel7.Text = "ASSEMBLY PATH";
            // 
            // assemblyBox
            // 
            this.assemblyBox.AllowDrop = true;
            this.assemblyBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.assemblyBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.assemblyBox.DefaultText = "";
            this.assemblyBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.assemblyBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.assemblyBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.assemblyBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.assemblyBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.assemblyBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.assemblyBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.assemblyBox.ForeColor = System.Drawing.Color.White;
            this.assemblyBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.assemblyBox.Location = new System.Drawing.Point(42, 62);
            this.assemblyBox.Name = "assemblyBox";
            this.assemblyBox.PasswordChar = '\0';
            this.assemblyBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.assemblyBox.PlaceholderText = "DRAG N DROP ASSEMBLY HERE";
            this.assemblyBox.SelectedText = "";
            this.assemblyBox.Size = new System.Drawing.Size(427, 34);
            this.assemblyBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.assemblyBox.TabIndex = 0;
            this.assemblyBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.assemblyBox_DragDrop);
            this.assemblyBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.assemblyBox_DragEnter);
            // 
            // protectionsPage
            // 
            this.protectionsPage.AutoScroll = true;
            this.protectionsPage.AutoScrollMargin = new System.Drawing.Size(0, 16);
            this.protectionsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.protectionsPage.Controls.Add(this.antiEmulatePanel);
            this.protectionsPage.Controls.Add(this.antitamperPanel);
            this.protectionsPage.Controls.Add(this.intencodingPanel);
            this.protectionsPage.Controls.Add(this.integrityPanel);
            this.protectionsPage.Controls.Add(this.l2fPanel);
            this.protectionsPage.Controls.Add(this.refproxyPanel);
            this.protectionsPage.Controls.Add(this.resourcesPanel);
            this.protectionsPage.Controls.Add(this.stringsPanel);
            this.protectionsPage.Controls.Add(this.codemutPanel);
            this.protectionsPage.Controls.Add(this.cflowPanel);
            this.protectionsPage.Controls.Add(this.antivirtPanel);
            this.protectionsPage.Controls.Add(this.antidumpPanel);
            this.protectionsPage.Controls.Add(this.antidebugPanel);
            this.protectionsPage.Controls.Add(this.antidecPanel);
            this.protectionsPage.Controls.Add(this.anticrackPanel);
            this.protectionsPage.Location = new System.Drawing.Point(279, 39);
            this.protectionsPage.Name = "protectionsPage";
            this.protectionsPage.Size = new System.Drawing.Size(721, 481);
            this.protectionsPage.TabIndex = 9;
            // 
            // antiEmulatePanel
            // 
            this.antiEmulatePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antiEmulatePanel.Controls.Add(this.antikauthEmulateSwitch);
            this.antiEmulatePanel.Controls.Add(this.guna2HtmlLabel69);
            this.antiEmulatePanel.Controls.Add(this.guna2VSeparator36);
            this.antiEmulatePanel.Location = new System.Drawing.Point(14, 200);
            this.antiEmulatePanel.Name = "antiEmulatePanel";
            this.antiEmulatePanel.Size = new System.Drawing.Size(667, 40);
            this.antiEmulatePanel.TabIndex = 12;
            this.antiEmulatePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antiEmulatePanel_MouseDown);
            // 
            // antikauthEmulateSwitch
            // 
            this.antikauthEmulateSwitch.Animated = true;
            this.antikauthEmulateSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antikauthEmulateSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antikauthEmulateSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antikauthEmulateSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antikauthEmulateSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antikauthEmulateSwitch.Location = new System.Drawing.Point(620, 10);
            this.antikauthEmulateSwitch.Name = "antikauthEmulateSwitch";
            this.antikauthEmulateSwitch.Size = new System.Drawing.Size(35, 20);
            this.antikauthEmulateSwitch.TabIndex = 10;
            this.antikauthEmulateSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antikauthEmulateSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antikauthEmulateSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antikauthEmulateSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antikauthEmulateSwitch.CheckedChanged += new System.EventHandler(this.antikauthEmulateSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel69
            // 
            this.guna2HtmlLabel69.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel69.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel69.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel69.IsContextMenuEnabled = false;
            this.guna2HtmlLabel69.IsSelectionEnabled = false;
            this.guna2HtmlLabel69.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel69.Name = "guna2HtmlLabel69";
            this.guna2HtmlLabel69.Size = new System.Drawing.Size(133, 18);
            this.guna2HtmlLabel69.TabIndex = 8;
            this.guna2HtmlLabel69.Text = "ANTI KEYAUTH EMULATING";
            // 
            // guna2VSeparator36
            // 
            this.guna2VSeparator36.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator36.FillThickness = 3;
            this.guna2VSeparator36.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator36.Name = "guna2VSeparator36";
            this.guna2VSeparator36.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator36.TabIndex = 9;
            // 
            // antitamperPanel
            // 
            this.antitamperPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antitamperPanel.Controls.Add(this.antiTamperSwitch);
            this.antitamperPanel.Controls.Add(this.guna2HtmlLabel67);
            this.antitamperPanel.Controls.Add(this.guna2VSeparator34);
            this.antitamperPanel.Location = new System.Drawing.Point(14, 246);
            this.antitamperPanel.Name = "antitamperPanel";
            this.antitamperPanel.Size = new System.Drawing.Size(667, 40);
            this.antitamperPanel.TabIndex = 13;
            this.antitamperPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antitamperPanel_MouseDown);
            // 
            // antiTamperSwitch
            // 
            this.antiTamperSwitch.Animated = true;
            this.antiTamperSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antiTamperSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antiTamperSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antiTamperSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antiTamperSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antiTamperSwitch.Location = new System.Drawing.Point(620, 10);
            this.antiTamperSwitch.Name = "antiTamperSwitch";
            this.antiTamperSwitch.Size = new System.Drawing.Size(35, 20);
            this.antiTamperSwitch.TabIndex = 10;
            this.antiTamperSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antiTamperSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antiTamperSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antiTamperSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antiTamperSwitch.CheckedChanged += new System.EventHandler(this.antiTamperSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel67
            // 
            this.guna2HtmlLabel67.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel67.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel67.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel67.IsContextMenuEnabled = false;
            this.guna2HtmlLabel67.IsSelectionEnabled = false;
            this.guna2HtmlLabel67.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel67.Name = "guna2HtmlLabel67";
            this.guna2HtmlLabel67.Size = new System.Drawing.Size(69, 18);
            this.guna2HtmlLabel67.TabIndex = 8;
            this.guna2HtmlLabel67.Text = "ANTI TAMPER";
            // 
            // guna2VSeparator34
            // 
            this.guna2VSeparator34.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator34.FillThickness = 3;
            this.guna2VSeparator34.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator34.Name = "guna2VSeparator34";
            this.guna2VSeparator34.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator34.TabIndex = 9;
            // 
            // intencodingPanel
            // 
            this.intencodingPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.intencodingPanel.Controls.Add(this.intencodeSwitch);
            this.intencodingPanel.Controls.Add(this.guna2HtmlLabel66);
            this.intencodingPanel.Controls.Add(this.guna2VSeparator33);
            this.intencodingPanel.Location = new System.Drawing.Point(14, 430);
            this.intencodingPanel.Name = "intencodingPanel";
            this.intencodingPanel.Size = new System.Drawing.Size(667, 40);
            this.intencodingPanel.TabIndex = 14;
            this.intencodingPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.intencodingPanel_MouseDown);
            // 
            // intencodeSwitch
            // 
            this.intencodeSwitch.Animated = true;
            this.intencodeSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.intencodeSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.intencodeSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.intencodeSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.intencodeSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.intencodeSwitch.Location = new System.Drawing.Point(620, 10);
            this.intencodeSwitch.Name = "intencodeSwitch";
            this.intencodeSwitch.Size = new System.Drawing.Size(35, 20);
            this.intencodeSwitch.TabIndex = 10;
            this.intencodeSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.intencodeSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.intencodeSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.intencodeSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.intencodeSwitch.CheckedChanged += new System.EventHandler(this.intencodeSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel66
            // 
            this.guna2HtmlLabel66.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel66.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel66.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel66.IsContextMenuEnabled = false;
            this.guna2HtmlLabel66.IsSelectionEnabled = false;
            this.guna2HtmlLabel66.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel66.Name = "guna2HtmlLabel66";
            this.guna2HtmlLabel66.Size = new System.Drawing.Size(105, 18);
            this.guna2HtmlLabel66.TabIndex = 8;
            this.guna2HtmlLabel66.Text = "INTEGERS ENCODING";
            // 
            // guna2VSeparator33
            // 
            this.guna2VSeparator33.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator33.FillThickness = 3;
            this.guna2VSeparator33.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator33.Name = "guna2VSeparator33";
            this.guna2VSeparator33.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator33.TabIndex = 9;
            // 
            // integrityPanel
            // 
            this.integrityPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.integrityPanel.Controls.Add(this.integritycheckSwitch);
            this.integrityPanel.Controls.Add(this.guna2HtmlLabel24);
            this.integrityPanel.Controls.Add(this.guna2VSeparator14);
            this.integrityPanel.Location = new System.Drawing.Point(14, 660);
            this.integrityPanel.Name = "integrityPanel";
            this.integrityPanel.Size = new System.Drawing.Size(667, 40);
            this.integrityPanel.TabIndex = 13;
            this.integrityPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.integrityPanel_MouseDown);
            // 
            // integritycheckSwitch
            // 
            this.integritycheckSwitch.Animated = true;
            this.integritycheckSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.integritycheckSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.integritycheckSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.integritycheckSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.integritycheckSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.integritycheckSwitch.Location = new System.Drawing.Point(620, 10);
            this.integritycheckSwitch.Name = "integritycheckSwitch";
            this.integritycheckSwitch.Size = new System.Drawing.Size(35, 20);
            this.integritycheckSwitch.TabIndex = 10;
            this.integritycheckSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.integritycheckSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.integritycheckSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.integritycheckSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.integritycheckSwitch.CheckedChanged += new System.EventHandler(this.integritycheckSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel24
            // 
            this.guna2HtmlLabel24.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel24.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel24.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel24.IsContextMenuEnabled = false;
            this.guna2HtmlLabel24.IsSelectionEnabled = false;
            this.guna2HtmlLabel24.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel24.Name = "guna2HtmlLabel24";
            this.guna2HtmlLabel24.Size = new System.Drawing.Size(89, 18);
            this.guna2HtmlLabel24.TabIndex = 8;
            this.guna2HtmlLabel24.Text = "INTEGRITY CHECK";
            // 
            // guna2VSeparator14
            // 
            this.guna2VSeparator14.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator14.FillThickness = 3;
            this.guna2VSeparator14.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator14.Name = "guna2VSeparator14";
            this.guna2VSeparator14.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator14.TabIndex = 9;
            // 
            // l2fPanel
            // 
            this.l2fPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.l2fPanel.Controls.Add(this.localtofieldSwitch);
            this.l2fPanel.Controls.Add(this.guna2HtmlLabel23);
            this.l2fPanel.Controls.Add(this.guna2VSeparator13);
            this.l2fPanel.Location = new System.Drawing.Point(14, 614);
            this.l2fPanel.Name = "l2fPanel";
            this.l2fPanel.Size = new System.Drawing.Size(667, 40);
            this.l2fPanel.TabIndex = 12;
            this.l2fPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.l2fPanel_MouseDown);
            // 
            // localtofieldSwitch
            // 
            this.localtofieldSwitch.Animated = true;
            this.localtofieldSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.localtofieldSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.localtofieldSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.localtofieldSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.localtofieldSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.localtofieldSwitch.Location = new System.Drawing.Point(620, 10);
            this.localtofieldSwitch.Name = "localtofieldSwitch";
            this.localtofieldSwitch.Size = new System.Drawing.Size(35, 20);
            this.localtofieldSwitch.TabIndex = 10;
            this.localtofieldSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.localtofieldSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.localtofieldSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.localtofieldSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.localtofieldSwitch.CheckedChanged += new System.EventHandler(this.localtofieldSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel23
            // 
            this.guna2HtmlLabel23.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel23.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel23.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel23.IsContextMenuEnabled = false;
            this.guna2HtmlLabel23.IsSelectionEnabled = false;
            this.guna2HtmlLabel23.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel23.Name = "guna2HtmlLabel23";
            this.guna2HtmlLabel23.Size = new System.Drawing.Size(82, 18);
            this.guna2HtmlLabel23.TabIndex = 8;
            this.guna2HtmlLabel23.Text = "LOCAL TO FIELD";
            // 
            // guna2VSeparator13
            // 
            this.guna2VSeparator13.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator13.FillThickness = 3;
            this.guna2VSeparator13.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator13.Name = "guna2VSeparator13";
            this.guna2VSeparator13.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator13.TabIndex = 9;
            // 
            // refproxyPanel
            // 
            this.refproxyPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.refproxyPanel.Controls.Add(this.referenceproxySwitch);
            this.refproxyPanel.Controls.Add(this.guna2HtmlLabel22);
            this.refproxyPanel.Controls.Add(this.guna2VSeparator12);
            this.refproxyPanel.Location = new System.Drawing.Point(14, 568);
            this.refproxyPanel.Name = "refproxyPanel";
            this.refproxyPanel.Size = new System.Drawing.Size(667, 40);
            this.refproxyPanel.TabIndex = 11;
            this.refproxyPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.refproxyPanel_MouseDown);
            // 
            // referenceproxySwitch
            // 
            this.referenceproxySwitch.Animated = true;
            this.referenceproxySwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.referenceproxySwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.referenceproxySwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.referenceproxySwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.referenceproxySwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.referenceproxySwitch.Location = new System.Drawing.Point(620, 10);
            this.referenceproxySwitch.Name = "referenceproxySwitch";
            this.referenceproxySwitch.Size = new System.Drawing.Size(35, 20);
            this.referenceproxySwitch.TabIndex = 10;
            this.referenceproxySwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.referenceproxySwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.referenceproxySwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.referenceproxySwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.referenceproxySwitch.CheckedChanged += new System.EventHandler(this.referenceproxySwitch_CheckedChanged);
            // 
            // guna2HtmlLabel22
            // 
            this.guna2HtmlLabel22.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel22.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel22.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel22.IsContextMenuEnabled = false;
            this.guna2HtmlLabel22.IsSelectionEnabled = false;
            this.guna2HtmlLabel22.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel22.Name = "guna2HtmlLabel22";
            this.guna2HtmlLabel22.Size = new System.Drawing.Size(97, 18);
            this.guna2HtmlLabel22.TabIndex = 8;
            this.guna2HtmlLabel22.Text = "REFERENCE PROXY";
            // 
            // guna2VSeparator12
            // 
            this.guna2VSeparator12.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator12.FillThickness = 3;
            this.guna2VSeparator12.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator12.Name = "guna2VSeparator12";
            this.guna2VSeparator12.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator12.TabIndex = 9;
            // 
            // resourcesPanel
            // 
            this.resourcesPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.resourcesPanel.Controls.Add(this.encoderesourcesSwitch);
            this.resourcesPanel.Controls.Add(this.guna2HtmlLabel21);
            this.resourcesPanel.Controls.Add(this.guna2VSeparator11);
            this.resourcesPanel.Location = new System.Drawing.Point(14, 522);
            this.resourcesPanel.Name = "resourcesPanel";
            this.resourcesPanel.Size = new System.Drawing.Size(667, 40);
            this.resourcesPanel.TabIndex = 11;
            this.resourcesPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.resourcesPanel_MouseDown);
            // 
            // encoderesourcesSwitch
            // 
            this.encoderesourcesSwitch.Animated = true;
            this.encoderesourcesSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encoderesourcesSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.encoderesourcesSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encoderesourcesSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.encoderesourcesSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.encoderesourcesSwitch.Location = new System.Drawing.Point(620, 10);
            this.encoderesourcesSwitch.Name = "encoderesourcesSwitch";
            this.encoderesourcesSwitch.Size = new System.Drawing.Size(35, 20);
            this.encoderesourcesSwitch.TabIndex = 10;
            this.encoderesourcesSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encoderesourcesSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.encoderesourcesSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.encoderesourcesSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.encoderesourcesSwitch.CheckedChanged += new System.EventHandler(this.encoderesourcesSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel21
            // 
            this.guna2HtmlLabel21.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel21.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel21.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel21.IsContextMenuEnabled = false;
            this.guna2HtmlLabel21.IsSelectionEnabled = false;
            this.guna2HtmlLabel21.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel21.Name = "guna2HtmlLabel21";
            this.guna2HtmlLabel21.Size = new System.Drawing.Size(177, 18);
            this.guna2HtmlLabel21.TabIndex = 8;
            this.guna2HtmlLabel21.Text = "ENCODE & COMPRESS RESOURCES";
            // 
            // guna2VSeparator11
            // 
            this.guna2VSeparator11.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator11.FillThickness = 3;
            this.guna2VSeparator11.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator11.Name = "guna2VSeparator11";
            this.guna2VSeparator11.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator11.TabIndex = 9;
            // 
            // stringsPanel
            // 
            this.stringsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.stringsPanel.Controls.Add(this.encodestringsSwitch);
            this.stringsPanel.Controls.Add(this.guna2HtmlLabel20);
            this.stringsPanel.Controls.Add(this.guna2VSeparator10);
            this.stringsPanel.Location = new System.Drawing.Point(14, 476);
            this.stringsPanel.Name = "stringsPanel";
            this.stringsPanel.Size = new System.Drawing.Size(667, 40);
            this.stringsPanel.TabIndex = 11;
            this.stringsPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.stringsPanel_MouseDown);
            // 
            // encodestringsSwitch
            // 
            this.encodestringsSwitch.Animated = true;
            this.encodestringsSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encodestringsSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.encodestringsSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encodestringsSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.encodestringsSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.encodestringsSwitch.Location = new System.Drawing.Point(620, 10);
            this.encodestringsSwitch.Name = "encodestringsSwitch";
            this.encodestringsSwitch.Size = new System.Drawing.Size(35, 20);
            this.encodestringsSwitch.TabIndex = 10;
            this.encodestringsSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.encodestringsSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.encodestringsSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.encodestringsSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.encodestringsSwitch.CheckedChanged += new System.EventHandler(this.encodestringsSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel20
            // 
            this.guna2HtmlLabel20.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel20.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel20.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel20.IsContextMenuEnabled = false;
            this.guna2HtmlLabel20.IsSelectionEnabled = false;
            this.guna2HtmlLabel20.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel20.Name = "guna2HtmlLabel20";
            this.guna2HtmlLabel20.Size = new System.Drawing.Size(199, 18);
            this.guna2HtmlLabel20.TabIndex = 8;
            this.guna2HtmlLabel20.Text = "ENCODE, COMPRESS & CACHE STRINGS";
            // 
            // guna2VSeparator10
            // 
            this.guna2VSeparator10.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator10.FillThickness = 3;
            this.guna2VSeparator10.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator10.Name = "guna2VSeparator10";
            this.guna2VSeparator10.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator10.TabIndex = 9;
            // 
            // codemutPanel
            // 
            this.codemutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.codemutPanel.Controls.Add(this.codemutationSettings);
            this.codemutPanel.Controls.Add(this.codemutationSwitch);
            this.codemutPanel.Controls.Add(this.guna2HtmlLabel19);
            this.codemutPanel.Controls.Add(this.guna2VSeparator9);
            this.codemutPanel.Location = new System.Drawing.Point(14, 384);
            this.codemutPanel.Name = "codemutPanel";
            this.codemutPanel.Size = new System.Drawing.Size(667, 40);
            this.codemutPanel.TabIndex = 11;
            this.codemutPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.codemutPanel_MouseDown);
            // 
            // codemutationSettings
            // 
            this.codemutationSettings.BackColor = System.Drawing.Color.Transparent;
            this.codemutationSettings.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.codemutationSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.codemutationSettings.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.codemutationSettings.Image = global::nigger2.codemutationSettings_Image;
            this.codemutationSettings.ImageOffset = new System.Drawing.Point(0, 0);
            this.codemutationSettings.ImageRotate = 0F;
            this.codemutationSettings.ImageSize = new System.Drawing.Size(20, 20);
            this.codemutationSettings.Location = new System.Drawing.Point(590, 8);
            this.codemutationSettings.Name = "codemutationSettings";
            this.codemutationSettings.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.codemutationSettings.Size = new System.Drawing.Size(24, 24);
            this.codemutationSettings.TabIndex = 10;
            this.codemutationSettings.Visible = false;
            this.codemutationSettings.Click += new System.EventHandler(this.codemutationSettings_Click);
            // 
            // codemutationSwitch
            // 
            this.codemutationSwitch.Animated = true;
            this.codemutationSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codemutationSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.codemutationSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codemutationSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.codemutationSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.codemutationSwitch.Location = new System.Drawing.Point(620, 10);
            this.codemutationSwitch.Name = "codemutationSwitch";
            this.codemutationSwitch.Size = new System.Drawing.Size(35, 20);
            this.codemutationSwitch.TabIndex = 10;
            this.codemutationSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codemutationSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.codemutationSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.codemutationSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.codemutationSwitch.CheckedChanged += new System.EventHandler(this.codemutationSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel19
            // 
            this.guna2HtmlLabel19.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel19.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel19.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel19.IsContextMenuEnabled = false;
            this.guna2HtmlLabel19.IsSelectionEnabled = false;
            this.guna2HtmlLabel19.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel19.Name = "guna2HtmlLabel19";
            this.guna2HtmlLabel19.Size = new System.Drawing.Size(83, 18);
            this.guna2HtmlLabel19.TabIndex = 8;
            this.guna2HtmlLabel19.Text = "CODE MUTATION";
            // 
            // guna2VSeparator9
            // 
            this.guna2VSeparator9.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator9.FillThickness = 3;
            this.guna2VSeparator9.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator9.Name = "guna2VSeparator9";
            this.guna2VSeparator9.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator9.TabIndex = 9;
            // 
            // cflowPanel
            // 
            this.cflowPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.cflowPanel.Controls.Add(this.controlflowSettings);
            this.cflowPanel.Controls.Add(this.controlflowSwitch);
            this.cflowPanel.Controls.Add(this.guna2HtmlLabel18);
            this.cflowPanel.Controls.Add(this.guna2VSeparator8);
            this.cflowPanel.Location = new System.Drawing.Point(14, 338);
            this.cflowPanel.Name = "cflowPanel";
            this.cflowPanel.Size = new System.Drawing.Size(667, 40);
            this.cflowPanel.TabIndex = 11;
            this.cflowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cflowPanel_MouseDown);
            // 
            // controlflowSettings
            // 
            this.controlflowSettings.BackColor = System.Drawing.Color.Transparent;
            this.controlflowSettings.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.controlflowSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.controlflowSettings.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.controlflowSettings.Image = global::nigger2.controlflowSettings_Image;
            this.controlflowSettings.ImageOffset = new System.Drawing.Point(0, 0);
            this.controlflowSettings.ImageRotate = 0F;
            this.controlflowSettings.ImageSize = new System.Drawing.Size(20, 20);
            this.controlflowSettings.Location = new System.Drawing.Point(590, 8);
            this.controlflowSettings.Name = "controlflowSettings";
            this.controlflowSettings.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.controlflowSettings.Size = new System.Drawing.Size(24, 24);
            this.controlflowSettings.TabIndex = 10;
            this.controlflowSettings.Click += new System.EventHandler(this.controlflowSettings_Click);
            // 
            // controlflowSwitch
            // 
            this.controlflowSwitch.Animated = true;
            this.controlflowSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controlflowSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.controlflowSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controlflowSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.controlflowSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.controlflowSwitch.Location = new System.Drawing.Point(620, 10);
            this.controlflowSwitch.Name = "controlflowSwitch";
            this.controlflowSwitch.Size = new System.Drawing.Size(35, 20);
            this.controlflowSwitch.TabIndex = 10;
            this.controlflowSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.controlflowSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.controlflowSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.controlflowSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.controlflowSwitch.CheckedChanged += new System.EventHandler(this.controlflowSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel18
            // 
            this.guna2HtmlLabel18.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel18.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel18.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel18.IsContextMenuEnabled = false;
            this.guna2HtmlLabel18.IsSelectionEnabled = false;
            this.guna2HtmlLabel18.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel18.Name = "guna2HtmlLabel18";
            this.guna2HtmlLabel18.Size = new System.Drawing.Size(80, 18);
            this.guna2HtmlLabel18.TabIndex = 8;
            this.guna2HtmlLabel18.Text = "CONTROL FLOW";
            // 
            // guna2VSeparator8
            // 
            this.guna2VSeparator8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator8.FillThickness = 3;
            this.guna2VSeparator8.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator8.Name = "guna2VSeparator8";
            this.guna2VSeparator8.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator8.TabIndex = 9;
            // 
            // antivirtPanel
            // 
            this.antivirtPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antivirtPanel.Controls.Add(this.antivirtualmachineSwitch);
            this.antivirtPanel.Controls.Add(this.guna2HtmlLabel17);
            this.antivirtPanel.Controls.Add(this.guna2VSeparator7);
            this.antivirtPanel.Location = new System.Drawing.Point(14, 292);
            this.antivirtPanel.Name = "antivirtPanel";
            this.antivirtPanel.Size = new System.Drawing.Size(667, 40);
            this.antivirtPanel.TabIndex = 11;
            this.antivirtPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antivirtPanel_MouseDown);
            // 
            // antivirtualmachineSwitch
            // 
            this.antivirtualmachineSwitch.Animated = true;
            this.antivirtualmachineSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antivirtualmachineSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antivirtualmachineSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antivirtualmachineSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antivirtualmachineSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antivirtualmachineSwitch.Location = new System.Drawing.Point(620, 10);
            this.antivirtualmachineSwitch.Name = "antivirtualmachineSwitch";
            this.antivirtualmachineSwitch.Size = new System.Drawing.Size(35, 20);
            this.antivirtualmachineSwitch.TabIndex = 10;
            this.antivirtualmachineSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antivirtualmachineSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antivirtualmachineSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antivirtualmachineSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antivirtualmachineSwitch.CheckedChanged += new System.EventHandler(this.antivirtualmachineSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel17
            // 
            this.guna2HtmlLabel17.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel17.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel17.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel17.IsContextMenuEnabled = false;
            this.guna2HtmlLabel17.IsSelectionEnabled = false;
            this.guna2HtmlLabel17.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel17.Name = "guna2HtmlLabel17";
            this.guna2HtmlLabel17.Size = new System.Drawing.Size(118, 18);
            this.guna2HtmlLabel17.TabIndex = 8;
            this.guna2HtmlLabel17.Text = "ANTI VIRTUAL MACHINE";
            // 
            // guna2VSeparator7
            // 
            this.guna2VSeparator7.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator7.FillThickness = 3;
            this.guna2VSeparator7.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator7.Name = "guna2VSeparator7";
            this.guna2VSeparator7.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator7.TabIndex = 9;
            // 
            // antidumpPanel
            // 
            this.antidumpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antidumpPanel.Controls.Add(this.antidumpSwitch);
            this.antidumpPanel.Controls.Add(this.guna2HtmlLabel16);
            this.antidumpPanel.Controls.Add(this.guna2VSeparator6);
            this.antidumpPanel.Location = new System.Drawing.Point(14, 154);
            this.antidumpPanel.Name = "antidumpPanel";
            this.antidumpPanel.Size = new System.Drawing.Size(667, 40);
            this.antidumpPanel.TabIndex = 11;
            this.antidumpPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antidumpPanel_MouseDown);
            // 
            // antidumpSwitch
            // 
            this.antidumpSwitch.Animated = true;
            this.antidumpSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidumpSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antidumpSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidumpSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antidumpSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antidumpSwitch.Location = new System.Drawing.Point(620, 10);
            this.antidumpSwitch.Name = "antidumpSwitch";
            this.antidumpSwitch.Size = new System.Drawing.Size(35, 20);
            this.antidumpSwitch.TabIndex = 10;
            this.antidumpSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidumpSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antidumpSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antidumpSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antidumpSwitch.CheckedChanged += new System.EventHandler(this.antidumpSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel16
            // 
            this.guna2HtmlLabel16.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel16.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel16.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel16.IsContextMenuEnabled = false;
            this.guna2HtmlLabel16.IsSelectionEnabled = false;
            this.guna2HtmlLabel16.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel16.Name = "guna2HtmlLabel16";
            this.guna2HtmlLabel16.Size = new System.Drawing.Size(58, 18);
            this.guna2HtmlLabel16.TabIndex = 8;
            this.guna2HtmlLabel16.Text = "ANTI DUMP";
            // 
            // guna2VSeparator6
            // 
            this.guna2VSeparator6.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator6.FillThickness = 3;
            this.guna2VSeparator6.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator6.Name = "guna2VSeparator6";
            this.guna2VSeparator6.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator6.TabIndex = 9;
            // 
            // antidebugPanel
            // 
            this.antidebugPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antidebugPanel.Controls.Add(this.antidebugSwitch);
            this.antidebugPanel.Controls.Add(this.guna2HtmlLabel15);
            this.antidebugPanel.Controls.Add(this.guna2VSeparator5);
            this.antidebugPanel.Location = new System.Drawing.Point(14, 108);
            this.antidebugPanel.Name = "antidebugPanel";
            this.antidebugPanel.Size = new System.Drawing.Size(667, 40);
            this.antidebugPanel.TabIndex = 12;
            this.antidebugPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antidebugPanel_MouseDown);
            // 
            // antidebugSwitch
            // 
            this.antidebugSwitch.Animated = true;
            this.antidebugSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidebugSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antidebugSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidebugSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antidebugSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antidebugSwitch.Location = new System.Drawing.Point(620, 10);
            this.antidebugSwitch.Name = "antidebugSwitch";
            this.antidebugSwitch.Size = new System.Drawing.Size(35, 20);
            this.antidebugSwitch.TabIndex = 10;
            this.antidebugSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidebugSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antidebugSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antidebugSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antidebugSwitch.CheckedChanged += new System.EventHandler(this.antidebugSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel15
            // 
            this.guna2HtmlLabel15.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel15.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel15.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel15.IsContextMenuEnabled = false;
            this.guna2HtmlLabel15.IsSelectionEnabled = false;
            this.guna2HtmlLabel15.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel15.Name = "guna2HtmlLabel15";
            this.guna2HtmlLabel15.Size = new System.Drawing.Size(62, 18);
            this.guna2HtmlLabel15.TabIndex = 8;
            this.guna2HtmlLabel15.Text = "ANTI DEBUG";
            // 
            // guna2VSeparator5
            // 
            this.guna2VSeparator5.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator5.FillThickness = 3;
            this.guna2VSeparator5.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator5.Name = "guna2VSeparator5";
            this.guna2VSeparator5.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator5.TabIndex = 9;
            // 
            // antidecPanel
            // 
            this.antidecPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.antidecPanel.Controls.Add(this.antidecompilerSwitch);
            this.antidecPanel.Controls.Add(this.guna2HtmlLabel14);
            this.antidecPanel.Controls.Add(this.guna2VSeparator4);
            this.antidecPanel.Location = new System.Drawing.Point(14, 62);
            this.antidecPanel.Name = "antidecPanel";
            this.antidecPanel.Size = new System.Drawing.Size(667, 40);
            this.antidecPanel.TabIndex = 11;
            this.antidecPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.antidecPanel_MouseDown);
            // 
            // antidecompilerSwitch
            // 
            this.antidecompilerSwitch.Animated = true;
            this.antidecompilerSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidecompilerSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.antidecompilerSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidecompilerSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.antidecompilerSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.antidecompilerSwitch.Location = new System.Drawing.Point(620, 10);
            this.antidecompilerSwitch.Name = "antidecompilerSwitch";
            this.antidecompilerSwitch.Size = new System.Drawing.Size(35, 20);
            this.antidecompilerSwitch.TabIndex = 10;
            this.antidecompilerSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.antidecompilerSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.antidecompilerSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.antidecompilerSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.antidecompilerSwitch.CheckedChanged += new System.EventHandler(this.antidecompilerSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel14
            // 
            this.guna2HtmlLabel14.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel14.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel14.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel14.IsContextMenuEnabled = false;
            this.guna2HtmlLabel14.IsSelectionEnabled = false;
            this.guna2HtmlLabel14.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel14.Name = "guna2HtmlLabel14";
            this.guna2HtmlLabel14.Size = new System.Drawing.Size(93, 18);
            this.guna2HtmlLabel14.TabIndex = 8;
            this.guna2HtmlLabel14.Text = "ANTI DECOMPILER";
            // 
            // guna2VSeparator4
            // 
            this.guna2VSeparator4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator4.FillThickness = 3;
            this.guna2VSeparator4.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator4.Name = "guna2VSeparator4";
            this.guna2VSeparator4.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator4.TabIndex = 9;
            // 
            // anticrackPanel
            // 
            this.anticrackPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.anticrackPanel.Controls.Add(this.anticrackSettings);
            this.anticrackPanel.Controls.Add(this.anticrackSwitch);
            this.anticrackPanel.Controls.Add(this.guna2HtmlLabel13);
            this.anticrackPanel.Controls.Add(this.guna2VSeparator3);
            this.anticrackPanel.Location = new System.Drawing.Point(14, 16);
            this.anticrackPanel.Name = "anticrackPanel";
            this.anticrackPanel.Size = new System.Drawing.Size(667, 40);
            this.anticrackPanel.TabIndex = 7;
            this.anticrackPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.anticrackPanel_MouseDown);
            // 
            // anticrackSettings
            // 
            this.anticrackSettings.BackColor = System.Drawing.Color.Transparent;
            this.anticrackSettings.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.anticrackSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anticrackSettings.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.anticrackSettings.Image = global::nigger2.anticrackSettings_Image;
            this.anticrackSettings.ImageOffset = new System.Drawing.Point(0, 0);
            this.anticrackSettings.ImageRotate = 0F;
            this.anticrackSettings.ImageSize = new System.Drawing.Size(20, 20);
            this.anticrackSettings.Location = new System.Drawing.Point(590, 8);
            this.anticrackSettings.Name = "anticrackSettings";
            this.anticrackSettings.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.anticrackSettings.Size = new System.Drawing.Size(24, 24);
            this.anticrackSettings.TabIndex = 10;
            this.anticrackSettings.Click += new System.EventHandler(this.anticrackSettings_Click);
            // 
            // anticrackSwitch
            // 
            this.anticrackSwitch.Animated = true;
            this.anticrackSwitch.BackColor = System.Drawing.Color.Transparent;
            this.anticrackSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.anticrackSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anticrackSwitch.Location = new System.Drawing.Point(620, 10);
            this.anticrackSwitch.Name = "anticrackSwitch";
            this.anticrackSwitch.Size = new System.Drawing.Size(35, 20);
            this.anticrackSwitch.TabIndex = 10;
            this.anticrackSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.anticrackSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.anticrackSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackSwitch.CheckedChanged += new System.EventHandler(this.anticrackSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel13
            // 
            this.guna2HtmlLabel13.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel13.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel13.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel13.IsContextMenuEnabled = false;
            this.guna2HtmlLabel13.IsSelectionEnabled = false;
            this.guna2HtmlLabel13.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel13.Name = "guna2HtmlLabel13";
            this.guna2HtmlLabel13.Size = new System.Drawing.Size(63, 18);
            this.guna2HtmlLabel13.TabIndex = 8;
            this.guna2HtmlLabel13.Text = "ANTI CRACK";
            // 
            // guna2VSeparator3
            // 
            this.guna2VSeparator3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator3.FillThickness = 3;
            this.guna2VSeparator3.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator3.Name = "guna2VSeparator3";
            this.guna2VSeparator3.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator3.TabIndex = 9;
            // 
            // anticrackPage
            // 
            this.anticrackPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.anticrackPage.Controls.Add(this.editProcList);
            this.anticrackPage.Controls.Add(this.customMsg);
            this.anticrackPage.Controls.Add(this.panel1);
            this.anticrackPage.Controls.Add(this.backfromACrack);
            this.anticrackPage.Controls.Add(this.excludeBox);
            this.anticrackPage.Controls.Add(this.webhookBox);
            this.anticrackPage.Controls.Add(this.panel6);
            this.anticrackPage.Controls.Add(this.panel5);
            this.anticrackPage.Controls.Add(this.panel4);
            this.anticrackPage.Controls.Add(this.guna2HtmlLabel11);
            this.anticrackPage.Controls.Add(this.guna2HtmlLabel28);
            this.anticrackPage.Location = new System.Drawing.Point(279, 39);
            this.anticrackPage.Name = "anticrackPage";
            this.anticrackPage.Size = new System.Drawing.Size(721, 481);
            this.anticrackPage.TabIndex = 10;
            // 
            // editProcList
            // 
            this.editProcList.BorderRadius = 6;
            this.editProcList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.editProcList.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.editProcList.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.editProcList.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.editProcList.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.editProcList.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.editProcList.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editProcList.ForeColor = System.Drawing.Color.White;
            this.editProcList.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.editProcList.HoverState.ForeColor = System.Drawing.Color.White;
            this.editProcList.Image = global::nigger1.edit_file_24px;
            this.editProcList.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.editProcList.ImageOffset = new System.Drawing.Point(10, 0);
            this.editProcList.ImageSize = new System.Drawing.Size(24, 24);
            this.editProcList.Location = new System.Drawing.Point(42, 400);
            this.editProcList.Name = "editProcList";
            this.editProcList.Size = new System.Drawing.Size(206, 44);
            this.editProcList.TabIndex = 17;
            this.editProcList.Text = "EDIT PROCESSES LIST";
            this.editProcList.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.editProcList.TextOffset = new System.Drawing.Point(14, 0);
            this.editProcList.Click += new System.EventHandler(this.editProcList_Click);
            // 
            // customMsg
            // 
            this.customMsg.AllowDrop = true;
            this.customMsg.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.customMsg.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.customMsg.DefaultText = "";
            this.customMsg.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.customMsg.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.customMsg.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.customMsg.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.customMsg.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.customMsg.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.customMsg.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.customMsg.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.customMsg.Location = new System.Drawing.Point(42, 352);
            this.customMsg.Name = "customMsg";
            this.customMsg.PasswordChar = '\0';
            this.customMsg.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.customMsg.PlaceholderText = "CUSTOM MESSAGE";
            this.customMsg.SelectedText = "";
            this.customMsg.Size = new System.Drawing.Size(410, 34);
            this.customMsg.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.customMsg.TabIndex = 16;
            this.customMsg.TextChanged += new System.EventHandler(this.customMsg_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel1.Controls.Add(this.bsodSwitch);
            this.panel1.Controls.Add(this.guna2HtmlLabel52);
            this.panel1.Controls.Add(this.guna2VSeparator1);
            this.panel1.Location = new System.Drawing.Point(42, 166);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 40);
            this.panel1.TabIndex = 12;
            // 
            // bsodSwitch
            // 
            this.bsodSwitch.Animated = true;
            this.bsodSwitch.BackColor = System.Drawing.Color.Transparent;
            this.bsodSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.bsodSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.bsodSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.bsodSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.bsodSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bsodSwitch.Location = new System.Drawing.Point(581, 10);
            this.bsodSwitch.Name = "bsodSwitch";
            this.bsodSwitch.Size = new System.Drawing.Size(35, 20);
            this.bsodSwitch.TabIndex = 10;
            this.bsodSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.bsodSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.bsodSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.bsodSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.bsodSwitch.CheckedChanged += new System.EventHandler(this.bsodSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel52
            // 
            this.guna2HtmlLabel52.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel52.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel52.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel52.IsContextMenuEnabled = false;
            this.guna2HtmlLabel52.IsSelectionEnabled = false;
            this.guna2HtmlLabel52.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel52.Name = "guna2HtmlLabel52";
            this.guna2HtmlLabel52.Size = new System.Drawing.Size(123, 18);
            this.guna2HtmlLabel52.TabIndex = 8;
            this.guna2HtmlLabel52.Text = "BLUE SCREEN OF DEATH";
            // 
            // guna2VSeparator1
            // 
            this.guna2VSeparator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator1.FillThickness = 3;
            this.guna2VSeparator1.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator1.Name = "guna2VSeparator1";
            this.guna2VSeparator1.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator1.TabIndex = 9;
            // 
            // backfromACrack
            // 
            this.backfromACrack.BackColor = System.Drawing.Color.Transparent;
            this.backfromACrack.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromACrack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromACrack.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromACrack.Image = global::nigger1.back_to_64px;
            this.backfromACrack.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromACrack.ImageRotate = 0F;
            this.backfromACrack.Location = new System.Drawing.Point(635, 395);
            this.backfromACrack.Name = "backfromACrack";
            this.backfromACrack.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromACrack.Size = new System.Drawing.Size(64, 64);
            this.backfromACrack.TabIndex = 15;
            this.backfromACrack.Click += new System.EventHandler(this.backfromACrack_Click);
            // 
            // excludeBox
            // 
            this.excludeBox.AllowDrop = true;
            this.excludeBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.excludeBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.excludeBox.DefaultText = "";
            this.excludeBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.excludeBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.excludeBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.excludeBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.excludeBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.excludeBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.excludeBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.excludeBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.excludeBox.Location = new System.Drawing.Point(42, 308);
            this.excludeBox.Name = "excludeBox";
            this.excludeBox.PasswordChar = '\0';
            this.excludeBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.excludeBox.PlaceholderText = "EXCLUDE PROCESS BY NAME";
            this.excludeBox.SelectedText = "";
            this.excludeBox.Size = new System.Drawing.Size(410, 34);
            this.excludeBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.excludeBox.TabIndex = 14;
            this.excludeBox.TextChanged += new System.EventHandler(this.excludeBox_TextChanged);
            // 
            // webhookBox
            // 
            this.webhookBox.AllowDrop = true;
            this.webhookBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.webhookBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.webhookBox.DefaultText = "";
            this.webhookBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.webhookBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.webhookBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.webhookBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.webhookBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.webhookBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.webhookBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.webhookBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.webhookBox.Location = new System.Drawing.Point(42, 264);
            this.webhookBox.Name = "webhookBox";
            this.webhookBox.PasswordChar = '\0';
            this.webhookBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.webhookBox.PlaceholderText = "DISCORD WEBHOOK TO RECIEVE CRACKER DATA";
            this.webhookBox.SelectedText = "";
            this.webhookBox.Size = new System.Drawing.Size(410, 34);
            this.webhookBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.webhookBox.TabIndex = 13;
            this.webhookBox.TextChanged += new System.EventHandler(this.webhookBox_TextChanged);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel6.Controls.Add(this.anticrackExclude);
            this.panel6.Controls.Add(this.guna2HtmlLabel26);
            this.panel6.Controls.Add(this.guna2VSeparator17);
            this.panel6.Location = new System.Drawing.Point(42, 212);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(627, 40);
            this.panel6.TabIndex = 12;
            // 
            // anticrackExclude
            // 
            this.anticrackExclude.Animated = true;
            this.anticrackExclude.BackColor = System.Drawing.Color.Transparent;
            this.anticrackExclude.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackExclude.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.anticrackExclude.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackExclude.CheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackExclude.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anticrackExclude.Location = new System.Drawing.Point(581, 10);
            this.anticrackExclude.Name = "anticrackExclude";
            this.anticrackExclude.Size = new System.Drawing.Size(35, 20);
            this.anticrackExclude.TabIndex = 10;
            this.anticrackExclude.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackExclude.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.anticrackExclude.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.anticrackExclude.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackExclude.CheckedChanged += new System.EventHandler(this.anticrackExclude_CheckedChanged);
            // 
            // guna2HtmlLabel26
            // 
            this.guna2HtmlLabel26.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel26.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel26.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel26.IsContextMenuEnabled = false;
            this.guna2HtmlLabel26.IsSelectionEnabled = false;
            this.guna2HtmlLabel26.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel26.Name = "guna2HtmlLabel26";
            this.guna2HtmlLabel26.Size = new System.Drawing.Size(99, 18);
            this.guna2HtmlLabel26.TabIndex = 8;
            this.guna2HtmlLabel26.Text = "EXCLUDE PROCESS";
            // 
            // guna2VSeparator17
            // 
            this.guna2VSeparator17.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator17.FillThickness = 3;
            this.guna2VSeparator17.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator17.Name = "guna2VSeparator17";
            this.guna2VSeparator17.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator17.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel5.Controls.Add(this.anticrackSilentMsg);
            this.panel5.Controls.Add(this.guna2HtmlLabel25);
            this.panel5.Controls.Add(this.guna2VSeparator16);
            this.panel5.Location = new System.Drawing.Point(42, 120);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(627, 40);
            this.panel5.TabIndex = 11;
            // 
            // anticrackSilentMsg
            // 
            this.anticrackSilentMsg.Animated = true;
            this.anticrackSilentMsg.BackColor = System.Drawing.Color.Transparent;
            this.anticrackSilentMsg.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSilentMsg.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.anticrackSilentMsg.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSilentMsg.CheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackSilentMsg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anticrackSilentMsg.Location = new System.Drawing.Point(581, 10);
            this.anticrackSilentMsg.Name = "anticrackSilentMsg";
            this.anticrackSilentMsg.Size = new System.Drawing.Size(35, 20);
            this.anticrackSilentMsg.TabIndex = 10;
            this.anticrackSilentMsg.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackSilentMsg.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.anticrackSilentMsg.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.anticrackSilentMsg.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackSilentMsg.CheckedChanged += new System.EventHandler(this.anticrackSilentMsg_CheckedChanged);
            // 
            // guna2HtmlLabel25
            // 
            this.guna2HtmlLabel25.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel25.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel25.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel25.IsContextMenuEnabled = false;
            this.guna2HtmlLabel25.IsSelectionEnabled = false;
            this.guna2HtmlLabel25.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel25.Name = "guna2HtmlLabel25";
            this.guna2HtmlLabel25.Size = new System.Drawing.Size(183, 18);
            this.guna2HtmlLabel25.TabIndex = 8;
            this.guna2HtmlLabel25.Text = "SILENTLY CLOSE WITHOUT MESSAGE";
            // 
            // guna2VSeparator16
            // 
            this.guna2VSeparator16.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator16.FillThickness = 3;
            this.guna2VSeparator16.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator16.Name = "guna2VSeparator16";
            this.guna2VSeparator16.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator16.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel4.Controls.Add(this.anticrackNormalMode);
            this.panel4.Controls.Add(this.guna2HtmlLabel12);
            this.panel4.Controls.Add(this.guna2VSeparator15);
            this.panel4.Location = new System.Drawing.Point(42, 74);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(627, 40);
            this.panel4.TabIndex = 8;
            // 
            // anticrackNormalMode
            // 
            this.anticrackNormalMode.Animated = true;
            this.anticrackNormalMode.BackColor = System.Drawing.Color.Transparent;
            this.anticrackNormalMode.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackNormalMode.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.anticrackNormalMode.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackNormalMode.CheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackNormalMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anticrackNormalMode.Location = new System.Drawing.Point(581, 10);
            this.anticrackNormalMode.Name = "anticrackNormalMode";
            this.anticrackNormalMode.Size = new System.Drawing.Size(35, 20);
            this.anticrackNormalMode.TabIndex = 10;
            this.anticrackNormalMode.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.anticrackNormalMode.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.anticrackNormalMode.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.anticrackNormalMode.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.anticrackNormalMode.CheckedChanged += new System.EventHandler(this.anticrackNormalMode_CheckedChanged);
            // 
            // guna2HtmlLabel12
            // 
            this.guna2HtmlLabel12.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel12.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel12.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel12.IsContextMenuEnabled = false;
            this.guna2HtmlLabel12.IsSelectionEnabled = false;
            this.guna2HtmlLabel12.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel12.Name = "guna2HtmlLabel12";
            this.guna2HtmlLabel12.Size = new System.Drawing.Size(78, 18);
            this.guna2HtmlLabel12.TabIndex = 8;
            this.guna2HtmlLabel12.Text = "NORMAL MODE";
            // 
            // guna2VSeparator15
            // 
            this.guna2VSeparator15.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator15.FillThickness = 3;
            this.guna2VSeparator15.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator15.Name = "guna2VSeparator15";
            this.guna2VSeparator15.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator15.TabIndex = 9;
            // 
            // guna2HtmlLabel11
            // 
            this.guna2HtmlLabel11.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel11.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel11.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel11.IsContextMenuEnabled = false;
            this.guna2HtmlLabel11.IsSelectionEnabled = false;
            this.guna2HtmlLabel11.Location = new System.Drawing.Point(104, 38);
            this.guna2HtmlLabel11.Name = "guna2HtmlLabel11";
            this.guna2HtmlLabel11.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel11.TabIndex = 4;
            this.guna2HtmlLabel11.Text = "SETTINGS";
            // 
            // guna2HtmlLabel28
            // 
            this.guna2HtmlLabel28.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel28.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel28.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel28.IsContextMenuEnabled = false;
            this.guna2HtmlLabel28.IsSelectionEnabled = false;
            this.guna2HtmlLabel28.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel28.Name = "guna2HtmlLabel28";
            this.guna2HtmlLabel28.Size = new System.Drawing.Size(63, 18);
            this.guna2HtmlLabel28.TabIndex = 3;
            this.guna2HtmlLabel28.Text = "ANTI CRACK";
            // 
            // controlflowPage
            // 
            this.controlflowPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.controlflowPage.Controls.Add(this.backfromCflow);
            this.controlflowPage.Controls.Add(this.panel9);
            this.controlflowPage.Controls.Add(this.panel10);
            this.controlflowPage.Controls.Add(this.guna2HtmlLabel31);
            this.controlflowPage.Controls.Add(this.guna2HtmlLabel32);
            this.controlflowPage.Location = new System.Drawing.Point(279, 39);
            this.controlflowPage.Name = "controlflowPage";
            this.controlflowPage.Size = new System.Drawing.Size(721, 481);
            this.controlflowPage.TabIndex = 11;
            // 
            // backfromCflow
            // 
            this.backfromCflow.BackColor = System.Drawing.Color.Transparent;
            this.backfromCflow.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromCflow.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromCflow.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromCflow.Image = global::nigger1.back_to_64px;
            this.backfromCflow.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromCflow.ImageRotate = 0F;
            this.backfromCflow.Location = new System.Drawing.Point(635, 395);
            this.backfromCflow.Name = "backfromCflow";
            this.backfromCflow.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromCflow.Size = new System.Drawing.Size(64, 64);
            this.backfromCflow.TabIndex = 13;
            this.backfromCflow.Click += new System.EventHandler(this.backfromCflow_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel9.Controls.Add(this.performancecflowMode);
            this.panel9.Controls.Add(this.guna2HtmlLabel29);
            this.panel9.Controls.Add(this.guna2VSeparator19);
            this.panel9.Location = new System.Drawing.Point(42, 120);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(627, 40);
            this.panel9.TabIndex = 11;
            // 
            // performancecflowMode
            // 
            this.performancecflowMode.Animated = true;
            this.performancecflowMode.BackColor = System.Drawing.Color.Transparent;
            this.performancecflowMode.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancecflowMode.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.performancecflowMode.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancecflowMode.CheckedState.InnerColor = System.Drawing.Color.White;
            this.performancecflowMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.performancecflowMode.Location = new System.Drawing.Point(581, 10);
            this.performancecflowMode.Name = "performancecflowMode";
            this.performancecflowMode.Size = new System.Drawing.Size(35, 20);
            this.performancecflowMode.TabIndex = 10;
            this.performancecflowMode.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancecflowMode.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.performancecflowMode.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.performancecflowMode.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.performancecflowMode.CheckedChanged += new System.EventHandler(this.performancecflowMode_CheckedChanged);
            // 
            // guna2HtmlLabel29
            // 
            this.guna2HtmlLabel29.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel29.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel29.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel29.IsContextMenuEnabled = false;
            this.guna2HtmlLabel29.IsSelectionEnabled = false;
            this.guna2HtmlLabel29.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel29.Name = "guna2HtmlLabel29";
            this.guna2HtmlLabel29.Size = new System.Drawing.Size(111, 18);
            this.guna2HtmlLabel29.TabIndex = 8;
            this.guna2HtmlLabel29.Text = "PERFORMANCE MODE";
            // 
            // guna2VSeparator19
            // 
            this.guna2VSeparator19.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator19.FillThickness = 3;
            this.guna2VSeparator19.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator19.Name = "guna2VSeparator19";
            this.guna2VSeparator19.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator19.TabIndex = 9;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel10.Controls.Add(this.strongcflowMode);
            this.panel10.Controls.Add(this.guna2HtmlLabel30);
            this.panel10.Controls.Add(this.guna2VSeparator20);
            this.panel10.Location = new System.Drawing.Point(42, 74);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(627, 40);
            this.panel10.TabIndex = 8;
            // 
            // strongcflowMode
            // 
            this.strongcflowMode.Animated = true;
            this.strongcflowMode.BackColor = System.Drawing.Color.Transparent;
            this.strongcflowMode.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongcflowMode.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.strongcflowMode.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongcflowMode.CheckedState.InnerColor = System.Drawing.Color.White;
            this.strongcflowMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.strongcflowMode.Location = new System.Drawing.Point(581, 10);
            this.strongcflowMode.Name = "strongcflowMode";
            this.strongcflowMode.Size = new System.Drawing.Size(35, 20);
            this.strongcflowMode.TabIndex = 10;
            this.strongcflowMode.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongcflowMode.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.strongcflowMode.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.strongcflowMode.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.strongcflowMode.CheckedChanged += new System.EventHandler(this.strongcflowMode_CheckedChanged);
            // 
            // guna2HtmlLabel30
            // 
            this.guna2HtmlLabel30.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel30.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel30.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel30.IsContextMenuEnabled = false;
            this.guna2HtmlLabel30.IsSelectionEnabled = false;
            this.guna2HtmlLabel30.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel30.Name = "guna2HtmlLabel30";
            this.guna2HtmlLabel30.Size = new System.Drawing.Size(75, 18);
            this.guna2HtmlLabel30.TabIndex = 8;
            this.guna2HtmlLabel30.Text = "STRONG MODE";
            // 
            // guna2VSeparator20
            // 
            this.guna2VSeparator20.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator20.FillThickness = 3;
            this.guna2VSeparator20.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator20.Name = "guna2VSeparator20";
            this.guna2VSeparator20.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator20.TabIndex = 9;
            // 
            // guna2HtmlLabel31
            // 
            this.guna2HtmlLabel31.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel31.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel31.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel31.IsContextMenuEnabled = false;
            this.guna2HtmlLabel31.IsSelectionEnabled = false;
            this.guna2HtmlLabel31.Location = new System.Drawing.Point(122, 38);
            this.guna2HtmlLabel31.Name = "guna2HtmlLabel31";
            this.guna2HtmlLabel31.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel31.TabIndex = 4;
            this.guna2HtmlLabel31.Text = "SETTINGS";
            // 
            // guna2HtmlLabel32
            // 
            this.guna2HtmlLabel32.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel32.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel32.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel32.IsContextMenuEnabled = false;
            this.guna2HtmlLabel32.IsSelectionEnabled = false;
            this.guna2HtmlLabel32.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel32.Name = "guna2HtmlLabel32";
            this.guna2HtmlLabel32.Size = new System.Drawing.Size(80, 18);
            this.guna2HtmlLabel32.TabIndex = 3;
            this.guna2HtmlLabel32.Text = "CONTROL FLOW";
            // 
            // codemutationPage
            // 
            this.codemutationPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.codemutationPage.Controls.Add(this.backfromMutation);
            this.codemutationPage.Controls.Add(this.panel11);
            this.codemutationPage.Controls.Add(this.panel12);
            this.codemutationPage.Controls.Add(this.guna2HtmlLabel34);
            this.codemutationPage.Controls.Add(this.guna2HtmlLabel35);
            this.codemutationPage.Location = new System.Drawing.Point(279, 39);
            this.codemutationPage.Name = "codemutationPage";
            this.codemutationPage.Size = new System.Drawing.Size(721, 481);
            this.codemutationPage.TabIndex = 12;
            // 
            // backfromMutation
            // 
            this.backfromMutation.BackColor = System.Drawing.Color.Transparent;
            this.backfromMutation.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromMutation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromMutation.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromMutation.Image = global::nigger1.back_to_64px;
            this.backfromMutation.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromMutation.ImageRotate = 0F;
            this.backfromMutation.Location = new System.Drawing.Point(635, 395);
            this.backfromMutation.Name = "backfromMutation";
            this.backfromMutation.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromMutation.Size = new System.Drawing.Size(64, 64);
            this.backfromMutation.TabIndex = 12;
            this.backfromMutation.Click += new System.EventHandler(this.backfromMutation_Click);
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel11.Controls.Add(this.performancemutationMode);
            this.panel11.Controls.Add(this.guna2HtmlLabel27);
            this.panel11.Controls.Add(this.guna2VSeparator18);
            this.panel11.Location = new System.Drawing.Point(42, 120);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(627, 40);
            this.panel11.TabIndex = 11;
            // 
            // performancemutationMode
            // 
            this.performancemutationMode.Animated = true;
            this.performancemutationMode.BackColor = System.Drawing.Color.Transparent;
            this.performancemutationMode.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancemutationMode.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.performancemutationMode.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancemutationMode.CheckedState.InnerColor = System.Drawing.Color.White;
            this.performancemutationMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.performancemutationMode.Location = new System.Drawing.Point(581, 10);
            this.performancemutationMode.Name = "performancemutationMode";
            this.performancemutationMode.Size = new System.Drawing.Size(35, 20);
            this.performancemutationMode.TabIndex = 10;
            this.performancemutationMode.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.performancemutationMode.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.performancemutationMode.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.performancemutationMode.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.performancemutationMode.CheckedChanged += new System.EventHandler(this.performancemutationMode_CheckedChanged);
            // 
            // guna2HtmlLabel27
            // 
            this.guna2HtmlLabel27.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel27.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel27.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel27.IsContextMenuEnabled = false;
            this.guna2HtmlLabel27.IsSelectionEnabled = false;
            this.guna2HtmlLabel27.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel27.Name = "guna2HtmlLabel27";
            this.guna2HtmlLabel27.Size = new System.Drawing.Size(111, 18);
            this.guna2HtmlLabel27.TabIndex = 8;
            this.guna2HtmlLabel27.Text = "PERFORMANCE MODE";
            // 
            // guna2VSeparator18
            // 
            this.guna2VSeparator18.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator18.FillThickness = 3;
            this.guna2VSeparator18.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator18.Name = "guna2VSeparator18";
            this.guna2VSeparator18.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator18.TabIndex = 9;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel12.Controls.Add(this.strongmutationMode);
            this.panel12.Controls.Add(this.guna2HtmlLabel33);
            this.panel12.Controls.Add(this.guna2VSeparator21);
            this.panel12.Location = new System.Drawing.Point(42, 74);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(627, 40);
            this.panel12.TabIndex = 8;
            // 
            // strongmutationMode
            // 
            this.strongmutationMode.Animated = true;
            this.strongmutationMode.BackColor = System.Drawing.Color.Transparent;
            this.strongmutationMode.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongmutationMode.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.strongmutationMode.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongmutationMode.CheckedState.InnerColor = System.Drawing.Color.White;
            this.strongmutationMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.strongmutationMode.Location = new System.Drawing.Point(581, 10);
            this.strongmutationMode.Name = "strongmutationMode";
            this.strongmutationMode.Size = new System.Drawing.Size(35, 20);
            this.strongmutationMode.TabIndex = 10;
            this.strongmutationMode.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.strongmutationMode.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.strongmutationMode.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.strongmutationMode.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.strongmutationMode.CheckedChanged += new System.EventHandler(this.strongmutationMode_CheckedChanged);
            // 
            // guna2HtmlLabel33
            // 
            this.guna2HtmlLabel33.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel33.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel33.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel33.IsContextMenuEnabled = false;
            this.guna2HtmlLabel33.IsSelectionEnabled = false;
            this.guna2HtmlLabel33.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel33.Name = "guna2HtmlLabel33";
            this.guna2HtmlLabel33.Size = new System.Drawing.Size(75, 18);
            this.guna2HtmlLabel33.TabIndex = 8;
            this.guna2HtmlLabel33.Text = "STRONG MODE";
            // 
            // guna2VSeparator21
            // 
            this.guna2VSeparator21.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator21.FillThickness = 3;
            this.guna2VSeparator21.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator21.Name = "guna2VSeparator21";
            this.guna2VSeparator21.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator21.TabIndex = 9;
            // 
            // guna2HtmlLabel34
            // 
            this.guna2HtmlLabel34.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel34.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel34.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel34.IsContextMenuEnabled = false;
            this.guna2HtmlLabel34.IsSelectionEnabled = false;
            this.guna2HtmlLabel34.Location = new System.Drawing.Point(125, 38);
            this.guna2HtmlLabel34.Name = "guna2HtmlLabel34";
            this.guna2HtmlLabel34.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel34.TabIndex = 4;
            this.guna2HtmlLabel34.Text = "SETTINGS";
            // 
            // guna2HtmlLabel35
            // 
            this.guna2HtmlLabel35.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel35.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel35.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel35.IsContextMenuEnabled = false;
            this.guna2HtmlLabel35.IsSelectionEnabled = false;
            this.guna2HtmlLabel35.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel35.Name = "guna2HtmlLabel35";
            this.guna2HtmlLabel35.Size = new System.Drawing.Size(83, 18);
            this.guna2HtmlLabel35.TabIndex = 3;
            this.guna2HtmlLabel35.Text = "CODE MUTATION";
            // 
            // codeencPage
            // 
            this.codeencPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.codeencPage.Controls.Add(this.codeencPanel);
            this.codeencPage.Controls.Add(this.guna2HtmlLabel38);
            this.codeencPage.Controls.Add(this.guna2HtmlLabel39);
            this.codeencPage.Location = new System.Drawing.Point(279, 39);
            this.codeencPage.Name = "codeencPage";
            this.codeencPage.Size = new System.Drawing.Size(721, 481);
            this.codeencPage.TabIndex = 13;
            // 
            // codeencPanel
            // 
            this.codeencPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.codeencPanel.Controls.Add(this.codeencSwitch);
            this.codeencPanel.Controls.Add(this.guna2HtmlLabel37);
            this.codeencPanel.Controls.Add(this.guna2VSeparator23);
            this.codeencPanel.Location = new System.Drawing.Point(42, 74);
            this.codeencPanel.Name = "codeencPanel";
            this.codeencPanel.Size = new System.Drawing.Size(627, 40);
            this.codeencPanel.TabIndex = 8;
            this.codeencPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.codeencPanel_MouseDown);
            // 
            // codeencSwitch
            // 
            this.codeencSwitch.Animated = true;
            this.codeencSwitch.BackColor = System.Drawing.Color.Transparent;
            this.codeencSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codeencSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.codeencSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codeencSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.codeencSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.codeencSwitch.Location = new System.Drawing.Point(581, 10);
            this.codeencSwitch.Name = "codeencSwitch";
            this.codeencSwitch.Size = new System.Drawing.Size(35, 20);
            this.codeencSwitch.TabIndex = 10;
            this.codeencSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.codeencSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.codeencSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.codeencSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.codeencSwitch.CheckedChanged += new System.EventHandler(this.codeencSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel37
            // 
            this.guna2HtmlLabel37.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel37.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel37.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel37.IsContextMenuEnabled = false;
            this.guna2HtmlLabel37.IsSelectionEnabled = false;
            this.guna2HtmlLabel37.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel37.Name = "guna2HtmlLabel37";
            this.guna2HtmlLabel37.Size = new System.Drawing.Size(144, 18);
            this.guna2HtmlLabel37.TabIndex = 8;
            this.guna2HtmlLabel37.Text = "ENCRYPT & STORE METHODS";
            // 
            // guna2VSeparator23
            // 
            this.guna2VSeparator23.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator23.FillThickness = 3;
            this.guna2VSeparator23.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator23.Name = "guna2VSeparator23";
            this.guna2VSeparator23.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator23.TabIndex = 9;
            // 
            // guna2HtmlLabel38
            // 
            this.guna2HtmlLabel38.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel38.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel38.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel38.IsContextMenuEnabled = false;
            this.guna2HtmlLabel38.IsSelectionEnabled = false;
            this.guna2HtmlLabel38.Location = new System.Drawing.Point(137, 38);
            this.guna2HtmlLabel38.Name = "guna2HtmlLabel38";
            this.guna2HtmlLabel38.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel38.TabIndex = 4;
            this.guna2HtmlLabel38.Text = "SETTINGS";
            // 
            // guna2HtmlLabel39
            // 
            this.guna2HtmlLabel39.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel39.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel39.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel39.IsContextMenuEnabled = false;
            this.guna2HtmlLabel39.IsSelectionEnabled = false;
            this.guna2HtmlLabel39.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel39.Name = "guna2HtmlLabel39";
            this.guna2HtmlLabel39.Size = new System.Drawing.Size(95, 18);
            this.guna2HtmlLabel39.TabIndex = 3;
            this.guna2HtmlLabel39.Text = "CODE ENCRYPTION";
            // 
            // renamerPage
            // 
            this.renamerPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.renamerPage.Controls.Add(this.cfexPanel);
            this.renamerPage.Controls.Add(this.customRenamer);
            this.renamerPage.Controls.Add(this.customBox);
            this.renamerPage.Controls.Add(this.guna2HtmlLabel43);
            this.renamerPage.Controls.Add(this.renamingOptions);
            this.renamerPage.Controls.Add(this.guna2HtmlLabel42);
            this.renamerPage.Controls.Add(this.renamePanel);
            this.renamerPage.Controls.Add(this.guna2HtmlLabel40);
            this.renamerPage.Controls.Add(this.guna2HtmlLabel41);
            this.renamerPage.Location = new System.Drawing.Point(279, 39);
            this.renamerPage.Name = "renamerPage";
            this.renamerPage.Size = new System.Drawing.Size(721, 481);
            this.renamerPage.TabIndex = 14;
            // 
            // cfexPanel
            // 
            this.cfexPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.cfexPanel.Controls.Add(this.cfexRenamerSwitch);
            this.cfexPanel.Controls.Add(this.guna2HtmlLabel65);
            this.cfexPanel.Controls.Add(this.guna2VSeparator32);
            this.cfexPanel.Location = new System.Drawing.Point(42, 74);
            this.cfexPanel.Name = "cfexPanel";
            this.cfexPanel.Size = new System.Drawing.Size(627, 40);
            this.cfexPanel.TabIndex = 16;
            this.cfexPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cfexPanel_MouseDown);
            // 
            // cfexRenamerSwitch
            // 
            this.cfexRenamerSwitch.Animated = true;
            this.cfexRenamerSwitch.BackColor = System.Drawing.Color.Transparent;
            this.cfexRenamerSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cfexRenamerSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.cfexRenamerSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cfexRenamerSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.cfexRenamerSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cfexRenamerSwitch.Location = new System.Drawing.Point(581, 10);
            this.cfexRenamerSwitch.Name = "cfexRenamerSwitch";
            this.cfexRenamerSwitch.Size = new System.Drawing.Size(35, 20);
            this.cfexRenamerSwitch.TabIndex = 10;
            this.cfexRenamerSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cfexRenamerSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.cfexRenamerSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.cfexRenamerSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.cfexRenamerSwitch.CheckedChanged += new System.EventHandler(this.cfexRenamerSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel65
            // 
            this.guna2HtmlLabel65.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel65.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel65.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel65.IsContextMenuEnabled = false;
            this.guna2HtmlLabel65.IsSelectionEnabled = false;
            this.guna2HtmlLabel65.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel65.Name = "guna2HtmlLabel65";
            this.guna2HtmlLabel65.Size = new System.Drawing.Size(559, 18);
            this.guna2HtmlLabel65.TabIndex = 8;
            this.guna2HtmlLabel65.Text = "CONFUSEREX RENAMER, MAKE SURE DLLS WITH EXE IN SAME PATH TO RESOLVE!, NOT WORKING" +
    " WITH CODE VIRT";
            // 
            // guna2VSeparator32
            // 
            this.guna2VSeparator32.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator32.FillThickness = 3;
            this.guna2VSeparator32.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator32.Name = "guna2VSeparator32";
            this.guna2VSeparator32.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator32.TabIndex = 9;
            // 
            // customRenamer
            // 
            this.customRenamer.Animated = true;
            this.customRenamer.BackColor = System.Drawing.Color.Transparent;
            this.customRenamer.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.customRenamer.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.customRenamer.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.customRenamer.CheckedState.InnerColor = System.Drawing.Color.White;
            this.customRenamer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.customRenamer.Location = new System.Drawing.Point(417, 343);
            this.customRenamer.Name = "customRenamer";
            this.customRenamer.Size = new System.Drawing.Size(35, 20);
            this.customRenamer.TabIndex = 15;
            this.customRenamer.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.customRenamer.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.customRenamer.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.customRenamer.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.customRenamer.CheckedChanged += new System.EventHandler(this.customRenamer_CheckedChanged);
            // 
            // customBox
            // 
            this.customBox.AllowDrop = true;
            this.customBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.customBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.customBox.DefaultText = "";
            this.customBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.customBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.customBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.customBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.customBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.customBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.customBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.customBox.ForeColor = System.Drawing.Color.White;
            this.customBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.customBox.Location = new System.Drawing.Point(42, 366);
            this.customBox.Name = "customBox";
            this.customBox.PasswordChar = '\0';
            this.customBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.customBox.PlaceholderText = "ADD SIGNATURE HERE";
            this.customBox.SelectedText = "";
            this.customBox.Size = new System.Drawing.Size(410, 34);
            this.customBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.customBox.TabIndex = 14;
            this.customBox.TextChanged += new System.EventHandler(this.customBox_TextChanged);
            // 
            // guna2HtmlLabel43
            // 
            this.guna2HtmlLabel43.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel43.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel43.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel43.IsContextMenuEnabled = false;
            this.guna2HtmlLabel43.IsSelectionEnabled = false;
            this.guna2HtmlLabel43.Location = new System.Drawing.Point(42, 342);
            this.guna2HtmlLabel43.Name = "guna2HtmlLabel43";
            this.guna2HtmlLabel43.Size = new System.Drawing.Size(101, 18);
            this.guna2HtmlLabel43.TabIndex = 11;
            this.guna2HtmlLabel43.Text = "CUSTOM RENAMING";
            // 
            // renamingOptions
            // 
            this.renamingOptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.renamingOptions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.renamingOptions.CheckOnClick = true;
            this.renamingOptions.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renamingOptions.ForeColor = System.Drawing.Color.White;
            this.renamingOptions.FormattingEnabled = true;
            this.renamingOptions.Items.AddRange(new object[] {
            "EVENTS",
            "FIELDS",
            "METHODS",
            "PARAMETERS",
            "PROPERTIES",
            "TYPES"});
            this.renamingOptions.Location = new System.Drawing.Point(42, 213);
            this.renamingOptions.Name = "renamingOptions";
            this.renamingOptions.Size = new System.Drawing.Size(206, 119);
            this.renamingOptions.TabIndex = 10;
            this.renamingOptions.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.renamingOptions_ItemCheck);
            // 
            // guna2HtmlLabel42
            // 
            this.guna2HtmlLabel42.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel42.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel42.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel42.IsContextMenuEnabled = false;
            this.guna2HtmlLabel42.IsSelectionEnabled = false;
            this.guna2HtmlLabel42.Location = new System.Drawing.Point(42, 183);
            this.guna2HtmlLabel42.Name = "guna2HtmlLabel42";
            this.guna2HtmlLabel42.Size = new System.Drawing.Size(46, 18);
            this.guna2HtmlLabel42.TabIndex = 9;
            this.guna2HtmlLabel42.Text = "OPTIONS";
            // 
            // renamePanel
            // 
            this.renamePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.renamePanel.Controls.Add(this.renamerSwitch);
            this.renamePanel.Controls.Add(this.guna2HtmlLabel36);
            this.renamePanel.Controls.Add(this.guna2VSeparator22);
            this.renamePanel.Location = new System.Drawing.Point(42, 120);
            this.renamePanel.Name = "renamePanel";
            this.renamePanel.Size = new System.Drawing.Size(627, 40);
            this.renamePanel.TabIndex = 8;
            this.renamePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.renamePanel_MouseDown);
            // 
            // renamerSwitch
            // 
            this.renamerSwitch.Animated = true;
            this.renamerSwitch.BackColor = System.Drawing.Color.Transparent;
            this.renamerSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.renamerSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.renamerSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.renamerSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.renamerSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.renamerSwitch.Location = new System.Drawing.Point(581, 10);
            this.renamerSwitch.Name = "renamerSwitch";
            this.renamerSwitch.Size = new System.Drawing.Size(35, 20);
            this.renamerSwitch.TabIndex = 10;
            this.renamerSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.renamerSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.renamerSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.renamerSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.renamerSwitch.CheckedChanged += new System.EventHandler(this.renamerSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel36
            // 
            this.guna2HtmlLabel36.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel36.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel36.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel36.IsContextMenuEnabled = false;
            this.guna2HtmlLabel36.IsSelectionEnabled = false;
            this.guna2HtmlLabel36.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel36.Name = "guna2HtmlLabel36";
            this.guna2HtmlLabel36.Size = new System.Drawing.Size(129, 18);
            this.guna2HtmlLabel36.TabIndex = 8;
            this.guna2HtmlLabel36.Text = "RENAMING OBFUSCATION";
            // 
            // guna2VSeparator22
            // 
            this.guna2VSeparator22.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator22.FillThickness = 3;
            this.guna2VSeparator22.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator22.Name = "guna2VSeparator22";
            this.guna2VSeparator22.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator22.TabIndex = 9;
            // 
            // guna2HtmlLabel40
            // 
            this.guna2HtmlLabel40.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel40.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel40.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel40.IsContextMenuEnabled = false;
            this.guna2HtmlLabel40.IsSelectionEnabled = false;
            this.guna2HtmlLabel40.Location = new System.Drawing.Point(95, 38);
            this.guna2HtmlLabel40.Name = "guna2HtmlLabel40";
            this.guna2HtmlLabel40.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel40.TabIndex = 4;
            this.guna2HtmlLabel40.Text = "SETTINGS";
            // 
            // guna2HtmlLabel41
            // 
            this.guna2HtmlLabel41.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel41.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel41.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel41.IsContextMenuEnabled = false;
            this.guna2HtmlLabel41.IsSelectionEnabled = false;
            this.guna2HtmlLabel41.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel41.Name = "guna2HtmlLabel41";
            this.guna2HtmlLabel41.Size = new System.Drawing.Size(52, 18);
            this.guna2HtmlLabel41.TabIndex = 3;
            this.guna2HtmlLabel41.Text = "RENAMER";
            // 
            // virtualizationPage
            // 
            this.virtualizationPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.virtualizationPage.Controls.Add(this.addAllMethods);
            this.virtualizationPage.Controls.Add(this.methodsList);
            this.virtualizationPage.Controls.Add(this.searchMethod);
            this.virtualizationPage.Controls.Add(this.methodBox);
            this.virtualizationPage.Controls.Add(this.guna2HtmlLabel44);
            this.virtualizationPage.Controls.Add(this.virtPanel);
            this.virtualizationPage.Controls.Add(this.guna2HtmlLabel47);
            this.virtualizationPage.Controls.Add(this.guna2HtmlLabel48);
            this.virtualizationPage.Location = new System.Drawing.Point(279, 39);
            this.virtualizationPage.Name = "virtualizationPage";
            this.virtualizationPage.Size = new System.Drawing.Size(721, 481);
            this.virtualizationPage.TabIndex = 15;
            // 
            // addAllMethods
            // 
            this.addAllMethods.BackColor = System.Drawing.Color.Transparent;
            this.addAllMethods.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.addAllMethods.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addAllMethods.HoverState.ImageSize = new System.Drawing.Size(17, 17);
            this.addAllMethods.Image = global::nigger1.uncheck_all_18px;
            this.addAllMethods.ImageOffset = new System.Drawing.Point(0, 0);
            this.addAllMethods.ImageRotate = 0F;
            this.addAllMethods.ImageSize = new System.Drawing.Size(18, 18);
            this.addAllMethods.Location = new System.Drawing.Point(639, 393);
            this.addAllMethods.Name = "addAllMethods";
            this.addAllMethods.PressedState.ImageSize = new System.Drawing.Size(16, 16);
            this.addAllMethods.Size = new System.Drawing.Size(30, 40);
            this.addAllMethods.TabIndex = 17;
            this.addAllMethods.Click += new System.EventHandler(this.addAllMethods_Click);
            // 
            // methodsList
            // 
            this.methodsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.methodsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.methodsList.ContextMenuStrip = this.guna2ContextMenuStrip1;
            this.methodsList.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.methodsList.ForeColor = System.Drawing.Color.White;
            this.methodsList.ImageIndex = 0;
            this.methodsList.ImageList = this.imageList1;
            this.methodsList.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.methodsList.Location = new System.Drawing.Point(42, 125);
            this.methodsList.Name = "methodsList";
            this.methodsList.SelectedImageIndex = 0;
            this.methodsList.Size = new System.Drawing.Size(627, 265);
            this.methodsList.StateImageList = this.imageList1;
            this.methodsList.TabIndex = 16;
            this.methodsList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.methodsList_NodeMouseClick);
            this.methodsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.methodsList_MouseClick);
            // 
            // guna2ContextMenuStrip1
            // 
            this.guna2ContextMenuStrip1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aDDMETHODToolStripMenuItem,
            this.rEMOVEMETHODToolStripMenuItem});
            this.guna2ContextMenuStrip1.Name = "guna2ContextMenuStrip1";
            this.guna2ContextMenuStrip1.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.guna2ContextMenuStrip1.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2ContextMenuStrip1.RenderStyle.ColorTable = null;
            this.guna2ContextMenuStrip1.RenderStyle.RoundedEdges = true;
            this.guna2ContextMenuStrip1.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
            this.guna2ContextMenuStrip1.RenderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.guna2ContextMenuStrip1.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.guna2ContextMenuStrip1.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
            this.guna2ContextMenuStrip1.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.guna2ContextMenuStrip1.Size = new System.Drawing.Size(158, 48);
            this.guna2ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.guna2ContextMenuStrip1_Opening);
            // 
            // aDDMETHODToolStripMenuItem
            // 
            this.aDDMETHODToolStripMenuItem.Name = "aDDMETHODToolStripMenuItem";
            this.aDDMETHODToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.aDDMETHODToolStripMenuItem.Text = "ADD METHOD";
            this.aDDMETHODToolStripMenuItem.Click += new System.EventHandler(this.aDDMETHODToolStripMenuItem_Click);
            // 
            // rEMOVEMETHODToolStripMenuItem
            // 
            this.rEMOVEMETHODToolStripMenuItem.Name = "rEMOVEMETHODToolStripMenuItem";
            this.rEMOVEMETHODToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.rEMOVEMETHODToolStripMenuItem.Text = "REMOVE METHOD";
            this.rEMOVEMETHODToolStripMenuItem.Click += new System.EventHandler(this.rEMOVEMETHODToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // searchMethod
            // 
            this.searchMethod.BackColor = System.Drawing.Color.Transparent;
            this.searchMethod.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.searchMethod.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchMethod.HoverState.ImageSize = new System.Drawing.Size(23, 23);
            this.searchMethod.Image = global::nigger1.google_web_search_24px;
            this.searchMethod.ImageOffset = new System.Drawing.Point(0, 0);
            this.searchMethod.ImageRotate = 0F;
            this.searchMethod.ImageSize = new System.Drawing.Size(24, 24);
            this.searchMethod.Location = new System.Drawing.Point(463, 426);
            this.searchMethod.Name = "searchMethod";
            this.searchMethod.PressedState.ImageSize = new System.Drawing.Size(22, 22);
            this.searchMethod.Size = new System.Drawing.Size(30, 40);
            this.searchMethod.TabIndex = 15;
            this.searchMethod.Click += new System.EventHandler(this.searchMethod_Click);
            // 
            // methodBox
            // 
            this.methodBox.AllowDrop = true;
            this.methodBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.methodBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.methodBox.DefaultText = "";
            this.methodBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.methodBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.methodBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.methodBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.methodBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.methodBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.methodBox.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.methodBox.ForeColor = System.Drawing.Color.White;
            this.methodBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.methodBox.Location = new System.Drawing.Point(42, 426);
            this.methodBox.Name = "methodBox";
            this.methodBox.PasswordChar = '\0';
            this.methodBox.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.methodBox.PlaceholderText = "EXAMPLE: MAIN";
            this.methodBox.SelectedText = "";
            this.methodBox.Size = new System.Drawing.Size(410, 34);
            this.methodBox.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.methodBox.TabIndex = 14;
            // 
            // guna2HtmlLabel44
            // 
            this.guna2HtmlLabel44.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel44.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel44.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel44.IsContextMenuEnabled = false;
            this.guna2HtmlLabel44.IsSelectionEnabled = false;
            this.guna2HtmlLabel44.Location = new System.Drawing.Point(42, 402);
            this.guna2HtmlLabel44.Name = "guna2HtmlLabel44";
            this.guna2HtmlLabel44.Size = new System.Drawing.Size(111, 18);
            this.guna2HtmlLabel44.TabIndex = 11;
            this.guna2HtmlLabel44.Text = "SEARCH FOR METHOD";
            // 
            // virtPanel
            // 
            this.virtPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.virtPanel.Controls.Add(this.virtSwitch);
            this.virtPanel.Controls.Add(this.guna2HtmlLabel46);
            this.virtPanel.Controls.Add(this.guna2VSeparator24);
            this.virtPanel.Location = new System.Drawing.Point(42, 74);
            this.virtPanel.Name = "virtPanel";
            this.virtPanel.Size = new System.Drawing.Size(627, 40);
            this.virtPanel.TabIndex = 8;
            this.virtPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.virtPanel_MouseDown);
            // 
            // virtSwitch
            // 
            this.virtSwitch.Animated = true;
            this.virtSwitch.BackColor = System.Drawing.Color.Transparent;
            this.virtSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.virtSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.virtSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.virtSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.virtSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.virtSwitch.Location = new System.Drawing.Point(581, 10);
            this.virtSwitch.Name = "virtSwitch";
            this.virtSwitch.Size = new System.Drawing.Size(35, 20);
            this.virtSwitch.TabIndex = 10;
            this.virtSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.virtSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.virtSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.virtSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.virtSwitch.CheckedChanged += new System.EventHandler(this.virtSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel46
            // 
            this.guna2HtmlLabel46.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel46.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel46.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel46.IsContextMenuEnabled = false;
            this.guna2HtmlLabel46.IsSelectionEnabled = false;
            this.guna2HtmlLabel46.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel46.Name = "guna2HtmlLabel46";
            this.guna2HtmlLabel46.Size = new System.Drawing.Size(110, 18);
            this.guna2HtmlLabel46.TabIndex = 8;
            this.guna2HtmlLabel46.Text = "VIRTUALIZE METHODS";
            // 
            // guna2VSeparator24
            // 
            this.guna2VSeparator24.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator24.FillThickness = 3;
            this.guna2VSeparator24.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator24.Name = "guna2VSeparator24";
            this.guna2VSeparator24.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator24.TabIndex = 9;
            // 
            // guna2HtmlLabel47
            // 
            this.guna2HtmlLabel47.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel47.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel47.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel47.IsContextMenuEnabled = false;
            this.guna2HtmlLabel47.IsSelectionEnabled = false;
            this.guna2HtmlLabel47.Location = new System.Drawing.Point(154, 38);
            this.guna2HtmlLabel47.Name = "guna2HtmlLabel47";
            this.guna2HtmlLabel47.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel47.TabIndex = 4;
            this.guna2HtmlLabel47.Text = "SETTINGS";
            // 
            // guna2HtmlLabel48
            // 
            this.guna2HtmlLabel48.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel48.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel48.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel48.IsContextMenuEnabled = false;
            this.guna2HtmlLabel48.IsSelectionEnabled = false;
            this.guna2HtmlLabel48.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel48.Name = "guna2HtmlLabel48";
            this.guna2HtmlLabel48.Size = new System.Drawing.Size(112, 18);
            this.guna2HtmlLabel48.TabIndex = 3;
            this.guna2HtmlLabel48.Text = "CODE VIRTUALIZATION";
            // 
            // projectPage
            // 
            this.projectPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.projectPage.Controls.Add(this.backfromProject);
            this.projectPage.Controls.Add(this.saveProject);
            this.projectPage.Controls.Add(this.loadProject);
            this.projectPage.Controls.Add(this.guna2HtmlLabel50);
            this.projectPage.Controls.Add(this.guna2HtmlLabel51);
            this.projectPage.Location = new System.Drawing.Point(279, 39);
            this.projectPage.Name = "projectPage";
            this.projectPage.Size = new System.Drawing.Size(721, 481);
            this.projectPage.TabIndex = 16;
            // 
            // backfromProject
            // 
            this.backfromProject.BackColor = System.Drawing.Color.Transparent;
            this.backfromProject.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromProject.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromProject.Image = global::nigger1.back_to_64px;
            this.backfromProject.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromProject.ImageRotate = 0F;
            this.backfromProject.Location = new System.Drawing.Point(635, 395);
            this.backfromProject.Name = "backfromProject";
            this.backfromProject.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromProject.Size = new System.Drawing.Size(64, 64);
            this.backfromProject.TabIndex = 13;
            this.backfromProject.Click += new System.EventHandler(this.backfromProject_Click);
            // 
            // saveProject
            // 
            this.saveProject.BorderRadius = 6;
            this.saveProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveProject.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.saveProject.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.saveProject.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.saveProject.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.saveProject.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.saveProject.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveProject.ForeColor = System.Drawing.Color.White;
            this.saveProject.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.saveProject.HoverState.ForeColor = System.Drawing.Color.White;
            this.saveProject.Image = global::nigger1.save_24px;
            this.saveProject.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.saveProject.ImageOffset = new System.Drawing.Point(10, 0);
            this.saveProject.ImageSize = new System.Drawing.Size(24, 24);
            this.saveProject.Location = new System.Drawing.Point(42, 123);
            this.saveProject.Name = "saveProject";
            this.saveProject.Size = new System.Drawing.Size(206, 44);
            this.saveProject.TabIndex = 10;
            this.saveProject.Text = "SAVE PROJECT";
            this.saveProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.saveProject.TextOffset = new System.Drawing.Point(14, 0);
            this.saveProject.Click += new System.EventHandler(this.saveProject_Click);
            // 
            // loadProject
            // 
            this.loadProject.BorderRadius = 6;
            this.loadProject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loadProject.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.loadProject.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.loadProject.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.loadProject.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.loadProject.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.loadProject.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadProject.ForeColor = System.Drawing.Color.White;
            this.loadProject.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.loadProject.HoverState.ForeColor = System.Drawing.Color.White;
            this.loadProject.Image = global::nigger1.import_file_24px;
            this.loadProject.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.loadProject.ImageOffset = new System.Drawing.Point(10, 0);
            this.loadProject.ImageSize = new System.Drawing.Size(24, 24);
            this.loadProject.Location = new System.Drawing.Point(42, 74);
            this.loadProject.Name = "loadProject";
            this.loadProject.Size = new System.Drawing.Size(206, 44);
            this.loadProject.TabIndex = 9;
            this.loadProject.Text = "LOAD PROJECT";
            this.loadProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.loadProject.TextOffset = new System.Drawing.Point(14, 0);
            this.loadProject.Click += new System.EventHandler(this.loadProject_Click);
            // 
            // guna2HtmlLabel50
            // 
            this.guna2HtmlLabel50.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel50.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel50.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel50.IsContextMenuEnabled = false;
            this.guna2HtmlLabel50.IsSelectionEnabled = false;
            this.guna2HtmlLabel50.Location = new System.Drawing.Point(91, 38);
            this.guna2HtmlLabel50.Name = "guna2HtmlLabel50";
            this.guna2HtmlLabel50.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel50.TabIndex = 4;
            this.guna2HtmlLabel50.Text = "SETTINGS";
            // 
            // guna2HtmlLabel51
            // 
            this.guna2HtmlLabel51.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel51.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel51.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel51.IsContextMenuEnabled = false;
            this.guna2HtmlLabel51.IsSelectionEnabled = false;
            this.guna2HtmlLabel51.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel51.Name = "guna2HtmlLabel51";
            this.guna2HtmlLabel51.Size = new System.Drawing.Size(48, 18);
            this.guna2HtmlLabel51.TabIndex = 3;
            this.guna2HtmlLabel51.Text = "PROJECT";
            // 
            // colorsPage
            // 
            this.colorsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.colorsPage.Controls.Add(this.guna2ImageButton_0);
            this.colorsPage.Controls.Add(this.setinRGB);
            this.colorsPage.Controls.Add(this.bVal);
            this.colorsPage.Controls.Add(this.gVal);
            this.colorsPage.Controls.Add(this.rVal);
            this.colorsPage.Controls.Add(this.guna2HtmlLabel2);
            this.colorsPage.Location = new System.Drawing.Point(279, 39);
            this.colorsPage.Name = "colorsPage";
            this.colorsPage.Size = new System.Drawing.Size(721, 481);
            this.colorsPage.TabIndex = 17;
            // 
            // guna2ImageButton_0
            // 
            this.guna2ImageButton_0.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton_0.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton_0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.guna2ImageButton_0.HoverState.ImageSize = new System.Drawing.Size(17, 17);
            this.guna2ImageButton_0.Image = global::nigger1.shuffle_18px;
            this.guna2ImageButton_0.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton_0.ImageRotate = 0F;
            this.guna2ImageButton_0.ImageSize = new System.Drawing.Size(18, 18);
            this.guna2ImageButton_0.Location = new System.Drawing.Point(220, 70);
            this.guna2ImageButton_0.Name = "guna2ImageButton_0";
            this.guna2ImageButton_0.PressedState.ImageSize = new System.Drawing.Size(16, 16);
            this.guna2ImageButton_0.Size = new System.Drawing.Size(30, 30);
            this.guna2ImageButton_0.TabIndex = 21;
            this.guna2ImageButton_0.Click += new System.EventHandler(this.randomRGB_Click);
            // 
            // setinRGB
            // 
            this.setinRGB.BackColor = System.Drawing.Color.Transparent;
            this.setinRGB.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.setinRGB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.setinRGB.HoverState.ImageSize = new System.Drawing.Size(17, 17);
            this.setinRGB.Image = global::nigger1.fill_color_18px;
            this.setinRGB.ImageOffset = new System.Drawing.Point(0, 0);
            this.setinRGB.ImageRotate = 0F;
            this.setinRGB.ImageSize = new System.Drawing.Size(18, 18);
            this.setinRGB.Location = new System.Drawing.Point(184, 70);
            this.setinRGB.Name = "setinRGB";
            this.setinRGB.PressedState.ImageSize = new System.Drawing.Size(16, 16);
            this.setinRGB.Size = new System.Drawing.Size(30, 30);
            this.setinRGB.TabIndex = 20;
            this.setinRGB.Click += new System.EventHandler(this.setinRGB_Click);
            // 
            // bVal
            // 
            this.bVal.AllowDrop = true;
            this.bVal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.bVal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.bVal.DefaultText = "124";
            this.bVal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.bVal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.bVal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.bVal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.bVal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.bVal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.bVal.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.bVal.ForeColor = System.Drawing.Color.White;
            this.bVal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.bVal.Location = new System.Drawing.Point(134, 62);
            this.bVal.Name = "bVal";
            this.bVal.PasswordChar = '\0';
            this.bVal.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.bVal.PlaceholderText = "B";
            this.bVal.SelectedText = "";
            this.bVal.Size = new System.Drawing.Size(40, 34);
            this.bVal.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.bVal.TabIndex = 17;
            this.bVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bVal.TextChanged += new System.EventHandler(this.bVal_TextChanged);
            // 
            // gVal
            // 
            this.gVal.AllowDrop = true;
            this.gVal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.gVal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.gVal.DefaultText = "230";
            this.gVal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.gVal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.gVal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.gVal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.gVal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.gVal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.gVal.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.gVal.ForeColor = System.Drawing.Color.White;
            this.gVal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.gVal.Location = new System.Drawing.Point(88, 62);
            this.gVal.Name = "gVal";
            this.gVal.PasswordChar = '\0';
            this.gVal.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.gVal.PlaceholderText = "G";
            this.gVal.SelectedText = "";
            this.gVal.Size = new System.Drawing.Size(40, 34);
            this.gVal.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.gVal.TabIndex = 16;
            this.gVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.gVal.TextChanged += new System.EventHandler(this.gVal_TextChanged);
            // 
            // rVal
            // 
            this.rVal.AllowDrop = true;
            this.rVal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.rVal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.rVal.DefaultText = "41";
            this.rVal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.rVal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.rVal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.rVal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.rVal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.rVal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.rVal.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F);
            this.rVal.ForeColor = System.Drawing.Color.White;
            this.rVal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.rVal.Location = new System.Drawing.Point(42, 62);
            this.rVal.Name = "rVal";
            this.rVal.PasswordChar = '\0';
            this.rVal.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.rVal.PlaceholderText = "R";
            this.rVal.SelectedText = "";
            this.rVal.Size = new System.Drawing.Size(40, 34);
            this.rVal.Style = Guna.UI2.WinForms.Enums.TextBoxStyle.Material;
            this.rVal.TabIndex = 15;
            this.rVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rVal.TextChanged += new System.EventHandler(this.rVal_TextChanged);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel2.IsContextMenuEnabled = false;
            this.guna2HtmlLabel2.IsSelectionEnabled = false;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(182, 18);
            this.guna2HtmlLabel2.TabIndex = 3;
            this.guna2HtmlLabel2.Text = "SET YOUR FAVOURITE COLOR IN RGB";
            // 
            // guna2HtmlLabel49
            // 
            this.guna2HtmlLabel49.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel49.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel49.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel49.IsContextMenuEnabled = false;
            this.guna2HtmlLabel49.IsSelectionEnabled = false;
            this.guna2HtmlLabel49.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel49.Name = "guna2HtmlLabel49";
            this.guna2HtmlLabel49.Size = new System.Drawing.Size(78, 18);
            this.guna2HtmlLabel49.TabIndex = 8;
            this.guna2HtmlLabel49.Text = "NORMAL MODE";
            // 
            // guna2HtmlLabel45
            // 
            this.guna2HtmlLabel45.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel45.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel45.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel45.IsContextMenuEnabled = false;
            this.guna2HtmlLabel45.IsSelectionEnabled = false;
            this.guna2HtmlLabel45.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel45.Name = "guna2HtmlLabel45";
            this.guna2HtmlLabel45.Size = new System.Drawing.Size(85, 18);
            this.guna2HtmlLabel45.TabIndex = 8;
            this.guna2HtmlLabel45.Text = "SILENTLY CLOSE";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel1.IsContextMenuEnabled = false;
            this.guna2HtmlLabel1.IsSelectionEnabled = false;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(99, 18);
            this.guna2HtmlLabel1.TabIndex = 8;
            this.guna2HtmlLabel1.Text = "EXCLUDE PROCESS";
            // 
            // settingsPage
            // 
            this.settingsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.settingsPage.Controls.Add(this.backfromSettings);
            this.settingsPage.Controls.Add(this.panel15);
            this.settingsPage.Controls.Add(this.panel16);
            this.settingsPage.Controls.Add(this.guna2HtmlLabel55);
            this.settingsPage.Controls.Add(this.guna2HtmlLabel56);
            this.settingsPage.Location = new System.Drawing.Point(279, 39);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(721, 481);
            this.settingsPage.TabIndex = 18;
            // 
            // backfromSettings
            // 
            this.backfromSettings.BackColor = System.Drawing.Color.Transparent;
            this.backfromSettings.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromSettings.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromSettings.Image = global::nigger1.back_to_64px;
            this.backfromSettings.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromSettings.ImageRotate = 0F;
            this.backfromSettings.Location = new System.Drawing.Point(635, 395);
            this.backfromSettings.Name = "backfromSettings";
            this.backfromSettings.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromSettings.Size = new System.Drawing.Size(64, 64);
            this.backfromSettings.TabIndex = 16;
            this.backfromSettings.Click += new System.EventHandler(this.backfromSettings_Click);
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel15.Controls.Add(this.autoDir);
            this.panel15.Controls.Add(this.guna2HtmlLabel53);
            this.panel15.Controls.Add(this.guna2VSeparator25);
            this.panel15.Location = new System.Drawing.Point(42, 120);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(627, 40);
            this.panel15.TabIndex = 11;
            // 
            // autoDir
            // 
            this.autoDir.Animated = true;
            this.autoDir.BackColor = System.Drawing.Color.Transparent;
            this.autoDir.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoDir.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.autoDir.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoDir.CheckedState.InnerColor = System.Drawing.Color.White;
            this.autoDir.Cursor = System.Windows.Forms.Cursors.Hand;
            this.autoDir.Location = new System.Drawing.Point(581, 10);
            this.autoDir.Name = "autoDir";
            this.autoDir.Size = new System.Drawing.Size(35, 20);
            this.autoDir.TabIndex = 10;
            this.autoDir.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.autoDir.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.autoDir.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.autoDir.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.autoDir.CheckedChanged += new System.EventHandler(this.autoDir_CheckedChanged);
            // 
            // guna2HtmlLabel53
            // 
            this.guna2HtmlLabel53.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel53.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel53.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel53.IsContextMenuEnabled = false;
            this.guna2HtmlLabel53.IsSelectionEnabled = false;
            this.guna2HtmlLabel53.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel53.Name = "guna2HtmlLabel53";
            this.guna2HtmlLabel53.Size = new System.Drawing.Size(156, 18);
            this.guna2HtmlLabel53.TabIndex = 8;
            this.guna2HtmlLabel53.Text = "AUTO DIR AFTER OBFUSCATION";
            // 
            // guna2VSeparator25
            // 
            this.guna2VSeparator25.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator25.FillThickness = 3;
            this.guna2VSeparator25.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator25.Name = "guna2VSeparator25";
            this.guna2VSeparator25.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator25.TabIndex = 9;
            // 
            // panel16
            // 
            this.panel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel16.Controls.Add(this.soundReminder);
            this.panel16.Controls.Add(this.guna2HtmlLabel54);
            this.panel16.Controls.Add(this.guna2VSeparator26);
            this.panel16.Location = new System.Drawing.Point(42, 74);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(627, 40);
            this.panel16.TabIndex = 8;
            // 
            // soundReminder
            // 
            this.soundReminder.Animated = true;
            this.soundReminder.BackColor = System.Drawing.Color.Transparent;
            this.soundReminder.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.soundReminder.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.soundReminder.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.soundReminder.CheckedState.InnerColor = System.Drawing.Color.White;
            this.soundReminder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.soundReminder.Location = new System.Drawing.Point(581, 10);
            this.soundReminder.Name = "soundReminder";
            this.soundReminder.Size = new System.Drawing.Size(35, 20);
            this.soundReminder.TabIndex = 10;
            this.soundReminder.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.soundReminder.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.soundReminder.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.soundReminder.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.soundReminder.CheckedChanged += new System.EventHandler(this.soundReminder_CheckedChanged);
            // 
            // guna2HtmlLabel54
            // 
            this.guna2HtmlLabel54.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel54.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel54.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel54.IsContextMenuEnabled = false;
            this.guna2HtmlLabel54.IsSelectionEnabled = false;
            this.guna2HtmlLabel54.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel54.Name = "guna2HtmlLabel54";
            this.guna2HtmlLabel54.Size = new System.Drawing.Size(155, 18);
            this.guna2HtmlLabel54.TabIndex = 8;
            this.guna2HtmlLabel54.Text = "SOUND REMINDER WHEN DONE";
            // 
            // guna2VSeparator26
            // 
            this.guna2VSeparator26.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator26.FillThickness = 3;
            this.guna2VSeparator26.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator26.Name = "guna2VSeparator26";
            this.guna2VSeparator26.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator26.TabIndex = 9;
            // 
            // guna2HtmlLabel55
            // 
            this.guna2HtmlLabel55.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel55.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel55.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel55.IsContextMenuEnabled = false;
            this.guna2HtmlLabel55.IsSelectionEnabled = false;
            this.guna2HtmlLabel55.Location = new System.Drawing.Point(75, 38);
            this.guna2HtmlLabel55.Name = "guna2HtmlLabel55";
            this.guna2HtmlLabel55.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel55.TabIndex = 4;
            this.guna2HtmlLabel55.Text = "SETTINGS";
            // 
            // guna2HtmlLabel56
            // 
            this.guna2HtmlLabel56.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel56.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel56.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel56.IsContextMenuEnabled = false;
            this.guna2HtmlLabel56.IsSelectionEnabled = false;
            this.guna2HtmlLabel56.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel56.Name = "guna2HtmlLabel56";
            this.guna2HtmlLabel56.Size = new System.Drawing.Size(32, 18);
            this.guna2HtmlLabel56.TabIndex = 3;
            this.guna2HtmlLabel56.Text = "XERIN";
            // 
            // guna2HtmlToolTip1
            // 
            this.guna2HtmlToolTip1.AllowLinksHandling = true;
            this.guna2HtmlToolTip1.AutomaticDelay = 400;
            this.guna2HtmlToolTip1.AutoPopDelay = 10000;
            this.guna2HtmlToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.guna2HtmlToolTip1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2HtmlToolTip1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlToolTip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(176)))), ((int)(((byte)(187)))));
            this.guna2HtmlToolTip1.InitialDelay = 400;
            this.guna2HtmlToolTip1.MaximumSize = new System.Drawing.Size(0, 0);
            this.guna2HtmlToolTip1.ReshowDelay = 50;
            this.guna2HtmlToolTip1.TitleFont = new System.Drawing.Font("Bahnschrift SemiCondensed", 10F);
            this.guna2HtmlToolTip1.TitleForeColor = System.Drawing.Color.White;
            this.guna2HtmlToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.guna2HtmlToolTip1.ToolTipTitle = "ABOUT PROTECTION";
            // 
            // embedPage
            // 
            this.embedPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.embedPage.Controls.Add(this.clearList);
            this.embedPage.Controls.Add(this.removeDll);
            this.embedPage.Controls.Add(this.addDlls);
            this.embedPage.Controls.Add(this.panel17);
            this.embedPage.Controls.Add(this.guna2HtmlLabel61);
            this.embedPage.Controls.Add(this.guna2HtmlLabel62);
            this.embedPage.Controls.Add(this.dllsList);
            this.embedPage.Location = new System.Drawing.Point(279, 39);
            this.embedPage.Name = "embedPage";
            this.embedPage.Size = new System.Drawing.Size(721, 481);
            this.embedPage.TabIndex = 19;
            // 
            // clearList
            // 
            this.clearList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.clearList.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.clearList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearList.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.clearList.Image = global::nigger1.empty_trash_20px;
            this.clearList.ImageOffset = new System.Drawing.Point(0, 0);
            this.clearList.ImageRotate = 0F;
            this.clearList.ImageSize = new System.Drawing.Size(20, 20);
            this.clearList.Location = new System.Drawing.Point(639, 193);
            this.clearList.Name = "clearList";
            this.clearList.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.clearList.Size = new System.Drawing.Size(30, 30);
            this.clearList.TabIndex = 19;
            this.clearList.Click += new System.EventHandler(this.clearList_Click);
            // 
            // removeDll
            // 
            this.removeDll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.removeDll.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.removeDll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removeDll.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.removeDll.Image = global::nigger1.cancel_20px;
            this.removeDll.ImageOffset = new System.Drawing.Point(0, 0);
            this.removeDll.ImageRotate = 0F;
            this.removeDll.ImageSize = new System.Drawing.Size(20, 20);
            this.removeDll.Location = new System.Drawing.Point(639, 157);
            this.removeDll.Name = "removeDll";
            this.removeDll.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.removeDll.Size = new System.Drawing.Size(30, 30);
            this.removeDll.TabIndex = 18;
            this.removeDll.Click += new System.EventHandler(this.removeDll_Click);
            // 
            // addDlls
            // 
            this.addDlls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.addDlls.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.addDlls.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addDlls.HoverState.ImageSize = new System.Drawing.Size(19, 19);
            this.addDlls.Image = global::nigger1.add_20px1;
            this.addDlls.ImageOffset = new System.Drawing.Point(0, 0);
            this.addDlls.ImageRotate = 0F;
            this.addDlls.ImageSize = new System.Drawing.Size(20, 20);
            this.addDlls.Location = new System.Drawing.Point(639, 123);
            this.addDlls.Name = "addDlls";
            this.addDlls.PressedState.ImageSize = new System.Drawing.Size(18, 18);
            this.addDlls.Size = new System.Drawing.Size(30, 30);
            this.addDlls.TabIndex = 17;
            this.addDlls.Click += new System.EventHandler(this.addDlls_Click);
            // 
            // panel17
            // 
            this.panel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel17.Controls.Add(this.embedderSwitch);
            this.panel17.Controls.Add(this.guna2HtmlLabel60);
            this.panel17.Controls.Add(this.guna2VSeparator30);
            this.panel17.Location = new System.Drawing.Point(42, 74);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(627, 40);
            this.panel17.TabIndex = 8;
            // 
            // embedderSwitch
            // 
            this.embedderSwitch.Animated = true;
            this.embedderSwitch.BackColor = System.Drawing.Color.Transparent;
            this.embedderSwitch.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.embedderSwitch.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.embedderSwitch.CheckedState.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.embedderSwitch.CheckedState.InnerColor = System.Drawing.Color.White;
            this.embedderSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.embedderSwitch.Location = new System.Drawing.Point(581, 10);
            this.embedderSwitch.Name = "embedderSwitch";
            this.embedderSwitch.Size = new System.Drawing.Size(35, 20);
            this.embedderSwitch.TabIndex = 10;
            this.embedderSwitch.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.embedderSwitch.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.embedderSwitch.UncheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.embedderSwitch.UncheckedState.InnerColor = System.Drawing.Color.White;
            this.embedderSwitch.CheckedChanged += new System.EventHandler(this.embedderSwitch_CheckedChanged);
            // 
            // guna2HtmlLabel60
            // 
            this.guna2HtmlLabel60.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel60.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel60.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel60.IsContextMenuEnabled = false;
            this.guna2HtmlLabel60.IsSelectionEnabled = false;
            this.guna2HtmlLabel60.Location = new System.Drawing.Point(13, 11);
            this.guna2HtmlLabel60.Name = "guna2HtmlLabel60";
            this.guna2HtmlLabel60.Size = new System.Drawing.Size(115, 18);
            this.guna2HtmlLabel60.TabIndex = 8;
            this.guna2HtmlLabel60.Text = "MERGE DLLS WITH EXE";
            // 
            // guna2VSeparator30
            // 
            this.guna2VSeparator30.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2VSeparator30.FillThickness = 3;
            this.guna2VSeparator30.Location = new System.Drawing.Point(0, 10);
            this.guna2VSeparator30.Name = "guna2VSeparator30";
            this.guna2VSeparator30.Size = new System.Drawing.Size(3, 20);
            this.guna2VSeparator30.TabIndex = 9;
            // 
            // guna2HtmlLabel61
            // 
            this.guna2HtmlLabel61.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel61.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel61.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel61.IsContextMenuEnabled = false;
            this.guna2HtmlLabel61.IsSelectionEnabled = false;
            this.guna2HtmlLabel61.Location = new System.Drawing.Point(100, 38);
            this.guna2HtmlLabel61.Name = "guna2HtmlLabel61";
            this.guna2HtmlLabel61.Size = new System.Drawing.Size(50, 18);
            this.guna2HtmlLabel61.TabIndex = 4;
            this.guna2HtmlLabel61.Text = "SETTINGS";
            // 
            // guna2HtmlLabel62
            // 
            this.guna2HtmlLabel62.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel62.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel62.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel62.IsContextMenuEnabled = false;
            this.guna2HtmlLabel62.IsSelectionEnabled = false;
            this.guna2HtmlLabel62.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel62.Name = "guna2HtmlLabel62";
            this.guna2HtmlLabel62.Size = new System.Drawing.Size(58, 18);
            this.guna2HtmlLabel62.TabIndex = 3;
            this.guna2HtmlLabel62.Text = "EMBEDDER";
            // 
            // dllsList
            // 
            this.dllsList.AllowDrop = true;
            this.dllsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.dllsList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dllsList.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dllsList.ForeColor = System.Drawing.Color.White;
            this.dllsList.FormattingEnabled = true;
            this.dllsList.ItemHeight = 14;
            this.dllsList.Location = new System.Drawing.Point(42, 123);
            this.dllsList.Name = "dllsList";
            this.dllsList.Size = new System.Drawing.Size(586, 280);
            this.dllsList.TabIndex = 16;
            this.dllsList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dllsList_DragDrop);
            this.dllsList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dllsList_DragEnter);
            // 
            // rightsPage
            // 
            this.rightsPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.rightsPage.Controls.Add(this.backfromRights);
            this.rightsPage.Controls.Add(this.guna2HtmlLabel4);
            this.rightsPage.Controls.Add(this.guna2HtmlLabel57);
            this.rightsPage.Controls.Add(this.label1);
            this.rightsPage.Controls.Add(this.appVersion);
            this.rightsPage.Controls.Add(this.guna2CirclePictureBox2);
            this.rightsPage.Controls.Add(this.guna2HtmlLabel3);
            this.rightsPage.Controls.Add(this.guna2HtmlLabel63);
            this.rightsPage.Location = new System.Drawing.Point(279, 39);
            this.rightsPage.Name = "rightsPage";
            this.rightsPage.Size = new System.Drawing.Size(721, 481);
            this.rightsPage.TabIndex = 20;
            // 
            // backfromRights
            // 
            this.backfromRights.BackColor = System.Drawing.Color.Transparent;
            this.backfromRights.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.backfromRights.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backfromRights.HoverState.ImageSize = new System.Drawing.Size(63, 63);
            this.backfromRights.Image = global::nigger1.back_to_64px;
            this.backfromRights.ImageOffset = new System.Drawing.Point(0, 0);
            this.backfromRights.ImageRotate = 0F;
            this.backfromRights.Location = new System.Drawing.Point(635, 395);
            this.backfromRights.Name = "backfromRights";
            this.backfromRights.PressedState.ImageSize = new System.Drawing.Size(62, 62);
            this.backfromRights.Size = new System.Drawing.Size(64, 64);
            this.backfromRights.TabIndex = 15;
            this.backfromRights.Click += new System.EventHandler(this.backfromRights_Click);
            // 
            // guna2HtmlLabel4
            // 
            this.guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel4.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel4.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel4.IsContextMenuEnabled = false;
            this.guna2HtmlLabel4.IsSelectionEnabled = false;
            this.guna2HtmlLabel4.Location = new System.Drawing.Point(80, 38);
            this.guna2HtmlLabel4.Name = "guna2HtmlLabel4";
            this.guna2HtmlLabel4.Size = new System.Drawing.Size(39, 18);
            this.guna2HtmlLabel4.TabIndex = 14;
            this.guna2HtmlLabel4.Text = "RIGHTS";
            // 
            // guna2HtmlLabel57
            // 
            this.guna2HtmlLabel57.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel57.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel57.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(230)))), ((int)(((byte)(124)))));
            this.guna2HtmlLabel57.IsContextMenuEnabled = false;
            this.guna2HtmlLabel57.IsSelectionEnabled = false;
            this.guna2HtmlLabel57.Location = new System.Drawing.Point(42, 38);
            this.guna2HtmlLabel57.Name = "guna2HtmlLabel57";
            this.guna2HtmlLabel57.Size = new System.Drawing.Size(37, 18);
            this.guna2HtmlLabel57.TabIndex = 13;
            this.guna2HtmlLabel57.Text = "USAGE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(47, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(647, 112);
            this.label1.TabIndex = 12;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // appVersion
            // 
            this.appVersion.BackColor = System.Drawing.Color.Transparent;
            this.appVersion.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appVersion.ForeColor = System.Drawing.Color.White;
            this.appVersion.IsContextMenuEnabled = false;
            this.appVersion.IsSelectionEnabled = false;
            this.appVersion.Location = new System.Drawing.Point(115, 100);
            this.appVersion.Name = "appVersion";
            this.appVersion.Size = new System.Drawing.Size(46, 18);
            this.appVersion.TabIndex = 11;
            this.appVersion.Text = "VERSION";
            // 
            // guna2CirclePictureBox2
            // 
            this.guna2CirclePictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2CirclePictureBox2.Image = global::nigger1.shield_64px;
            this.guna2CirclePictureBox2.ImageRotate = 0F;
            this.guna2CirclePictureBox2.Location = new System.Drawing.Point(42, 76);
            this.guna2CirclePictureBox2.Name = "guna2CirclePictureBox2";
            this.guna2CirclePictureBox2.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CirclePictureBox2.Size = new System.Drawing.Size(64, 64);
            this.guna2CirclePictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.guna2CirclePictureBox2.TabIndex = 10;
            this.guna2CirclePictureBox2.TabStop = false;
            this.guna2CirclePictureBox2.UseTransparentBackground = true;
            // 
            // guna2HtmlLabel3
            // 
            this.guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel3.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel3.IsContextMenuEnabled = false;
            this.guna2HtmlLabel3.IsSelectionEnabled = false;
            this.guna2HtmlLabel3.Location = new System.Drawing.Point(115, 117);
            this.guna2HtmlLabel3.Name = "guna2HtmlLabel3";
            this.guna2HtmlLabel3.Size = new System.Drawing.Size(72, 18);
            this.guna2HtmlLabel3.TabIndex = 4;
            this.guna2HtmlLabel3.Text = "INX COMPANY";
            // 
            // guna2HtmlLabel63
            // 
            this.guna2HtmlLabel63.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel63.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel63.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel63.IsContextMenuEnabled = false;
            this.guna2HtmlLabel63.IsSelectionEnabled = false;
            this.guna2HtmlLabel63.Location = new System.Drawing.Point(115, 83);
            this.guna2HtmlLabel63.Name = "guna2HtmlLabel63";
            this.guna2HtmlLabel63.Size = new System.Drawing.Size(85, 18);
            this.guna2HtmlLabel63.TabIndex = 3;
            this.guna2HtmlLabel63.Text = "XERINFUSACTOR";
            // 
            // guna2Elipse2
            // 
            this.guna2Elipse2.BorderRadius = 2;
            this.guna2Elipse2.TargetControl = this.pagesSeparator;
            // 
            // killSwitch
            // 
            this.killSwitch.Enabled = true;
            this.killSwitch.Interval = 1000;
            // 
            // XGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 520);
            this.Controls.Add(this.header);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.assemblyPage);
            this.Controls.Add(this.protectionsPage);
            this.Controls.Add(this.colorsPage);
            this.Controls.Add(this.projectPage);
            this.Controls.Add(this.virtualizationPage);
            this.Controls.Add(this.renamerPage);
            this.Controls.Add(this.codeencPage);
            this.Controls.Add(this.codemutationPage);
            this.Controls.Add(this.controlflowPage);
            this.Controls.Add(this.settingsPage);
            this.Controls.Add(this.embedPage);
            this.Controls.Add(this.anticrackPage);
            this.Controls.Add(this.rightsPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::nigger3.iconap;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 520);
            this.Name = "XGui";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XerinFuscator GUI";
            this.Load += new System.EventHandler(this.XGui_Load);
            this.Shown += new System.EventHandler(this.XGui_Shown);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox1)).EndInit();
            this.assemblyPage.ResumeLayout(false);
            this.assemblyPage.PerformLayout();
            this.preserveridsPanel.ResumeLayout(false);
            this.preserveridsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox3)).EndInit();
            this.OptimizePanel.ResumeLayout(false);
            this.OptimizePanel.PerformLayout();
            this.simplePanel.ResumeLayout(false);
            this.simplePanel.PerformLayout();
            this.UnverifiableCodeAttributePanel.ResumeLayout(false);
            this.UnverifiableCodeAttributePanel.PerformLayout();
            this.protectionsPage.ResumeLayout(false);
            this.antiEmulatePanel.ResumeLayout(false);
            this.antiEmulatePanel.PerformLayout();
            this.antitamperPanel.ResumeLayout(false);
            this.antitamperPanel.PerformLayout();
            this.intencodingPanel.ResumeLayout(false);
            this.intencodingPanel.PerformLayout();
            this.integrityPanel.ResumeLayout(false);
            this.integrityPanel.PerformLayout();
            this.l2fPanel.ResumeLayout(false);
            this.l2fPanel.PerformLayout();
            this.refproxyPanel.ResumeLayout(false);
            this.refproxyPanel.PerformLayout();
            this.resourcesPanel.ResumeLayout(false);
            this.resourcesPanel.PerformLayout();
            this.stringsPanel.ResumeLayout(false);
            this.stringsPanel.PerformLayout();
            this.codemutPanel.ResumeLayout(false);
            this.codemutPanel.PerformLayout();
            this.cflowPanel.ResumeLayout(false);
            this.cflowPanel.PerformLayout();
            this.antivirtPanel.ResumeLayout(false);
            this.antivirtPanel.PerformLayout();
            this.antidumpPanel.ResumeLayout(false);
            this.antidumpPanel.PerformLayout();
            this.antidebugPanel.ResumeLayout(false);
            this.antidebugPanel.PerformLayout();
            this.antidecPanel.ResumeLayout(false);
            this.antidecPanel.PerformLayout();
            this.anticrackPanel.ResumeLayout(false);
            this.anticrackPanel.PerformLayout();
            this.anticrackPage.ResumeLayout(false);
            this.anticrackPage.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.controlflowPage.ResumeLayout(false);
            this.controlflowPage.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.codemutationPage.ResumeLayout(false);
            this.codemutationPage.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.codeencPage.ResumeLayout(false);
            this.codeencPage.PerformLayout();
            this.codeencPanel.ResumeLayout(false);
            this.codeencPanel.PerformLayout();
            this.renamerPage.ResumeLayout(false);
            this.renamerPage.PerformLayout();
            this.cfexPanel.ResumeLayout(false);
            this.cfexPanel.PerformLayout();
            this.renamePanel.ResumeLayout(false);
            this.renamePanel.PerformLayout();
            this.virtualizationPage.ResumeLayout(false);
            this.virtualizationPage.PerformLayout();
            this.guna2ContextMenuStrip1.ResumeLayout(false);
            this.virtPanel.ResumeLayout(false);
            this.virtPanel.PerformLayout();
            this.projectPage.ResumeLayout(false);
            this.projectPage.PerformLayout();
            this.colorsPage.ResumeLayout(false);
            this.colorsPage.PerformLayout();
            this.settingsPage.ResumeLayout(false);
            this.settingsPage.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel16.ResumeLayout(false);
            this.panel16.PerformLayout();
            this.embedPage.ResumeLayout(false);
            this.embedPage.PerformLayout();
            this.panel17.ResumeLayout(false);
            this.panel17.PerformLayout();
            this.rightsPage.ResumeLayout(false);
            this.rightsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2CirclePictureBox2)).EndInit();
            this.ResumeLayout(false);

	}

	static XGui()
	{
		ctx = null;
		protectionManager = new ProtectionManager();
	}
}
