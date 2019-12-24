"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

const privateSendMessage = function() {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("messageInput").value = ""
    document.getElementById("messageInput").focus();
}

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, date) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = `<strong>${date} - ${user}</strong>: ${msg}`;
    var li = document.createElement("li");
    li.innerHTML = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
    focusOnNewMessage();
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function(event) {
    privateSendMessage();
    event.preventDefault();
});
document.getElementById("messageInput").addEventListener("keypress", function (event) {
    if (event.keyCode === 13) {
        privateSendMessage();
        event.preventDefault();
    }
});

const focusOnNewMessage = function() {
    const element = document.getElementById("messageListDiv");
    element.scrollTo(0, element.scrollHeight);
}