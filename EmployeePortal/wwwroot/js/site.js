// Function to display the comingSoonPopup
function showComingSoonPopup() {
    document.getElementById("comingSoonPopup").style.display = "flex";
}

// Function to close the comingSoonPopup
function closePopup() {
    document.getElementById("comingSoonPopup").style.display = "none";
}

// jQuery: Convert to true sentence case
function toSentenceCase(text) {
    if (!text) return "";
    text = text.trim();
    return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
}

$(function () {
    $(".social-posts-container p.card-text").each(function () {
        let currentText = $(this).text();
        $(this).text(toSentenceCase(currentText));
    });
});