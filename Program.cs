// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

string result = GetUniqueIdentifier();
Console.WriteLine("Unique computer identifier: " + result);

static string GetUniqueIdentifier()
{
    StringBuilder sb = new StringBuilder();
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        ManagementClass mc = new ManagementClass("Win32_NetworkAdapter");
        ManagementObjectCollection moc = mc.GetInstances();
        foreach (ManagementObject mo in moc)
        {
            string macAddress = mo["MacAddress"]?.ToString();
            if (!string.IsNullOrEmpty(macAddress))
            {
                sb.Append(macAddress);
            }
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = "ifconfig";
        p.Start();

        string output = p.StandardOutput.ReadToEnd();
        string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            if (line.StartsWith("ether"))
            {
                string macAddress = line.Split(' ')[1].Trim();
                sb.Append(macAddress);
            }
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.FileName = "ifconfig";
        p.Start();

        string output = p.StandardOutput.ReadToEnd();
        string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            if (line.StartsWith("ether"))
            {
                string macAddress = line.Split(' ')[1].Trim();
                sb.Append(macAddress);
            }
        }
    }

    string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(sb.ToString()));
    return result;
}
