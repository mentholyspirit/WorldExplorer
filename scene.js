const engine = require("~engine")

module.exports.onStart = async function() {
  console.log("Scene: onStart")
}

module.exports.onUpdate = async function(dt) {
  console.log("Scene: onUpdate: begin: " + dt)
  
  console.log("Scene: onUpdate: sendMessage: " + dt)
  await engine.sendMessage("" + dt)

  console.log("Scene: onUpdate: end: " + dt)
}