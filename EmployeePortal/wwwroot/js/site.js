document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    const auth = urlParams.get("auth");
    const status = urlParams.get("status");

    if (auth === "google" && status === "success") {
        if (typeof showSignInSuccess === "function") {
            showSignInSuccess();
        } 

        // Clear query string after signing in using google
        const newUrl = window.location.origin + window.location.pathname;
        window.history.replaceState({}, document.title, newUrl);
    }
});

// jQuery: Convert to true sentence case
function toSentenceCase(text) {
    if (!text) return "";
    text = text.trim();
    return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase();
}

// Function to display the comingSoonPopup
function showComingSoonPopup() {
    document.getElementById("comingSoonPopup").style.display = "flex";
}

// Function to close the comingSoonPopup
function closePopup() {
    document.getElementById("comingSoonPopup").style.display = "none";
}

function showSignInSuccess() {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: 'success',
        title: 'Signed in successfully',
        showConfirmButton: false,
        timer: 1500,
        timerProgressBar: true
    });
}

function showErrorPopup(message) {
    Swal.fire({
        icon: 'error',
        title: 'Sign in failed',
        text: message,
        confirmButtonText: 'Try Again',
        confirmButtonColor: '#d33'
    });
}

function signUpErrorPopup(message) {
    Swal.fire({
        icon: 'error',
        title: 'Account already exists',
        text: message,
        confirmButtonText: 'Try Again',
        confirmButtonColor: '#d33'
    });
}

function confirmDelete() {
    return Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: "This action cannot be undone!",
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33'
    });
}

$(function () {

    // Show or hide go-to-top button on scroll
    $(window).on("scroll", function () {
        if ($(this).scrollTop() > 200) {
            $("#backToTop").fadeIn();
        } else {
            $("#backToTop").fadeOut();
        }
    });

    // Smooth scroll to top on click
    $("#backToTop").on("click", function () {
        $("html, body").animate({ scrollTop: 0 }, 600); // 600ms animation
        return false;
    });

    $(".social-posts-container p.card-text").each(function () {
        let currentText = $(this).text();
        $(this).text(toSentenceCase(currentText));
    });

    // Delegate the event to the parent container
    $("#postsContainer").on("click", ".post-delete-span", function () {
        let postId = $(this).closest(".card").attr("id");
        if (!postId) return;

        confirmDelete().then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/api/deletepost?id=${postId}`,
                    type: 'POST',
                    success: function (response) {
                        $("#" + postId).remove();
                        Swal.fire('Deleted!', 'Post deleted successfully!', 'success');
                    },
                    error: function () {
                        Swal.fire('Error!', 'Something went wrong while deleting the post.', 'error');
                    }
                });
            }
        });
    });

    $("#createPostForm").on("submit", function (e) {
        e.preventDefault();

        let form = $(this);

        $.ajax({
            url: '/Dashboard/CreatePost',
            type: 'POST',
            data: form.serialize(),
            success: function (response) {
                console.log("Response:", response);
                if (response.success) {
                    let newCard = `
                        <div id="${response.id}" class="card mb-3">
                            <div class="card-body">
                                <h5 class="card-title">${response.title}</h5>
                                <p class="card-text">${response.description}</p>
                                ${response.image ? `<img src="${response.image}" class="img-fluid" />` : ""}
                                <p class="card-text">
                                    <small class="text-muted">Published by ${response.author} on ${response.date}</small>
                                </p>
                                <div class="post-meta-icons">
                                    <span onclick="showComingSoonPopup()"><i class="fa fa-thumbs-up"></i> Like</span>
                                    <span onclick="showComingSoonPopup()"><i class="fa fa-comment"></i> Comment</span>
                                    <span onclick="showComingSoonPopup()"><i class="fa fa-share"></i> Share</span>
                                    <span class="post-delete-span"><i class="fa fa-trash-can"></i> Delete</span>
                                </div>
                            </div>
                        </div>`;
                    $(".social-posts-container").prepend(newCard);
                    $("#createPostForm")[0].reset();
                    showErrorPopup("Post created successfully!");
                }
            },
            error: function (xhr) {
                console.error("Error:", xhr);
                showErrorPopup("Error: " + xhr.status + " " + xhr.responseText);
            }
        });
    });

    // Show login-container and open Sign In by default
    $("#SignInButton").on("click", function (e) {
        e.preventDefault();
        $(".login-container").fadeIn();
        $("#signin-container").show();
        $("#signup-container").hide();
    });

    // Tab toggle logic
    $(".tab-toggle a").on("click", function () {
        let clickedText = $(this).text().trim();

        $(this).addClass().removeClass("active");
        $(this).siblings("active");

        if (clickedText === "Sign in") {
            $("#signin-container").show();
            $("#signup-container").hide();
        } else {
            $("#signup-container").show();
            $("#signin-container").hide();
        }
    });

    //sign in using user form - ajax call
    $("#signin-form").on("submit", function (e) {
        e.preventDefault();

        let form = $(this);
        $.ajax({
            url: '/SignInUsingUsernamePassword',
            type: "POST",
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    showSignInSuccess();
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 1600); // 1600ms = 1.6 seconds
                } else {
                    showErrorPopup(response.message || "Invalid login details");
                }
            },
            error: function () {
                showErrorPopup("Something went wrong. Please try again.");
            }
        });
    });

    //sign up using user form - ajax call

    $("#signup-form").on("submit", function (e) {
        e.preventDefault();

        let form = $(this);
        $.ajax({
            url: '/SignUpUsingForm',
            type: "POST",
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    showSignInSuccess();
                    setTimeout(function () {
                        window.location.href = response.redirectUrl;
                    }, 1600); // 1600ms = 1.6 seconds
                } else {
                    signUpErrorPopup(response.message);
                }
            },
            error: function () {
                signUpErrorPopup("Something went wrong. Please try again.");
            }
        });
    });

    $("#contactUsForm").on("submit", function (e) {
        e.preventDefault(); // prevent normal page reload

        $.ajax({
            url: '/CreateEnquiry', 
            type: 'POST',
            data: $(this).serialize(), // includes anti-forgery token + form fields
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: response.message,
                        confirmButtonColor: '#3085d6'
                    });
                    $("#contactUsForm")[0].reset();
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: response.message || "Something went wrong, please try again."
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                Swal.fire({
                    icon: 'error',
                    title: 'Server Error',
                    text: 'Please try again later.'
                });
            }
        });
    });  
});
