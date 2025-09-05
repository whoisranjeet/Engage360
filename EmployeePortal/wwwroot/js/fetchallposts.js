let page = 1;
let isLoading = false;
let hasMore = true;
const pageSize = 6;

function loadPosts() {
    if (isLoading || !hasMore) return;
    isLoading = true;

    $.ajax({
        url: `/api/posts?page=${page}&pageSize=${pageSize}`,
        type: "GET",
        beforeSend: function () {
            $("#loading").addClass("show"); // show spinner immediately
        },
        success: function (posts) {
            // Add 0.4 second delay before rendering posts
            setTimeout(() => {
                if (posts.length === 0) {
                    hasMore = false;
                    $("#endMessage").show();
                } else {
                    posts.forEach(p => {
                        // Handle image
                        let imageHtml = "";
                        if (p.imageData && p.imageData.length > 0) {
                            imageHtml = `
                                            <img src="data:image/jpeg;base64,${p.imageData}"
                                                 alt="Post Image" class="img-fluid" />`;
                        }

                        // Handle delete button based on logged-in user & role
                        let deleteHtml = "";                        

                        if (isAdmin || p.author === currentUser) {
                            deleteHtml = `
                                            <span class="post-delete-span" data-id="${p.id}">
                                                <i class="fa fa-trash-can"></i> Delete
                                            </span>`;
                        }

                        // Build post card
                        let card = `
                                        <div id="${p.id}" class="card">
                                            <div class="card-body">
                                                <h5 class="card-title">${p.title}</h5>
                                                <p class="card-text">${p.description}</p>
                                                ${imageHtml}
                                                <p class="card-text">
                                                    <small class="text-muted">
                                                        Published by ${p.author} on ${p.dateOfPublishing}
                                                    </small>
                                                </p>
                                                <div class="post-meta-icons">
                                                    <span onclick="showComingSoonPopup()"><i class="fa fa-thumbs-up"></i> Like</span>
                                                    <span onclick="showComingSoonPopup()"><i class="fa fa-comment"></i> Comment</span>
                                                    <span onclick="showComingSoonPopup()"><i class="fa fa-share"></i> Share</span>
                                                    ${deleteHtml}
                                                </div>
                                            </div>
                                        </div>
                                    `;

                        $("#postsContainer").append(card);
                    });

                    page++;
                }

                // hide spinner after delay
                $("#loading").removeClass("show");
                isLoading = false;
            }, 400); // 0.4 second delay
        },
        error: function () {
            console.error("Failed to load posts");
            $("#loading").removeClass("show");
            isLoading = false;
        }
    });
}

// Infinite Scroll
$(window).on("scroll", function () {
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
        loadPosts();
    }
});

// Initial load
$(function () {
    loadPosts();
});