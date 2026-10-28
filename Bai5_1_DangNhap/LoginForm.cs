using MySql.Data.MySqlClient;

namespace Bai5_1_DangNhap;

public sealed class LoginForm : Form
{
    private readonly TextBox txtUsername = new();
    private readonly TextBox txtPassword = new();
    private readonly CheckBox chkShowPassword = new();
    private readonly string connectionString =
        "Server=localhost;Port=3306;Database=ql_dangnhap;Uid=root;Pwd=YOUR_MYSQL_PASSWORD;";

    public LoginForm()
    {
        Text = "Đăng nhập";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(560, 340);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.FromArgb(248, 249, 252);
        Font = new Font("Segoe UI", 10F);

        var title = new Label { Text = "ĐĂNG NHẬP", Font = new Font("Segoe UI", 22F, FontStyle.Bold), AutoSize = true, Location = new Point(175, 35) };
        var lblUsername = new Label { Text = "Tên đăng nhập:", AutoSize = true, Location = new Point(70, 112) };
        txtUsername.SetBounds(210, 106, 260, 30);
        txtUsername.Text = "nguyen";
        var lblPassword = new Label { Text = "Mật khẩu:", AutoSize = true, Location = new Point(70, 158) };
        txtPassword.SetBounds(210, 152, 260, 30);
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.Text = "123456";

        chkShowPassword.Text = "Hiện mật khẩu";
        chkShowPassword.AutoSize = true;
        chkShowPassword.Location = new Point(210, 190);
        chkShowPassword.CheckedChanged += (_, _) => txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

        var btnLogin = new Button { Text = "Đăng nhập", Location = new Point(210, 235), Size = new Size(125, 38), BackColor = Color.FromArgb(35, 110, 230), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.Click += BtnLogin_Click;
        var btnExit = new Button { Text = "Thoát", Location = new Point(355, 235), Size = new Size(115, 38) };
        btnExit.Click += (_, _) => Close();

        AcceptButton = btnLogin;
        Controls.AddRange(new Control[] { title, lblUsername, txtUsername, lblPassword, txtPassword, chkShowPassword, btnLogin, btnExit });
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            const string sql = "SELECT COUNT(*) FROM NguoiDung WHERE username = @username AND password = @password";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            MessageBox.Show(count > 0 ? "Đăng nhập thành công!" : "Sai tên đăng nhập hoặc mật khẩu!",
                count > 0 ? "Thông báo" : "Lỗi", MessageBoxButtons.OK,
                count > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể kết nối hoặc truy vấn MySQL.\n" + ex.Message, "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
