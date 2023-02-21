using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        List<string> fileList;
        string insertText;
        int insertPosition;
        int deletePosition;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            fileList.Clear();
            
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                fileList = openFileDialog1.FileNames.ToList();
            }

            if (fileList.Count > 0)
                MessageBox.Show("파일 선택 완료");
        }

        private async void btnStart_ClickAsync(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(insertCharText.Text) && String.IsNullOrEmpty(deleteCharText.Text))
            {
                MessageBox.Show("작업을 아무것도 선택하지 않았습니다.");
                return;
            }

            if (fileList.Count == 0)
            {
                MessageBox.Show("작업을 아무것도 선택하지 않았습니다.");
                return;
            }

            if (!String.IsNullOrEmpty(insertCharText.Text))
            {
                string[] parse = insertCharText.Text.Split(",");
                insertText = parse[0];
                insertPosition = Convert.ToInt32(parse[1]);

                await InsertCharAsync(insertText, insertPosition);
            }

            if (!String.IsNullOrEmpty(deleteCharText.Text))
            {
                deletePosition = Convert.ToInt32(deleteCharText.Text);
                await DeleteCharAsync(deletePosition);
            }

            MessageBox.Show("작업 완료!!!");
        }

        private Task InsertCharAsync(string character, int position)
        {
            return Task.Run(() => {
                foreach (var file in fileList)
                {

                    var reader = File.ReadAllBytes(file);
                    byte[] data = Encoding.ASCII.GetBytes(character);
                    var a = reader.ToList();
                    a.Insert(position, data[0]);
                    File.WriteAllBytes(file, a.ToArray());
                }
            });
        }

        private Task DeleteCharAsync(int position)
        {
            return Task.Run(() => {
                foreach (var file in fileList)
                {
                    var reader = File.ReadAllBytes(file);
                    var a = reader.ToList();
                    a.RemoveAt(position);
                    File.WriteAllBytes(file, a.ToArray());
                }
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fileList = new List<string>();
        }
    }
}
