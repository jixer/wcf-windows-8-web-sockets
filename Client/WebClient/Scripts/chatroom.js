/// <reference path="jquery-1.7.1.js" />

$(document).ready(function () {
    // create websocket connection
    var mysocket = new WebSocket("ws://localhost:1397/SocketChatService.svc");

    mysocket.onopen = function (event) {
        // once the connection is open, login to the service
        var msg = { Action: Action.Login, Username: username };
        mysocket.send(JSON.stringify(msg));
    };

    mysocket.onmessage = function (event) {
        // parse the response
        var chatMsg = JSON.parse(event.data);

        // create the span tag and an empty message
        var p = $("<span>");
        var msg = "";

        // if the message is from the logged on user, then set the color to green and add "(me)" to the from section
        if (chatMsg.From == username) {
            msg = chatMsg.From + " (me): " + chatMsg.MessageText;
            p.addClass("myMessage");
        }
        else {
            msg = chatMsg.From + ": " + chatMsg.MessageText;
            p.addClass("message");
        }

        // append a break to the inner text
        msg += "<br />";

        // set the contents of the span with the message and <br> tag
        p.html(msg);

        // append the span to the main chat window
        $("#log").append($(p));
    };

    // register the send button click
    $("#send_button").click(function () {
        // create the message and send it over the Web Socket
        var msg = { Action: Action.Message, Message: { From: username, MessageText: $("#message").val() } };
        mysocket.send(JSON.stringify(msg));

        // reset the message box to blank
        $("#message").val("");
    });
});

// enumeration for action type
var Action = {
    Login: 0,
    Logout: 1,
    Message: 2
}