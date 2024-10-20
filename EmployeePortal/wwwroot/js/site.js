document.addEventListener("DOMContentLoaded", function () {
    const socialButtons = document.querySelectorAll('.social-buttons button');

    socialButtons.forEach(button => {
        button.addEventListener('click', function () {
            alert(`Social login using ${this.textContent} is not yet implemented.`);
        });
    });
});

