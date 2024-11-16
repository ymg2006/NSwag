import { exec } from 'child_process';
import fs from 'fs';
import path from 'path';

const cwd = path.dirname(process.argv[1]);
const configPath = path.join(cwd, `service.config.nswag`);
const configFileAccess = fs.existsSync(configPath);
if (!configFileAccess) {
    const errorMessage = "Target service [" + targetService + "] config dose not exists, \n Please confirm path [" + configPath + "] is exist.";
    throw new Error(errorMessage);
}

let execPath = "";
if (process.platform.startsWith("win")) {
    execPath = path.join(cwd, 'bin', 'win-x64', 'NSwag.exe');
} else if (process.platform.startsWith("linux")) {
    execPath = path.join(cwd, 'bin', 'linux-x64', 'NSwag');
    fs.access(execPath, fs.constants.X_OK, function (error) {
        if (error) {
            exec('sudo chmod', ['+', 'x', execPath], function (err, stdout, stderr) {
                if (err) {
                    console.error(err);
                }
            })
        }
    });
} else if (process.platform.startsWith("mac")) {
    execPath = path.join(cwd, 'bin', 'osx-x64', 'NSwag');
    fs.access(execPath, fs.constants.X_OK, function (error) {
        if (error) {
            exec('sudo chmod', ['+', 'x', execPath], function (err, stdout, stderr) {
                if (err) {
                    console.error(err);
                }
            })
        }
    });
}
exec(`"${execPath}" -c "nswag/service.config.nswag" -fc "kebab-case"`,
    function (err, stdout, stderr) {
        if (err) {
            console.error(err);
        }
        console.log(stdout)
    });
