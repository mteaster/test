console.log("messageboard.js");

function edit(id)
{
    console.log("edit post with id " + id);

    var form = $("<p>form goes here</>");

    $("#content-" + id).hide();
    $("#post-" + id).append(form);
}
