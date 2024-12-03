using System;
using energy_raport_app;
using Eto.Drawing;
using Eto.Forms;

public class UserDashboard : Form
{
    private User _user;

    public UserDashboard(User user)
    {
        _user = user;

        // Window settings
        Title = "User Dashboard";
        MinimumSize = new Size(600, 400);

        // Welcome label
        var label = new Label
        {
            Text = $"Hello {_user.Naam}!",
            Font = SystemFonts.Bold(14),
            TextAlignment = TextAlignment.Center
        };

        // WebView control for displaying HTML content
        var webView = new WebView
        {
            Size = new Size(900, 500)
        };

        // Add content to the layout
        Content = new StackLayout
        {
            Padding = 10,
            Spacing = 10,
            Items =
            {
                label,
                webView
            }
        };

        // Menubalk
        Menu = CreateMenu();
        webView.LoadHtml(GenerateHelloWorldHtml());
    }
    private string GenerateHelloWorldHtml()
    {
        return @"
        
        <!DOCTYPE html>
<html>
  <head>
    <title>ECharts Example</title>
    <script src=""https://cdn.jsdelivr.net/npm/echarts@5.0.2/dist/echarts.min.js""></script>
  </head>
  <body>
    <div id=""echart"" style=""width: 600px; height: 400px;""></div>
    <script>
      var myChart = echarts.init(document.getElementById('echart'));

      var option = {
        title: {
          text: 'Company Performance'
        },
        xAxis: {
          type: 'category',
          data: ['2013', '2014', '2015', '2016']
        },
        yAxis: {
          type: 'value'
        },
        series: [{
          data: [1000, 1170, 660, 1030],
          type: 'line'
        }]
      };

      myChart.setOption(option);
    </script>
  </body>
</html>";
    }

    private MenuBar CreateMenu()
    {
        var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
        quitCommand.Executed += (sender, e) => Application.Instance.Quit();

        var aboutCommand = new Command { MenuText = "About..." };
        aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

        return new MenuBar
        {
            ApplicationItems =
            {
                new ButtonMenuItem { Text = "&Preferences..." }
            },
            QuitItem = quitCommand,
            AboutItem = aboutCommand
        };
    }
}