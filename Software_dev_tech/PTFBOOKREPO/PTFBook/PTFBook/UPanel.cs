using System;
using System.Windows.Forms;

namespace PTFBook
{
    public partial class TPanel : Form
    {
        private TControl control;

        public TPanel()
        {
            InitializeComponent();
            control = new TControl();
            
            try
            {
                control.LoadFromFile();
            }
            catch (Exception ex)
            {
                // Игнорируем ошибку при первом запуске
            }

            UpdateList();
        }

        private void TPanel_Load(object sender, EventArgs e)
        {
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < control.RecordsCount(); i++)
            {
                string record = control.ReadRecord(i);
                if (!string.IsNullOrEmpty(record))
                {
                    listBox1.Items.Add(record);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Пожалуйста, введите имя абонента", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtPhone.MaskCompleted)
            {
                MessageBox.Show("Пожалуйста, введите номер телефона полностью", "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            control.AddRecord(name, phone);
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            txtName.Focus();
            lblResult.Text = $"Записей: {control.RecordsCount()}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtName.Text.Trim();
            string searchPhone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(searchName) && string.IsNullOrWhiteSpace(searchPhone))
            {
                MessageBox.Show("Введите имя или номер телефона для поиска", "Поиск", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int index = control.FindRecordUniversal(searchName, searchPhone);

            if (index != -1)
            {
                listBox1.SelectedIndex = index;
                listBox1.TopIndex = index;
                lblResult.Text = $"Найдено: {index + 1}";
            }
            else
            {
                MessageBox.Show("Совпадений не найдено", "Результат поиска", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblResult.Text = "Совпадений не найдено";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"Записей: {control.RecordsCount()}";
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления в списке", "Удаление", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            lblResult.Text = "Книга очищена";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите создать новую книгу? Все несохраненные данные будут стерты?", 
                "Новая книга", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                control.ClearBook();
                UpdateList();
                txtName.Clear();
                txtPhone.Clear();
                lblResult.Text = "Создана новая книга";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                if (!txtPhone.MaskCompleted)
                {
                    MessageBox.Show("Пожалуйста, введите номер телефона полностью", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                control.UpdateRecord(listBox1.SelectedIndex, txtName.Text, txtPhone.Text);
                UpdateList();
                lblResult.Text = "Запись изменена";
            }
            else
            {
                MessageBox.Show("Сначала выберите строку в списке!", "Редактирование", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Файлы (*.*)|*.*";
            saveFileDialog.Title = "Сохранить книгу";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.SaveToFile();
                    lblResult.Text = "Сохранено в файл";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"Записей: {control.RecordsCount()}";
            }
        }
    }
}
