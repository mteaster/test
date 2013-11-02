console.log("messageboard.js");

function edit(id)
{
    console.log("edit post with id " + id);
    $("li." + id + " div").hide();
}
