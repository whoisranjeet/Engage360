// Save the original alert function
const originalAlert = window.alert;

// Override the alert function
window.alert = function (message) {
    // Call the custom modal display function
    showCustomAlert(message);
};

// Custom function to display the message
function showCustomAlert(message) {
    // Create the modal structure
    let modal = document.createElement("div");
    modal.classList.add("custom-alert");

    let content = document.createElement("div");
    content.classList.add("custom-alert-content");

    let messageElem = document.createElement("p");
    messageElem.textContent = message;
    content.appendChild(messageElem);

    let button = document.createElement("button");
    button.textContent = "OK";
    button.classList.add("btn-ok");
    button.onclick = function () {
        // Close the modal
        document.body.removeChild(modal);
    };
    content.appendChild(button);

    modal.appendChild(content);
    document.body.appendChild(modal);
}

// Function to display the popup
function showComingSoonPopup() {
    document.getElementById("comingSoonPopup").style.display = "flex";
}

// Function to close the popup
function closePopup() {
    document.getElementById("comingSoonPopup").style.display = "none";
}
