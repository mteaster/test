console.log("messageboard.js");

function edit(id)
{
    console.log("editing post " + id);

    var form = $('<form method="post" action="/Dashboard/EditPost?postId=' + id + '><fieldset><legend>Post Message Form</legend><textarea class="text-box multi-line" data-val="true" id="Content" name="Content"></textarea><br><input value="Post" type="submit"><button onclick="cancel(' + id + ')">Cancel</button></fieldset></form>');

    $("#content-" + id).hide();
    $("#post-" + id).append(form);
}

function cancel(id)
{
    console.log("canceling edit of post " + id);

    $("#edit-" + id).remove();
    $("#content-" + id).show();
}