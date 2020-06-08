const readline = require('readline');
const childProcess = require('child_process');

const server = childProcess.spawn(
  'node',
  ['../child/index.js'],
  {
    stdio : ['inherit', 'inherit', 'inherit', 'pipe', 'pipe']
  }
);

const reader = server.stdio[4];
const writer = server.stdio[3]; 

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

reader.on('data', data => console.log(data.toString()));

var waitForMessage = function () {
  rl.question('', (message) => {
  
    if(message === 'exit') {
      rl.close();
    }
    writer.write(message + '\n');
    waitForMessage();
  });
};

waitForMessage(); 