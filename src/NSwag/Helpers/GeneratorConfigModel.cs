using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Serilog;

namespace NSwag.Helpers;

public class GeneratorConfigModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? ConfigPath { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string DtoPath { get; set; } = "";

    public string FileCase { get; set; } = "";

    public void SetConfigPath(string arg, string currentDirectory)
    {
        var tmpPath = arg;
        tmpPath = tmpPath.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        if (Path.IsPathRooted(tmpPath))
        {
            ConfigPath = tmpPath;
            return;
        }

        if (arg.StartsWith("." + Path.DirectorySeparatorChar))
        {
            tmpPath = Path.GetFullPath(Path.Combine(currentDirectory, arg));
            ConfigPath = tmpPath;
            return;
        }

        tmpPath = Path.GetFullPath(Path.Combine(currentDirectory, arg));
        ConfigPath = tmpPath;
        List<string> files;
        if (string.IsNullOrWhiteSpace(ConfigPath))
        {
            files = Directory.GetFiles(currentDirectory, "*.nswag").ToList();
            if (files.Any())
            {
                ConfigPath = files[0];
            }
        }


        if (string.IsNullOrWhiteSpace(ConfigPath))
        {
            currentDirectory = AppContext.BaseDirectory;
            Log.Information("CurrentDirectory By [AppContext.BaseDirectory]:{0}", currentDirectory);
            files = Directory.GetFiles(currentDirectory, "*.nswag").ToList();
            if (files.Any())
            {
                ConfigPath = files[0];
            }
        }

        if (string.IsNullOrWhiteSpace(ConfigPath))
        {
            currentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!;
            Log.Information(
                "CurrentDirectory By [Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)]:{0}",
                currentDirectory);
            files = Directory.GetFiles(currentDirectory, "*.nswag").ToList();
            if (files.Any())
            {
                ConfigPath = files[0];
            }
        }

        if (string.IsNullOrWhiteSpace(ConfigPath))
        {
            currentDirectory = Directory.GetCurrentDirectory();
            Log.Information("CurrentDirectory By [Directory.GetCurrentDirectory()]:{0}", currentDirectory);
            files = Directory.GetFiles(currentDirectory, "*.nswag").ToList();
            if (files.Any())
            {
                ConfigPath = files[0];
            }
        }
    }
}