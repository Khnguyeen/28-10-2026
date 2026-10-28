using MySql.Data.MySqlClient;
using System.Data;

namespace Bai5_2_HocVien;

public sealed class HocVienForm : Form
{
    private readonly TextBox txtName = new();
    private readonly TextBox txtPhone = new();
    private readonly DateTimePicker dtpBirthDate = new();
    private readonly ComboBox cboCourse = new();
    private readonly RadioButton rdoMale = new();
    private readonly RadioButton rdoFemale = new();
    private readonly CheckBox chkPaid = new();
    private readonly ErrorProvider errorProvider = new();
    private readonly DataGridView dgvStudents = new();
    private readonly string connectionString =
        "Server=localhost;Port=3306;Database=ql_hocvien;Uid=root;Pwd=YOUR_MYSQL_PASSWORD;";

    public HocVienForm()
    {
        Text = "Quản lý đăng ký học viên";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1000, 650);
        MinimumSize = new Size(900, 600);
        BackColor = Color.FromArgb(248, 249, 252);
        Font = new Font("Segoe UI", 9.5F);
        BuildLayout();
        Load += (_, _) => LoadStudents();
    }

    private void BuildLayout()
    {
        var title = new Label { Text = "ĐĂNG KÝ HỌC VIÊN", Font = new Font("Segoe UI", 20F, FontStyle.Bold), AutoSize = true, Location = new Point(30, 20) };
        var group = new GroupBox { Text = "Thông tin học viên", Location = new Point(25, 70), Size = new Size(950, 230), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };

        AddLabel(group, "Họ tên:", 25, 35);
        txtName.SetBounds(145, 30, 300, 30);
        group.Controls.Add(txtName);
        AddLabel(group, "Số điện thoại:", 500, 35);
        txtPhone.SetBounds(625, 30, 280, 30);
        group.Controls.Add(txtPhone);

        AddLabel(group, "Ngày sinh:", 25, 80);
        dtpBirthDate.SetBounds(145, 75, 300, 30);
        dtpBirthDate.Format = DateTimePickerFormat.Custom;
        dtpBirthDate.CustomFormat = "dd/MM/yyyy";
        dtpBirthDate.MaxDate = DateTime.Today;
        group.Controls.Add(dtpBirthDate);

        AddLabel(group, "Khóa học:", 500, 80);
        cboCourse.SetBounds(625, 75, 280, 30);
        cboCourse.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCourse.Items.AddRange(new object[] { "Lập trình C#", "Java", "Thiết kế Web", "Cơ sở dữ liệu" });
        cboCourse.SelectedIndex = 0;
        group.Controls.Add(cboCourse);

        AddLabel(group, "Giới tính:", 25, 125);
        rdoMale.Text = "Nam";
        rdoMale.SetBounds(145, 120, 70, 30);
        rdoFemale.Text = "Nữ";
        rdoFemale.SetBounds(225, 120, 70, 30);
        group.Controls.AddRange(new Control[] { rdoMale, rdoFemale });

        chkPaid.Text = "Đã đóng học phí";
        chkPaid.SetBounds(625, 120, 180, 30);
        group.Controls.Add(chkPaid);

        var btnRegister = new Button { Text = "Lưu đăng ký", Location = new Point(145, 170), Size = new Size(130, 35), BackColor = Color.FromArgb(35, 110, 230), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        btnRegister.FlatAppearance.BorderSize = 0;
        btnRegister.Click += BtnRegister_Click;
        var btnClear = new Button { Text = "Làm mới", Location = new Point(290, 170), Size = new Size(110, 35) };
        btnClear.Click += (_, _) => ClearForm();
        var btnReload = new Button { Text = "Tải danh sách", Location = new Point(415, 170), Size = new Size(130, 35) };
        btnReload.Click += (_, _) => LoadStudents();
        group.Controls.AddRange(new Control[] { btnRegister, btnClear, btnReload });

        var listTitle = new Label { Text = "DANH SÁCH HỌC VIÊN ĐÃ ĐĂNG KÝ", Font = new Font("Segoe UI", 12F, FontStyle.Bold), AutoSize = true, Location = new Point(30, 315) };
        dgvStudents.SetBounds(25, 350, 950, 270);
        dgvStudents.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvStudents.ReadOnly = true;
        dgvStudents.AllowUserToAddRows = false;
        dgvStudents.AllowUserToDeleteRows = false;
        dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvStudents.MultiSelect = false;
        dgvStudents.BackgroundColor = Color.White;
        dgvStudents.RowHeadersVisible = false;

        Controls.AddRange(new Control[] { title, group, listTitle, dgvStudents });
        errorProvider.ContainerControl = this;
    }

    private static void AddLabel(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label { Text = text, AutoSize = true, Location = new Point(x, y + 5) });
    }

    private void BtnRegister_Click(object? sender, EventArgs e)
    {
        errorProvider.Clear();
        string name = txtName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(name))
        {
            errorProvider.SetError(txtName, "Vui lòng nhập họ tên học viên.");
            valid = false;
        }

        if (!string.IsNullOrWhiteSpace(phone) &&
            (phone.Any(c => !char.IsDigit(c)) || phone.Length < 9 || phone.Length > 15))
        {
            errorProvider.SetError(txtPhone, "Số điện thoại phải có 9–15 chữ số.");
            valid = false;
        }
        if (!valid) return;

        string gender = rdoMale.Checked ? "Nam" : rdoFemale.Checked ? "Nữ" : "Chưa chọn";
        try
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            const string sql = "INSERT INTO HocVien (ho_ten, sdt, ngay_sinh, khoa_hoc, gioi_tinh, da_dong_hoc_phi) VALUES (@name, @phone, @birthDate, @course, @gender, @paid)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : phone);
            cmd.Parameters.AddWithValue("@birthDate", dtpBirthDate.Value.Date);
            cmd.Parameters.AddWithValue("@course", cboCourse.Text);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@paid", chkPaid.Checked ? 1 : 0);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Đăng ký thành công, dữ liệu đã được lưu vào MySQL.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            LoadStudents();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể lưu dữ liệu vào MySQL.\n" + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadStudents()
    {
        try
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            const string sql = "SELECT id AS 'Mã', ho_ten AS 'Họ tên', sdt AS 'Số điện thoại', ngay_sinh AS 'Ngày sinh', khoa_hoc AS 'Khóa học', gioi_tinh AS 'Giới tính', CASE WHEN da_dong_hoc_phi = 1 THEN 'Đã đóng' ELSE 'Chưa đóng' END AS 'Học phí' FROM HocVien ORDER BY id DESC";
            using var adapter = new MySqlDataAdapter(sql, conn);
            var table = new DataTable();
            adapter.Fill(table);
            dgvStudents.DataSource = table;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải danh sách học viên.\n" + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearForm()
    {
        errorProvider.Clear();
        txtName.Clear();
        txtPhone.Clear();
        dtpBirthDate.Value = DateTime.Today;
        if (cboCourse.Items.Count > 0) cboCourse.SelectedIndex = 0;
        rdoMale.Checked = false;
        rdoFemale.Checked = false;
        chkPaid.Checked = false;
        txtName.Focus();
    }
}
