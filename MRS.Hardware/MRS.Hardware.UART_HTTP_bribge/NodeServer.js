var net = require('net');
var Url = require('url');

var httpConnections = {};
var tcpConnections = {};

Commands = {


}


function ProcessHttpCommand(req, res) {
    if (req.method == "POST") {
        var port = req.headers.port;
        var host = req.headers.host;
        var url = "http://" + host + ":" + port + req.url;
        url = Url.parse(url.toLowerCase(), true);
        if (url.query.command) {
            if (Commands[url.query.command]) {
                Commands[url.query.command]();
            }
        }
        res.end(200);
    }
    else {
        var key = Math.random() + "";
        httpConnections[key] = { request: req, response: res };
    }
}


function SendToHTTP(req, res) {
    var command = req.url
}

function SendToUART(data) {
    for (var key in tcpConnections) {
        var sock = tcpConnections[key];
        if (sock) {
            sock.write(data);
        }
    }
}

var server = net.createServer(function (c) { //'connection' listener
    console.log(c);
    var key = Math.random() + "";    
    tcpConnections[key] = c;
    c.on('data', function (data) {
        SendToHTTP(data);
    });
    c.on('end', function () {
        console.log('server disconnected');
        tcpConnections
        delete tcpConnections[key];
    });
});

server.listen(4000, function () { //'listening' listener
    console.log('server bound');
});


var http = require('http');
http.createServer(ProcessHttpCommand).listen(5000, '127.0.0.1');
console.log('Http Server running at http://127.0.0.1:5000/');

