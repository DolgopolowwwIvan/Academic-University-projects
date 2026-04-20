// MainForm.Designer.cs - Дизайнер формы
namespace CalculatorFractions
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.copyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.fractionModeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numberModeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayLabel = new System.Windows.Forms.Label();
            this.MemoryStatusLabel = new System.Windows.Forms.Label();
            this.BtnMC = new System.Windows.Forms.Button();
            this.BtnMR = new System.Windows.Forms.Button();
            this.BtnMPlus = new System.Windows.Forms.Button();
            this.BtnMS = new System.Windows.Forms.Button();
            this.BtnC = new System.Windows.Forms.Button();
            this.BtnCE = new System.Windows.Forms.Button();
            this.BtnBackspace = new System.Windows.Forms.Button();
            this.BtnDiv = new System.Windows.Forms.Button();
            this.Btn7 = new System.Windows.Forms.Button();
            this.Btn8 = new System.Windows.Forms.Button();
            this.Btn9 = new System.Windows.Forms.Button();
            this.BtnMul = new System.Windows.Forms.Button();
            this.BtnSqr = new System.Windows.Forms.Button();
            this.Btn4 = new System.Windows.Forms.Button();
            this.Btn5 = new System.Windows.Forms.Button();
            this.Btn6 = new System.Windows.Forms.Button();
            this.BtnSub = new System.Windows.Forms.Button();
            this.BtnRev = new System.Windows.Forms.Button();
            this.Btn1 = new System.Windows.Forms.Button();
            this.Btn2 = new System.Windows.Forms.Button();
            this.Btn3 = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnSign = new System.Windows.Forms.Button();
            this.Btn0 = new System.Windows.Forms.Button();
            this.BtnSeparator = new System.Windows.Forms.Button();
            this.BtnEqual = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editMenu, this.viewMenu, this.helpMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(400, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editMenu
            // 
            this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyItem, this.pasteItem});
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(60, 20);
            this.editMenu.Text = "&Правка";
            // 
            // copyItem
            // 
            this.copyItem.Name = "copyItem";
            this.copyItem.Size = new System.Drawing.Size(124, 22);
            this.copyItem.Text = "&Копировать";
            this.copyItem.Click += new System.EventHandler(this.CopyClick);
            // 
            // pasteItem
            // 
            this.pasteItem.Name = "pasteItem";
            this.pasteItem.Size = new System.Drawing.Size(124, 22);
            this.pasteItem.Text = "&Вставить";
            this.pasteItem.Click += new System.EventHandler(this.PasteClick);
            // 
            // viewMenu
            // 
            this.viewMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fractionModeItem, this.numberModeItem});
            this.viewMenu.Name = "viewMenu";
            this.viewMenu.Size = new System.Drawing.Size(47, 20);
            this.viewMenu.Text = "&Вид";
            // 
            // fractionModeItem
            // 
            this.fractionModeItem.Name = "fractionModeItem";
            this.fractionModeItem.Size = new System.Drawing.Size(117, 22);
            this.fractionModeItem.Text = "&Дробь";
            this.fractionModeItem.Click += new System.EventHandler(this.FractionModeClick);
            // 
            // numberModeItem
            // 
            this.numberModeItem.Name = "numberModeItem";
            this.numberModeItem.Size = new System.Drawing.Size(117, 22);
            this.numberModeItem.Text = "&Число";
            this.numberModeItem.Click += new System.EventHandler(this.NumberModeClick);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(65, 20);
            this.helpMenu.Text = "&Справка";
            // 
            // aboutItem
            // 
            this.aboutItem.Name = "aboutItem";
            this.aboutItem.Size = new System.Drawing.Size(158, 22);
            this.aboutItem.Text = "&О программе...";
            this.aboutItem.Click += new System.EventHandler(this.AboutClick);
            // 
            // DisplayLabel
            // 
            this.DisplayLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DisplayLabel.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DisplayLabel.Location = new System.Drawing.Point(12, 35);
            this.DisplayLabel.Name = "DisplayLabel";
            this.DisplayLabel.Size = new System.Drawing.Size(376, 45);
            this.DisplayLabel.TabIndex = 1;
            this.DisplayLabel.Text = "0/1";
            this.DisplayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MemoryStatusLabel
            // 
            this.MemoryStatusLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.MemoryStatusLabel.Location = new System.Drawing.Point(12, 85);
            this.MemoryStatusLabel.Name = "MemoryStatusLabel";
            this.MemoryStatusLabel.Size = new System.Drawing.Size(30, 20);
            this.MemoryStatusLabel.TabIndex = 2;
            this.MemoryStatusLabel.Text = "   ";
            // 
            // BtnMC
            // 
            this.BtnMC.Location = new System.Drawing.Point(12, 115);
            this.BtnMC.Name = "BtnMC";
            this.BtnMC.Size = new System.Drawing.Size(60, 35);
            this.BtnMC.TabIndex = 3;
            this.BtnMC.Text = "MC";
            this.BtnMC.UseVisualStyleBackColor = true;
            this.BtnMC.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnMR
            // 
            this.BtnMR.Location = new System.Drawing.Point(82, 115);
            this.BtnMR.Name = "BtnMR";
            this.BtnMR.Size = new System.Drawing.Size(60, 35);
            this.BtnMR.TabIndex = 4;
            this.BtnMR.Text = "MR";
            this.BtnMR.UseVisualStyleBackColor = true;
            this.BtnMR.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnMPlus
            // 
            this.BtnMPlus.Location = new System.Drawing.Point(152, 115);
            this.BtnMPlus.Name = "BtnMPlus";
            this.BtnMPlus.Size = new System.Drawing.Size(60, 35);
            this.BtnMPlus.TabIndex = 5;
            this.BtnMPlus.Text = "M+";
            this.BtnMPlus.UseVisualStyleBackColor = true;
            this.BtnMPlus.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnMS
            // 
            this.BtnMS.Location = new System.Drawing.Point(222, 115);
            this.BtnMS.Name = "BtnMS";
            this.BtnMS.Size = new System.Drawing.Size(60, 35);
            this.BtnMS.TabIndex = 6;
            this.BtnMS.Text = "MS";
            this.BtnMS.UseVisualStyleBackColor = true;
            this.BtnMS.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnC
            // 
            this.BtnC.Location = new System.Drawing.Point(12, 160);
            this.BtnC.Name = "BtnC";
            this.BtnC.Size = new System.Drawing.Size(60, 35);
            this.BtnC.TabIndex = 7;
            this.BtnC.Text = "C";
            this.BtnC.UseVisualStyleBackColor = true;
            this.BtnC.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnCE
            // 
            this.BtnCE.Location = new System.Drawing.Point(82, 160);
            this.BtnCE.Name = "BtnCE";
            this.BtnCE.Size = new System.Drawing.Size(60, 35);
            this.BtnCE.TabIndex = 8;
            this.BtnCE.Text = "CE";
            this.BtnCE.UseVisualStyleBackColor = true;
            this.BtnCE.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnBackspace
            // 
            this.BtnBackspace.Location = new System.Drawing.Point(152, 160);
            this.BtnBackspace.Name = "BtnBackspace";
            this.BtnBackspace.Size = new System.Drawing.Size(60, 35);
            this.BtnBackspace.TabIndex = 9;
            this.BtnBackspace.Text = "<-";
            this.BtnBackspace.UseVisualStyleBackColor = true;
            this.BtnBackspace.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnDiv
            // 
            this.BtnDiv.Location = new System.Drawing.Point(222, 160);
            this.BtnDiv.Name = "BtnDiv";
            this.BtnDiv.Size = new System.Drawing.Size(60, 35);
            this.BtnDiv.TabIndex = 10;
            this.BtnDiv.Text = "/";
            this.BtnDiv.UseVisualStyleBackColor = true;
            this.BtnDiv.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn7
            // 
            this.Btn7.Location = new System.Drawing.Point(12, 205);
            this.Btn7.Name = "Btn7";
            this.Btn7.Size = new System.Drawing.Size(60, 35);
            this.Btn7.TabIndex = 11;
            this.Btn7.Text = "7";
            this.Btn7.UseVisualStyleBackColor = true;
            this.Btn7.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn8
            // 
            this.Btn8.Location = new System.Drawing.Point(82, 205);
            this.Btn8.Name = "Btn8";
            this.Btn8.Size = new System.Drawing.Size(60, 35);
            this.Btn8.TabIndex = 12;
            this.Btn8.Text = "8";
            this.Btn8.UseVisualStyleBackColor = true;
            this.Btn8.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn9
            // 
            this.Btn9.Location = new System.Drawing.Point(152, 205);
            this.Btn9.Name = "Btn9";
            this.Btn9.Size = new System.Drawing.Size(60, 35);
            this.Btn9.TabIndex = 13;
            this.Btn9.Text = "9";
            this.Btn9.UseVisualStyleBackColor = true;
            this.Btn9.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnMul
            // 
            this.BtnMul.Location = new System.Drawing.Point(222, 205);
            this.BtnMul.Name = "BtnMul";
            this.BtnMul.Size = new System.Drawing.Size(60, 35);
            this.BtnMul.TabIndex = 14;
            this.BtnMul.Text = "*";
            this.BtnMul.UseVisualStyleBackColor = true;
            this.BtnMul.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnSqr
            // 
            this.BtnSqr.Location = new System.Drawing.Point(292, 205);
            this.BtnSqr.Name = "BtnSqr";
            this.BtnSqr.Size = new System.Drawing.Size(96, 35);
            this.BtnSqr.TabIndex = 15;
            this.BtnSqr.Text = "Sqr";
            this.BtnSqr.UseVisualStyleBackColor = true;
            this.BtnSqr.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn4
            // 
            this.Btn4.Location = new System.Drawing.Point(12, 250);
            this.Btn4.Name = "Btn4";
            this.Btn4.Size = new System.Drawing.Size(60, 35);
            this.Btn4.TabIndex = 16;
            this.Btn4.Text = "4";
            this.Btn4.UseVisualStyleBackColor = true;
            this.Btn4.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn5
            // 
            this.Btn5.Location = new System.Drawing.Point(82, 250);
            this.Btn5.Name = "Btn5";
            this.Btn5.Size = new System.Drawing.Size(60, 35);
            this.Btn5.TabIndex = 17;
            this.Btn5.Text = "5";
            this.Btn5.UseVisualStyleBackColor = true;
            this.Btn5.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn6
            // 
            this.Btn6.Location = new System.Drawing.Point(152, 250);
            this.Btn6.Name = "Btn6";
            this.Btn6.Size = new System.Drawing.Size(60, 35);
            this.Btn6.TabIndex = 18;
            this.Btn6.Text = "6";
            this.Btn6.UseVisualStyleBackColor = true;
            this.Btn6.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnSub
            // 
            this.BtnSub.Location = new System.Drawing.Point(222, 250);
            this.BtnSub.Name = "BtnSub";
            this.BtnSub.Size = new System.Drawing.Size(60, 35);
            this.BtnSub.TabIndex = 19;
            this.BtnSub.Text = "-";
            this.BtnSub.UseVisualStyleBackColor = true;
            this.BtnSub.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnRev
            // 
            this.BtnRev.Location = new System.Drawing.Point(292, 250);
            this.BtnRev.Name = "BtnRev";
            this.BtnRev.Size = new System.Drawing.Size(96, 35);
            this.BtnRev.TabIndex = 20;
            this.BtnRev.Text = "Rev";
            this.BtnRev.UseVisualStyleBackColor = true;
            this.BtnRev.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn1
            // 
            this.Btn1.Location = new System.Drawing.Point(12, 295);
            this.Btn1.Name = "Btn1";
            this.Btn1.Size = new System.Drawing.Size(60, 35);
            this.Btn1.TabIndex = 21;
            this.Btn1.Text = "1";
            this.Btn1.UseVisualStyleBackColor = true;
            this.Btn1.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn2
            // 
            this.Btn2.Location = new System.Drawing.Point(82, 295);
            this.Btn2.Name = "Btn2";
            this.Btn2.Size = new System.Drawing.Size(60, 35);
            this.Btn2.TabIndex = 22;
            this.Btn2.Text = "2";
            this.Btn2.UseVisualStyleBackColor = true;
            this.Btn2.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn3
            // 
            this.Btn3.Location = new System.Drawing.Point(152, 295);
            this.Btn3.Name = "Btn3";
            this.Btn3.Size = new System.Drawing.Size(60, 35);
            this.Btn3.TabIndex = 23;
            this.Btn3.Text = "3";
            this.Btn3.UseVisualStyleBackColor = true;
            this.Btn3.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(222, 295);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(60, 35);
            this.BtnAdd.TabIndex = 24;
            this.BtnAdd.Text = "+";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnSign
            // 
            this.BtnSign.Location = new System.Drawing.Point(292, 295);
            this.BtnSign.Name = "BtnSign";
            this.BtnSign.Size = new System.Drawing.Size(96, 35);
            this.BtnSign.TabIndex = 25;
            this.BtnSign.Text = "+/-";
            this.BtnSign.UseVisualStyleBackColor = true;
            this.BtnSign.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Btn0
            // 
            this.Btn0.Location = new System.Drawing.Point(12, 340);
            this.Btn0.Name = "Btn0";
            this.Btn0.Size = new System.Drawing.Size(60, 35);
            this.Btn0.TabIndex = 26;
            this.Btn0.Text = "0";
            this.Btn0.UseVisualStyleBackColor = true;
            this.Btn0.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnSeparator
            // 
            this.BtnSeparator.Location = new System.Drawing.Point(82, 340);
            this.BtnSeparator.Name = "BtnSeparator";
            this.BtnSeparator.Size = new System.Drawing.Size(60, 35);
            this.BtnSeparator.TabIndex = 27;
            this.BtnSeparator.Text = "/";
            this.BtnSeparator.UseVisualStyleBackColor = true;
            this.BtnSeparator.Click += new System.EventHandler(this.ButtonClick);
            // 
            // BtnEqual
            // 
            this.BtnEqual.Location = new System.Drawing.Point(152, 340);
            this.BtnEqual.Name = "BtnEqual";
            this.BtnEqual.Size = new System.Drawing.Size(236, 35);
            this.BtnEqual.TabIndex = 28;
            this.BtnEqual.Text = "=";
            this.BtnEqual.UseVisualStyleBackColor = true;
            this.BtnEqual.Click += new System.EventHandler(this.ButtonClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 390);
            this.Controls.Add(this.BtnEqual);
            this.Controls.Add(this.BtnSeparator);
            this.Controls.Add(this.Btn0);
            this.Controls.Add(this.BtnSign);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.Btn3);
            this.Controls.Add(this.Btn2);
            this.Controls.Add(this.Btn1);
            this.Controls.Add(this.BtnRev);
            this.Controls.Add(this.BtnSub);
            this.Controls.Add(this.Btn6);
            this.Controls.Add(this.Btn5);
            this.Controls.Add(this.Btn4);
            this.Controls.Add(this.BtnSqr);
            this.Controls.Add(this.BtnMul);
            this.Controls.Add(this.Btn9);
            this.Controls.Add(this.Btn8);
            this.Controls.Add(this.Btn7);
            this.Controls.Add(this.BtnDiv);
            this.Controls.Add(this.BtnBackspace);
            this.Controls.Add(this.BtnCE);
            this.Controls.Add(this.BtnC);
            this.Controls.Add(this.BtnMS);
            this.Controls.Add(this.BtnMPlus);
            this.Controls.Add(this.BtnMR);
            this.Controls.Add(this.BtnMC);
            this.Controls.Add(this.MemoryStatusLabel);
            this.Controls.Add(this.DisplayLabel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Калькулятор простых дробей";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editMenu;
        private System.Windows.Forms.ToolStripMenuItem copyItem;
        private System.Windows.Forms.ToolStripMenuItem pasteItem;
        private System.Windows.Forms.ToolStripMenuItem viewMenu;
        private System.Windows.Forms.ToolStripMenuItem fractionModeItem;
        private System.Windows.Forms.ToolStripMenuItem numberModeItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem aboutItem;
        private System.Windows.Forms.Label DisplayLabel;
        private System.Windows.Forms.Label MemoryStatusLabel;
        private System.Windows.Forms.Button BtnMC;
        private System.Windows.Forms.Button BtnMR;
        private System.Windows.Forms.Button BtnMPlus;
        private System.Windows.Forms.Button BtnMS;
        private System.Windows.Forms.Button BtnC;
        private System.Windows.Forms.Button BtnCE;
        private System.Windows.Forms.Button BtnBackspace;
        private System.Windows.Forms.Button BtnDiv;
        private System.Windows.Forms.Button Btn7;
        private System.Windows.Forms.Button Btn8;
        private System.Windows.Forms.Button Btn9;
        private System.Windows.Forms.Button BtnMul;
        private System.Windows.Forms.Button BtnSqr;
        private System.Windows.Forms.Button Btn4;
        private System.Windows.Forms.Button Btn5;
        private System.Windows.Forms.Button Btn6;
        private System.Windows.Forms.Button BtnSub;
        private System.Windows.Forms.Button BtnRev;
        private System.Windows.Forms.Button Btn1;
        private System.Windows.Forms.Button Btn2;
        private System.Windows.Forms.Button Btn3;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnSign;
        private System.Windows.Forms.Button Btn0;
        private System.Windows.Forms.Button BtnSeparator;
        private System.Windows.Forms.Button BtnEqual;
    }
}
