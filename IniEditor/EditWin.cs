using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IniEditor
{

    public partial class EditWin : Form
    {
        public EditWin()
        {
            InitializeComponent();
        }

        public MethodArgs Execute(List<string> sections, string sectionName, string keyName, string value)
        {
            MethodArgs result = new MethodArgs() { iniSection = sectionName, iniKey = keyName, iniValue = value};

            SetWindowTitle(sectionName);
            LoadSectionsBox(sections);

            sectionsBox.Text = sectionName;
            keyBox.Text = keyName;
            valueBox.Text = value;

            result.dialogResult = ShowDialog();

            if (result.dialogResult == DialogResult.OK)
            {
                result.iniSection = sectionsBox.Text;
                result.iniKey = keyBox.Text;
                result.iniValue = valueBox.Text;
            }

            return result;
        }

        private void SetWindowTitle(string sectionName)
        {
            if (sectionName == "")
            {
                Text = "Create new key";
            }
            else
            {
                Text = "Edit Key";
            }
        }

        private void LoadSectionsBox(List<string> sections)
        {
            sectionsBox.Items.Clear();
            foreach (var str in sections)
            {
                sectionsBox.Items.Add(str);
            }
        }
    }


    public class MethodArgs
    {
        public MethodArgs()
        {
            dialogResult = new DialogResult();
        }
        
        public string iniSection;
        public string iniKey;
        public string iniValue;
        public DialogResult dialogResult;
    }
}
