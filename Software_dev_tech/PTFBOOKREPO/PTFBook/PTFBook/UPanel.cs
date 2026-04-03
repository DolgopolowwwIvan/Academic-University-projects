using System;
using System.Windows.Forms;

namespace PTFBook
{
    public partial class TPanel : Form
    {
         TControl control;

        public TPanel()
        {
            InitializeComponent();
            control = new TControl();
            
            // Загружаем данные при старте (из проекта товарища)
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

         void TPanel_Load(object sender, EventArgs e)
        {
        }

         void UpdateList()
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

         void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Пожалуйста, введите имя абонента", "Ошибка", 
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

         void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtName.Text.Trim();
            string searchPhone = txtPhone.Text.Trim();

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

         void btnDelete_Click(object sender, EventArgs e)
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

         void btnClear_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            lblResult.Text = "Книга очищена";
        }

         void btnAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

         void btnCreate_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите создать новую книгу? Все несохраненные данные будут стерты.", 
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

         void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string currentName = control.ReadFirstName(listBox1.SelectedIndex);
                string currentPhone = control.ReadPhone(listBox1.SelectedIndex);
                
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

         void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Файлы (*.*)|*.*";
            saveFileDialog.Title = "Сохранить книгу";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.SaveToFile();
                    lblResult.Text = $"Сохранено в файл";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

         void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Файлы (*.*)|*.*";
            openFileDialog.Title = "Открыть книгу";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.LoadFromFile(openFileDialog.FileName);
                    UpdateList();
                    lblResult.Text = $"Загружено: {control.RecordsCount()} записей";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки: {ex.Message}", "Ошибка", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

         void MenuAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

         void listBox1_DoubleClick(object sender, EventArgs e)
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

         void TPanel_Load(object sender, EventArgs e)
        {

        }

        // ��� �
         void UpdateList()
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

        // � "�"
         void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblResult.Text = "�� ��� �  �";
                return;
            }

            control.AddRecord(txtName.Text, txtPhone.Text);
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = $" �. : {control.RecordsCount()}";
        }

        // � ""
         void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtName.Text;
            string searchPhone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(searchName) && string.IsNullOrWhiteSpace(searchPhone))
            {
                lblResult.Text = " ��� ���  ��� �";
                return;
            }

            listBox1.Items.Clear();
            int found = 0;
            for (int i = 0; i < control.RecordsCount(); i++)
            {
                string firstName = control.ReadFirstName(i);
                string phone = control.ReadPhone(i);

                if ((!string.IsNullOrWhiteSpace(searchName) && firstName.Contains(searchName)) ||
                    (!string.IsNullOrWhiteSpace(searchPhone) && phone.Contains(searchPhone)))
                {
                    listBox1.Items.Add(control.ReadRecord(i));
                    found++;
                }
            }
            lblResult.Text = $": {found}";
        }

        // � ""
         void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"� . : {control.RecordsCount()}";
            }
            else
            {
                lblResult.Text = "� � ��� �";
            }
        }

        // � "�"
         void btnClear_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            lblResult.Text = "���  ";
        }

        // � ""
         void btnAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        // � ""
         void btnCreate_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = "  ��� ";
        }

        // � "�"
         void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                txtName.Text = control.ReadFirstName(listBox1.SelectedIndex);
                txtPhone.Text = control.ReadPhone(listBox1.SelectedIndex);
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = ": � � �  �";
            }
            else
            {
                lblResult.Text = "� � ��� ��";
            }
        }

        // � "��"
         void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "���  (*.json)|*.json|���  (*.*)|*.*";
            saveFileDialog.Title = "�� ��� ";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.CreateFile(saveFileDialog.FileName);
                    control.SaveBookToFile();
                    lblResult.Text = $"�� � ����: {saveFileDialog.FileName}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"� ���: {ex.Message}";
                }
            }
        }

        // � "��" (���� )
         void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "���  (*.json)|*.json|���  (*.*)|*.*";
            openFileDialog.Title = "�� ��� ";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.LoadFromFile(openFileDialog.FileName);
                    UpdateList();
                    lblResult.Text = $"�� �� : {openFileDialog.FileName}. : {control.RecordsCount()}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"� �: {ex.Message}";
                }
            }
        }

        // ���� "" -> "� ��"
         void MenuAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        //  ���� ��� �
         void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"� . : {control.RecordsCount()}";
            }
        }
    }
}