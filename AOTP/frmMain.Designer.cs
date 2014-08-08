namespace AOTP
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbEnc = new System.Windows.Forms.RadioButton();
            this.rbDec = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.btnGo = new System.Windows.Forms.Button();
            this.pbFile = new System.Windows.Forms.ProgressBar();
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // lbFiles
            // 
            this.lbFiles.AllowDrop = true;
            this.lbFiles.FormattingEnabled = true;
            this.lbFiles.Location = new System.Drawing.Point(12, 49);
            this.lbFiles.Name = "lbFiles";
            this.lbFiles.Size = new System.Drawing.Size(365, 173);
            this.lbFiles.TabIndex = 3;
            this.lbFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragDrop);
            this.lbFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbFiles_DragEnter);
            this.lbFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFiles_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "1. Select files to add to the list or drag them onto it";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(404, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "2. Order the list with the up and down buttons so the first to be released file i" +
                "s on top.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "3. Select the operation";
            // 
            // rbEnc
            // 
            this.rbEnc.AutoSize = true;
            this.rbEnc.Checked = true;
            this.rbEnc.Location = new System.Drawing.Point(12, 250);
            this.rbEnc.Name = "rbEnc";
            this.rbEnc.Size = new System.Drawing.Size(156, 17);
            this.rbEnc.TabIndex = 7;
            this.rbEnc.TabStop = true;
            this.rbEnc.Text = "Encrypt and generate a key";
            this.rbEnc.UseVisualStyleBackColor = true;
            // 
            // rbDec
            // 
            this.rbDec.AutoSize = true;
            this.rbDec.Location = new System.Drawing.Point(12, 272);
            this.rbDec.Name = "rbDec";
            this.rbDec.Size = new System.Drawing.Size(267, 17);
            this.rbDec.TabIndex = 8;
            this.rbDec.Text = "Decrypt and assume the key is the last file in the list";
            this.rbDec.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 300);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "4.";
            // 
            // OFD
            // 
            this.OFD.Filter = "All Files|*.*";
            this.OFD.Multiselect = true;
            this.OFD.Title = "Select source file";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(31, 295);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 10;
            this.btnGo.Text = "Go...";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // pbFile
            // 
            this.pbFile.Location = new System.Drawing.Point(6, 350);
            this.pbFile.Name = "pbFile";
            this.pbFile.Size = new System.Drawing.Size(407, 23);
            this.pbFile.TabIndex = 12;
            // 
            // pbTotal
            // 
            this.pbTotal.Location = new System.Drawing.Point(6, 379);
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(407, 23);
            this.pbTotal.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 330);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "5. Just watch";
            // 
            // btnRemove
            // 
            this.btnRemove.Image = global::AOTP.Properties.Resources.del;
            this.btnRemove.Location = new System.Drawing.Point(383, 85);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 30);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::AOTP.Properties.Resources.add;
            this.btnAdd.Location = new System.Drawing.Point(383, 49);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 30);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::AOTP.Properties.Resources.down;
            this.btnDown.Location = new System.Drawing.Point(383, 192);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(30, 30);
            this.btnDown.TabIndex = 5;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = global::AOTP.Properties.Resources.up;
            this.btnUp.Location = new System.Drawing.Point(383, 156);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(30, 30);
            this.btnUp.TabIndex = 4;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // FBD
            // 
            this.FBD.Description = "Select destination folder";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 412);
            this.Controls.Add(this.pbTotal);
            this.Controls.Add(this.pbFile);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.rbDec);
            this.Controls.Add(this.rbEnc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "AOTP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbEnc;
        private System.Windows.Forms.RadioButton rbDec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ProgressBar pbFile;
        private System.Windows.Forms.ProgressBar pbTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.FolderBrowserDialog FBD;
    }
}

