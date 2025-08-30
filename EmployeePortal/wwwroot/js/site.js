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

$(function () {
    $(document).on("click", ".social-posts-container .post-delete-span", function () {
        let postId = $(this).closest(".card").attr("id");

        if (!postId) return;

        if (!confirm("Are you sure you want to delete this post?")) return;

        $.ajax({
            url: '/Dashboard/DeletePost',
            type: 'POST',
            data: { id: postId }, 
            success: function (response) {
                $("#" + postId).remove();
                alert("Post deleted successfully!");
            },
            error: function () {
                alert("Something went wrong while deleting the post.");
            }
        });
    });
});

$(function () {
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
                    alert("Post created successfully!");
                }
            },
            error: function (xhr) {
                console.error("Error:", xhr);
                alert("Error: " + xhr.status + " " + xhr.responseText);
            }
        });
    });
});
