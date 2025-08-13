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

// SignIn SignUp Toggle
const signInTab = document.getElementById("signInTab");
const signUpTab = document.getElementById("signUpTab");
const signInForm = document.getElementById("signInForm");
const signUpForm = document.getElementById("signUpForm");
const formHeading = document.getElementById("formHeading");

signInTab.addEventListener("click", () => {
signInTab.classList.add("active");
signUpTab.classList.remove("active");
signInForm.classList.add("active");
signUpForm.classList.remove("active");
formHeading.textContent = "Welcome back";
});

signUpTab.addEventListener("click", () => {
signUpTab.classList.add("active");
signInTab.classList.remove("active");
signUpForm.classList.add("active");
signInForm.classList.remove("active");
formHeading.textContent = "Create an account";
});