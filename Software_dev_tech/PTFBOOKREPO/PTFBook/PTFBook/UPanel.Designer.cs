using System;
using System.Windows.Forms;

namespace PTFBook
{
    partial class TPanel
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;

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
            btnSave = new Button();
            btnClear = new Button();
            listBox1 = new ListBox();
            btnDelete = new Button();
            btnEdit = new Button();
            btnAbout = new Button();
            btnCreate = new Button();
            btnSearch = new Button();
            btnAdd = new Button();
            txtName = new TextBox();
            txtPhone = new MaskedTextBox();
            lblResult = new Label();
            menuStrip1 = new MenuStrip();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.Location = new Point(698, 122);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 63);
            btnSave.TabIndex = 3;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(698, 37);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(130, 63);
            btnClear.TabIndex = 4;
            btnClear.Text = "Очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(31, 37);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(643, 319);
            listBox1.TabIndex = 5;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(698, 206);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(130, 63);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(698, 291);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(130, 63);
            btnEdit.TabIndex = 6;
            btnEdit.Text = "Изменить";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(31, 377);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(130, 63);
            btnCreate.TabIndex = 8;
            btnCreate.Text = "Создать";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(698, 560);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(130, 63);
            btnSearch.TabIndex = 9;
            btnSearch.Text = "Найти";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnAbout
            // 
            btnAbout.Location = new Point(698, 645);
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new Size(130, 35);
            btnAbout.TabIndex = 15;
            btnAbout.Text = "Справка";
            btnAbout.UseVisualStyleBackColor = true;
            btnAbout.Click += MenuAbout_Click;

            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(698, 471);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(130, 63);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(31, 471);
            txtName.Name = "txtName";
            txtName.PlaceholderText = "ФИО";
            txtName.Size = new Size(347, 23);
            txtName.TabIndex = 11;
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(402, 471);
            txtPhone.Mask = "8 (999) 000-00-00";
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(272, 23);
            txtPhone.TabIndex = 12;
            // 
            // lblResult
            // 
            lblResult.Location = new Point(31, 560);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(643, 63);
            lblResult.TabIndex = 13;
            lblResult.Text = "Готово";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(879, 24);
            menuStrip1.TabIndex = 14;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(65, 20);
            menuHelp.Text = "Справка";
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(149, 22);
            menuAbout.Text = "О программе";
            menuAbout.Click += MenuAbout_Click;
            // 
            // TPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(879, 700);
            Controls.Add(menuStrip1);
            Controls.Add(lblResult);
            Controls.Add(txtPhone);
            Controls.Add(txtName);
            Controls.Add(btnAdd);
            Controls.Add(btnSearch);
            Controls.Add(btnCreate);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(listBox1);
            Controls.Add(btnClear);
            Controls.Add(btnSave);
            Controls.Add(btnAbout);
            MainMenuStrip = menuStrip1;
            Name = "TPanel";
            Text = "Телефонная книга";
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnSave;
        private Button btnClear;
        private ListBox listBox1;
        private Button btnDelete;
        private Button btnEdit;
        private Button btnCreate;
        private Button btnSearch;
        private Button btnAdd;
        private TextBox txtName;
        private MaskedTextBox txtPhone;
        private Label lblResult;
        private Button btnAbout;
    }
}