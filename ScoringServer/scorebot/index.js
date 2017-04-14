var util = require("../util");
var config = require("../config");

var scorebot = {};

// Just test data for now
var scores = [{
    name: "Team1",
    score: 0
}, {
    name: "Team2",
    score: 0
}];

scorebot.start = function() {
    console.log("Started!");
    scorebot.check();
};

scorebot.check = function() {
    console.log("Eventually this will do stuff... maybe");
    scores.forEach(function(item) {
        item.score++;
    });
    setTimeout(scorebot.check, util.randomRange(config.scoreIntervalMin, config.scoreIntervalMax));
}

scorebot.getScores = function() {
    return scores;
}

module.exports = scorebot;