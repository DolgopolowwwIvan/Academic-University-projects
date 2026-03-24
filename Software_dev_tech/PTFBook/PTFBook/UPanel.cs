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
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblResult.Text = "��������� ��� � ����� ��������";
                return;
            }

            control.AddRecord(txtName.Text, txtPhone.Text);
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = $"������� ��������. �����: {control.RecordsCount()}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchName = txtName.Text;
            string searchPhone = txtPhone.Text;

            if (string.IsNullOrWhiteSpace(searchName) && string.IsNullOrWhiteSpace(searchPhone))
            {
                lblResult.Text = "������� ��� ��� ����� ��� ������";
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
            lblResult.Text = $"�������: {found}";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"������ �������. �����: {control.RecordsCount()}";
            }
            else
            {
                lblResult.Text = "�������� ������ ��� ��������";
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            lblResult.Text = "���������� ����� �������";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            control.ClearBook();
            UpdateList();
            txtName.Clear();
            txtPhone.Clear();
            lblResult.Text = "������� ����� ���������� �����";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                txtName.Text = control.ReadFirstName(listBox1.SelectedIndex);
                txtPhone.Text = control.ReadPhone(listBox1.SelectedIndex);
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = "��������������: �������� ������ � ������� ��������";
            }
            else
            {
                lblResult.Text = "�������� ������ ��� ���������";
            }
        }

        // ������ "���������"
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "���������� ����� (*.json)|*.json|��� ����� (*.*)|*.*";
            saveFileDialog.Title = "��������� ���������� �����";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.CreateFile(saveFileDialog.FileName);
                    control.SaveBookToFile();
                    lblResult.Text = $"��������� � ����: {saveFileDialog.FileName}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"������ ����������: {ex.Message}";
                }
            }
        }

        // ������ "���������" (���� �����)
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "���������� ����� (*.json)|*.json|��� ����� (*.*)|*.*";
            openFileDialog.Title = "��������� ���������� �����";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    control.LoadFromFile(openFileDialog.FileName);
                    UpdateList();
                    lblResult.Text = $"��������� �� �����: {openFileDialog.FileName}. �����: {control.RecordsCount()}";
                }
                catch (Exception ex)
                {
                    lblResult.Text = $"������ ��������: {ex.Message}";
                }
            }
        }

        // ���� "�������" -> "� ���������"
        private void MenuAbout_Click(object sender, EventArgs e)
        {
            TAboutBox aboutBox = new TAboutBox();
            aboutBox.ShowDialog(this);
        }

        // ������� ���� ��� ��������
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                control.DeleteSelectedRecord(listBox1.SelectedIndex);
                UpdateList();
                lblResult.Text = $"������ �������. �����: {control.RecordsCount()}";
            }
        }
    }
}