using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XCore.Embed;
using XCore.Protections;
using XCore.VirtHelper;
using XProtections.AntiCrack.Globals;
using XProtections.ControlFlow;

public class ControlProj
{
    public static string path = "Empty";

    public static string projLocation = string.Empty;

    public static string modLocation = string.Empty;

    public static List<Protection> addedProt = ProtectionManager.Protections;

    public static List<string> virtedMethods = Helper.names;

    public static List<string> dlls = Embeder.dlls;

    public static List<string> dlls2 = new List<string>();

    public static List<bool> rChecked = new List<bool> { false, false, false, false, false, false };

    public static bool isall = false;

    public static bool antiemulate = false;

    public static bool renamer = false;

    public static bool cfexrenamer = false;

    public static bool antiCrack = false;

    public static bool antiDebug = false;

    public static bool antiDecombiler = false;

    public static bool antiDump = false;

    public static bool antiVM = false;

    public static bool stringsEncoder = false;

    public static bool intencoding = false;

    public static bool antitamper = false;

    public static bool balancedcodemutation = false;

    public static bool cflowStrong = false;

    public static bool cflowPerformance = false;

    public static bool resourcesEncoder = false;

    public static bool refProxy = false;

    public static bool controlFlow = false;

    public static bool codeEncryption = false;

    public static bool codeVirt = false;

    public static bool localtofield = false;

    public static bool integrityCheck = false;

    public static bool RenameEvents = false;

    public static bool RenameFields = false;

    public static bool RenameMethods = false;

    public static bool RenameParameters = false;

    public static bool RenameProperties = false;

    public static bool RenameTypes = false;

    public static bool embeder = false;

    public static string webhook = string.Empty;

    public static string api = string.Empty;

    public static string exclude = string.Empty;

    public static string customMsg = string.Empty;

    public static bool normal = false;

    public static bool excludep = false;

    public static bool silentMsg = false;

    public static bool bsod = false;

    public static int R = 41;

    public static int G = 230;

    public static int B = 124;

    public static string Custom { get; set; }

    public static bool CustomRN { get; set; }

