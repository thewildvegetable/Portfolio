const xxh = require('xxhashjs');

// our socketio instance
let io;

// //Helper Functions
const doHash = string => xxh.h32(`${string}${new Date().getTime()}`, 0xFEFACADE).toString(16);

// sends a socket error
const socketErr = (socket, msg) => {
  socket.emit(Messages.C_Error, { msg });
};

// sets the socket to default state
const setUpSockets = (sock) => {
  const socket = sock;

};

module.exports = setUpSockets;