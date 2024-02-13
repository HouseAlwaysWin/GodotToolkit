using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using System.Reflection;

namespace GodotToolkit;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        DisplayVersionInTitle();

        this.MinimumSize = new System.Drawing.Size(1250, 800);
        var blazor = new BlazorWebView()
        {
            Dock = DockStyle.Fill,
            HostPage = "wwwroot/index.html",
            Services = Startup.Services!
        };

        blazor.RootComponents.Add<Main>("#app");
        Controls.Add(blazor);
    }
    private void DisplayVersionInTitle()
    {
        // 获取当前程序集的版本信息
        Version version = Assembly.GetEntryAssembly()?.GetName().Version;

        if (version != null)
        {
            string versionString = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            Text = $"Godot Toolkit v{versionString}";
        }
    }
}
