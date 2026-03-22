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
            UpdateList();
        }

        private void TPanel_Load(object sender, EventArgs e)
        {

        }

        // Обновление списка
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

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblResult.Text = "Заполните ФИО и номер телефона";
                return;
            }

            control.AddRecord(txtName.Text, txtPhone.Text);
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = $"Абонент добавлен. Всего: {control.RecordsCount()}";
        }

        // Кнопка "Найти"
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtName.Text;
            string searchPhone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(searchName) && string.IsNullOrWhiteSpace(searchPhone))
            {
                lblResult.Text = "Введите ФИО или номер для поиска";
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
            lblResult.Text = $"Найдено: {found}";
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"Запись удалена. Всего: {control.RecordsCount()}";
            }
            else
            {
                lblResult.Text = "Выберите запись для удаления";
            }
        }

        // Кнопка "Очистить"
        private void btnClear_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            lblResult.Text = "Телефонная книга очищена";
        }

        // Кнопка "Создать"
        private void btnCreate_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = "Создана новая телефонная книга";
        }

        // Кнопка "Изменить"
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                txtName.Text = control.ReadFirstName(listBox1.SelectedIndex);
                txtPhone.Text = control.ReadPhone(listBox1.SelectedIndex);
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = "Редактирование: измените данные и нажмите Добавить";
            }
            else
            {
                lblResult.Text = "Выберите запись для изменения";
            }
        }

        // Кнопка "Сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Телефонная книга (*.json)|*.json|Все файлы (*.*)|*.*";
            saveFileDialog.Title = "Сохранить телефонную книгу";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.CreateFile(saveFileDialog.FileName);
                    control.SaveBookToFile();
                    lblResult.Text = $"Сохранено в файл: {saveFileDialog.FileName}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"Ошибка сохранения: {ex.Message}";
                }
            }
        }

        // Кнопка "Загрузить" (если нужна)
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Телефонная книга (*.json)|*.json|Все файлы (*.*)|*.*";
            openFileDialog.Title = "Загрузить телефонную книгу";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.LoadFromFile(openFileDialog.FileName);
                    UpdateList();
                    lblResult.Text = $"Загружено из файла: {openFileDialog.FileName}. Всего: {control.RecordsCount()}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"Ошибка загрузки: {ex.Message}";
                }
            }
        }

        // Меню "Справка" -> "О программе"
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        // Двойной клик для удаления
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"Запись удалена. Всего: {control.RecordsCount()}";
            }
        }
    }
}