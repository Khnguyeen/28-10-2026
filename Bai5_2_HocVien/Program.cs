namespace Bai5_2_HocVien;
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new HocVienForm());
    }
}
