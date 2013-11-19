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
        #log .text {
            display: inline-block;
        }
        
        #log .date {
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
	    }

        function getState() {
		    var rq = AX.Get("state.ashx", stateLoaded);
	        rq.onerror = stateError;
	    }
        
        function stateError() {
	        DOM("#log").div(".error", "error in request");
	        window.setTimeout(getState, 5000);
	    }

	    function stateLoaded(result) {
		    result = JSON.parse(result);
		    if (window.lastState) {
			    if (result.state == lastState) {
        		    window.setTimeout(getState, 8000);
				    return;
			    }
		    }
		    lastState = result.state;
		    var nfo = DOM("#log").div(".info");
		    nfo.div(".date.text", (new Date()).formatTime());
		    nfo.div(".state.text", result.statename);
		    window.setTimeout(getState, 4000);
	    }

	    WS.DOMload(init);
    </script>
    <div id="line">
    </div>
    <div id="log">
    </div>
    <p>
    <asp:Button ID="Button1" runat="server" Text="state" onclick="Button1_Click" />
    <asp:Button ID="Button2" runat="server" Text="STOP" onclick="stop_Click" />
    <asp:Button ID="Button3" runat="server" Text="M1" onclick="Button3_Click" />
    <asp:Button ID="Button5" runat="server" Text="M1b" onclick="M1b_Click" />
    <asp:Button ID="Button4" runat="server" Text="M2" onclick="Button4_Click" />
    <asp:Button ID="Button6" runat="server" Text="M2b" onclick="M2b_Click" />
    </p>
    </form>
</body>
</html>
