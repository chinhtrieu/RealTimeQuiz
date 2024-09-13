"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messenger").build();

//Disable the send button until connection is established.
document.getElementById("submitButton").disabled = true;

connection.on("LeaderboardUpdate", function (leaderboard) {
    console.log(leaderboard);

    const ldb = document.getElementById('leaderBoad');
    if (leaderboard.members) {
        ldb.style.visibility = 'visible'; 
        const listElement = document.getElementById('itemList');
        listElement.innerHTML = ''; // Clear any existing content
        leaderboard.members.forEach(item => {
            const listItem = document.createElement('li');
            listItem.className = "list-group-item";
            listItem.textContent = item.userId;
            listElement.appendChild(listItem);
        });
    }
    else {
        ldb.style.visibility = 'hidden';        
    }
  
});

connection.start().then(function () {
    var userId = document.getElementById("UserId").value;
    var quizId = document.getElementById("QuizId").value;
    connection.invoke("GetLeaderboard").catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("submitButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("submitButton").addEventListener("click", function (event) {
    var userId = document.getElementById("UserId").value;
    var quizId = document.getElementById("QuizId").value;
    var answer = document.getElementById("answer").value;
    
    connection.invoke("SubmitAnswer", quizId, userId, answer).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});