const {
    app,
    BrowserWindow,
    Menu
} = require('electron');

const path = require('path');
const url = require('url');
process.env.NODE_ENV = 'production'; // if you running publish.sh or publish.cmd, you must enable this.
const debugMode = false; // you must start all projects (DeviceManager.API, DeviceManager.Web, electron) with DEBUGGING manually, look it: ./vscode/launch.json

let mainWindow;
const os = require('os');
var apiProcess = null;

// #region Events
app.on('ready', init);

app.on('window-all-closed', function () {
    if (process.platform !== 'darwin') {
        app.quit()
    }
});

app.on('activate', function () {
    if (mainWindow === null) {
        createMainWindow()
    }
});

process.on('exit', function () {
    console.log('Exit electron application..');
    if (process.env.NODE_ENV === 'production') {
        apiProcess.kill();
    }
});
// #endregion

function init() {
    if (!debugMode) {
        startNetCoreApi();
    }
    createMainWindow();
}

function createMainWindow() {
    console.log('start');
    //create new window
    mainWindow = new BrowserWindow({
        width: 920,
        height: 600,
        frame: true,
        resizable: true
    });
    if (!debugMode) {
        mainWindow.loadURL('http://localhost:5001');
    } else {
        mainWindow.loadURL('http://localhost:4200');
    }

    // Quit app when closed
    mainWindow.on('close', function (e) {
        mainWindow = null;
    })
    // Create menu  
    const mainMenuTemplate = [{
        label: 'File',
        submenu: [{
            label: 'Quit',
            accelerator: process.platform == 'darwin' ? 'Command+Q' : 'Ctrl+q',
            click() {
                app.quit();
            }
        }]
    }];

    // if mac, add empty object to menu
    if (process.platform == 'darwin') {
        mainMenuTemplate.unshift({});
    }

    // Add developer tools item if not in production
    if (process.env.NODE_ENV !== 'production') {
        mainMenuTemplate.push({
            label: 'Developer Tools',
            submenu: [{
                    label: 'Toggle DevTools',
                    accelerator: process.platform == 'darwin' ? 'Command+i' : 'Ctrl+i',
                    click(item, focusedWindow) {
                        focusedWindow.toggleDevTools();
                    }
                },
                {
                    role: 'reload'
                }
            ]
        })
    }

    // Build menu from temmplate 
    const mainMenu = Menu.buildFromTemplate(mainMenuTemplate);
    // Insert menu
    Menu.setApplicationMenu(mainMenu);
}


function startNetCoreApi() {
    var spawn = require('child_process').spawn;

    var workingDirectory = path.join(__dirname, '../../dist/DeviceManager');

    if (process.env.NODE_ENV === 'production') {
        workingDirectory = path.join(__dirname, '../dist/DeviceManager');
    }

    var apiPath = path.join(workingDirectory, '/DeviceManager.API.exe');

    if (os.platform() === 'darwin') {
        apiPath = path.join(workingDirectory, '//DeviceManager.API');
    }

    console.log(apiPath);

    apiProcess = spawn(apiPath, {
        cwd: workingDirectory
    });

    apiProcess.stdout.on('data', (data) => {
        console.log(`stdout: ${data}`);
        if (mainWindow == null) {
            console.log('createMainWindow');
            createMainWindow();
        }
    });
};