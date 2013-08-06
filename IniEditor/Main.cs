using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniFiles;

namespace IniEditor
{
    public partial class Main : Form
    {
        IniFile iniFile;
        
        public Main()
        {
            InitializeComponent();
        }

        private ListViewGroup GetGroup(string sectionName)
        {
            foreach (ListViewGroup group in listView1.Groups)
            {
                if (group.Header.ToUpper() == sectionName.ToUpper())
                {
                    return group;
                }
            }

            ListViewGroup newGroup = new ListViewGroup(sectionName);
            listView1.Groups.Add(newGroup);
            return newGroup;
        }

        private void AddToListView(string sectionName, string keyName, string value)
        {
            ListViewItem item = new ListViewItem(new string[] { keyName, value });
            item.Group = GetGroup(sectionName);            
            listView1.Items.Add(item);
        }

        private void BuildListView()
        {
            this.Text = "IniFile Editor :: " + iniFile.fileName;

            listView1.Items.Clear();

            IniSection section;
            string value;

            foreach (string sectionName in iniFile.ReadSections()) 
            {
                section = iniFile.ReadSection(sectionName);
                foreach (string keyName in section.Keys)
                {
                    section.TryGetValue(keyName, out value);
                    AddToListView(sectionName, keyName, value);
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (DialogResult.OK == dr)
            {
                iniFile = new IniFile(openFileDialog1.FileName);
                BuildListView();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iniFile = new IniFile("");
            BuildListView();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (iniFile.fileName == "")
            {
                DialogResult dr = saveFileDialog1.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                iniFile.fileName = saveFileDialog1.FileName;
            }

            iniFile.Save();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem item = listView1.SelectedItems[0];

            string keyName = item.Text;
            string value = item.SubItems[1].Text;
            string sectionName = item.Group.Header;

            List<string> sections = new List<string>();
            for (int i = 0; i < listView1.Groups.Count; i++)
            {
                sections.Add(listView1.Groups[i].Header);
            }

            var form = new EditWin();
            var result = form.Execute(sections, sectionName, keyName, value);

            if (result.dialogResult == DialogResult.Cancel) return;

            if ((sectionName != result.iniSection) ||
               (keyName != result.iniKey) ||
               (value != result.iniValue))
            {
                // delete original key
                iniFile.DeleteKey(sectionName, keyName);
            }

            iniFile.WriteString(result.iniSection, result.iniKey, result.iniValue);

            BuildListView();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;

            ListViewItem item = listView1.SelectedItems[0];
            iniFile.DeleteKey(item.Group.Header, item.Text);
            listView1.Items.Remove(item);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> sections = new List<string>();
            for (int i = 0; i < listView1.Groups.Count; i++)
            {
                sections.Add(listView1.Groups[i].Header);
            }

            var result = new EditWin().Execute(sections, "", "", "");
            if (result.dialogResult != DialogResult.OK) return;

            iniFile.WriteString(result.iniSection, result.iniKey, result.iniValue);

            BuildListView();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            iniFile = new IniFile("");
        }
    }
}
