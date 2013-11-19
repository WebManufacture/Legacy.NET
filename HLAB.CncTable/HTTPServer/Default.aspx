<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MRS.Hardware.HTTPServer._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type">
    <link type='text/css' rel='stylesheet' href="http://services.web-manufacture.net/Styles/System.default.css" />

    <script type="text/javascript" src="http://services.web-manufacture.net/Base/v1.3/Utils.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/DOM.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Events.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Log.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Url.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Ajax.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Jasp.js"></script>

    <script type="text/javascript" src="http://Services.web-manufacture.net/Base/v1.3/Modules.js"></script>

    <style type="text/css">
        #log .log .log-item {
            display: inline-block;
            padding: 3px 5px;
        }
        
        #log .log .date-time {
            width: 150px;
            color: navy;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript">
	    function init() {
	        getState();
	        window.setInterval(getState, 1000);
	        getLogs();
	    }

	    function getLogs() {
	        var url = "state.ashx?type=logs";
	        if (window.lastTime) {
	            url += "&lastdate=" + window.lastTime.valueOf();
	        }
	        window.lastTime = new Date();
	        var rq = AX.Get(url, logsLoaded);
	        window.setTimeout(getLogs, 2000);
	    }

	    function logsLoaded(result) {
	        result = JSON.parse(result);
	        if (result.length > 0) {
	            for (var i = 0; i < result.length; i++) {
	                var lg = DOM("#log").div(".log");
	                lg.div(".log-item.date-time", new Date(result[i].date).formatTime());
	                lg.div(".log-item.message", new Date(result[i].message));
	            }
	        }
	    }

        function getState() {
		    var rq = AX.Get("state.ashx", stateLoaded);
	        rq.onerror = stateError;
	    }
        
        function stateError() {
            DOM("#state").set("error in request").add(".error");
	    }

	    function stateLoaded(result) {
	        result = JSON.parse(result);
	        DOM("#state").set(result.state).add("." + result.state);
	        if (result.lastState) {
	            result = lastState.lastState;
	            DOM("#XSteps").set(result.xSteps);
	            DOM("#YSteps").set(result.ySteps);
	            DOM("#XLimit").set(result.xLimit);
	            DOM("#YLimit").set(result.yLimit);
	            DOM("#MState").set(result.state);
	        }
	    }

	    WS.DOMload(init);
    </script>
    <div class='left-panel' style="float: left; width: 200px;">
        <asp:DropDownList runat="server" ID="ddlPorts" style="width: 100%;"/>
        <asp:Button ID="btnConnect" runat="server" Text="Connect" onclick="btnConnect_Click" />
        <div id="state" style='font-size: 24px;'></div>
        <div class="coords">X: <span id="XSteps"></span></div>
        <div class="coords">Y: <span id="YSteps"></span></div>
        <div class="coords">XL: <span id="XLimit"></span></div>
        <div class="coords">YL: <span id="YLimit"></span></div>
        <div class="coords">State: <span id="MState"></span></div>
    </div>
    <div id="line">
    </div>
    <div id="log">
    </div>
    </form>
</body>
</html>
