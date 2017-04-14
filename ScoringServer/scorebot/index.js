var util = require("../util");
var config = require("../config");

var scorebot = {};

scorebot.start = function() {
    console.log("Started!");
    scorebot.check();
};

scorebot.check = function() {
    console.log("Eventually this will do stuff... maybe");
    setTimeout(scorebot.check, util.randomRange(config.scoreIntervalMin, config.scoreIntervalMax));
}

module.exports = scorebot;