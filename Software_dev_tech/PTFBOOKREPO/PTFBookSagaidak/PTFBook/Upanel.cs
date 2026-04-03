using System;
using System.Windows.Forms;

namespace PTFBook
{
    public partial class Upanel : Form
    {
        private TControl controller = new TControl();

        public Upanel()
        {
            InitializeComponent();
            this.Text = "Телефонная книга";

            controller.LoadFromFile();
            UpdateList();
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < controller.RecordsCount(); i++)
            {
                listBox1.Items.Add(controller.ReadRecord(i));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string sName = txtName.Text.Trim();
            string sNumber = txtNumber.Text.Trim();

            if (!string.IsNullOrWhiteSpace(sName))
            {
                controller.AddRecord(sName, sNumber);
                UpdateList();

                txtName.Clear();
                txtNumber.Clear();
                txtName.Focus();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите имя абонента");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            controller.ClearBook();
            UpdateList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            controller.SaveToFile();
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                controller.DeleteAt(listBox1.SelectedIndex);
                UpdateList();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления в списке");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index != -1)
            {
                controller.UpdateRecord(index, txtName.Text, txtNumber.Text);
                UpdateList();
            }
            else
            {
                MessageBox.Show("Сначала выберите строку в списке!");
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите создать новую книгу? Все несохраненные данные будут стерты.",
                                         "Новая книга", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                controller.ClearBook();
                UpdateList();
                txtName.Clear();
                txtNumber.Clear();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sName = txtName.Text.Trim();
            string sNumber = txtNumber.Text.Trim();

            if (string.IsNullOrWhiteSpace(sName) && string.IsNullOrWhiteSpace(sNumber))
            {
                MessageBox.Show("Введите имя или номер телефона для поиска");
                return;
            }

            int index = controller.FindRecordUniversal(sName, sNumber);

            if (index != -1)
            {
                listBox1.SelectedIndex = index;
                listBox1.TopIndex = index;
            }
            else
            {
                MessageBox.Show("Совпадений не найдено");
            }
        }
    }
}
