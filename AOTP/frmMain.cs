using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AOTP
{
    public partial class frmMain : Form
    {
        private BackgroundWorker BW;

        public frmMain()
        {
            InitializeComponent();
            BW = new BackgroundWorker();
            BW.WorkerReportsProgress = true;

            BW.DoWork += new DoWorkEventHandler(BW_DoWork);
            BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BW_RunWorkerCompleted);
            BW.ProgressChanged += new ProgressChangedEventHandler(BW_ProgressChanged);
        }

        void BW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbTotal.Value = e.ProgressPercentage;
        }

        void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ena(true);
            pbFile.Value = pbTotal.Value = 0;
            MessageBox.Show("Operation completed");
        }

        void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            pbTotal.Value = 0;
            if (rbEnc.Checked)
            {
                encrypt(e.Argument.ToString());
            }
            else
            {
                decrypt(e.Argument.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addFiles();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            removeSelected();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            moveUp();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            moveDown();
        }

        private void lbFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = e.Handled = true;
                removeSelected();
            }
            else if (e.KeyCode == Keys.Insert)
            {
                e.SuppressKeyPress = e.Handled = true;
                addFiles();
            }
            else if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.Up)
                {
                    e.SuppressKeyPress = e.Handled = true;
                    moveUp();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    e.SuppressKeyPress = e.Handled = true;
                    moveDown();
                }
            }
        }

        private void addFiles()
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                lbFiles.Items.AddRange(OFD.FileNames);
            }
        }

        private void removeSelected()
        {
            if (lbFiles.SelectedIndex >= 0)
            {
                int index = lbFiles.SelectedIndex;
                lbFiles.Items.RemoveAt(index);
                if (index < lbFiles.Items.Count)
                {
                    lbFiles.SelectedIndex = index;
                }
            }
        }

        private void moveDown()
        {
            int index = lbFiles.SelectedIndex;
            if (index >= 0 && index < lbFiles.Items.Count - 1)
            {
                lbFiles.Items.Insert(index + 2, lbFiles.Items[index]);
                lbFiles.Items.RemoveAt(index);
                lbFiles.SelectedIndex = index + 1;
            }
        }

        private void moveUp()
        {
            int index = lbFiles.SelectedIndex;
            if (index > 0 && index < lbFiles.Items.Count)
            {
                lbFiles.Items.Insert(index - 1, lbFiles.Items[index]);
                lbFiles.Items.RemoveAt(index + 1);
                lbFiles.SelectedIndex = index - 1;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!BW.IsBusy)
            {

                if (FBD.ShowDialog() == DialogResult.OK)
                {
                    ena(false);
                    BW.RunWorkerAsync(FBD.SelectedPath);
                }
            }
            else
            {
                ena(false);
            }
        }

        private void encrypt(string path)
        {
            xorProgressHandler xPH = new xorProgressHandler(xorProgress);

            OTP.xorProgress += xPH;

            string[] Files = new string[lbFiles.Items.Count];
            int i;
            for (i = 0; i < Files.Length; i++)
            {
                Files[i] = lbFiles.Items[i].ToString();
            }

            //encrypt
            long longest = 0L;

            foreach (string s in Files)
            {
                FileInfo F = new FileInfo(s);
                if (F.Length == 0)
                {
                    MessageBox.Show("Error on file " + F.FullName + "\r\n\r\nYou cannot encrypt 0 byte files.\r\n(You technically could but it is totally mean)\r\n\r\nOperation is aborted", "0-byte file detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (F.Length + (long)OTP.generateHeader(F.Name, F.Length).Length > longest)
                {
                    longest = F.Length + (long)OTP.generateHeader(F.Name, F.Length).Length;
                }
            }

            //ready to encrypt. Generate key
            using (var keyOut = File.Create(path + "\\key.bin"))
            {
                OTP.generateKey(longest, OTP.RandomSeed, keyOut);
                BW.ReportProgress(100 / (Files.Length + 1));
            }

            i = 0;

            foreach (string s in Files)
            {
                if (i == 0)
                {
                    using (var keyIn = File.OpenRead(path + "\\key.bin"))
                    {
                        using (var FileOut = File.Create(string.Format("{0}\\enc_{1}.bin", path, Files.Length - i)))
                        {
                            OTP.xorFile(s, keyIn, FileOut);
                        }
                    }
                }
                else
                {
                    //encrypt using previously encrypted file
                    using (var keyIn = File.OpenRead(string.Format("{0}\\enc_{1}.bin", FBD.SelectedPath, Files.Length - i + 1)))
                    {
                        using (var FileOut = File.Create(string.Format("{0}\\enc_{1}.bin", FBD.SelectedPath, Files.Length - i)))
                        {
                            OTP.xorFile(s, keyIn, FileOut);
                        }
                    }
                }
                i++;
                BW.ReportProgress(i * 100 / (Files.Length + 1));
            }

            OTP.xorProgress -= xPH;

            BW.ReportProgress(100);
        }

        private void decrypt(string path)
        {
            xorProgressHandler xPH = new xorProgressHandler(xorProgress);

            OTP.xorProgress += xPH;

            string[] Files = new string[lbFiles.Items.Count];
            for (int i = 0; i < Files.Length; i++)
            {
                Files[i] = lbFiles.Items[i].ToString();
            }

            //decrypt

            //this loop does not decrypts the last file as it is assumed to be key only
            for (int i = 0; i < Files.Length - 1; i++)
            {
                using (var InFile = File.OpenRead(Files[i]))
                {
                    using (var InKey = File.OpenRead(Files[i + 1]))
                    {
                        OTP.HeaderProps HP = OTP.decryptHeader(InFile, InKey);

                        if (HP.FileLength >= 0)
                        {

                            using (var OutFile = File.Create(path + "\\" + HP.FileName))
                            {
                                using (var RangeIn = new RangedStream(InFile, 0, HP.FileLength))
                                {
                                    using (var RangeKey = new RangedStream(InKey, 0, HP.FileLength))
                                    {
                                        OTP.xor(RangeIn, RangeKey, OutFile);
                                        BW.ReportProgress((i + 1) * 100 / Files.Length);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error decoding File " + Files[i] + "\r\nProbably wrong key.", "Operation aborted");
                            break;
                        }
                    }
                }
            }
            OTP.xorProgress -= xPH;
            BW.ReportProgress(100);
        }

        private void xorProgress(int i)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    xorProgress(i);
                });
            }
            else
            {
                pbFile.Value = i;
            }
        }

        private void ena(bool p)
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button || c is ListBox || c is RadioButton)
                {
                    c.Enabled = p;
                }
            }
        }

        private void lbFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lbFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            lbFiles.Items.AddRange(files);
        }
    }
}
