document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    const auth = urlParams.get("auth");
    const status = urlParams.get("status");

    if (auth === "google" && status === "success") {
        if (typeof showSignInSuccess === "function") {
            showSignInSuccess();
        } 

        // Clear query string after handling
        const newUrl = window.location.origin + window.location.pathname;
        window.history.replaceState({}, document.title, newUrl);
    }
});

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

    $(".social-posts-container .post-delete-span").on('click', function () {
        let postId = $(this).closest(".card").attr("id");

        if (!postId) return;

        if (!confirm("Are you sure you want to delete this post?")) return;

        $.ajax({
            url: '/Dashboard/DeletePost',
            type: 'POST',
            data: { id: postId },
            success: function (response) {
                $("#" + postId).remove();
                showErrorPopup("Post deleted successfully!");
            },
            error: function () {
                showErrorPopup("Something went wrong while deleting the post.");
            }
        });
    });

    $("#createPostForm").on("submit", function (e) {
        e.preventDefault();

        let formData = new FormData(this);
        let token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/Dashboard/CreatePost',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            headers: {
                "RequestVerificationToken": token
            },
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
                    showErrorPopup(response.message || "Invalid details");
                }
            },
            error: function () {
                showErrorPopup("Something went wrong. Please try again.");
            }
        });
    });

    function showErrorPopup(message) {
        Swal.fire({
            icon: 'error',
            title: 'Login Failed',
            text: message,
            confirmButtonText: 'Try Again',
            confirmButtonColor: '#d33'
        });
    }

});






let page = 1;
let loading = false;
let noMorePosts = false;

async function loadPosts() {
    if (loading || noMorePosts) return;
    loading = true;
    document.getElementById("loading").style.display = "block";

    const response = await fetch(`/YourControllerName/GetPosts?page=${page}&pageSize=5`);
    const posts = await response.json();

    if (posts.length === 0) {
        noMorePosts = true;
        document.getElementById("no-more-posts").style.display = "block";
    } else {
        const container = document.getElementById("posts-container");

        posts.forEach(post => {
            const card = document.createElement("div");
            card.classList.add("card");
            card.innerHTML = `
                    <div class="card-body">
                        <h5 class="card-title">${post.title}</h5>
                        <p class="card-text">${post.description}</p>
                        ${post.imageData
                    ? `<img src="data:image/jpeg;base64,${post.imageData}" alt="Post Image" class="img-fluid" />`
                    : ""}
                        <p class="card-text"><small class="text-muted">
                            Published by ${post.author} on ${post.dateOfPublishing}
                        </small></p>
                        <div class="post-meta-icons">
                            <span onclick="showComingSoonPopup()"><i class="fa fa-thumbs-up"></i> Like</span>
                            <span onclick="showComingSoonPopup()"><i class="fa fa-comment"></i> Comment</span>
                            <span onclick="showComingSoonPopup()"><i class="fa fa-share"></i> Share</span>
                        </div>
                    </div>
                `;
            container.appendChild(card);
        });

        page++;
    }

    document.getElementById("loading").style.display = "none";
    loading = false;
}

// Initial load
loadPosts();

// Infinite scroll
window.addEventListener("scroll", () => {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 200) {
        loadPosts();
    }
});