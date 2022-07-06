// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//This code gets our antiforgery cookie and saves it in a variable.
const xsrfToken = document.cookie //The cookie string
    .split("; ") //Split the string by the semi-colon
    .find(row => row.startsWith("XSRF-TOKEN=")) //Search for the correct name
    .split("=")[1]; //Split using the equal sign as the delimeter. Take the 2nd value from the array.

//This function will be triggered by the modify button.
//This function will be called when we want to edit a post. It will get all of the information about a post from the database 
//and populate the post create area with the information, then the body of the post textarea will be focused.
async function PopulatePostArea(postId) {
    //Send an asynchronous request to the backend.
    //The first parameter is the URL we want to send the request to, the second is an options object
    //The URL is a concatenated string to include the postId.
    let response = await fetch('/api/Post/' + postId, {
        method: 'GET', //use the GET verb for GETTING objects.
        credentials: 'same-origin', //Set the credentials header
        headers: {
            'Content-Type': 'application/json', //Tell the server what kind of content we're sending. This is important.
            "X-XSRF-TOKEN": xsrfToken //Send the antiforgery token so the server knows that the request is authentic.
        }
    });

    //Get the json result of our previous response.
    response = await response.json();

    //If the response object is not null, populate the textbox and textarea. Focus the textarea last so that the user doesn't have to click on it.
    if (response != null) {
        document.getElementById("postId").value = response.id;
        document.getElementById("postTitleInput").value = response.title;
        document.getElementById("postBodyInput").value = response.body;
        document.getElementById("postBodyInput").focus();
    } else {
        //If the response is null display an error message on the page.
        alert("There was an error loading the post.");
    }
}

//This function will be triggered when you click the save button.
//This function will be used as the main call from our site to create and update posts.
async function CreatePost() {
    var postId = document.getElementById("postId").value; //Get the postId from the hidden element on the page.
    var title = document.getElementById("postTitleInput").value; //Get the value from the title textbox.
    var body = document.getElementById("postBodyInput").value; //Get the value fromt he body textarea.

    if (postId != "") {
        //If the post id is populated, update the post using the function we created below.
        UpdatePost(postId, title, body);
        return; //Exit the function so the rest below does not run.
    }

    //Create a post object
    let post = {
        key: title,
        value: body
    }

    //Send an asynchronous request to the backend.
    //The first parameter is the URL we want to send the request to, the second is an options object
    let response = await fetch('/api/Post', {
        method: 'POST', //use the POST verb for CREATING the object
        credentials: 'same-origin', //Set the credentials header
        headers: {
            'Content-Type': 'application/json', //Tell the server what kind of content we're sending. This is important.
            "X-XSRF-TOKEN": xsrfToken //Send the antiforgery token so the server knows that the request is authentic.
        },
        body: JSON.stringify(post), //Stringify the post object we created above.
    })

    //Get the json result of our previous response.
    response = await response.json();

    if (response) {
        //Reload the page if we get a sucessful response.
        window.location.reload();
    }
}

//This function will take in a postId, title, and body as parameters. All are strings.
async function UpdatePost(postId, title, body) {
    let post = {
        key: title,
        value: body
    }

    //Send an asynchronous request to the backend.
    //The first parameter is the URL we want to send the request to, the second is an options object
    //The URL is a concatenated string to include the postId.
    let response = await fetch('/api/Post/' + postId, {
        method: 'PATCH', //Use the PATCH verb for UPDATING objects.
        credentials: 'same-origin', //Set the credentials header
        headers: {
            'Content-Type': 'application/json', //Tell the server what kind of content we're sending. This is important.
            "X-XSRF-TOKEN": xsrfToken //Send the antiforgery token so the server knows that the request is authentic.
        },
        body: JSON.stringify(post), //Stringify the post object we created above.
    })

    //Get the json result of our previous response.
    response = await response.json();
    
    if (response) {
        //Reload the page if we get a sucessful response.
        window.location.reload();
    }
}

//This function will be triggered by the delete button.
// to send the request to the server 
async function DeletePost(postId) {
    let response = fetch('/api/Post/' + postId, {
        method: 'DELETE', //Use the DELETE verb for DELETING objects.
        credentials: 'same-origin', //Set the credentials header
        headers: {
            'Content-Type': 'application/json', //Tell the server what kind of content we're sending. This is important.
            "X-XSRF-TOKEN": xsrfToken //Send the antiforgery token so the server knows that the request is authentic.
        }
    })

    //Get the json result of our previous response.
    response = await response.json();
    
    if (response) {
        //Reload the page if we get a sucessful response.
        window.location.reload();
    }
}