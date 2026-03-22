namespace PTFBook
{
    partial class TAboutBox
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
            lblTitle = new Label();
            lblVersion = new Label();
            lblAuthor = new Label();
            lblDescription = new Label();
            btnOK = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(120, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(300, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Телефонная книга";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            lblVersion.Location = new Point(120, 70);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(300, 25);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "Версия: 1.0";
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblAuthor
            // 
            lblAuthor.Location = new Point(120, 100);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(300, 25);
            lblAuthor.TabIndex = 2;
            lblAuthor.Text = "Автор: Студент";
            lblAuthor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDescription
            // 
            lblDescription.Location = new Point(120, 135);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(300, 80);
            lblDescription.TabIndex = 3;
            lblDescription.Text = "Приложение для ведения телефонной книги.\r\nДобавление, удаление, поиск и сохранение контактов.";
            lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(220, 230);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(100, 35);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(20, 20);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 80);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            pictureBox1.BackColor = SystemColors.ControlLight;
            // 
            // TAboutBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 280);
            Controls.Add(pictureBox1);
            Controls.Add(btnOK);
            Controls.Add(lblDescription);
            Controls.Add(lblAuthor);
            Controls.Add(lblVersion);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TAboutBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "О программе";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        private Label lblTitle;
        private Label lblVersion;
        private Label lblAuthor;
        private Label lblDescription;
        private Button btnOK;
        private PictureBox pictureBox1;
    }
}