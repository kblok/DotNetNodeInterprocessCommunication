
const fs = require('fs');
const reader = fs.createReadStream(null, {fd: parseInt(process.argv[2], 10)});
const writer = fs.createWriteStream(null, {fd: parseInt(process.argv[3], 10)});

reader.on('data', data => writer.write('echo: ' + data + '\n'));

setInterval(()=> {}, 1000 * 60 * 60);