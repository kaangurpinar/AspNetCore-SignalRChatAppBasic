var createRoomButton = document.getElementById('create-room-button');
var createRoomModal = document.getElementById('create-room-modal');

//var checkRoomPasswordButton = document.getElementById('check-room-password-button');
//var checkRoomPasswordModal = document.getElementById('check-room-password-modal');
/*
checkRoomPasswordButton.addEventListener('click', function () {
    checkRoomPasswordModal.classList.add('active');
});
*/
createRoomButton.addEventListener('click', function () {
    createRoomModal.classList.add('active');
});

function closeModal() {
    createRoomModal.classList.remove('active');
}
/*
function closePasswordModal() {
    checkRoomPasswordModal.classList.remove('active');
}
*/
