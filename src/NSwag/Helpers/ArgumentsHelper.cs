using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NSwag.Helpers;

public class ArgumentsHelper
{
    public static GeneratorConfigModel ReadArgs(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var model = new GeneratorConfigModel();
        var queue = new Queue<string>(args);
        while (queue.Any())
        {
            var arg = queue.Dequeue();
            if (string.IsNullOrWhiteSpace(arg))
            {
                continue;
            }
            if (arg.Equals("-c", StringComparison.OrdinalIgnoreCase) || arg.Equals("--config", StringComparison.OrdinalIgnoreCase))
            {
                model.SetConfigPath(queue.Dequeue(), currentDirectory);
            }

            if (arg.Equals("-dp", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("--dto-path", StringComparison.OrdinalIgnoreCase))
            {
                model.DtoPath = queue.Dequeue();
            }
            if (arg.Equals("-fc", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("--file-case", StringComparison.OrdinalIgnoreCase))
            {
                model.FileCase = queue.Dequeue();
            }
        }
        return model;
    }
}