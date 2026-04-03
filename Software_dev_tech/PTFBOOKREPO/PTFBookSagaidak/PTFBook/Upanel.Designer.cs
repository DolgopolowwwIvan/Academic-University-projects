namespace PTFBook
{
    partial class Upanel
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
            button1 = new Button();
            listBox1 = new ListBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            btnAdd = new Button();
            button7 = new Button();
            label3 = new Label();
            label4 = new Label();
            txtName = new TextBox();
            txtNumber = new MaskedTextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(666, 55);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 0;
            button1.Text = "Очистить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnClear_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(13, 55);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(637, 229);
            listBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Location = new Point(666, 121);
            button2.Name = "button2";
            button2.Size = new Size(112, 34);
            button2.TabIndex = 2;
            button2.Text = "Сохранить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnSave_Click;
            // 
            // button3
            // 
            button3.Location = new Point(666, 186);
            button3.Name = "button3";
            button3.Size = new Size(112, 34);
            button3.TabIndex = 3;
            button3.Text = "Удалить";
            button3.UseVisualStyleBackColor = true;
            button3.Click += btnDelet_Click;
            // 
            // button4
            // 
            button4.Location = new Point(666, 250);
            button4.Name = "button4";
            button4.Size = new Size(112, 34);
            button4.TabIndex = 4;
            button4.Text = "Изменить";
            button4.UseVisualStyleBackColor = true;
            button4.Click += btnEdit_Click;
            // 
            // button5
            // 
            button5.Location = new Point(13, 305);
            button5.Name = "button5";
            button5.Size = new Size(112, 34);
            button5.TabIndex = 5;
            button5.Text = "Создать";
            button5.UseVisualStyleBackColor = true;
            button5.Click += btnCreate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(666, 413);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 34);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // button7
            // 
            button7.Location = new Point(666, 472);
            button7.Name = "button7";
            button7.Size = new Size(112, 34);
            button7.TabIndex = 8;
            button7.Text = "Найти";
            button7.UseVisualStyleBackColor = true;
            button7.Click += btnSearch_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 380);
            label3.Name = "label3";
            label3.Size = new Size(52, 25);
            label3.TabIndex = 10;
            label3.Text = "ФИО";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(400, 380);
            label4.Name = "label4";
            label4.Size = new Size(69, 25);
            label4.TabIndex = 11;
            label4.Text = "Номер";
            // 
            // txtName
            // 
            txtName.Location = new Point(13, 417);
            txtName.Name = "txtName";
            txtName.Size = new Size(350, 31);
            txtName.TabIndex = 12;
            // 
            // txtNumber
            // 
            txtNumber.Location = new Point(400, 417);
            txtNumber.Mask = "(999) 000-0000";
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(250, 31);
            txtNumber.TabIndex = 14;
            // 
            // Upanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(798, 544);
            Controls.Add(txtNumber);
            Controls.Add(txtName);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button7);
            Controls.Add(btnAdd);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(button1);
            MinimumSize = new Size(820, 600);
            Name = "Upanel";
            Text = "Телефонная книга";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private ListBox listBox1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button btnAdd;
        private Button button7;
        private Label label3;
        private Label label4;
        private TextBox txtName;
        private MaskedTextBox txtNumber;
    }
}