/// <reference path="jquery-1.7.1.js" />
$(document).ready(function () {
    var mysocket = new WebSocket("ws://localhost:1397/SocketChatService.svc");

    mysocket.onopen = function (event) {
        var msg = { Action: Action.Login, Username: username };
        mysocket.send(JSON.stringify(msg));
    };

    mysocket.onmessage = function (event) {
        var chatMsg = JSON.parse(event.data);
        var p = $("<p></p>");
        var msg = "";
        if (chatMsg.From == username) {
            msg = chatMsg.From + " (me): " + chatMsg.MessageText;
            p.addClass("myMessage");
        }
        else {
            msg = chatMsg.From + ": " + chatMsg.MessageText;
            p.addClass("message");
        }

        p.text(msg);
        $("#log").append($(p));
    };

    mysocket.onerror = function (error) {
        alert(error);
    };

    $("#send_button").click(function () {
        var msg = { Action: Action.Message, Message: { From: username, MessageText: $("#message").val() } };
        mysocket.send(JSON.stringify(msg));
        $("#message").val("");
    });
});

var Action = {
    Login: 0,
    Logout: 1,
    Message: 2
}