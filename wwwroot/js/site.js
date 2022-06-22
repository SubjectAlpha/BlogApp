// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const xsrfToken = document.cookie
    .split("; ")
    .find(row => row.startsWith("XSRF-TOKEN="))
    .split("=")[1];

async function CreatePost() {

    var postId = document.getElementById("postId").value;
    console.log("postid", postId);
    if (postId != "") {
        UpdatePost();
        return;
    }

    var title = document.getElementById("postTitleInput").value;
    var body = document.getElementById("postBodyInput").value;

    let post = {
        key: title,
        value: body
    }

    let response = await fetch('/api/Post/Create', {
        method: 'POST',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            "X-XSRF-TOKEN": xsrfToken
        },
        body: JSON.stringify(post),
    })

    response = await response.json();

    if (response) {
        window.location.reload();
    }
}

async function PopulatePostArea(postId) {
    let response = await fetch('/api/Post/' + postId, {
        method: 'GET',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            "X-XSRF-TOKEN": xsrfToken
        }
    });
    
    response = await response.json();
    
    if (response) {
        document.getElementById("postId").value = data.id;
        document.getElementById("postTitleInput").value = data.title;
        document.getElementById("postBodyInput").value = data.body;
        document.getElementById("postBodyInput").focus();
    }
}

async function UpdatePost() {
    var postId = document.getElementById("postId").value;
    var title = document.getElementById("postTitleInput").value;
    var body = document.getElementById("postBodyInput").value;

    let post = {
        key: title,
        value: body
    }

    let response = await fetch('/api/Post/' + postId, {
        method: 'PATCH',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            "X-XSRF-TOKEN": xsrfToken
        },
        body: JSON.stringify(post),
    })

    response = await response.json();
    
    if (response) {
        window.location.reload();
    }
}

async function DeletePost(postId) {
    let response = fetch('/api/Post/' + postId, {
        method: 'DELETE',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json',
            "X-XSRF-TOKEN": xsrfToken
        }
    })

    response = await response.json();
    
    if (response) {
        window.location.reload();
    }
}