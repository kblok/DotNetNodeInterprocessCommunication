
const readline = require('readline');
const fs = require('fs');
const reader = fs.createReadStream(null, {fd: 3});
const writer = fs.createWriteStream(null, {fd: 4});

reader.on('data', data => writer.write('echo: ' + data));


setInterval(()=> {}, 1000 * 60 * 60);