    public void Save(MemoryStream xerinConfig)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog
        {
            FileName = "Saved Project.xml",
            Title = "Save Xerin's Project",
            Filter = "Project File(*.xml)|*.xml"
        };
        if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveFileDialog.FileName))
        {
            WriteProject(xerinConfig);
            File.WriteAllBytes(Path.GetFullPath(saveFileDialog.FileName), xerinConfig.ToArray());
            xerinConfig.Flush();
        }
    }

    public bool WriteProject(MemoryStream xerinConfig)
    {
        using (XmlWriter xmlWriter = XmlWriter.Create(xerinConfig, new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            CloseOutput = true,
            OmitXmlDeclaration = true
        }))
        {
            xmlWriter.WriteStartElement("XerinFuscator");
            xmlWriter.WriteElementString("R", Convert.ToString(R));
            xmlWriter.WriteElementString("G", Convert.ToString(G));
            xmlWriter.WriteElementString("B", Convert.ToString(B));
            xmlWriter.WriteElementString("File", path);
            xmlWriter.WriteElementString("Webhook", Global.webhookLink);
            xmlWriter.WriteElementString("Exclude", Global.excludeString);
            xmlWriter.WriteElementString("Message", Global.customMessage);
            xmlWriter.WriteElementString("Normal", Global.Normal.ToString());
            xmlWriter.WriteElementString("Silent", Global.Silent.ToString());
            xmlWriter.WriteElementString("Bsod", Global.Bsod.ToString());
            if (ControlFlow.isPerformance)
            {
                xmlWriter.WriteElementString("ControlFlow", "Performance");
            }
            if (ControlFlow.isStrong)
            {
                xmlWriter.WriteElementString("ControlFlow", "Strong");
            }
            if (Embeder.isEmbed)
            {
                xmlWriter.WriteElementString("Embed", "True");
            }
            if (dlls.Count > 0)
            {
                xmlWriter.WriteStartElement("Dlls");
                foreach (string dll in dlls)
                {
                    xmlWriter.WriteElementString("Path", dll);
                }
                xmlWriter.WriteEndElement();
            }
            if (addedProt.Count > 0)
            {
                xmlWriter.WriteStartElement("Protections");
                foreach (Protection item in addedProt)
                {
                    xmlWriter.WriteElementString("Protection", item.name.Replace(" ", ""));
                }
                xmlWriter.WriteEndElement();
            }
            if (CustomRN)
            {
                xmlWriter.WriteElementString("Custom", "True");
                xmlWriter.WriteElementString("Signature", Custom);
            }
            xmlWriter.WriteStartElement("ROptions");
            if (rChecked[0])
            {
                xmlWriter.WriteElementString("Flag", "Events");
            }
            if (rChecked[1])
            {
                xmlWriter.WriteElementString("Flag", "Fields");
            }
            if (rChecked[2])
            {
                xmlWriter.WriteElementString("Flag", "Methods");
            }
            if (rChecked[3])
            {
                xmlWriter.WriteElementString("Flag", "Parameters");
            }
            if (rChecked[4])
            {
                xmlWriter.WriteElementString("Flag", "Properties");
            }
            if (rChecked[5])
            {
                xmlWriter.WriteElementString("Flag", "Types");
            }
            xmlWriter.WriteEndElement();
            if (Helper.isEnabled)
            {
                xmlWriter.WriteElementString("Virtualization", "True");
            }
            if (isall)
            {
                xmlWriter.WriteElementString("VirtAll", "True");
            }
            if (virtedMethods.Count > 0)
            {
                xmlWriter.WriteStartElement("Virtualized");
                xmlWriter.WriteStartElement("Methods");
                if (Helper.names != null)
                {
                    foreach (string item2 in Helper.names.Distinct())
                    {
                        xmlWriter.WriteElementString("VMethod", item2);
                    }
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
            xmlWriter.Flush();
            xmlWriter.Close();
        }
        return true;
    }

    public void Load(MemoryStream xerinConfig)
    {
        XElement xElement = XDocument.Load(new StreamReader(xerinConfig)).Element("XerinFuscator");
        if (xElement == null)
        {
            return;
        }
        R = Convert.ToInt32(xElement.Element("R").Value);
        G = Convert.ToInt32(xElement.Element("G").Value);
        B = Convert.ToInt32(xElement.Element("B").Value);
        path = xElement.Element("File").Value;
        webhook = xElement.Element("Webhook").Value;
        if (xElement.Element("Exclude").Value != null)
        {
            exclude = xElement.Element("Exclude").Value;
            excludep = true;
        }
        customMsg = xElement.Element("Message").Value;
        if (xElement.Element("Normal")?.Value == "True")
        {
            normal = true;
        }
        if (xElement.Element("Silent")?.Value == "True")
        {
            silentMsg = true;
        }
        if (xElement.Element("Silent")?.Value == "True")
        {
            silentMsg = true;
        }
        if (xElement.Element("Bsod")?.Value == "True")
        {
            bsod = true;
        }
        if (xElement.Element("Embed")?.Value == "True")
        {
            embeder = true;
        }
        if (xElement.Element("ControlFlow")?.Value == "Performance")
        {
            cflowPerformance = true;
        }
        if (xElement.Element("ControlFlow")?.Value == "Strong")
        {
            cflowStrong = true;
        }
        XElement xElement2 = xElement.Element("Dlls");
        if (xElement2 != null)
        {
            foreach (XElement item in xElement2.Elements("Path"))
            {
                dlls2.Add(item.Value);
            }
        }
        XElement xElement3 = xElement.Element("Protections");
        if (xElement3 != null)
        {
            foreach (XElement item2 in xElement3.Elements("Protection"))
            {
                switch (item2.Value)
                {
                    case "AntiTamper":
                        antitamper = true;
                        break;
                    case "Localtofield":
                        localtofield = true;
                        break;
                    case "AntiVM":
                        antiVM = true;
                        break;
                    case "cfexRenamer":
                        cfexrenamer = true;
                        break;
                    case "AntiCrack":
                        antiCrack = true;
                        break;
                    case "StringsEncoding":
                        stringsEncoder = true;
                        break;
                    case "AntiDump":
                        antiDump = true;
                        break;
                    case "AntiDecompiler":
                        antiDecombiler = true;
                        break;
                    case "IntegrityCheck":
                        integrityCheck = true;
                        break;
                    case "ControlFlow":
                        controlFlow = true;
                        break;
                    case "Renamer":
                        renamer = true;
                        break;
                    case "MildReferenceProxy":
                        refProxy = true;
                        break;
                    case "ResourcesEncoding":
                        resourcesEncoder = true;
                        break;
                    case "CodeEncryption":
                        codeEncryption = true;
                        break;
                    case "Performancemutationstage":
                        balancedcodemutation = true;
                        break;
                    case "Intsencoding":
                        intencoding = true;
                        break;
                    case "AntiDebug":
                        antiDebug = true;
                        break;
                    case "AntiEmulate":
                        antiemulate = true;
                        break;
                }
            }
        }
        XElement xElement4 = xElement.Element("Config");
        if (xElement4 != null)
        {
            foreach (XElement item3 in xElement4.Elements("Config"))
            {
                string value = item3.Value;
                string text = value;
                if (!(text == "Webhook"))
                {
                    if (text == "API")
                    {
                        api = xElement.Element("Config").Value;
                    }
                }
                else
                {
                    webhook = xElement.Element("Config").Value;
                }
            }
        }
        if (xElement.Element("Custom")?.Value == "True")
        {
            CustomRN = true;
            Custom = xElement.Element("Signature")?.Value;
        }
        XElement xElement5 = xElement.Element("ROptions");
        if (xElement5 != null)
        {
            foreach (XElement item4 in xElement5.Elements("Flag"))
            {
                switch (item4.Value)
                {
                    case "Types":
                        RenameTypes = true;
                        break;
                    case "Properties":
                        RenameProperties = true;
                        break;
                    case "Parameters":
                        RenameParameters = true;
                        break;
                    case "Methods":
                        RenameMethods = true;
                        break;
                    case "Fields":
                        RenameFields = true;
                        break;
                    case "Events":
                        RenameEvents = true;
                        break;
                }
            }
        }
        XElement xElement6 = xElement.Element("Virtualization");
        if (xElement6 != null && xElement6.Value == "True")
        {
            codeVirt = true;
            XElement xElement7 = xElement.Element("Virtualized");
            if (xElement7 != null)
            {
                XElement xElement8 = xElement7.Element("Methods");
                if (xElement8 != null)
                {
                    foreach (XElement item5 in xElement8.Elements("VMethod"))
                    {
                        Helper.AddProtection(item5.Value);
                    }
                }
            }
        }
        if (xElement.Element("VirtAll")?.Value == "True")
        {
            isall = true;
        }
    }
}
